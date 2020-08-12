using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core;
using SharedActivities.Core.ViewModels.Exercises.Results;

namespace SharedActivities.Droid.Views.Exercises.Results {
    public class ScoringView : CrossFragment<ScoringViewModel> {
        private ProgressBar progressBar;
        private TextView titleTextView;
        private TextView scoreTextView;
        private TextView commentTextView;
        private TextView pointsTextView;
        private TextView multiplierTextView;
        private TextView perfectBonusTextView;
        private LottieAnimationView waitingImageView;
        private LottieAnimationView resultImageView;



        public ScoringView() {

        }



        public override void OnResume() {
            base.OnResume();

        }

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.scoring, container, false);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
            titleTextView = view.FindViewById<TextView>(Resource.Id.titleTextView);
            scoreTextView = view.FindViewById<TextView>(Resource.Id.scoreTextView);
            commentTextView = view.FindViewById<TextView>(Resource.Id.commentTextView);
            pointsTextView = view.FindViewById<TextView>(Resource.Id.pointsTextView);
            multiplierTextView = view.FindViewById<TextView>(Resource.Id.multiplierTextView);
            perfectBonusTextView = view.FindViewById<TextView>(Resource.Id.perfectBonusTextView);
            progressBar.Max = 100;
            progressBar.Progress = 0;
            waitingImageView = view.FindViewById<LottieAnimationView>(Resource.Id.waitingImageView);
            resultImageView = view.FindViewById<LottieAnimationView>(Resource.Id.resultImageView);

            pointsTextView.Text = string.Format(Core.Resx.String.PlusPoints, 0);


            BindAlpha(resultImageView, vm => vm.ResultAlpha);
            BindAlpha(waitingImageView, vm => vm.WaitingAlpha);

            BindText(multiplierTextView, vm => vm.MultiplierText);
            BindText(commentTextView, vm => vm.FeedbackString);
            BindVisiblitiy(multiplierTextView, vm => vm.MultiplierTextVisible);
            BindVisiblitiy(perfectBonusTextView, vm => vm.PerfectBonusTextVisible);
            BindText(perfectBonusTextView, vm => vm.PerfectBonusText);
            BindAlpha(commentTextView, vm => vm.ResultAlpha);

            BindText(pointsTextView, vm => vm.PointsText);
            BindText(scoreTextView, vm => vm.ScoreText);
            BindText(multiplierTextView, vm => vm.MultiplierText);
            Bind(value => progressBar.Progress = Convert.ToInt32(value * 100), vm => vm.Progress);

            return view;
        }





        public override void RefreshUILocale() {
            waitingImageView.SetAnimationFromJson(ViewModel.ThinkingImageJson, "waiting");
            resultImageView.SetAnimationFromJson(ViewModel.ResultImageJson, "ResultImageJson");
        }
    }
}
