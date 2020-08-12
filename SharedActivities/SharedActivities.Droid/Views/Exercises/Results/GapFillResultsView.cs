using System;
using System.Drawing;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Core.ViewModels.Exercises.Results;
using SharedActivities.Droid.CustomViews;
using Xamarin.Essentials;

namespace SharedActivities.Droid.Views.Exercises.Results {
    public class GapFillResultsView : CrossFragment<GapFillResultsViewModel> {
        private View view;
        private CrossContainerView genericScoringContainer;
        private TextView titleTextView;
        private RecyclerView resultsList;
        private GapfillRecyclerAdapter resultsListAdapter;



        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            view = inflater.Inflate(Resource.Layout.score_page_with_explanation, container, false);
            genericScoringContainer = view.FindViewById<CrossContainerView>(Resource.Id.topFrame);
            titleTextView = view.FindViewById<TextView>(Resource.Id.resultsTitleTextView);
            resultsList = view.FindViewById<RecyclerView>(Resource.Id.resultsList);
            resultsList.SetLayoutManager(new LinearLayoutManager(this.Activity));
            resultsListAdapter = new GapfillRecyclerAdapter(ViewModel.GapFillViewModel);
            resultsList.SetAdapter(resultsListAdapter);
            view.ViewTreeObserver.GlobalLayout += ViewTreeObserver_GlobalLayout;
            return view;

        }

        private void ViewTreeObserver_GlobalLayout(object sender, EventArgs e) {
            view.ViewTreeObserver.GlobalLayout -= ViewTreeObserver_GlobalLayout;
            //make the scoring container 75% of the view
            genericScoringContainer.LayoutParameters.Height = Convert.ToInt32(view.Height * 0.75);
        }

        private void CloseButton_Click(object sender, EventArgs e) {
            this.Dismiss();
        }

        public override void RefreshUILocale() {
            titleTextView.Text = ViewModel.GapFillViewModel.TitleText;
        }

        private class GapfillRecyclerAdapter : RecyclerView.Adapter {
            GapFillViewModel viewModel;
            public GapfillRecyclerAdapter(GapFillViewModel viewModel) {
                this.viewModel = viewModel;
            }

            public override int ItemCount => viewModel.PhraseCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (GapfillViewHolder)holder;
                viewHolder.LineTextView.Tag = position;
                viewHolder.LineTextView.Text = viewModel.GetPhrase(position);
                for (int i = 0; i < viewHolder.LineTextView.ReplaceableTextCount; i++) {
                    viewHolder.LineTextView.ReplaceText(i, viewModel.GetAnswerTagString(position, i));
                    viewHolder.LineTextView.SetLinkBackgroundColor(i, viewModel.GetTagColor(position, i));

                }
                if (string.IsNullOrWhiteSpace(viewModel.GetRoleImageJson(position))) {
                    viewHolder.CharacterImageView.Visibility = ViewStates.Gone;
                } else {
                    viewHolder.CharacterImageView.Visibility = ViewStates.Visible;
                    viewHolder.CharacterImageView.SetAnimationFromJson(viewModel.GetRoleImageJson(position), viewModel.GetRoleImageJson(position));
                }
                viewHolder.RoleNameText.Visibility = string.IsNullOrWhiteSpace(viewModel.GetRoleName(position)) ? ViewStates.Gone : ViewStates.Visible;
                viewHolder.RoleNameText.Text = viewModel.GetRoleName(position);
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.gap_fill_cell, parent, false);
                var viewHolder = new GapfillViewHolder(view);
                viewHolder.LineTextView.LinkColor = Color.White.ToPlatformColor();
                viewHolder.LineTextView.LinkBackgroundColor = viewModel.DefaultTagColor.ToPlatformColor();
                viewHolder.LineTextView.LinkUnderLine = false;
                return viewHolder;
            }
        }
        public class GapfillViewHolder : RecyclerView.ViewHolder {
            public ReplaceableSpanTextView LineTextView { get; }
            public TextView RoleNameText { get; }
            public LottieAnimationView CharacterImageView { get; }

            public GapfillViewHolder(View itemView) : base(itemView) {
                LineTextView = ItemView.FindViewById<ReplaceableSpanTextView>(Resource.Id.lineTextView);
                RoleNameText = ItemView.FindViewById<TextView>(Resource.Id.roleTextView);
                CharacterImageView = itemView.FindViewById<LottieAnimationView>(Resource.Id.characterImageView);
            }
        }
    }
}
