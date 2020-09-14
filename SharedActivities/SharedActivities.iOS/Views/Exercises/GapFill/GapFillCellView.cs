using System;

using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.iOS.CustomViews;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS.Views.Exercises.GapFill {
    public partial class GapFillCellView : UITableViewCell {

        public static readonly NSString Key = new NSString(nameof(GapFillCellView));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);
        public int Position { get; private set; } = -1;
        public ReplaceableTextUITextView ReplaceableTextView => ReplaceableText;
        readonly string spacing = "" + '\u00A0' + '\u00A0' + '\u00A0';
        string SpacedText(string text) => spacing + text + spacing;
        public GapFillCellView(IntPtr handle) : base(handle) {
        }




        internal void Setup(GapFillViewModel viewModel, int row) {
            Position = row;
            ReplaceableTextView.LinkColor = UIColor.White;
            ReplaceableTextView.LinkBackgroundColor = viewModel.DefaultTagColor.ToPlatformColor();
            ReplaceableText.Text = viewModel.GetPhrase(row);
            //BackgroundColor =   GlobalColorPalette.Light.ToPlatformColor().ColorWithAlpha(logic.GetRoleNumber(Position) * 0.1f);


            for (int i = 0; i < ReplaceableText.ReplaceableTextCount; i++) {
                ReplaceableText.ReplaceText(i, SpacedText(viewModel.GetAnswerTagString(Position, i)));
                ReplaceableText.SetLinkBackgroundColor(i, viewModel.GetTagColor(row, i));
            }

            if (viewModel.GetRoleImageJson(row) == string.Empty) {
                ImageWidthConstraint.Constant = 0;
            } else {
                ImageWidthConstraint.Constant = 80;
                
                foreach (var view in FaceImageView.Subviews) {
                    view.RemoveFromSuperview();
                    view.Dispose();
                }
                FaceImageView.AddLottieToView(viewModel.GetRoleImageJson(row));
            }
            NameLabel.Text = viewModel.GetRoleName(row);
        }





    }
}