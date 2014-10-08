using System;
using Xamarin.Forms;
using ServiceStack;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BauerCircles.XForms.Shared
{
	public class App
	{
		private static AbsoluteLayout _absLayout;
		private const double CIRCLE_SIZE = 100;
		private static int _numCols = 0;
		private static int _numRows = 0;
		private static double _leftPadding = 0;
		private static double _topPadding = 0;

		public static Page GetMainPage ()
		{	
			 _absLayout = new AbsoluteLayout();

			_absLayout.Children.Add(new ColouredFrame(CIRCLE_SIZE), new Point(0,0));

			_absLayout.SizeChanged += HandleSizeChanged;
			var singletap = new TapGestureRecognizer ();
			singletap.Tapped+= addFrame;
			_absLayout.GestureRecognizers.Add (singletap);

			return new ContentPage { 
				Content = _absLayout,
				BackgroundColor = Color.White
			};
		}

		private static void addFrame (object sender, EventArgs e)
		{
			var x = _leftPadding + ((_absLayout.Children.Count - 1) % _numCols) * CIRCLE_SIZE;
			var y = _topPadding +  Math.Floor ((_absLayout.Children.Count - 1) / (double)_numCols) * CIRCLE_SIZE;


			var newrect = new Rectangle (x, 
				              y, 
				              CIRCLE_SIZE, 
				              CIRCLE_SIZE);

			//move the last Frame to the first grid spot before adding a new one in the center
			_absLayout.Children.Last ().LayoutTo (newrect);
			AbsoluteLayout.SetLayoutBounds (_absLayout.Children.Last (),newrect);


			//add a new frame in the screen center
			if (_absLayout.Children.Count < _numCols * _numRows)
				_absLayout.Children.Add (new ColouredFrame (CIRCLE_SIZE), new Point (_absLayout.Width / 2 - CIRCLE_SIZE / 2, _absLayout.Height / 2 - CIRCLE_SIZE / 2));
		}

		private static void HandleSizeChanged (object sender, EventArgs e)
		{
			double width = _absLayout.Width;
			double height = _absLayout.Height;

			_numCols = (int) Math.Floor (width / (CIRCLE_SIZE));
			_numRows = (int) Math.Floor (height / (CIRCLE_SIZE));
			_leftPadding = (width - (_numCols * CIRCLE_SIZE)) / 2;
			_topPadding = (height - (_numRows * CIRCLE_SIZE)) / 2;

			for (int i = 0; i < _absLayout.Children.Count - 1; i++) {
				var x =  (i % _numCols) * CIRCLE_SIZE;
				var y = Math.Floor (i / (double)_numCols) * CIRCLE_SIZE;
				AbsoluteLayout.SetLayoutBounds (_absLayout.Children [i],
					new Rectangle (x + _leftPadding, y + _topPadding, CIRCLE_SIZE, CIRCLE_SIZE));
			}

			AbsoluteLayout.SetLayoutBounds (_absLayout.Children.Last (), 
				new Rectangle (width / 2 - CIRCLE_SIZE / 2, 
					height / 2 - CIRCLE_SIZE / 2, 
					CIRCLE_SIZE, 
					CIRCLE_SIZE));
		}
	}


}

