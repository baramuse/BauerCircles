using System;
using Xamarin.Forms;
using ServiceStack;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace test.xforms.shared
{
	public class App
	{
		public static Page GetMainPage ()
		{	

			AndroidPclExportClient.Configure();
			//			var nrb = new Button () {
			//				Text = "NR",
			//				BackgroundColor = Color.Lime,
			//				WidthRequest = 50,
			//				HeightRequest = 50
			//			};
			//
			//			var rb = new Button () {
			//				BorderRadius = 25,
			//				Text = "R",
			//				BackgroundColor = Color.Aqua,
			//				WidthRequest = 50,
			//				HeightRequest = 50
			//			};



			//			rb.GestureRecognizers = new TapGestureRecognizer () {
			//			
			//			};

			var absLayout = new AbsoluteLayout();
			absLayout.Children.Add(new MyFrame(25), new Point(20,20));
			absLayout.Children.Add(new MyFrame(25), new Point(100,20));

			//			var tapped = new TapGestureRecognizer () {
			//				NumberOfTapsRequired = 1
			//			};
			//			tapped.Tapped += (sender, e) => {
			//				absLayout.Children.Add(generateNewButton(), e.
			//			};



			return new ContentPage { 
				Content = absLayout
			};
		}

		private static View generateNewButton()
		{
			//			var b = new Button () {
			//				BorderRadius = 25,
			//				Text = "R",
			//				BackgroundColor = generateNewColor(),
			//				WidthRequest = 50,
			//				HeightRequest = 50
			//			};
			//
			//
			//			var singletap = new TapGestureRecognizer ();
			//			singletap.Tapped+= async (object sender, EventArgs e) => 
			//			{
			//				var color = await GetNewRandomColorAsync();
			//				var newcolor = Color.FromHex(color.Hex);
			//				((BoxView)sender).Color = newcolor;
			//			};
			//			//singletap.Tapped += (sender, e) => ((BoxView)sender).Color = generateNewColor();
			//			var doubletap = new TapGestureRecognizer () {
			//				NumberOfTapsRequired = 2
			//			};
			//			doubletap.Tapped += (sender, e) => ((BoxView)sender).Rotation+=45;
			//
			//			b.GestureRecognizers.Add (singletap);
			//			b.GestureRecognizers.Add (doubletap);
			//
			//			var box = new MyBoxView () {
			//				WidthRequest = 50,
			//				HeightRequest = 50,
			//				Color = generateNewColor()
			//			};
			//			box.GestureRecognizers.Add (singletap);
			//			box.GestureRecognizers.Add (doubletap);
			//
			//			var frame = new Frame () {
			//				HasShadow = true,
			//				WidthRequest = 50,
			//				HeightRequest = 50,
			//			};



			return null;
		}


		private static Color[] _colors = new Color[]{
			Color.Aqua,
			Color.Black,
			Color.Blue,
			Color.Yellow
		};

		private static Color generateNewColor()
		{
			return _colors [new Random ().Next (_colors.Length)];
		}

		public static async Task<RandomColorData> GetNewRandomColorAsync()
		{
			var res = await new JsonServiceClient ("http://www.colourlovers.com/api/")
				.GetAsync (new NewRandomColorRequest (){Format = "json"});
			return res.First ();
		}
	}

	public class MyFrame : Frame
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

		public MyFrame (double height) : base()
		{
			var layout = new AbsoluteLayout () {
				WidthRequest = height,
				HeightRequest = height,

			};

			_loadingIndicator = new ActivityIndicator () {
				IsRunning = false,
				WidthRequest = height,
				HeightRequest = height,
			};

			_colorLabel = new Label () 
			{
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				WidthRequest = height,
				HeightRequest = height,
				IsVisible = false,
				BackgroundColor = Color.Red
			};

			layout.Children.Add (_loadingIndicator);
			layout.Children.Add (_colorLabel);

			var singletap = new TapGestureRecognizer ();
			singletap.Tapped+= (object sender, EventArgs e) => 
			{
				ChangeColor();
			};
			var doubletap = new TapGestureRecognizer () {
				NumberOfTapsRequired = 2
			};
			doubletap.Tapped += (sender, e) => {
				Device.BeginInvokeOnMainThread(() => _colorLabel.IsVisible = !_colorLabel.IsVisible);
			};


			layout.GestureRecognizers.Add (doubletap);
			//layout.GestureRecognizers.Add (singletap);

			this.Padding = new Thickness (2);
			this.Content = layout;
			this.HasShadow = true;
		}

		public void ChangeColor()
		{
			Device.BeginInvokeOnMainThread (async () => {
				_colorLabel.IsVisible=false;
				_loadingIndicator.IsRunning = true;
				_loadingIndicator.IsVisible = true;

				_colorData = await App.GetNewRandomColorAsync();
				_colorLabel.Text = _colorData.Title;
				this.BackgroundColor = Color.FromHex (_colorData.Hex);
				_loadingIndicator.IsRunning = false;
				_loadingIndicator.IsVisible = false;
			});
		}
	}

	[Route("colors/random")]
	public class NewRandomColorRequest : IReturn<List<RandomColorData>>
	{
		public string Format { get; set; }
	}

	public class RandomColorData
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Hex { get; set; }
	}
}

