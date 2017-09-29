﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Taction {

	partial class Config {

		public void LoadLayout(string path = null) {

			if (path == null) {

				if (!File.Exists(FileLayoutPath)) {

					var encoding = System.Text.Encoding.UTF8;
					var text = encoding.GetString(Properties.Resources.DefaultConfigLayoutJson);
					File.WriteAllText(FileLayoutPath, text, encoding);
				}

				path = FileLayoutPath;
			}

			if (!File.Exists(path)) {

				((App)App.Current).notificationIcon.ShowBalloonTip(
					"Error",
					Taction.Properties.Resources.DefaultNotificationBubbleErrorMessage,
					Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Error
				);
				return;
			}

			JObject json;
			using (var reader = File.OpenText(path))
			using (var jsonReader = new JsonTextReader(reader)) {

				// @TODO File load error, etc
				json = JObject.Load(jsonReader);
			}

			LoadLayout(json);
		}

		public void LoadLayout(JObject json) {

			// Validation check
			if (!json.IsValid(layoutJsonSchema, out IList<ValidationError> errors)) {

				var errMsgs = new List<string>();
				foreach (var error in errors)
					errMsgs.Add(ParseError(error));

				var errMsg = string.Join(Environment.NewLine, errMsgs);

				throw new FormatException(errMsg);
			}

			// Populate
			this.layout = JsonConvert.DeserializeObject<Layout>(JsonConvert.SerializeObject(json));
		}

		// -- STATIC MEMBERS -- //

		private static string _FileLayoutPath;

		/// <summary>
		/// Cached file path of the config layout file.
		/// </summary>
		public static string FileLayoutPath {
			get {
				if (_FileLayoutPath == null) {

					_FileLayoutPath = string.Format(@"{0}\{1}",
						App.AppDataDir,
						Properties.Resources.ConfigLayoutFileName
					);
				}

				return _FileLayoutPath;
			}
		}

		// -- INTERNAL CLASSES -- //

		/// <summary>
		/// Configuration root definition
		/// </summary>
		public class Layout : IPanelItemSpecs {

			private float _opacity;
			private float _opacityHide;
			private uint _fadeAnimationTime;

			public int size { get; set; }
			public List<IPanelItemSpecs> items { get; set; }

			[JsonConverter(typeof(PanelOrientationConverter))]
			public System.Windows.Controls.Orientation orientation { get; set; }

			[DefaultValue(1)]
			[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
			public float opacity {
				get => _opacity;
				set {
					if (value < 0) value = 0;
					else if (value > 1) value = 1;
					_opacity = value;
				}
			}

			[DefaultValue(0)]
			[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
			public float opacityHide {
				get => _opacityHide;
				set {
					if (value < 0) value = 0;
					else if (value > 1) value = 1;
					_opacityHide = value;
				}
			}

			public bool disableHide { get; set; }

			[DefaultValue(500)]
			[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
			public uint fadeAnimationTime {
				get => _fadeAnimationTime;
				set {
					if (value < 0) value = 0;
					_fadeAnimationTime = value;
				}
			}

			public bool disableFadeAnimation { get; set; }
		}

		[JsonPanelItemCandidates(typeof(ButtonSpecs), typeof(PivotSpecs), typeof(MoverSpecs))]
		public interface IPanelItemSpecs {

			int size { get; set; }

			[JsonConverter(typeof(PanelItemsConverter))]
			List<IPanelItemSpecs> items { get; set; }
		}

		[JsonPanelItemType("button")]
		public class ButtonSpecs : IPanelItemSpecs {

			public int size { get; set; }
			public string text { get; set; }
			public List<IPanelItemSpecs> items { get; set; }

			[JsonProperty("command")]
			public string keyCommand { get; set; }
		}

		[JsonPanelItemType("pivot")]
		public class PivotSpecs : IPanelItemSpecs {

			public int size { get; set; }
			public List<IPanelItemSpecs> items { get; set; }
		}

		[JsonPanelItemType("mover")]
		public class MoverSpecs : IPanelItemSpecs {

			public int size { get; set; }
			public List<IPanelItemSpecs> items { get; set; }

			[DefaultValue("☰☰")]
			[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
			public string text { get; set; }
		}
	}
}