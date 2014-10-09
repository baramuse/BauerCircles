
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BauerCircles.Shared;
using System.Threading.Tasks;
using Android.Graphics;

namespace BauerCircles.Android
{
	public class BauerCircle : View
	{
		public BauerCircle (Context context) :
			base (context)
		{
			Initialize ();
		}

		public BauerCircle (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public BauerCircle (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			this.SetBackgroundResource (Resource.Drawable.CircleDrawable);

			this.Touch+= HandleTouch;
		}

		void HandleTouch (object sender, TouchEventArgs e)
		{
			MotionEventActions action = e.Event.Action & MotionEventActions.Mask;

			switch (action) {
			case MotionEventActions.Up:
				changeBackgroundColor ();
				break;
			default:
				break;
			}
		}

		private async Task changeBackgroundColor()
		{
			var color = await ColourLoverClient.GetNewRandomColorAsync ();
			this.Background.SetColorFilter (new global::Android.Graphics.Color (color.Rgb.Red, color.Rgb.Green, color.Rgb.Blue), PorterDuff.Mode.Multiply);

			//this.SetBackgroundColor (new global::Android.Graphics.Color (color.Rgb.Red, color.Rgb.Green, color.Rgb.Blue));
		}

	}
}

