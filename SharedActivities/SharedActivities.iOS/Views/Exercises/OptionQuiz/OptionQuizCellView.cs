using System;
using CoreAnimation;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz {
    public partial class OptionQuizCellView : UICollectionViewCell {
        private CAShapeLayer border;

        public string OptionText { get => OptionLabel.Text; set => OptionLabel.AttributedText = Functions.GetAttributedStringFromBracketedPlainText(value, UIColor.Blue); }
        public int LastPostion { get; internal set; }

        public OptionQuizCellView(IntPtr handle) : base(handle) {

        }

        public override void LayoutSubviews() {
            base.LayoutSubviews();
            //border?.RemoveFromSuperLayer();
            //border?.Dispose();
            border = BorderView.AddDashedBorder(UIColor.Black);

            //OptionLabel.Layer.BorderWidth = 1;
            //OptionLabel.Layer.BorderColor = UIColor.Black.CGColor;
        }
    }
}