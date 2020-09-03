using System;
using System.Drawing;
using Airbnb.Lottie;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.WordWeb {
    public partial class WordWebDotCell : UITableViewCell {
        public static readonly NSString Key = new NSString();
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);
        private Color color = Color.Gray;

        public Color Color {
            get => color;
            set {
                color = value;
                if (Dot != null) {
                    Dot.ColorAll(value);
                }
            }
        }
        public LOTAnimationView Dot { get; private set; }
        public UIView DotContainer => DotView;
        public int Position { get; set; }
        public int MainPhrasePosition { get; set; }

        public CGRect DotFrame => DotView.Frame;

        public WordWebDotCell(IntPtr handle) : base(handle) {
        }

        public override void LayoutSubviews() {
            base.LayoutSubviews();
            if (DotView.Subviews == null || DotView.Subviews.Length == 0) {
                Dot = DotView.AddLottieToView(Core.Resx.Lottie.circle_animation);
                Dot.ColorAll(Color);
            }
        }

    }
}