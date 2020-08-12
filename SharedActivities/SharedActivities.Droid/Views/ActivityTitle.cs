using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core;
using SharedActivities.Core.ViewModels;

namespace SharedActivities.Droid.Views {
    public class ActivityTitle : CrossFragment<ActivityTitleViewModel> {

        private TextView titleTextView;
        private LottieAnimationView questionIconLottieView;
        private TextView subTitleTextView;
        private TextView situationTextView;



        public ActivityTitle() {
        }

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.activity_title, container, false);
            titleTextView = view.FindViewById<TextView>(Resource.Id.titleTextView);
            subTitleTextView = view.FindViewById<TextView>(Resource.Id.subTitleTextView);
            subTitleTextView.Visibility = ViewStates.Gone;
            questionIconLottieView = view.FindViewById<LottieAnimationView>(Resource.Id.questionIconLottieView);
            situationTextView = view.FindViewById<TextView>(Resource.Id.situationTextView);
            //ViewModel.TextChanged += (s, e) => RefreshUILocale();
            return view;
        }

        public override void OnFirstOnResume() {
            base.OnFirstOnResume();
            if(!string.IsNullOrEmpty(ViewModel.IconLottie)) {
                questionIconLottieView.SetAnimationFromJson(ViewModel.IconLottie, "IconLottie");
            }
            

        }


        public override void RefreshUILocale() {
            titleTextView.Visibility = string.IsNullOrEmpty(ViewModel.TitleText) ? ViewStates.Gone : ViewStates.Visible;
            titleTextView.Text = ViewModel.TitleText;
            subTitleTextView.Visibility = string.IsNullOrEmpty(ViewModel.SubtitleText) ? ViewStates.Gone : ViewStates.Visible;
            subTitleTextView.Text = ViewModel.SubtitleText;
            situationTextView.Visibility = string.IsNullOrEmpty(ViewModel.SituationText) ? ViewStates.Gone : ViewStates.Visible;
            situationTextView.Text = ViewModel.SituationText;
        }
    }
}
