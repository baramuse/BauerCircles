using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using BauerCircles.Shared;

namespace BauerCircles.iOS
{
	public class DraggableBall : UITextView
	{
		private RectangleF _originalFrame;
		private RandomColorData _colorData;
		private bool _showTextColor = false;

		public DraggableBall (RectangleF frame) : base (frame)
		{
			var pangesture = new UIPanGestureRecognizer (detectPan);

			this.AddGestureRecognizer (pangesture);
			this.Layer.CornerRadius = 25;
			this.Layer.BorderColor = UIColor.Black.CGColor;
			this.Layer.BorderWidth = 2;
			this.BackgroundColor = UIColor.Orange;
			this.TextColor = UIColor.White;

			var doubletouch = new UITapGestureRecognizer (doubletapped);
			doubletouch.NumberOfTapsRequired = 2;
			this.AddGestureRecognizer (doubletouch);

			var singletouch = new UITapGestureRecognizer (tapped);
			singletouch.NumberOfTapsRequired = 1;
			singletouch.RequireGestureRecognizerToFail (doubletouch);
			this.AddGestureRecognizer (singletouch);

			this.TextAlignment = UITextAlignment.Center;
		}

		private async void tapped (UITapGestureRecognizer g)
		{
			UIView.Animate (0.1, 0, UIViewAnimationOptions.Autoreverse, () => this.BackgroundColor = this.BackgroundColor.ColorWithAlpha (0.5f), null);
			_colorData = await ColourLoverClient.GetNewRandomColorAsync ();
			this.Layer.RemoveAllAnimations ();
			InvokeOnMainThread (() => {
				UIView.Animate (1, () => this.BackgroundColor = UIColor.FromRGB (_colorData.Rgb.Red / 255f, _colorData.Rgb.Green / 255f, _colorData.Rgb.Blue / 255f));
				if (_showTextColor)
					this.Text = _colorData.Title;
			});
		}

		private void doubletapped (UITapGestureRecognizer g)
		{
			InvokeOnMainThread (() => {
				if (_colorData != null) {
					_showTextColor = !_showTextColor;
					this.Text = _showTextColor ? _colorData.Title : string.Empty;
				}
			});
		}

		private void detectPan (UIPanGestureRecognizer gesture)
		{
			this.Superview.BringSubviewToFront (this);

			if (gesture.State == UIGestureRecognizerState.Began) {
				_originalFrame = this.Frame;
			} else if (gesture.State == UIGestureRecognizerState.Changed) {
				var translate = gesture.TranslationInView (gesture.View);

				var newFrame = _originalFrame;

				newFrame.X += translate.X;
				newFrame.Y += translate.Y;

				gesture.View.Frame = newFrame;
			} else if (gesture.State == UIGestureRecognizerState.Ended || gesture.State == UIGestureRecognizerState.Cancelled) {
				var newFrame = gesture.View.Frame;

				//prevent the circle from going outside of the screen bounds
				newFrame.X = Math.Max (newFrame.X, 0.0f);
				newFrame.X = Math.Min (newFrame.X, gesture.View.Superview.Bounds.Size.Width - newFrame.Size.Width);

				newFrame.Y = Math.Max (newFrame.Y, 0.0f);
				newFrame.Y = Math.Min (newFrame.Y, gesture.View.Superview.Bounds.Size.Height - newFrame.Size.Height);

				UIView.Animate (0.5, () => gesture.View.Frame = newFrame);
			}
		}
	}

	public partial class BauerCirclesViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public BauerCirclesViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			addNewBall (UIScreen.MainScreen.Bounds.Size.Width / 2, UIScreen.MainScreen.Bounds.Size.Height / 2);

			var touch = new UITapGestureRecognizer (tapped);
			this.View.AddGestureRecognizer (touch);
		}

		private void tapped (UITapGestureRecognizer g)
		{
			var position = g.LocationOfTouch (0, this.View);
			addNewBall (position.X, position.Y);
		}

		private void addNewBall (float x, float y)
		{
			var newball = new DraggableBall (new RectangleF (0, 0, 50, 50));
			this.View.Add (newball);

			UIView.Animate (0.5, () => newball.Frame = new RectangleF (x, y, 50, 50));
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

