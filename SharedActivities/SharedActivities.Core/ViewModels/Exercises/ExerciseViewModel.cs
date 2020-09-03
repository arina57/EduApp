using CrossLibrary;
using CrossLibrary.Interfaces;
using System;
using SharedActivities.Core.Models;
using SharedActivities.Core.ViewModels.Exercises.Results;
using SharedActivities.Core.Data;

namespace SharedActivities.Core.ViewModels.Exercises {
    public abstract class ExerciseViewModel : CrossViewModelExtra, IExerciseLogic {


        public abstract string TitleText { get; }

        public abstract string SubtitleText { get; }

        public abstract string SituationText { get; }
        private ActivityTitleViewModel activityTitleViewModel;
        private DateTime appearedTime;
        private TimeSpan TimeSinceAppeared => DateTime.Now - appearedTime;

        protected ExerciseViewModel(IActivityDataModel activityDataModel) {
            ActivityData = activityDataModel.ActivityData;
        }

        public ActivityTitleViewModel ActivityTitleViewModel {
            get {
                if (activityTitleViewModel == null) {
                    activityTitleViewModel = new ActivityTitleViewModel(this);
                }
                return activityTitleViewModel;
            }
        }
        ICrossContainerView ActivityTitle => FindCrossContainerView("activityTitle");

        public abstract int NumberOfCorrectAnswers { get; }

        public abstract int TotalNumberOfQuestions { get; }

        public virtual ScoringViewModel ScoringViewModel { get; protected set; }
        public virtual CrossViewModel ResultsViewModel { get; protected set; }
        public virtual bool HasResultsView => ScoringViewModel?.TotalNumberOfQuestions > 0;

        public virtual bool Finished { get; protected set; }
        public abstract bool UseFinishButton { get; }
        public virtual bool UseRetryButton => true;
        public IdentityModel ActivityData { get; }

        public event EventHandler ExerciseFinished;
        public event EventHandler TextChanged;

        public override void RefreshUILocale() {
            base.RefreshUILocale();
            ActivityTitleViewModel.RefreshUILocale();
        }

        public override void ViewCreated() {
            base.ViewCreated();
            //ActivityTitle?.ShowView(ActivityTitleViewModel);
        }
        protected virtual void OnTextChanged() {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract bool QuestionAnsweredCorrectly(int questionNumber);

        public virtual void Reset() {
            Finished = false;
        }

        public void Finish() {
            Finished = true;
            ModuleDatabaseQueries.LocalDatabase.IncrementTimeInActivity(ActivityData, TimeSinceAppeared);
            appearedTime = DateTime.Now;
            ExerciseFinished?.Invoke(this, new EventArgs());
        }

        public override void ViewDisappearing() {
            if (!Finished) {
                ModuleDatabaseQueries.LocalDatabase.IncrementTimeInActivity(ActivityData, TimeSinceAppeared);
                appearedTime = DateTime.Now;
            }
        }

        public override void ViewAppeared() {
            appearedTime = DateTime.Now;
        }
    }
}
