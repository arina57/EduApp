using System;
using CrossLibrary.Interfaces;
using SharedActivities.Core.CrossPlatformInterfaces;
using SharedActivities.Core.ViewModels.Exercises.Interfaces;
using SharedActivities.Core.ViewModels.Exercises.Results;

namespace SharedActivities.Core.ViewModels.Exercises {
    public abstract class OptionQuizContainerViewModel : ExerciseViewModel, IDiscreteProgressTracker {
        #region interface implimentation
        public event EventHandler QuizReset;
        public event EventHandler ProgressChanged;

        protected abstract bool AnswerGrid { get; }
        ICrossContainerView QuizContainerView => FindCrossContainerView("questionsView");

        public override string TitleText => OptionQuizViewModel.TitleText;

        public override string SubtitleText => OptionQuizViewModel.SubtitleText;

        public override string SituationText => OptionQuizViewModel.SituationText;

        public override int NumberOfCorrectAnswers => OptionQuizViewModel.NumberOfCorrectAnswers;

        public override int TotalNumberOfQuestions => OptionQuizViewModel.TotalNumberOfQuestions;
        public override ScoringViewModel ScoringViewModel => OptionQuizViewModel.ScoringViewModel;
        public override CrossViewModelExtra ResultsViewModel => OptionQuizViewModel.ResultsViewModel;
        public override bool Finished => OptionQuizViewModel.Finished;

        public override bool UseFinishButton => optionQuizViewModel.UseFinishButton;
        public int CurrentQuestionNumber => OptionQuizViewModel.CurrentQuestionNumber;


        public GlobalEnums.ProgressState AnswerProgress(int questionNumber) => OptionQuizViewModel.AnswerProgress(questionNumber);

        #endregion

        public CrossViewModelExtra ResultViewModel => new OptionQuizResultViewModel(OptionQuizViewModel);
        public override void Reset() {
            OptionQuizViewModel.Reset();
            RefreshUILocale();
        }
        public int CurrentQuestionIndex => OptionQuizViewModel.CurrentQuestionIndex;

        private OptionQuizViewModel optionQuizViewModel;
        public OptionQuizViewModel OptionQuizViewModel {
            get {
                if (optionQuizViewModel == null) {
                    optionQuizViewModel = new OptionQuizViewModel(optionQuiz, AnswerGrid);
                    optionQuizViewModel.ExerciseFinished += this.OptionQuizViewModel_ExerciseFinished;
                    optionQuizViewModel.TextChanged += this.OptionQuizViewModel_TextChanged;
                    optionQuizViewModel.QuizReset += this.OptionQuizViewModel_QuizReset;
                    optionQuizViewModel.ProgressChanged += this.OptionQuizViewModel_ProgressChanged;
                }
                return optionQuizViewModel;
            }
        }

        private void OptionQuizViewModel_ProgressChanged(object sender, EventArgs e) {
            ProgressChanged?.Invoke(sender, e);
        }

        private void OptionQuizViewModel_QuizReset(object sender, EventArgs e) {
            QuizReset?.Invoke(sender, e);
        }

        private void OptionQuizViewModel_TextChanged(object sender, EventArgs e) {
            OnTextChanged();
        }

        private void OptionQuizViewModel_ExerciseFinished(object sender, EventArgs e) {
            Finish();
        }

        IOptionQuizExercise optionQuiz;
        public OptionQuizContainerViewModel(IOptionQuizExercise optionQuiz) : base(optionQuiz) {
            this.optionQuiz = optionQuiz;
        }


        public override void ViewCreated() {
            base.ViewCreated();
            QuizContainerView.ShowView(OptionQuizViewModel);
        }

    }
}
