using System;
using System.Drawing;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SharedActivities.iOS.CustomViews {
    [Register("CircularProgressBar")]
    public class CircularProgressBar : UIView {
        public CircularProgressBar() : base() {
            Initialize();
        }

        protected internal CircularProgressBar(IntPtr handle) : base(handle) {
        }

        public CircularProgressBar(RectangleF bounds) : base(bounds) {
            Initialize();
        }

        void Initialize() {
        }


        private CAShapeLayer foregroundLayer = new CAShapeLayer();
        private CAShapeLayer backgroundLayer = new CAShapeLayer();

        public float LineWidth {
            get => (float)foregroundLayer.LineWidth;
            set {
                foregroundLayer.LineWidth = value;
                backgroundLayer.LineWidth = BackgroundLineWidth;
            }
        }

        public float BackgroundLineWidth => LineWidth - (0.20f * LineWidth);



        private bool layoutDone = false;
        private CABasicAnimation animation;

        public void SetProgress(float progress, bool withAnimation) {

            //Make sure progress is between 0 and 1
            progress = progress > 1 ? 1 : progress;
            progress = progress < 0 ? 0 : progress;
            foregroundLayer.StrokeEnd = progress;
            if (withAnimation) {

                animation.From = new NSNumber(0);
                animation.To = new NSNumber(progress);
                animation.Duration = 2d;
                animation.FillMode = CAFillMode.Forwards;
                animation.RemovedOnCompletion = false;
                foregroundLayer.AddAnimation(animation, "foregroundAnimation");
            }
        }

        public async Task SetProgressAsync(float progress, int millisDuration = 2000) {
            var taskCompletionSource = new TaskCompletionSource<object>();
            EventHandler<CAAnimationStateEventArgs> completionEvent = (s, e) => taskCompletionSource.TrySetResult(s);

            try {
                animation.AnimationStopped += completionEvent;
                //Make sure progress is between 0 and 1
                progress = progress > 1 ? 1 : progress;
                progress = progress < 0 ? 0 : progress;
                foregroundLayer.StrokeEnd = progress;
                animation.From = new NSNumber(0);
                animation.To = new NSNumber(progress);
                animation.Duration = millisDuration / 1000f;
                animation.FillMode = CAFillMode.Forwards;
                animation.RemovedOnCompletion = false;
                foregroundLayer.AddAnimation(animation, "foregroundAnimation");
                await taskCompletionSource.Task;
            } finally {
                animation.AnimationStopped -= completionEvent;
            }


        }

        private float Radius => (Math.Min((float)Frame.Width, (float)Frame.Height) - LineWidth) / 2;
        private CGPoint PathCenter => this.ConvertPointFromView(Center, Superview);

        public CGColor BarBackgroundColor { get; set; } = UIColor.LightGray.CGColor;
        public CGColor BarColor { get; set; } = UIColor.Blue.CGColor;

        private void MakeBar() {
            Layer.Sublayers = null;
            DrawBackgroundLayer();
            DrawForgroundLayer();
        }

        private void DrawBackgroundLayer() {
            var path = UIBezierPath.FromArc(PathCenter, Radius, 0, (nfloat)(Math.PI * 2), true);
            backgroundLayer.Path = path.CGPath;
            backgroundLayer.StrokeColor = BarBackgroundColor;
            backgroundLayer.LineWidth = BackgroundLineWidth;
            backgroundLayer.FillColor = UIColor.Clear.CGColor;
            Layer.AddSublayer(backgroundLayer);

        }

        private void DrawForgroundLayer() {
            var startAngle = -Math.PI / 2;
            var endAngle = 2 * Math.PI + startAngle;
            var path = UIBezierPath.FromArc(PathCenter, Radius, (nfloat)startAngle, (nfloat)endAngle, true);
            foregroundLayer.Path = path.CGPath;
            foregroundLayer.LineCap = CAShapeLayer.CapRound;
            foregroundLayer.LineWidth = LineWidth;
            foregroundLayer.FillColor = UIColor.Clear.CGColor;
            foregroundLayer.StrokeColor = BarColor;
            Layer.AddSublayer(foregroundLayer);
        }


        public override void AwakeFromNib() {
            base.AwakeFromNib();
            animation = CABasicAnimation.FromKeyPath("strokeEnd");
            MakeBar();
        }



        public override void LayoutSublayersOfLayer(CALayer layer) {
            base.LayoutSublayersOfLayer(layer);
            if (!layoutDone) {
                MakeBar();
                layoutDone = true;
            }
        }

    }
}