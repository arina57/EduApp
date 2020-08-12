using System;
using System.Drawing;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core;
using SharedActivities.Core.ViewModels;
using Xamarin.Essentials;
using static SharedActivities.Core.ViewModels.UnitPracticeViewModel;

namespace SharedActivities.Droid.Views {
    public class UnitPracticeView : CrossFragment<UnitPracticeViewModel>, ViewTreeObserver.IOnGlobalLayoutListener {
        private ConstraintLayout rootView;
        private TextView doneButton;
        private RecyclerView activitySelectorRecyclerView;
        private LottieAnimationView activitySelectorLeft;
        private LottieAnimationView activitySelectorRight;

        private ActivitySelectorAdapter adapter;



     


        public UnitPracticeView() {
        }

        public override void Prepare(UnitPracticeViewModel model) {
            base.Prepare(model);
            ViewModel.PageIndexChanged += this.ViewModel_PageChanged;
        }



        private void ViewModel_PageChanged(object sender, PageChangedEventArgs e) {
            adapter.NotifyItemChanged(e.FromPage);
            adapter.NotifyItemChanged(e.ToPage);
            RefreshResetButton();
        }


        public override void RefreshUILocale() {
            RefreshResetButton();

            adapter.NotifyDataSetChanged();
        }

        private void RefreshResetButton() {
            if (doneButton.Visibility != ViewStates.Visible && ViewModel.ShowDoneButton) {
                doneButton.Alpha = 0;
                doneButton.Animate().Alpha(1);
            } else if (doneButton.Visibility == ViewStates.Visible && !ViewModel.ShowDoneButton) {
                doneButton.Alpha = 1;
                doneButton.Animate().Alpha(0);
            }
            doneButton.Text = ViewModel.DoneButtonText;
            doneButton.Visibility = ViewModel.ShowDoneButton ? ViewStates.Visible : ViewStates.Gone;
        }

        public override void OnFirstOnResume() {
            base.OnFirstOnResume();
            if (ViewModel.PageCount > 9) {
                activitySelectorLeft.Visibility = ViewStates.Gone;
                activitySelectorRight.Visibility = ViewStates.Gone;
            } else {
                //activitySelectorLeft.SetImageDrawable(await activitySelectorLeftImage);
                //activitySelectorRight.SetImageDrawable(await activitySelectorRightImage);
            }

        }

        public override void OnResume() {
            base.OnResume();

        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Use this to return your custom view for this Fragment
            rootView = (ConstraintLayout)inflater.Inflate(Resource.Layout.unit_practice, container, false);
            doneButton = rootView.FindViewById<TextView>(Resource.Id.doneButton);
            doneButton.SetBackgroundColor(GlobalColorPalette.Medium.ToPlatformColor());
            activitySelectorRecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.activitySelectorRecyclerView);
            activitySelectorLeft = rootView.FindViewById<LottieAnimationView>(Resource.Id.activitySelectorLeft);
            activitySelectorRight = rootView.FindViewById<LottieAnimationView>(Resource.Id.activitySelectorRight);

            adapter = new ActivitySelectorAdapter(this);
            this.activitySelectorRecyclerView.SetAdapter(adapter);
            rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
            doneButton.Click += TryAgainButton_Click;
            //return AddToolbarTo(rootView);
            return rootView;
        }



        private async void TryAgainButton_Click(object sender, EventArgs e) {
            await ViewModel.DoneButtonPressed();
        }

        //private void Logic_ExerciseFinished(object sender, EventArgs e) {
        //    adapter.NotifyItemChanged(ViewModel.CurrentPage);
        //    RefreshUILocale();
        //}

        public void OnGlobalLayout() {
            rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);

            adapter.NotifyDataSetChanged();
        }

        private class ActivitySelectorAdapter : RecyclerView.Adapter {
            UnitPracticeViewModel ViewModel => fragment.ViewModel;
            UnitPracticeView fragment;
            public ActivitySelectorAdapter(UnitPracticeView fragment) {
                this.fragment = fragment;
            }

            public override int ItemCount => ViewModel.PageCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (ViewHolder)holder;
                var maxWidth = Convert.ToInt32(fragment.rootView.Width / 10.5f);

                if (maxWidth > 0) {
                    viewHolder.ItemView.LayoutParameters.Width = maxWidth;
                    viewHolder.ItemView.LayoutParameters.Height = maxWidth;
                }
                viewHolder.TextView.Text = (position + 1).ToString();

                if (ViewModel.CurrentPage == position) {
                    viewHolder.TextView.SetBackgroundColor(GlobalColorPalette.Light.ToPlatformColor());
                } else {
                    viewHolder.TextView.SetBackgroundColor(Color.Transparent.ToPlatformColor());
                }

                viewHolder.DoneImageView.Visibility = ViewModel.ExerciseDone(position) ? ViewStates.Visible : ViewStates.Gone;
                if (ViewModel.ShowScore(position)) {
                    viewHolder.ScoreTextView.Visibility = ViewStates.Visible;
                    viewHolder.ScoreTextView.Text = ViewModel.ScoreText(position);
                } else {
                    viewHolder.ScoreTextView.Visibility = ViewStates.Invisible;
                }
                if (ViewModel.ExerciseDone(position)) {
                    viewHolder.DoneImageView.SetAnimationFromJson(ViewModel.GetDoneImage(position), "done" + position);
                    viewHolder.DoneImageView.RepeatCount = -1;
                    viewHolder.DoneImageView.PlayAnimation();
                }

            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.unit_practice_activity_select_cell, parent, false);
                var viewHolder = new ViewHolder(view);
                view.Click += async (s, e) => {
                    var index = viewHolder.AdapterPosition;
                    await ViewModel.ChangePage(index);
                };
                return viewHolder;
            }

            private class ViewHolder : RecyclerView.ViewHolder {
                public TextView TextView { get; set; }
                public LottieAnimationView DoneImageView { get; set; }
                public TextView ScoreTextView { get; set; }
                public ViewHolder(View itemView) : base(itemView) {
                    TextView = itemView.FindViewById<TextView>(Resource.Id.textView);
                    DoneImageView = itemView.FindViewById<LottieAnimationView>(Resource.Id.doneImageView);
                    ScoreTextView = itemView.FindViewById<TextView>(Resource.Id.scoreTextView);
                    TextView.SetTextColor(GlobalColorPalette.Medium.ToPlatformColor());
                }
            }
        }
    }
}
