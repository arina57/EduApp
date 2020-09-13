using System;
using Android.OS;
using Android.Views;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Droid.CustomViews;

namespace SharedActivities.Droid.Views.Exercises {
    public class BasicOptionQuizView : CrossFragment<BasicOptionQuizViewModel> {
        private DiscreteProgressView progressView;
        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public BasicOptionQuizView() {

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.basic_option_quiz, container, false);
            progressView = view.FindViewById<DiscreteProgressView>(Resource.Id.progressView);
            progressView.ProgressTracker = ViewModel;
            return view;
        }

        public override void RefreshUILocale() {
        }

        public override void OnPause() {
            base.OnPause();

        }
    }
}