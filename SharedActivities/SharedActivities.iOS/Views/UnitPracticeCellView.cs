using System;

using Foundation;
using UIKit;

namespace SharedActivities.iOS.Views {
    public partial class UnitPracticeCellView : UICollectionViewCell {



        public static readonly NSString Key = new NSString(nameof(UnitPracticeCellView));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);

        private int _pageNumber;

        public int LastPostion { get; set; }

        public int PageNumber {
            get => _pageNumber;
            set {
                _pageNumber = value;
                PageNumberLabel.Text = _pageNumber.ToString();
            }
        }
        public string ScoreText {
            get => ScoreLabel.Text;
            set => ScoreLabel.Text = value;
        }

        public void SetImageJson(string json) {
            foreach(var view in DoneImage.Subviews) {
                view.RemoveFromSuperview();
                view.Dispose();
            }
            if (!string.IsNullOrWhiteSpace(json)) {
                DoneImage.AddLottieToView(json);
            }
        }

        public UIColor CellBackgroundColor {
            get => PageNumberLabel.BackgroundColor;
            set => PageNumberLabel.BackgroundColor = value;
        }


        public UnitPracticeCellView(IntPtr handle) : base(handle) {

        }

        public override void AwakeFromNib() {
            base.AwakeFromNib();
            this.Layer.BorderWidth = 1;
            this.Layer.BorderColor = UIColor.Black.CGColor;

        }
    }
}
