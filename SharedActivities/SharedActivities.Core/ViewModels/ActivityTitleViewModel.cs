using System;
using SharedActivities.Core.ViewModels.Exercises;

namespace SharedActivities.Core.ViewModels {
    public class ActivityTitleViewModel : CrossViewModelExtra {

        IExerciseLogic viewModel;
        public ActivityTitleViewModel(IExerciseLogic viewModel) {
            this.viewModel = viewModel;
        }

        public string SubtitleText => viewModel.SubtitleText;
        public string SituationText => viewModel.SituationText;
        public string TitleText => viewModel.TitleText;

        public string IconLottie { get; set; }
    }
}
