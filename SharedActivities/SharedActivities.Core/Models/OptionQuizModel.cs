using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SharedActivities.Core.ViewModels.Exercises.Interfaces;

namespace SharedActivities.Core.Models.OptionQuizModel {
    [Serializable]
    [XmlRoot("OptionQuizes")]
    public class OptionQuizes {
        [XmlElement("OptionQuiz")]
        public List<OptionQuizData> Items { get; set; }

        public OptionQuizData this[int index] => Items[index];
        public int Count => Items.Count;
    }

    [Serializable]
    public class OptionQuizData : IOptionQuizExercise, IActivityDataModel {
        [XmlElement("ActivityData")]
        public IdentityModel ActivityData { get; set; }


        [XmlElement("IsOrdered")]
        public bool IsOrdered { get; set; }

        [XmlElement("Situation", IsNullable = true)]
        public TranslatedText Situation { get; set; }

        [XmlArray("QuestionAnswerSets")]
        [XmlArrayItem("QuestionAnswerSet", typeof(QuestionAnswerSet))]
        public List<QuestionAnswerSet> QuestionAnswerSets { get; set; }

        public int QuestionAnswerSetCount => QuestionAnswerSets.Count;
        public IQuestionAnswerSet GetQuestionAnswerSet(int index) {
            return QuestionAnswerSets[index];
        }
    }

    [Serializable]
    public class QuestionAnswerSet : IQuestionAnswerSet {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Question", IsNullable = true)]
        public string Question { get; set; }

        [XmlArray("AnswerOptions")]
        [XmlArrayItem("AnswerOption", typeof(AnswerOption))]
        public List<AnswerOption> AnswerOptions { get; set; }

        [XmlElement("Explaination", IsNullable = true)]
        public TranslatedText Explaination { get; set; }

    }
}
