using System;
namespace SharedActivities.Core.ViewModels.Exercises.Results {
    public class PhraseMatchResultsViewModel : CrossViewModelExtra {
        public PhraseMatchResultsViewModel(PhraseMatchViewModel phraseMatchingPoolViewModel) : base() {
            this.PhraseMatchingPoolViewModel = phraseMatchingPoolViewModel;
        }

        public PhraseMatchViewModel PhraseMatchingPoolViewModel { get; }

        public override void ViewCreated() {
            base.ViewCreated();
            var containerView = FindCrossContainerView("genericScoringContainer");
            containerView.ShowView(PhraseMatchingPoolViewModel.ScoringViewModel);
        }
    }
}
