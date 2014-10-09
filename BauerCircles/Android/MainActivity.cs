using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace BauerCircles.Android
{
	[Activity (Label = "Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private RelativeLayout _mainLayout;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			_mainLayout = this.FindViewById<RelativeLayout> (Resource.Id.root_layout);
			_mainLayout.Touch+= mainviewTouched;

			addCircle (120, 120);
		}

		private void addCircle(int x, int y)
		{
			var newcircle = new BauerCircle (this);
			var layoutparams = new RelativeLayout.LayoutParams (120, 120) {
				LeftMargin = x - 60,
				TopMargin = y - 60
			};

			_mainLayout.AddView (newcircle, layoutparams);
		}

		void mainviewTouched (object sender, View.TouchEventArgs e)
		{
			MotionEventActions action = e.Event.Action & MotionEventActions.Mask;

			switch (action) {
			case MotionEventActions.Up:
				var x = e.Event.GetX ();
				var y = e.Event.GetY ();
				addCircle ((int)x, (int)y);

				break;
			default:
				break;
			}
		}

	}
}


