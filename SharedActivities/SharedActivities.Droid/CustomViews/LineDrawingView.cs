using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using SharedActivities.Core.Models;
using Xamarin.Essentials;

namespace SharedActivities.Droid.CustomViews {
    [Register("sharedactivities.droid.customviews.LineDrawingView")]
    public class LineDrawingView : View {
        public List<ColoredLine> Lines { get; set; } = new List<ColoredLine>();

        public ColoredLine Line { get; set; }
        public LineDrawingView(Context context) : base(context) {
        }

        public LineDrawingView(Context context, IAttributeSet attrs) : base(context, attrs) {
        }

        public LineDrawingView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) {
        }

        public LineDrawingView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) {
        }

        protected LineDrawingView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
        }


        protected override void OnDraw(Canvas canvas) {
            var viewRect = new Rect();
            this.GetGlobalVisibleRect(viewRect);

            foreach (ColoredLine line in Lines) {
                canvas.DrawLine(
                    line.Start.X,
                    line.Start.Y,
                    line.Stop.X,
                    line.Stop.Y,
                    new Paint() { Color = line.Color.ToPlatformColor(), StrokeWidth = line.Width });
            }
            if (Line != null) {
                canvas.DrawLine(
                    Line.Start.X,
                    Line.Start.Y,
                    Line.Stop.X,
                    Line.Stop.Y,
                    new Paint() { Color = Line.Color.ToPlatformColor(), StrokeWidth = Line.Width });
            }
        }


    }
}