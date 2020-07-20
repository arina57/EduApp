using System;
using SQLite;

namespace SharedActivities.Core.Models.Database {
    /// <summary>
    /// Save indiviual attempt stats
    /// </summary>
    public class AttemptRecord  {
        [PrimaryKey, AutoIncrement]
        public virtual int InstallAttemptId { get; set; } = 0;
        public virtual int AppSessionId { get; set; } = 0;
        public virtual int SessionAttempt { get; set; } = 0;
        public virtual int ProductId { get; set; }
        public virtual int UnitId { get; set; } = 0;
        public virtual int ActivityId { get; set; } = 0;
        public virtual int ItemsAnswered { get; set; } = 0;
        public virtual int ItemsCorrect { get; set; } = 0;
        public virtual int Points { get; set; } = 0;
        public virtual bool ActivityCompleted { get; set; }
        public virtual bool ActivitySkipped { get; set; }
        public virtual DateTime AttemptTime { get; set; } = DateTime.Now;
        public virtual TimeSpan AudioRecorded { get; set; } = new TimeSpan();
        public virtual TimeSpan AudioPlayedBack { get; set; } = new TimeSpan();
        public virtual int RecordedTimes { get; set; } = 0;
        public virtual int PlayBackTimes { get; set; } = 0;
        public virtual TimeSpan TimeInActivity { get; set; } = new TimeSpan();
        public bool EndedIncomplete { get; set; } = false;


        [Ignore]
        public bool NoLongerActive => EndedIncomplete || ActivitySkipped || ActivityCompleted;
        public AttemptRecord() {
        }

        public AttemptRecord(IdentityModel activityDataModel, int sessionAttempt) {
            ProductId = activityDataModel.CourseId;
            UnitId = activityDataModel.UnitId;
            ActivityId = activityDataModel.ActivityId;
            AppSessionId = GlobalItems.Settings.AppSessionCount;
            SessionAttempt = sessionAttempt;
        }
    }
}
