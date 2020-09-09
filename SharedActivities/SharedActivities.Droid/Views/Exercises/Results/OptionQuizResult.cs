using System;
using System.Threading.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Core.ViewModels.Exercises.Interfaces;
using SharedActivities.Core.ViewModels.Exercises.Results;

namespace SharedActivities.Droid.Views.Exercises.Results {
    public class OptionQuizResult : CrossFragment<OptionQuizResultViewModel> {
        private View view;
        private CrossContainerView genericScoringContainer;
        private TextView titleTextView;
        private RecyclerView resultsList;
        private ResultsListAdapter resultsListAdapter;



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
            resultsListAdapter = new ResultsListAdapter(ViewModel.OptionQuiz);
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
            titleTextView.Text = ViewModel.OptionQuiz.TitleText;
        }

        private class ResultsListAdapter : RecyclerView.Adapter {
            private OptionQuizViewModel viewModel;
            private RecyclerView.RecycledViewPool viewPool;

            public ResultsListAdapter(OptionQuizViewModel viewModel) {
                this.viewModel = viewModel;
                viewPool = new RecyclerView.RecycledViewPool();
            }

            public override int ItemCount => viewModel.TotalNumberOfQuestions;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (ResultsCellViewHolder)holder;
                var correct = viewModel.QuestionAnsweredCorrectly(position);

                var questionText = viewModel.GetQuestionText(position);
                if (string.IsNullOrWhiteSpace(questionText)) {
                    viewHolder.QuestionTextView.Visibility = ViewStates.Gone;
                } else {
                    viewHolder.QuestionTextView.Visibility = ViewStates.Visible;
                    viewHolder.QuestionTextView.Text = questionText;
                }

                _ = viewHolder.SetCorrect(correct);
                //viewHolder.AnswerCheckImageView.SetColorFilter(correct ? Color.DarkGreen : Color.Red);
                viewHolder.AnswerTextView.SetBracketedText(viewModel.GetEnteredAnswerText(position));
                viewHolder.AnswerTextView.SetTextColor(Color.Black);
                viewHolder.ExplainationTextView.SetBracketedText(viewModel.GetExplainationFor(position));

            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.result_explanation_cell, parent, false);


                var viewHolder = new ResultsCellViewHolder(view, viewModel);
                view.Click += (s, e) => {
                    viewHolder.Expanded = !viewHolder.Expanded;
                };

                viewHolder.PossibleAnswersRecyclerView.SetRecycledViewPool(viewPool);
                return viewHolder;
            }

            private class ResultsCellViewHolder : RecyclerView.ViewHolder {
                private PossibleAnswersAdapter possibleAnswersAdapter;

                public View View { get; }


                public LottieAnimationView AnswerCheckImageView { get; }
                public TextView QuestionTextView { get; }
                public TextView AnswerTextView { get; }
                private LottieAnimationView ExplandImageView { get; }
                private View DividerView1 { get; }
                public RecyclerView PossibleAnswersRecyclerView { get; }
                public View DividerView2 { get; }
                public TextView ExplainationTitleTextView { get; }
                public TextView ExplainationTextView { get; }

                private View DividerView3 { get; }
                private bool expanded;
                private Task imageLoadTask;

                public bool Expanded {
                    get => expanded;
                    set {
                        expanded = value;
                        bool hasExplain = !string.IsNullOrWhiteSpace(ExplainationTextView.Text);

                        //DividerView1.Visibility = expanded ? ViewStates.Visible : ViewStates.Gone;
                        PossibleAnswersRecyclerView.Visibility = expanded ? ViewStates.Visible : ViewStates.Gone;
                        DividerView2.Visibility = expanded ? ViewStates.Visible : ViewStates.Gone;
                        ExplainationTitleTextView.Visibility = expanded && hasExplain ? ViewStates.Visible : ViewStates.Gone;
                        ExplainationTextView.Visibility = expanded && hasExplain ? ViewStates.Visible : ViewStates.Gone;
                        //ExplandImageView.SetImageResource(expanded ? Android.Resource.Drawable.ArrowUpFloat : Android.Resource.Drawable.ArrowDownFloat);
                        if(expanded) {
                            ExplandImageView.SetAnimationFromJson(Core.Resx.Lottie.expand_less, "expand_less");
                        } else {
                            ExplandImageView.SetAnimationFromJson(Core.Resx.Lottie.expand_more, "expand_more");
                        }
                        
                        DividerView3.Visibility = expanded && hasExplain ? ViewStates.Visible : ViewStates.Gone;
                    }
                }


                public async Task SetCorrect(bool correct) {
                    await imageLoadTask;
                    var correctFrameNumber = Convert.ToInt32(AnswerCheckImageView.Composition.StartFrame);
                    var incorrectFrameNumber = Convert.ToInt32(AnswerCheckImageView.Composition.EndFrame);
                    AnswerCheckImageView.Frame = correct ? correctFrameNumber : incorrectFrameNumber;
                }

                public ResultsCellViewHolder(View view, IExplanationLogic logic) : base(view) {
                    this.View = view;
                    AnswerCheckImageView = view.FindViewById<LottieAnimationView>(Resource.Id.answerCheckImageView);
                    QuestionTextView = view.FindViewById<TextView>(Resource.Id.questionTextView);
                    AnswerTextView = view.FindViewById<TextView>(Resource.Id.answerTextView);
                    ExplandImageView = view.FindViewById<LottieAnimationView>(Resource.Id.explandImageView);
                    DividerView1 = view.FindViewById<View>(Resource.Id.dividerView1);
                    PossibleAnswersRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.possibleAnswersRecyclerView);
                    DividerView2 = view.FindViewById<View>(Resource.Id.dividerView2);
                    ExplainationTitleTextView = view.FindViewById<TextView>(Resource.Id.explainationTitleTextView);
                    ExplainationTextView = view.FindViewById<TextView>(Resource.Id.explainationTextView);
                    DividerView3 = view.FindViewById<View>(Resource.Id.dividerView3);
                    PossibleAnswersRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));
                    possibleAnswersAdapter = new PossibleAnswersAdapter(this, logic);
                    PossibleAnswersRecyclerView.SetAdapter(possibleAnswersAdapter);
                    ExplainationTitleTextView.Text = Core.Resx.String.ExplainationTitle;

                    imageLoadTask = AnswerCheckImageView.SetAnimationFromJsonAsync(Core.Resx.Lottie.marubatsu, "Resx.Lottie.marubatsu");
                    Expanded = false;
                }

                private class PossibleAnswersAdapter : RecyclerView.Adapter {
                    private ResultsCellViewHolder resultsCellViewHolder;
                    private IExplanationLogic chooseBestSentenceLogic;
                    public PossibleAnswersAdapter(ResultsCellViewHolder resultsCellViewHolder, IExplanationLogic logic) {
                        this.resultsCellViewHolder = resultsCellViewHolder;
                        this.chooseBestSentenceLogic = logic;
                    }

                    public override int ItemCount => chooseBestSentenceLogic.GetAnswerOptionCount(resultsCellViewHolder.LayoutPosition);

                    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                        var viewHolder = (AnswerCellViewHolder)holder;
                        viewHolder.AnswerOptionTextView.SetBracketedText(chooseBestSentenceLogic.GetAnswerOptionText(resultsCellViewHolder.LayoutPosition, position));
                        var correct = chooseBestSentenceLogic.GetAnswerOptionCorrect(resultsCellViewHolder.LayoutPosition, position);
                        _ = viewHolder.SetCorrect(correct);
                        //viewHolder.CorrectAnswerCheckImageView.SetColorFilter(correct ? Color.DarkGreen : Color.Red);
                        viewHolder.AnswerOptionTextView.SetTextColor(correct ? Color.DarkGreen : Color.Red);

                    }

                    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                        var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.result_explanation_answer_cell, parent, false);
                        
                        var viewHolder = new AnswerCellViewHolder(view);

                        return viewHolder;
                    }

                    private class AnswerCellViewHolder : RecyclerView.ViewHolder {
                        public LottieAnimationView CorrectAnswerCheckImageView { get; }
                        public TextView AnswerOptionTextView { get; }

                        private Task imageLoadTask;

                        public async Task SetCorrect(bool correct) {
                                await imageLoadTask;
                            var correctFrameNumber = Convert.ToInt32(CorrectAnswerCheckImageView.Composition.StartFrame);
                            var incorrectFrameNumber = Convert.ToInt32(CorrectAnswerCheckImageView.Composition.EndFrame);
                            CorrectAnswerCheckImageView.Frame = correct ? correctFrameNumber : incorrectFrameNumber;
                        }

                        public AnswerCellViewHolder(View view) : base(view) {
                            CorrectAnswerCheckImageView = view.FindViewById<LottieAnimationView>(Resource.Id.correctAnswerCheckImageView);
                            AnswerOptionTextView = view.FindViewById<TextView>(Resource.Id.answerOptionTextView);
                            imageLoadTask = CorrectAnswerCheckImageView.SetAnimationFromJsonAsync(Core.Resx.Lottie.marubatsu, "Resx.Lottie.marubatsu");
                        }
                    }
                }
            }
        }
    }
}