﻿using System;
using UIKit;
using SharedLibrary.iOS;
using Airbnb.Lottie;
using System.Threading.Tasks;
using CrossLibrary.iOS;
using CrossLibrary.iOS.Views;
using Xamarin.Essentials;
using SharedActivities.Core.ViewModels;
using SharedActivities.Core;

namespace SharedActivities.iOS.Views {
    public partial class PracticeHeading : CrossUIViewController<PracticeHeadingViewModel> {


        public PracticeHeading() : base() {
        }

        private LOTAnimationView pointsImage;


        public async Task RefreshScoreAsync() {

            var animateText = PointsText.AnimateTextNumberAsync(200, ViewModel.PreviousPoints, ViewModel.Points);
            await UIView.AnimateAsync(0.1f, () => {
                pointsImage.Transform = CoreGraphics.CGAffineTransform.MakeScale(0.5f, 0.5f);
                pointsImage.Alpha = 0.5f;
                //PointsText.Transform = CoreGraphics.CGAffineTransform.MakeScale(0.5f, 0.5f);
                PointsText.Alpha = 0.5f;
            });
            var animateImage = UIView.AnimateAsync(0.1f, () => {
                pointsImage.Transform = CoreGraphics.CGAffineTransform.MakeScale(1f, 1f);
                pointsImage.Alpha = 1f;
                //PointsText.Transform = CoreGraphics.CGAffineTransform.MakeScale(1f, 1f);
                PointsText.Alpha = 1f;
            });
            ViewModel.PointsRefreshed();
            await Task.WhenAll(animateText, animateImage);
        }
        public override bool ViewCreated { get; protected set; } = false;


        public override void ViewDidLoad() {
            base.ViewDidLoad();
            //var titlebarImage = Functions.LottieFromString(Resx.Lottie.UnitPractice_TitleBar, GlobalColorPalette.VeryDark);
            //FrameView.InsertSubviewBelow(titlebarImage, FrameView.Subviews[0]);
            //titlebarImage.FillParentContraints();
            //titlebarImage.ContentMode = UIViewContentMode.ScaleAspectFit;

            pointsImage = Functions.LottieFromString(ViewModel.PointsImageJson);
            PointsImageFrame.AddSubview(pointsImage);
            pointsImage.FillParentContraints();
            pointsImage.ContentMode = UIViewContentMode.ScaleAspectFit;

            var doneImage = Functions.LottieFromString(ViewModel.CompletedImageJson);
            var perfectImage = Functions.LottieFromString(ViewModel.PerfectImageJson);
            CompletedView.AddSubview(doneImage);
            PerfectView.AddSubview(perfectImage);
            doneImage.FillParentContraints();
            perfectImage.FillParentContraints();
            doneImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            PerfectView.ContentMode = UIViewContentMode.ScaleAspectFit;


            TitleText.Font = UIFont.FromName("Champion-HTF-Welterweight", TitleText.Font.PointSize);
            SubtitleText.Font = UIFont.FromName("ChaletComprime-CologneSixty", TitleText.Font.PointSize);
            ChapterNumberText.Font = UIFont.FromName("Delicious-Heavy", TitleText.Font.PointSize);

            TitleBackgroundView.BackgroundColor = GlobalColorPalette.VeryDark.ToPlatformColor();
            ChapterNumberText.TextColor = GlobalColorPalette.VeryDark.ToPlatformColor();
            PointsText.TextColor = GlobalColorPalette.VeryDark.ToPlatformColor();

            BindText(PointsText, vm => vm.PointsText);
            BindText(ChapterNumberText, vm => vm.ChapterNumber);
            BindText(TitleText, vm => vm.ActivityName);
            BindText(SubtitleText, vm => vm.SubtitleText);
            BindVisiblitiy(PerfectView, vm => vm.ShowPerfectImage);
            BindVisiblitiy(CompletedView, vm => vm.ShowCompletedImage);
            BindText(TimesPerfectText, vm => vm.TimesPerfectText);
            BindText(TimesCompletedText, vm => vm.TimesCompletedText);

        }


        public override void RefreshUILocale() {
            //PointsText.Text = ViewModel.Points.ToString();
            //ChapterNumberText.Text = ViewModel.ChapterNumber.ToString();

            //TitleText.Text = ViewModel.ActivityName;
            //SubtitleText.Text = ViewModel.SubtitleText;

            //PerfectView.Hidden = !ViewModel.ShowPerfectImage;
            //CompletedView.Hidden = !ViewModel.ShowCompletedImage;
            //TimesPerfectText.Text = ViewModel.TimesPerfectText;
            //TimesCompletedText.Text = ViewModel.TimesCompletedText;

            if (ViewModel.PointsChanged) {
                RefreshScoreAsync();
            }
        }
    }
}