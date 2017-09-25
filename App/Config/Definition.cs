﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArtTouchPanel {

	partial class Config {

		/// <summary>
		/// Configuration root definition
		/// </summary>
		public class Data : PanelSpecs {

			private float _opacity;
			private float _opacityHide;

			[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
			public float opacity {
				get => _opacity;
				set {
					if (value < 0) value = 0;
					else if (value > 1) value = 1;
					_opacity = value;
				}
			}

			[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
			public float opacityHide {
				get => _opacityHide;
				set {
					if (value < 0) value = 0;
					else if (value > 1) value = 1;
					_opacityHide = value;
				}
			}

			[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
			public bool disableHide { get; set; }
		}

		public enum PanelAlignment {
			Vertical,
			Horizontal
		}

		public interface IPanelItemSpecs {

			int size { get; set; }
		}

		public class ButtonSpecs : IPanelItemSpecs {

			public int size { get; set; }
			public string keyCommand { get; set; }
		}

		public class PanelSpecs : IPanelItemSpecs {

			public int size { get; set; }

			[JsonConverter(typeof(PanelAlignmentConverter))]
			public PanelAlignment alignment { get; set; }

			[JsonConverter(typeof(PanelItemsConverter))]
			public List<IPanelItemSpecs> items;
		}
	}
}