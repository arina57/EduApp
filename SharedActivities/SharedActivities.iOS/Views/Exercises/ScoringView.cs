using System;
using Airbnb.Lottie;
using CrossLibrary.iOS.Views;
using SharedActivities.Core;
using SharedActivities.Core.ViewModels.Exercises.Results;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS.Views.Exercises {
    public partial class ScoringView : CrossUIViewController<ScoringViewModel> {
        private LOTAnimationView waitingImage;
        private LOTAnimationView resultImage;

        public ScoringView(IntPtr handle) : base(handle) {
        }

        public ScoringView() {
        }


        public override void AwakeFromNib() {
            base.AwakeFromNib();



        }


        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            ProgressBar.LineWidth = 8f;
            ProgressBar.BarColor = GlobalColorPalette.Light.ToPlatformColor().CGColor;

            waitingImage = ImageFrame.AddLottieToView(ViewModel.ThinkingImageJson,
              (GlobalColorPalette.VeryLight, "VeryLight"),
              (GlobalColorPalette.Medium, "Medium"),
              (GlobalColorPalette.VeryDark, "VeryDark"));

            resultImage = ImageFrame.AddLottieToView(ViewModel.ResultImageJson,
                (GlobalColorPalette.VeryLight, "VeryLight"),
                (GlobalColorPalette.Medium, "Medium"),
                (GlobalColorPalette.VeryDark, "VeryDark"));

            BindAlpha(resultImage, vm => vm.ResultAlpha);
            BindText(MultiplierLabel, vm => vm.MultiplierText);
            BindText(CommentLabel, vm => vm.FeedbackString);
            BindVisiblitiy(MultiplierLabel, vm => vm.MultiplierTextVisible);
            BindVisiblitiy(PerfectBonusLabel, vm => vm.PerfectBonusTextVisible);
            BindText(PerfectBonusLabel, vm => vm.PerfectBonusText);
            BindAlpha(CommentLabel, vm => vm.ResultAlpha);
            BindAlpha(waitingImage, vm => vm.WaitingAlpha);
            BindText(PointsLabel, vm => vm.PointsText);
            BindText(ScoreLabel, vm => vm.ScoreText);
            BindText(MultiplierLabel, vm => vm.MultiplierText);
            Bind(value => ProgressBar.SetProgress(value, false), vm => vm.Progress);
        }

        public override void ViewDidDisappear(bool animated) {
            base.ViewDidDisappear(animated);
            waitingImage.RemoveFromSuperview();
            resultImage.RemoveFromSuperview();
            waitingImage.Dispose();
            waitingImage = null;
            resultImage.Dispose();
            resultImage = null;
            UnbindAll();

        }

        public override void ViewDidAppear(bool animated) {
            base.ViewDidAppear(animated);
            RefreshUILocale();
        }

        public override void RefreshUILocale() {

        }


        public override void ViewDidLoad() {
            base.ViewDidLoad();
        }
    }
}