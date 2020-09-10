using System;
namespace SharedActivities.Core.ViewModels.Exercises.Results {
    public class PhraseMatchResultsViewModel : CrossViewModelExtra {
        public PhraseMatchResultsViewModel(PhraseMatchViewModel phraseMatchViewModel) : base() {
            this.PhraseMatchViewModel = phraseMatchViewModel;
        }

        public PhraseMatchViewModel PhraseMatchViewModel { get; }

        public override void ViewCreated() {
            base.ViewCreated();
            var containerView = FindCrossContainerView("genericScoringContainer");
            containerView.ShowView(PhraseMatchViewModel.ScoringViewModel);
        }
    }
}
