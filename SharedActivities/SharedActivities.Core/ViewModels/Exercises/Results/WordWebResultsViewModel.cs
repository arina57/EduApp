using System;
namespace SharedActivities.Core.ViewModels.Exercises.Results {
    public class WordWebResultsViewModel : CrossViewModelExtra {
        public WordWebViewModel WordWebViewModel { get; }
        public WordWebResultsViewModel(WordWebViewModel wordWebViewModel) {
            this.WordWebViewModel = wordWebViewModel;
        }

        public override void ViewCreated() {
            base.ViewCreated();
            var containerView = FindCrossContainerView("genericScoringContainer");
            containerView.ShowView(WordWebViewModel.ScoringViewModel);
        }
    }
}
