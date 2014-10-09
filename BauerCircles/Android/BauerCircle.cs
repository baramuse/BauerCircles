
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
	public class BauerCircle : View, GestureDetector.IOnDoubleTapListener, GestureDetector.IOnGestureListener
	{
		public bool OnDown (MotionEvent e)
		{
			throw new NotImplementedException ();
		}

		public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			throw new NotImplementedException ();
		}

		public void OnLongPress (MotionEvent e)
		{
			throw new NotImplementedException ();
		}

		public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			throw new NotImplementedException ();
		}

		public void OnShowPress (MotionEvent e)
		{
			throw new NotImplementedException ();
		}

		public bool OnSingleTapUp (MotionEvent e)
		{
			return true;
		}

		bool GestureDetector.IOnDoubleTapListener.OnDoubleTap (MotionEvent e)
		{
			return true;
		}

		bool GestureDetector.IOnDoubleTapListener.OnDoubleTapEvent (MotionEvent e)
		{
			return true;
		}

		bool GestureDetector.IOnDoubleTapListener.OnSingleTapConfirmed (MotionEvent e)
		{
			throw new NotImplementedException ();
		}

		private GestureDetector _detector;

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
			_detector = new GestureDetector (this);
			_detector.SetOnDoubleTapListener (this);

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

