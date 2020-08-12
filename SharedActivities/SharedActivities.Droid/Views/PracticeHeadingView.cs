using System;
using System.Drawing;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using Plugin.CurrentActivity;
using SharedActivities.Core;
using SharedActivities.Core.ViewModels;
using Xamarin.Essentials;

namespace SharedActivities.Droid.Views {
    public class PracticeHeadingView : CrossFragment<PracticeHeadingViewModel> {


        private TextView pointsText;
        private LottieAnimationView pointsImageView;
        private TextView chapterNumberTextView;
        private TextView activityTitleTextView;
        private TextView timesCompletedTextView;
        private TextView timesPerfectTextView;
        private TextView subtitleTextView;
        private LottieAnimationView completedImageView;
        private LottieAnimationView perfectImageView;
        //private TextView chapterTextView;

        private Animation inSet;




        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public PracticeHeadingView() {

        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.practice_heading, container, false);
            //chapterTextView = view.FindViewById<TextView>(Resource.Id.chapterTextView);
            chapterNumberTextView = view.FindViewById<TextView>(Resource.Id.chapterNumberTextView);
            activityTitleTextView = view.FindViewById<TextView>(Resource.Id.activityTitleTextView);
            pointsText = view.FindViewById<TextView>(Resource.Id.pointsText);
            pointsImageView = view.FindViewById<LottieAnimationView>(Resource.Id.pointsImageView);
            subtitleTextView = view.FindViewById<TextView>(Resource.Id.subtitleTextView);
            timesCompletedTextView = view.FindViewById<TextView>(Resource.Id.timesCompletedTextView);
            timesPerfectTextView = view.FindViewById<TextView>(Resource.Id.timesPerfectTextView);
            completedImageView = view.FindViewById<LottieAnimationView>(Resource.Id.completedImageView);
            perfectImageView = view.FindViewById<LottieAnimationView>(Resource.Id.perfectImageView);
            inSet = AnimationUtils.LoadAnimation(CrossCurrentActivity.Current.Activity, Resource.Animation.ScaleFadeIn);


            activityTitleTextView.SetBackgroundColor(GlobalColorPalette.VeryDark.ToPlatformColor());
            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.White.ToPlatformColor());
            gd.SetStroke(6, GlobalColorPalette.VeryDark.ToPlatformColor());
            chapterNumberTextView.Background = gd;
            chapterNumberTextView.SetTextColor(GlobalColorPalette.VeryDark.ToPlatformColor());
            subtitleTextView.SetTextColor(GlobalColorPalette.VeryDark.ToPlatformColor());

            BindText(pointsText, vm => vm.PointsText);
            BindText(chapterNumberTextView, vm => vm.ChapterNumber);
            BindText(activityTitleTextView, vm => vm.ActivityName);
            BindText(subtitleTextView, vm => vm.SubtitleText);
            BindVisiblitiy(perfectImageView, vm => vm.ShowPerfectImage);
            BindVisiblitiy(completedImageView, vm => vm.ShowCompletedImage);
            BindText(timesPerfectTextView, vm => vm.TimesPerfectText);
            BindText(timesCompletedTextView, vm => vm.TimesCompletedText);
            return view;
        }





        public override void RefreshUILocale() {
            if (ViewModel.PointsChanged) {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                RefreshScoreAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }









        public async Task RefreshScoreAsync() {

            pointsText.StartAnimation(inSet);
            pointsImageView.StartAnimation(inSet);
            var animation = pointsText.AnimateTextNumberAsync(200, ViewModel.PreviousPoints, ViewModel.Points);
            ViewModel.PointsRefreshed();
            await animation;

        }

        public override void OnFirstOnResume() {
            base.OnFirstOnResume();
            completedImageView.SetAnimationFromJson(ViewModel.CompletedImageJson, "CompletedImageJson");
            perfectImageView.SetAnimationFromJson(ViewModel.PerfectImageJson, "PerfectImageJson");
            pointsImageView.SetAnimationFromJson(ViewModel.PointsImageJson, "PointsImageJson");
        }
    }
}
