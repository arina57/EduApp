using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossLibrary.Interfaces;
using SharedActivities.Core.Data;
using SharedActivities.Core.Models;
using SharedActivities.Core.ViewModels.Exercises;

namespace SharedActivities.Core.ViewModels {
    public class UnitPracticeViewModel : CrossViewModelExtra {
        public event EventHandler<PageChangedEventArgs> PageIndexChanged;
        public event EventHandler ActivityChanged;
        public event EventHandler ScoreChanged;
        public event EventHandler ExerciseFinished;

        private PracticeHeadingViewModel practiceHeadingViewModel;
        ICrossContainerView ExerciseContainer => FindCrossContainerView("exerciseContainer");
        ICrossContainerView TitleContainer => FindCrossContainerView("title");

        public int Unit { get; }
        public int CourseId { get; }

        private List<IActivityDataModel> exerciseData;
        private ExerciseViewModel[] exerciseViewModels;
        private ModuleFunctions moduleFunctions;
        public int CurrentActivityIndex { get; set; } = 0;
        public int CurrentPage { get; set; } = 0;
        public bool CurrentActivityIsRepeat => CurrentPage >= uniqueExerciseCount;
        public int PageCount => exerciseData.Count;
        private int uniqueExerciseCount;

        //public int ExerciseCount => exerciseData.Count;

        //Lazily instantiate viewmodel
        public ExerciseViewModel CurrentExerciseLogic {
            get {
                if (exerciseViewModels[CurrentActivityIndex] == null) {
                    exerciseViewModels[CurrentActivityIndex] = moduleFunctions.GetViewModelFor(CurrentActivityData);
                    exerciseViewModels[CurrentActivityIndex].ExerciseFinished += this.Logic_ExerciseFinished;
                    exerciseViewModels[CurrentActivityIndex].ScoringViewModel.ScoreChanged += ScoringViewModel_ScoreChanged;
                }
                return exerciseViewModels[CurrentActivityIndex];
            }
        }

        private void ScoringViewModel_ScoreChanged(object sender, EventArgs e) {
            practiceHeadingViewModel.RefreshUILocale();
            ScoreChanged?.Invoke(sender, e);
        }

        private bool PageOpened(int index) => exerciseViewModels[index] != null;

        public IActivityDataModel CurrentActivityData => exerciseData[CurrentActivityIndex];

        public bool ExerciseDone(int index) => PageOpened(index) && exerciseViewModels[index].Finished;

        public bool ExerciseIsScored(int index) => PageOpened(index) ? exerciseViewModels[index].TotalNumberOfQuestions > 0 : false;
        public bool ShowScore(int index) => PageOpened(index) ? ExerciseDone(index) && ExerciseIsScored(index) : false;
        public string ScoreText(int index) => PageOpened(index) && ExerciseDone(index) && ExerciseIsScored(index) ?
            exerciseViewModels[index].NumberOfCorrectAnswers + "/" + exerciseViewModels[index].TotalNumberOfQuestions : string.Empty;
        public bool ScorePerfect(int index) => PageOpened(index) ? exerciseViewModels[index].ScoringViewModel.Perfect : false;



        public UnitPracticeViewModel(int courseId, int unit, ModuleFunctions moduleFunctions) {
            this.moduleFunctions = moduleFunctions;
            this.Unit = unit;
            this.CourseId = courseId;
            this.exerciseData = moduleFunctions.AllActivityDataModel.Where(exercise => exercise.ActivityData.Matches(courseId, unit)).OrderBy(exercise => exercise.ActivityData.OrderPriority).ToList();
            uniqueExerciseCount = exerciseData.Count;
            this.exerciseData.AddRange(exerciseData.Where(exercise => exercise.ActivityData.IsRepeatActivity));

            exerciseViewModels = new ExerciseViewModel[exerciseData.Count];
            practiceHeadingViewModel = new PracticeHeadingViewModel(CurrentActivityData.ActivityData);
        }

        private async void Logic_ExerciseFinished(object sender, EventArgs e) {
            if (CurrentExerciseLogic.HasResultsView) {
                await CurrentExerciseLogic.DismissAsync();
                ExerciseContainer.ShowView(CurrentExerciseLogic.ResultsViewModel);
            }
            ExerciseFinished?.Invoke(sender, e);
            practiceHeadingViewModel.RefreshUILocale();
            RefreshUILocale();
        }

        public override void ViewAppearing() {
            base.ViewAppearing();
            //ExerciseContainer.ShowView(CurrentExerciseLogic);
        }

        public override void ViewCreated() {
            base.ViewCreated();
            ModuleDatabaseQueries.LocalDatabase.EndAllCurrentAttempts();
            TitleContainer.ShowView(practiceHeadingViewModel);
            ExerciseContainer.ShowView(CurrentExerciseLogic);
        }



        public async Task DoneButtonPressed() {
            if (CurrentExerciseLogic.Finished) {
                CurrentExerciseLogic.Reset();
                if (ExerciseContainer.SubCrossViewModel != CurrentExerciseLogic) {
                    await ExerciseContainer.SubCrossViewModel.DismissAsync();
                    ExerciseContainer.ShowView(CurrentExerciseLogic);
                }
            } else if (CurrentExerciseLogic.UseFinishButton) {
                CurrentExerciseLogic.Finish();
            }


            RefreshUILocale();
        }

        public async Task ChangePage(int page) {
            if (CurrentPage != page) {
                var oldPage = CurrentPage;
                CurrentPage = page;
                var activityIndex = CurrentPage;
                if (CurrentActivityIndex != activityIndex) {
                    CurrentActivityIndex = activityIndex;
                    await ExerciseContainer.SubCrossViewModel.DismissAsync();
                    if (!CurrentExerciseLogic.Finished || !CurrentExerciseLogic.HasResultsView) {
                        ExerciseContainer.ShowView(CurrentExerciseLogic);
                    } else {
                        ExerciseContainer.ShowView(CurrentExerciseLogic.ResultsViewModel);
                    }
                }
                practiceHeadingViewModel.SetActivityData(CurrentActivityData.ActivityData);

                practiceHeadingViewModel.IsRepeatActiviy = CurrentActivityIsRepeat;
                practiceHeadingViewModel.RefreshUILocale();
                RefreshUILocale();
                PageIndexChanged?.Invoke(CurrentExerciseLogic, new PageChangedEventArgs(oldPage, page));

            }
        }

        public class PageChangedEventArgs : EventArgs {
            public int FromPage { get; }
            public int ToPage { get; }
            public PageChangedEventArgs(int fromPage, int toPage) {
                FromPage = fromPage;
                ToPage = toPage;
            }
        }
        public string DoneJson => Resx.Lottie.done_icon;
        public string PerfectJson => Resx.Lottie.donePerfect_icon;

        public string DoneButtonText => CurrentExerciseLogic.Finished && CurrentExerciseLogic.UseRetryButton ? Resx.String.TryAgain :
            CurrentExerciseLogic.UseFinishButton ? Resx.String.CheckAnswersButton : string.Empty;
        public bool ShowDoneButton => (CurrentExerciseLogic.Finished && CurrentExerciseLogic.UseRetryButton) || CurrentExerciseLogic.UseFinishButton;

        public string GetDoneImage(int position) {
            if (!ExerciseDone(position)) {
                return string.Empty;
            } else if (ScorePerfect(position)) {
                return PerfectJson;
            } else {
                return DoneJson;
            }
        }


    }
}
