﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Taction.Config;

namespace Taction.UIElement {

	/// <summary>
	/// A button that supports holding down to execute key command.
	/// </summary>
	internal class HoldButton : CustomButton {

		internal KeyCommand KeyCommand { set; get; }

		public HoldButton(IPanelItemSpecs specs) {

			var s = (HoldButtonSpecs)specs;
			KeyCommand = s.KeyCommand;
		}

		protected override void OnTouchDown(TouchEventArgs e) {

			base.OnTouchDown(e);

			// Set activation flag
			Tag = true;

			App.Instance.InputSimulator.SimulateKeyDown(KeyCommand);
		}

		protected override void OnTouchLeave(TouchEventArgs e) {

			// Activation flag check
			// @NOTE Needed because TouchLeave can be triggered without TouchDown
			if (Tag == null) return;
			Tag = null;

			App.Instance.InputSimulator.SimulateKeyUp(KeyCommand);
		}

	}
}
