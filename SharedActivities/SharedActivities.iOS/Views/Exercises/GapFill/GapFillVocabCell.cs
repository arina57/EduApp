using System;

using Foundation;
using SharedActivities.Core;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS.Views.Exercises.GapFill {
    public partial class GapFillVocabCell : UICollectionViewCell {

        public static readonly NSString Key = new NSString(nameof(GapFillVocabCell));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);

        public string Text { get => VocabLabel.Text; set => VocabLabel.Text = value; }
        public UILabel Label => VocabLabel;
        public GapFillVocabCell(IntPtr handle) : base(handle) {
        }
        public int Position { get; set; } = -1;


        public override void AwakeFromNib() {
            base.AwakeFromNib();
            VocabLabel.BackgroundColor = GlobalColorPalette.Light.ToPlatformColor();
        }
    }
}
