using System;
namespace SharedActivities.Core.ViewModels.Exercises.Results {
    public class OptionQuizResultViewModel : CrossViewModelExtra {
        public OptionQuizResultViewModel(OptionQuizViewModel optionQuiz) : base() {
            this.OptionQuiz = optionQuiz;
        }

        public OptionQuizViewModel OptionQuiz { get; }
        public string ExplanationTitle => "Explanation Text";

        public override void ViewCreated() {
            base.ViewCreated();
            var containerView = FindCrossContainerView("genericScoringContainer");
            containerView.ShowView(OptionQuiz.ScoringViewModel);
        }
    }
}