using System;
using Xamarin.Forms;

namespace BauerCircles.XForms.Shared
{
	/// <summary>
	/// Custom ContentView holding a label and an activity indicator
	/// </summary>
	public class ColouredFrame : ContentView
	{
		private RandomColorData _colorData;
		public RandomColorData ColorData {
			get {
				return _colorData;
			}
			set {
				if (value == _colorData)
					return;
				this.BackgroundColor = Color.FromHex (value.Hex);
				_colorData = value;
			}
		}

		private ActivityIndicator _loadingIndicator;
		private Label _colorLabel;
		private TapGestureRecognizer _singleTap;
		private TapGestureRecognizer _doubleTap;

		public ColouredFrame (double height) : base()
		{
			var inner = height;
			_colorData = new RandomColorData ();
			var layout = new AbsoluteLayout () {
				WidthRequest = inner,
				HeightRequest = inner,
			};

			_loadingIndicator = new ActivityIndicator () {
				IsRunning = true,
				IsVisible = false,
				WidthRequest = inner,
				HeightRequest = inner
			};

			_colorLabel = new Label () 
			{
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				WidthRequest = inner,
				HeightRequest = inner,
				Text = "Tap Me!",
				TextColor = Color.White
			};

			layout.Children.Add (_loadingIndicator);
			layout.Children.Add (_colorLabel);

			_singleTap = new TapGestureRecognizer ();
			_singleTap.Tapped+= SingleTap;

			//doubletap only works on iOs for now
			_doubleTap = new TapGestureRecognizer () {
				NumberOfTapsRequired = 2
			};
			_doubleTap.Tapped += (sender, e) => {
				Device.BeginInvokeOnMainThread(() => _colorLabel.Text = string.IsNullOrEmpty(_colorLabel.Text) ? _colorData.Title : string.Empty);
			};

			this.GestureRecognizers.Add (_doubleTap);
			this.GestureRecognizers.Add (_singleTap);


			this.Content = new Frame()
			{
				Content = layout,
				HasShadow = true,
				BackgroundColor = Color.Navy,
				OutlineColor = Color.Gray,
				Padding = new Thickness(1)
			};
		}

		void SingleTap (object sender, EventArgs e)
		{
			ChangeColor();
		}

		public void ChangeColor()
		{
			Device.BeginInvokeOnMainThread (async () => {
				_loadingIndicator.IsVisible = true;
				_colorData = await ColourLoverClient.GetNewRandomColorAsync();
				_colorLabel.Text = string.IsNullOrEmpty(_colorLabel.Text) ? string.Empty : _colorData.Title;
				this.Content.BackgroundColor = Color.FromHex (_colorData.Hex);
				_loadingIndicator.IsVisible = false;
			});
		}
	}
}

