using System;
using System.Collections.Generic;
using System.Linq;

using CrossLibrary;
using SharedActivities.Core.ViewModels.Exercises.Interfaces;
using SharedActivities.Core.CrossPlatformInterfaces;
using SharedActivities.Core.Models;
using SharedActivities.Core.ViewModels.Exercises.Results;
using static SharedActivities.Core.GlobalEnums;

namespace SharedActivities.Core.ViewModels.Exercises {
    public class OptionQuizViewModel : ExerciseViewModel, IDiscreteProgressTracker, IExplanationLogic {


        public event EventHandler ProgressChanged;
        public event EventHandler QuizReset;
        public bool AnswerGrid { get; set; } = false;
        #region public properties
        public bool AcceptAnswers { get; set; } = true;
        public override string TitleText => string.Empty;
        public override string SubtitleText => ActivityData.ActivityDescription;
        public override string SituationText => OptionQuiz.Situation.GetString();
        public int CurrentQuestionNumber { get; protected set; } // Returns the question number as in the order they are shown in ie first question is 0
        public int CurrentQuestionIndex => questionIndexes[CurrentQuestionNumber]; //Get's the original index of the question
        public string ScoreText => NumberOfCorrectAnswers + "/" + TotalNumberOfQuestions;
        public override int TotalNumberOfQuestions => OptionQuiz.QuestionAnswerSetCount;
        public int CurrectQuestionAnswerOptionCount => Finished ? 0 : CurrentQuestionAnswerOptionsCount;

        public override bool Finished => CurrentQuestionNumber >= TotalNumberOfQuestions;

        public override int NumberOfCorrectAnswers {
            get {
                int correctCount = 0;
                for (int i = 0; i < enteredAnswers.Count; i++) {
                    if (QuestionAnsweredCorrectly(i)) {
                        correctCount++;
                    }
                }
                return correctCount;
            }
        }

        public int CurrentQuestionAnswerOptionsCount => Finished ? 0 : GetAnswerOptionCountFor(CurrentQuestionIndex);

        #endregion


        #region Non-public properies & fields

        protected List<int> enteredAnswers;

        protected bool AnswersShuffled => true;
        protected bool QuestionsShuffled => !OptionQuiz.IsOrdered;

        protected int[] questionIndexes; //these will be shuffled if the questions aren't ordered
        protected List<int>[] answerOptionIndexes; //these should be shuffled in most cases
        public IOptionQuizExercise OptionQuiz { get; }



        #endregion

        public OptionQuizViewModel(IOptionQuizExercise optionQuiz, bool useGrid = false) : base(optionQuiz) {
            this.OptionQuiz = optionQuiz;
            AnswerGrid = useGrid;
            ScoringViewModel = new ScoringViewModel(optionQuiz.ActivityData, this, false);
            ResultsViewModel = new OptionQuizResultViewModel(this);
            Reset();
        }

        protected AnswerOption GetCurrentQuestionAnswerOption(int answerNumber) =>
            GetAnswerOption(CurrentQuestionNumber, answerNumber);

        /// <summary>
        /// Gets unshuffled answer options
        /// </summary>
        /// <param name="questionIndex"></param>
        /// <param name="answerIndex"></param>
        /// <returns></returns>
        protected AnswerOption GetAnswerOptionForQuestionAndAnswerIndex(int questionIndex, int answerIndex) =>
            OptionQuiz.GetQuestionAnswerSet(questionIndex).AnswerOptions[answerIndex];


        protected List<AnswerOption> GetAnswerOptionForQuestionIndex(int questionIndex) =>
            OptionQuiz.GetQuestionAnswerSet(questionIndex).AnswerOptions;

        private IQuestionAnswerSet CurrentQuestionAnswerSet => GetQuestionAnswerSet(CurrentQuestionNumber);

        public string CurrentQuestionText => Finished ? string.Empty : CurrentQuestionAnswerSet.Question;

        public override bool UseFinishButton => false;

        public string GetQuestionText(int questionNumber) => OptionQuiz.GetQuestionAnswerSet(questionIndexes[questionNumber]).Question;

        protected IQuestionAnswerSet GetQuestionAnswerSet(int questionNumber) =>
            OptionQuiz.GetQuestionAnswerSet(questionIndexes[questionNumber]);

        protected AnswerOption GetAnswerOption(int questionNumber, int answerNumber) =>
            GetAnswerOptionForQuestionAndAnswerIndex(questionIndexes[questionNumber], answerOptionIndexes[questionNumber][answerNumber]);

        protected int GetAnswerOptionCountFor(int questionIndex) => OptionQuiz.GetQuestionAnswerSet(questionIndex).AnswerOptions.Count;

        public int GetAnswerOptionCount(int questionNumber) => GetAnswerOptionCountFor(questionIndexes[questionNumber]);

        public string GetEnteredAnswerText(int questionNumber) => GetAnswerOption(questionNumber, enteredAnswers[questionNumber]).Text;

        public string GetAnswerOptionText(int answerOptionNumber) =>
            Finished ? string.Empty : GetCurrentQuestionAnswerOption(answerOptionNumber).Text;

        public string GetAnswerOptionText(int questionNumber, int answerNumber) => GetAnswerOption(questionNumber, answerNumber).Text;

        public bool GetAnswerOptionCorrect(int questionNumber, int answerNumber) => GetAnswerOption(questionNumber, answerNumber).Correct;


        public string GetExplainationFor(int questionNumber) => GetQuestionAnswerSet(questionNumber)?.Explaination?.GetString() ?? string.Empty;

        public override bool QuestionAnsweredCorrectly(int questionNumber) => AnswerProgress(questionNumber) == ProgressState.Correct;

        public ProgressState AnswerProgress(int questionNumber) {
            return enteredAnswers[questionNumber] == -1 ? ProgressState.NotDone :
                GetAnswerOption(questionNumber, enteredAnswers[questionNumber]).Correct ?
                ProgressState.Correct : ProgressState.Incorrect;
        }

        public List<ProgressState> GetAnswerProgress() {
            List<ProgressState> progress = new List<ProgressState>();
            for (int i = 0; i < TotalNumberOfQuestions; i++) {
                progress.Add(AnswerProgress(i));
            }
            return progress;
        }

        public bool CheckAnswer(int option) {
            return GetAnswerOption(CurrentQuestionNumber, option).Correct;
        }

        public void SetAnswer(int option) {
            if (AcceptAnswers) {
                enteredAnswers[CurrentQuestionNumber] = option;
                var correct = QuestionAnsweredCorrectly(CurrentQuestionNumber);
                CurrentQuestionNumber++;

                ScoringViewModel.SetAnswered(correct);
                ProgressChanged?.Invoke(this, new ScoreChangedEventArgs(correct));
                OnTextChanged();
                if (Finished) {
                    Finish();
                }
            }
        }



        public override void Reset() {
            CurrentQuestionNumber = 0;

            questionIndexes = Enumerable.Range(0, TotalNumberOfQuestions).ToArray();
            answerOptionIndexes = new List<int>[TotalNumberOfQuestions];
            if (QuestionsShuffled) {
                questionIndexes.Shuffle();
            }

            enteredAnswers = new List<int>();
            Random rng = new Random();
            for (int i = 0; i < TotalNumberOfQuestions; i++) {
                answerOptionIndexes[i] = Enumerable.Range(0, GetAnswerOptionCount(i)).ToList();
                if (AnswersShuffled) {
                    answerOptionIndexes[i].Shuffle();
                }
                enteredAnswers.Add(-1);
            }
            QuizReset?.Invoke(this, new EventArgs());
        }


    }
}
