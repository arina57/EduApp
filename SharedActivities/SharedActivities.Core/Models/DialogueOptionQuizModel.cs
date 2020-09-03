using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SharedActivities.Core.ViewModels.Exercises.Interfaces;

namespace SharedActivities.Core.Models.DialogueOptionQuizModel {
    [XmlRoot("DialogueOptionQuizes")]
    public class DialogueOptionQuizes {
        [XmlElement("DialogueOptionQuiz")]
        public List<DialogueOptionQuiz> Items { get; set; }
        [XmlIgnore]
        public DialogueOptionQuiz this[int index] => Items[index];
        [XmlIgnore]
        public int Count => Items.Count;

    }

    public class DialogueOptionQuiz : IOptionQuizExercise, IActivityDataModel {
        [XmlElement("ActivityData")]
        public IdentityModel ActivityData { get; set; }


        [XmlElement("Situation", IsNullable = true)]
        public TranslatedText Situation { get; set; }

        public int GetRoleCount() {
            return Lines.GroupBy(line => line.RoleId).Count();
        }

        public List<int> GetRoleIds() {
            return Lines.GroupBy(line => line.RoleId).Select(group => group.First().RoleId).ToList();
        }

        public IQuestionAnswerSet GetQuestionAnswerSet(int index) {
            return Lines[index];
        }

        [XmlIgnore]
        public int QuestionAnswerSetCount => Lines.Count;


        [XmlArray("Lines")]
        [XmlArrayItem("Line", typeof(Line))]
        public List<Line> Lines { get; set; }
        [XmlIgnore]
        public bool IsOrdered => true;
    }

    [Serializable]
    public class Line : IQuestionAnswerSet {
        [XmlElement("Order")]
        public int Order { get; set; }

        [XmlElement("RoleId")]
        public int RoleId { get; set; }

        [XmlArray("AnswerOptions")]
        [XmlArrayItem("AnswerOption", typeof(AnswerOption))]
        public List<AnswerOption> AnswerOptions { get; set; }

        [XmlElement("Explaination", IsNullable = true)]
        public TranslatedText Explaination { get; set; }
        [XmlIgnore]
        public string Question => string.Empty;
    }


}