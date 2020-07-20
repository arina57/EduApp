using System;
using CrossLibrary;

namespace SharedActivities.Core.ViewModels.Exercises.Results {
    public class GapFillResultsViewModel : CrossViewModel {
        public GapFillViewModel GapFillViewModel { get; }
        public GapFillResultsViewModel(GapFillViewModel gapFillViewModel) {
            this.GapFillViewModel = gapFillViewModel;
        }

        public override void ViewCreated() {
            base.ViewCreated();
            var containerView = FindCrossContainerView("genericScoringContainer");
            containerView.ShowView(GapFillViewModel.ScoringViewModel);
        }
    }
}
