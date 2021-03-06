﻿using System.Windows;
using System.Windows.Input;

namespace Taction.UIElement {

	internal class MoveButton : CustomButton {

		public MoveButton(IPanelItemSpecs specs) {

			// Special default text
			if (Content == null)
				Content = "☰☰";

			// Add move cursor
			var setter = new Setter {
				Property = CursorProperty,
				Value = Cursors.SizeAll
			};

			// Style is locked, so copy to modify
			var s = new Style(GetType(), Style);
			s.Setters.Add(setter);
			Style = s;
		}

		protected override void OnMouseDown(MouseButtonEventArgs e) {

			base.OnMouseDown(e);

			// Press event is never triggered, so manually trigger it
			IsPressed = true;

			if (e.ChangedButton != MouseButton.Left)
				return;

			e.Handled = true;

			Window.GetWindow(this).DragMove();

			// DragMove is blocking, so this works
			IsPressed = false;
		}

		protected override void OnMouseMove(MouseEventArgs e) {

			base.OnMouseMove(e);

			// Prevent hide due to mouse/pen cursor
			e.Handled = true;
		}
	}
}
