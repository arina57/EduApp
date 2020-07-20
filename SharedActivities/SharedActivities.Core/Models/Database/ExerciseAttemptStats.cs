using System;
using SQLite;

namespace SharedActivities.Core.Models.Database {
    /// <summary>
    /// This save how many times a exercise was done cumulatively.
    /// This does not show progress over time.
    /// Detailed stats are kept in AttemptRecord
    /// </summary>
    public class ExerciseAttemptStats {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }

        public int UnitId { get; set; }

        public int ActivityId { get; set; }

        public int TimesCompleted { get; set; }
        public int TimesCompletedWithFullPoints { get; set; }
        public int ItemsAnswered { get; set; }
        public int ItemsCorrect { get; set; }
        public int Points { get; set; }
        public TimeSpan AudioRecorded { get; set; }
        public TimeSpan AudioPlayedBack { get; set; }
        public TimeSpan TimeInActivity { get; set; }

        public ExerciseAttemptStats() {
            CourseId = 0;
            UnitId = 0;
            ActivityId = 0;
            TimesCompleted = 0;
            TimesCompletedWithFullPoints = 0;
            ItemsAnswered = 0;
            ItemsCorrect = 0;
            Points = 0;
            AudioRecorded = new TimeSpan();
            AudioPlayedBack = new TimeSpan();
            TimeInActivity = new TimeSpan();
        }

        public ExerciseAttemptStats(IdentityModel activityDataModel) {
            CourseId = activityDataModel.CourseId;
            UnitId = activityDataModel.UnitId;
            ActivityId = activityDataModel.ActivityId;
        }


    }
}
