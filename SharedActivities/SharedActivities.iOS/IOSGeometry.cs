using System;
using CoreGraphics;
using UIKit;
using static CrossLibrary.MathAndGeometry;

namespace SharedActivities.iOS {
    public static class IOSGeometry {

        public static DPoint ToDPoint(this CGPoint point) {
            return new DPoint(point.X, point.Y);
        }

        public static FPoint ToFPoint(this CGPoint point) {
            return new FPoint((float)point.X, (float)point.Y);
        }

        public static CGPoint ToCGPoint(this DPoint point) {
            return new CGPoint(point.X, point.Y);
        }

        public static CGPoint ToCGPoint(this FPoint point) {
            return new CGPoint(point.X, point.Y);
        }

        public static CGRect ToCGRect(this FloatRect rect) {
            return new CGRect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static FloatRect ToFRect(this CGRect rect) {
            return new FloatRect((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }

        public static CGPoint GlobalToView(this CGPoint point, UIView view) {
            return UIApplication.SharedApplication.Delegate.GetWindow().ConvertPointToView(point, view);
        }

        public static CGPoint GlobalToView(this DPoint point, UIView view) {
            return UIApplication.SharedApplication.Delegate.GetWindow().ConvertPointToView(point.ToCGPoint(), view);
        }

        public static CGRect GlobalToView(this CGRect rect, UIView view) {
            return UIApplication.SharedApplication.Delegate.GetWindow().ConvertRectToView(rect, view);
        }

        public static CGRect GlobalToView(this FloatRect rect, UIView view) {
            return UIApplication.SharedApplication.Delegate.GetWindow().ConvertRectToView(rect.ToCGRect(), view);
        }

        public static CGPoint ViewToGlobal(this CGPoint point, UIView view) {
            return view.ConvertPointToView(point, UIApplication.SharedApplication.Delegate.GetWindow());
        }

    }
}
