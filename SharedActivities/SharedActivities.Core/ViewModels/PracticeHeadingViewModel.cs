using System;
using SharedActivities.Core.Data;
using SharedActivities.Core.Models;
using SharedActivities.Core.Models.Database;

namespace SharedActivities.Core.ViewModels {
    public class PracticeHeadingViewModel : CrossViewModelExtra {

        public PracticeHeadingViewModel(IdentityModel activityData) {
            SetActivityData(activityData);
        }

        public bool ShowPerfectImage => TimesPerfect > 0;
        public bool ShowCompletedImage => TimesCompleted > 0;
        public string TimesPerfectText => TimesPerfect > 1 ? TimesPerfect.ToString() : string.Empty;
        public string TimesCompletedText => TimesCompleted > 1 ? TimesCompleted.ToString() : string.Empty;
        public int TimesPerfect => currectStats?.TimesCompletedWithFullPoints ?? 0;
        public int TimesCompleted => currectStats?.TimesCompleted ?? 0;
        public int PreviousPoints { get; private set; } = ModuleDatabaseQueries.LocalDatabase.GetTotalPoints();
        public int Points => ModuleDatabaseQueries.LocalDatabase.GetTotalPoints();
        public string PointsText => Points.ToString();

        public bool PointsChanged => PreviousPoints != Points;
        private ExerciseAttemptStats currectStats;
        IdentityModel activityData;
        private bool isRepeatActiviy;

        public void SetActivityData(IdentityModel activityData) {
            this.activityData = activityData;
            currectStats = ModuleDatabaseQueries.LocalDatabase.GetExistOrNewExerciseAttemptStats(activityData);
            RefreshUILocale();
        }

        public override void RefreshUILocale() {
            base.RefreshUILocale();
            RefreshBindings();
        }




        public bool IsRepeatActiviy {
            get => isRepeatActiviy;
            set {
                isRepeatActiviy = value;
                ProperyChanged(() => ActivityName);
            }
        }
        public string ActivityName => activityData.ActivityName;
        public string SubtitleText => activityData.UnitName;
        public string ChapterNumber => activityData.UnitId.ToString();

        public string CompletedImageJson => Resx.Lottie.done_icon;
        public string PerfectImageJson => Resx.Lottie.donePerfect_icon;

        public string PointsImageJson => Resx.Lottie.pointsIcon;

        public void PointsRefreshed() {
            PreviousPoints = Points;
        }
    }
}
