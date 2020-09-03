using System;
using CrossLibrary.iOS.Views;
using SharedActivities.Core;
using SharedActivities.Core.ViewModels;
using UIKit;

namespace SharedActivities.iOS.Views {
    public partial class ActivityTitle : CrossUIViewController<ActivityTitleViewModel> {
        public ActivityTitle(IntPtr handle) : base(handle) {
        }

        public ActivityTitle() {
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();

            QuestionIconView.AddLottieToView(ViewModel.IconLottie, (GlobalColorPalette.Light, "Light"));
        }



        public override void RefreshUILocale() {
            TitleLabel.Text = ViewModel.TitleText;
            SubTitleLabel.Text = ViewModel.SubtitleText;
            SituationLabel.Text = ViewModel.SituationText;

            if (string.IsNullOrEmpty(TitleLabel.Text)) {
                TitleGapConstraint.Constant = 0;
                TitleHeightConstraint.Priority = 999;
            } else {
                TitleGapConstraint.Constant = 6;
                TitleHeightConstraint.Priority = 1;
            }
            if (string.IsNullOrEmpty(SubTitleLabel.Text)) {
                SubtitleGapConstraint.Constant = 0;
                SubtitleHeightContraint.Priority = 999;
            } else {
                SubtitleGapConstraint.Constant = 6;
                SubtitleHeightContraint.Priority = 1;
            }
        }


    }
}