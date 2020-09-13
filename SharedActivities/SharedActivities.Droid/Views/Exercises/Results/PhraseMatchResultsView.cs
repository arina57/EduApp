using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Core.ViewModels.Exercises.Results;

namespace SharedActivities.Droid.Views.Exercises.Results {
    public class PhraseMatchResultsView : CrossFragment<PhraseMatchResultsViewModel> {
        private TextView titleTextView;
        private RecyclerView resultsList;
        //private ResultsListAdapter resultsListAdapter;
        MainPhraseOptionsAdapter mainPhraseOptionsAdapter;
        private View view;
        private CrossContainerView genericScoringContainer;

        public PhraseMatchResultsView() {
        }

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
            mainPhraseOptionsAdapter = new MainPhraseOptionsAdapter(this.ViewModel.PhraseMatchViewModel);
            resultsList.OverScrollMode = OverScrollMode.Never;
            resultsList.SetAdapter(mainPhraseOptionsAdapter);
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
            titleTextView.Text = string.Empty;
        }
        private class MainPhraseOptionsAdapter : RecyclerView.Adapter {
            PhraseMatchViewModel viewModel;
            private RecyclerView.RecycledViewPool subRecyclerViewViewPool;

            public MainPhraseOptionsAdapter(PhraseMatchViewModel viewModel) {
                this.viewModel = viewModel;
                subRecyclerViewViewPool = new RecyclerView.RecycledViewPool();
            }

            public override int ItemCount => viewModel.MainPhraseCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (MainPhraseOptionsViewHolder)holder;
                viewHolder.TextView.Text = viewModel.GetMainPhrase(position);
                viewHolder.View.Tag = viewModel.GetMainPhraseId(position);
                //viewHolder.View.SetBackgroundColor(position % 2 == 0 ? Color.White : Color.LightBlue);
                viewHolder.AnswerAdapter.NotifyDataSetChanged();
                viewHolder.View.Background?.ClearColorFilter();

                viewHolder.AnswerCheckImageView.Visibility = viewModel.Finished ? ViewStates.Visible : ViewStates.Gone;
                var correct = viewModel.QuestionAnsweredCorrectly(position);


                _ = viewHolder.SetCorrectAsync(correct);

            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.phrase_match_cell, parent, false);
                var viewHolder = new MainPhraseOptionsViewHolder(view, viewModel);
                viewHolder.MatchingMatchesRecyclerView.SetRecycledViewPool(subRecyclerViewViewPool);
                return viewHolder;
            }
        }

        public class MainPhraseOptionsViewHolder : RecyclerView.ViewHolder {
            private Task imageLoadTask;

            public LottieAnimationView AnswerCheckImageView { get; }
            public TextView TextView { get; }
            public View View { get; }
            public RecyclerView MatchingMatchesRecyclerView { get; }
            public AnswersMatchesAdapter AnswerAdapter { get; }

            public async Task SetCorrectAsync(bool correct) {
                    await imageLoadTask;
                    var correctFrameNumber = Convert.ToInt32(AnswerCheckImageView.Composition.StartFrame);
                    var incorrectFrameNumber = Convert.ToInt32(AnswerCheckImageView.Composition.EndFrame);
                    AnswerCheckImageView.Frame = correct ? correctFrameNumber : incorrectFrameNumber;
            }

            public MainPhraseOptionsViewHolder(View view, PhraseMatchViewModel viewModel) : base(view) {
                View = view;
                AnswerCheckImageView = view.FindViewById<LottieAnimationView>(Resource.Id.answerCheckImageView);
                this.TextView = view.FindViewById<TextView>(Resource.Id.textView);
                MatchingMatchesRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.matchingMatchesRecyclerView);
                MatchingMatchesRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));
                this.AnswerAdapter = new AnswersMatchesAdapter(this, viewModel);
                MatchingMatchesRecyclerView.SetAdapter(AnswerAdapter);

                imageLoadTask = AnswerCheckImageView.SetAnimationFromJsonAsync(Core.Resx.Lottie.marubatsu, "Resx.Lottie.marubatsu");
            }

        }


        public class AnswersMatchesAdapter : RecyclerView.Adapter {
            MainPhraseOptionsViewHolder mainPhraseOptionsViewHolder;
            private PhraseMatchViewModel viewModel;

            public AnswersMatchesAdapter(MainPhraseOptionsViewHolder mainPhraseOptionsViewHolder, PhraseMatchViewModel viewModel) {
                this.mainPhraseOptionsViewHolder = mainPhraseOptionsViewHolder;
                this.viewModel = viewModel;
            }

            public override int ItemCount => viewModel.GetAnswerCount(mainPhraseOptionsViewHolder.LayoutPosition);

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (AnswerMatchesViewHolder)holder;
                viewHolder.Bind(viewModel, mainPhraseOptionsViewHolder.LayoutPosition, position);
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.wordweb_cell, parent, false);
                view.SetBackgroundResource(Resource.Drawable.dashedboxsolid);
                var viewHolder = new AnswerMatchesViewHolder(view);
                return viewHolder;
            }


            public class AnswerMatchesViewHolder : RecyclerView.ViewHolder {
                public TextView TextView { get; }

                public AnswerMatchesViewHolder(View view) : base(view) {
                    this.TextView = view.FindViewById<TextView>(Resource.Id.textView);
                }

                public void Bind(PhraseMatchViewModel viewModel, int mainPhrase, int position) {
                    TextView.Tag = viewModel.GetAnswerId(mainPhrase, position);
                    TextView.Text = viewModel.GetAnswerPhrase(mainPhrase, position);
                    TextView.Visibility = ViewStates.Visible;
                }
            }
        }
    }
}