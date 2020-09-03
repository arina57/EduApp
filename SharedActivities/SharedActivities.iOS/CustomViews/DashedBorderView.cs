using System;
using CoreAnimation;
using Foundation;
using UIKit;

namespace SharedActivities.iOS.CustomViews {

    [Register("DashedBorderView")]
    public class DashedBorderView : UIView {
        CAShapeLayer dashedBorder = new CAShapeLayer();



        [Export("BorderColor")]
        public UIColor BorderColor { get; private set; } = UIColor.Black;
        [Export("DashWidth")]
        public NSNumber DashWidth { get; private set; } = 2;
        [Export("SpaceWidth")]
        public NSNumber SpaceWidth { get; private set; } = 2;
        [Export("CornerRadius")]
        public nfloat CornerRadius { get; private set; } = 4;


        protected internal DashedBorderView(IntPtr handle) : base(handle) {
        }

        public DashedBorderView() {
        }

        public override void AwakeFromNib() {
            base.AwakeFromNib();
            this.Layer.AddSublayer(dashedBorder);
            ApplyDashBorder();
        }

        public override void LayoutSublayersOfLayer(CALayer layer) {
            base.LayoutSublayersOfLayer(layer);
            ApplyDashBorder();
        }



        private void ApplyDashBorder() {
            dashedBorder.StrokeColor = BorderColor.CGColor;
            dashedBorder.LineDashPattern = new NSNumber[] { DashWidth, SpaceWidth };
            dashedBorder.FillColor = null;
            dashedBorder.CornerRadius = CornerRadius;
            dashedBorder.Path = UIBezierPath.FromRect(this.Bounds).CGPath;
            dashedBorder.Frame = this.Bounds;
        }

    }
}