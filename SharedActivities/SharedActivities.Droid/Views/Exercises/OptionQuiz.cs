using System;
using System.Threading.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Airbnb.Lottie;
using CrossLibrary.Droid.Views;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Droid.CustomViews;
using SharedActivities.Droid.Helpers;

namespace SharedActivities.Droid.Views.Exercises {

    public class OptionQuiz : CrossFragment<OptionQuizViewModel>, ViewTreeObserver.IOnGlobalLayoutListener {
        private View rootView;
        private TextView titleText;
        private TextView questionTextView;
        private RecyclerView answersRecyclerView;
        private LottieAnimationView resultImageView;
        private AnswerListAdapter answersListAdapter;
        private AnimationSet scoreAnimationSet;
        private AlphaAnimation fadeInAnimation;
        private AlphaAnimation fadeOutAnimation;
        int correctFrameNumber = 0;
        int incorrectFrameNumber = 0;
        public bool FillParent { get; }

        public IExerciseLogic ExerciseLogic => ViewModel;

        public OptionQuiz(bool fillParent = false) {
            FillParent = fillParent;
        }

        public OptionQuiz() {
            FillParent = false;
        }


        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            rootView = inflater.Inflate(Resource.Layout.option_quiz, container, false);

            titleText = rootView.FindViewById<TextView>(Resource.Id.titleTextView);
            questionTextView = rootView.FindViewById<TextView>(Resource.Id.questionTextView);
            answersRecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.answersRecyclerView);
            if (FillParent) {
                rootView.LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;
                answersRecyclerView.LayoutParameters.Height = ConstraintLayout.LayoutParams.MatchConstraint;
            }
            resultImageView = rootView.FindViewById<LottieAnimationView>(Resource.Id.resultImageView);

            if (ViewModel.AnswerGrid) {
                answersRecyclerView.SetLayoutManager(new GridLayoutManager(this.Activity, 2));
            } else {
                answersRecyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));
            }

            answersRecyclerView.OverScrollMode = OverScrollMode.Never;

            var inAnimation = new ScaleAnimation(0.5f, 1f, 0.5f, 1f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f) { FillEnabled = true, FillAfter = true, Duration = 600 };
            fadeInAnimation = new AlphaAnimation(0f, 1f) { FillEnabled = true, FillAfter = true, Duration = 500 };
            fadeOutAnimation = new AlphaAnimation(1f, 0f) { FillEnabled = true, FillAfter = true, Duration = 500, StartOffset = 100 };
            var outAnimation = new ScaleAnimation(1f, 0f, 1f, 0f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f) { FillEnabled = true, FillAfter = true, Duration = 500, StartOffset = inAnimation.StartOffset + inAnimation.Duration + 100 };


            scoreAnimationSet = new AnimationSet(true);
            scoreAnimationSet.AddAnimation(inAnimation);
            scoreAnimationSet.AddAnimation(fadeInAnimation);
            scoreAnimationSet.AddAnimation(outAnimation);
            answersListAdapter = new AnswerListAdapter(this);
            answersRecyclerView.SetAdapter(answersListAdapter);
            rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);

            return rootView;
        }
        public void OnGlobalLayout() {
            rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            answersListAdapter.NotifyDataSetChanged();
        }


        public async Task ShowResultAnimation(bool correct) {
            if (resultImageView.Composition == null) {
                await resultImageView.SetAnimationFromJsonAsync(Core.Resx.Lottie.marubatsu, "Resx.Lottie.marubatsu");
                correctFrameNumber = Convert.ToInt32(resultImageView.Composition.StartFrame);
                incorrectFrameNumber = Convert.ToInt32(resultImageView.Composition.EndFrame);
            }
            resultImageView.Visibility = ViewStates.Visible;
            //resultImageView.SetColorFilter(correct ?  Color.DarkGreen : Color.Red);
            resultImageView.Frame = correct ? correctFrameNumber : incorrectFrameNumber;
            //resultImageView.SetImageResource(correct ? Resource.Drawable.correct : Resource.Drawable.incorrect);
            resultImageView.Animation = scoreAnimationSet;
            answersRecyclerView.Animation = fadeOutAnimation;
            answersRecyclerView.Animation.Start();
            await resultImageView.Animation.StartAsync();
            resultImageView.Visibility = ViewStates.Gone;

            answersRecyclerView.Animation = fadeInAnimation;
            answersRecyclerView.Animation.Start();
        }

        public override void RefreshUILocale() {
            answersListAdapter.NotifyDataSetChanged();
            questionTextView.Text = ViewModel.CurrentQuestionText;
            questionTextView.Visibility = string.IsNullOrWhiteSpace(questionTextView.Text) ? ViewStates.Gone : ViewStates.Visible;
        }


        public override void OnPause() {
            base.OnPause();
        }

        private class AnswerListAdapter : RecyclerView.Adapter {
            private OptionQuiz orderedOptionQuiz;
            FontTextViewResizingSyncer fontTextViewResizingSyncer = new FontTextViewResizingSyncer();
            public AnswerListAdapter(OptionQuiz orderedOptionQuiz) {
                this.orderedOptionQuiz = orderedOptionQuiz;
            }

            public override int ItemCount => orderedOptionQuiz.ViewModel.CurrectQuestionAnswerOptionCount;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
                var viewHolder = (ViewHolder)holder;
                if (orderedOptionQuiz.ViewModel.AnswerGrid) {
                    viewHolder.View.LayoutParameters.Height = (int)((float)orderedOptionQuiz.answersRecyclerView.Height / ItemCount * 2);
                } else {
                    viewHolder.View.LayoutParameters.Height = (int)((float)orderedOptionQuiz.answersRecyclerView.Height / ItemCount);
                }
                viewHolder.AnswerButton.SetBracketedText(orderedOptionQuiz.ViewModel.GetAnswerOptionText(position), Color.Navy);
                viewHolder.AnswerButton.SetAutoSizeTextTypeUniformWithConfiguration(5, 25, 1, (int)ComplexUnitType.Sp);
                //viewHolder.View.LayoutParameters.Height = (int)((float)orderedOptionQuiz.answersRecyclerView.Height / orderedOptionQuiz.logic.CurrectQuestionAnswerOptionCount * 2);
            }



            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
                View view;
                if (orderedOptionQuiz.ViewModel.AnswerGrid) {
                    view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.option_quiz_one_in_four_cell, parent, false);
                    view.LayoutParameters.Height = (int)((float)parent.Height / ItemCount * 2);
                    //view.LayoutParameters.Width = (int)((float)parent.Width / 2);
                } else {
                    view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.option_quiz_answer_cell, parent, false);
                    view.LayoutParameters.Height = (int)((float)parent.Height / ItemCount);
                }


                ViewHolder viewHolder = new ViewHolder(view);
                fontTextViewResizingSyncer.AddSyncedItem(viewHolder.AnswerButton);

                viewHolder.AnswerButton.Click += async (s, e) => {

                    if (viewHolder.LayoutPosition > -1 && orderedOptionQuiz.ViewModel.AcceptAnswers) {
                        orderedOptionQuiz.ViewModel.AcceptAnswers = false; //prevents more clicks while awaiting
                        var correct = orderedOptionQuiz.ViewModel.CheckAnswer(viewHolder.LayoutPosition); //check the answer without setting it.
                        viewHolder.View.SetBackgroundColor(correct ? Color.Green : Color.Red);
                        await orderedOptionQuiz.ShowResultAnimation(correct); //give feedback
                        orderedOptionQuiz.ViewModel.AcceptAnswers = true;
                        orderedOptionQuiz.ViewModel.SetAnswer(viewHolder.LayoutPosition); //set the actual answer
                        viewHolder.View.SetBackgroundColor(Color.Transparent);
                        orderedOptionQuiz.RefreshUILocale();
                    }
                };
                return viewHolder;
            }



            private class ViewHolder : RecyclerView.ViewHolder {
                public FontTextView AnswerButton { get; }
                public View View { get; }
                public ViewHolder(View itemView) : base(itemView) {
                    View = itemView;
                    AnswerButton = itemView.FindViewById<FontTextView>(Resource.Id.answerButton);
                }
            }
        }
    }
}