using System;
namespace SharedActivities.Core.Models {
    public class IdentityModel : IEquatable<IdentityModel>, IEquatable<object> {
        public virtual int CourseId { get; set; }
        public virtual int UnitId { get; set; }
        public virtual int ActivityId { get; set; }
        public virtual int OrderPriority { get; set; }
        public virtual TranslatedText ActivityNameTranslatedText { get; set; }
        public virtual TranslatedText UnitNameTranslatedText { get; set; }
        public virtual TranslatedText ActivityDescriptionTranslatedText { get; set; }

        public virtual bool IsRepeatActivity { get; set; } = false;

        public virtual string ActivityName {
            get => ActivityNameTranslatedText?.GetString() ?? string.Empty;
            set => ActivityNameTranslatedText = new TranslatedText(value);
        }

        public virtual string UnitName {
            get => UnitNameTranslatedText?.GetString() ?? string.Empty;
            set => UnitNameTranslatedText = new TranslatedText(value);
        }
        public virtual string ActivityDescription {
            get => ActivityDescriptionTranslatedText?.GetString() ?? string.Empty;
            set => ActivityDescriptionTranslatedText = new TranslatedText(value);
        }

        public bool Matches(int courseId) =>
            this.CourseId == courseId;


        public bool Matches(int courseId, int unitId) =>
            this.CourseId == courseId
            && this.UnitId == unitId;

        public bool Matches(int courseId, int unitId, int activityId) =>
            this.CourseId == courseId
            && this.UnitId == unitId
            && this.ActivityId == activityId;

        public bool MatchesActivity(int courseId, int activityId) =>
        this.CourseId == courseId
        && this.ActivityId == activityId;


        public virtual bool IsAvailable { get; set; }





        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
                {
                int hash = 17;
                //hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + CourseId.GetHashCode();
                hash = hash * 23 + UnitId.GetHashCode();
                hash = hash * 23 + ActivityId.GetHashCode();
                hash = hash * 23 + OrderPriority.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj) {
            return obj is IdentityModel && this == (IdentityModel)obj;
        }

        public bool Equals(IdentityModel other) {
            return this == other;
        }
    }
}
