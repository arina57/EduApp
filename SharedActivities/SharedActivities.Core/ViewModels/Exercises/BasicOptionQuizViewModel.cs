using System;
using SharedActivities.Core.ViewModels.Exercises.Interfaces;

namespace SharedActivities.Core.ViewModels.Exercises {
    public class BasicOptionQuizViewModel : OptionQuizContainerViewModel {
        public BasicOptionQuizViewModel(IOptionQuizExercise optionQuiz) : base(optionQuiz) {
        }
        protected override bool AnswerGrid => true;

        public override bool QuestionAnsweredCorrectly(int questionNumber) => OptionQuizViewModel.QuestionAnsweredCorrectly(questionNumber);
    }
}
