using System;
using System.Xml.Serialization;

namespace SharedActivities.Core.Models {
    [Serializable]
    public class AnswerOption {
        [XmlElement("Correct")]
        public bool Correct { get; set; }

        [XmlElement("Text")]
        public string Text { get; set; }

        /// <summary>
        /// This is needed for deserialization
        /// </summary>
        public AnswerOption() {
        }

        public AnswerOption(string text, bool correct) {
            Text = text;
            Correct = correct;
        }
    }
}
