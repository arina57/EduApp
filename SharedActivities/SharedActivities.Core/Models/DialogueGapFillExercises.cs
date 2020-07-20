using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SharedActivities.Core.Models {
    [Serializable]
    [XmlRoot("DialogueGapFillExercises")]
    public class DialogueGapFillExercises {
        [XmlElement("DialogueGapFillExercise")]
        public List<DialogueGapFillExercise> Items { get; set; }
        public DialogueGapFillExercise this[int index] => Items[index];
        public int Count => Items.Count;
    }

    public class DialogueGapFillExercise : IActivityDataModel, IGapFillModel {
        public IdentityModel ActivityData { get; set; }
        [XmlElement("Title")]
        public TranslatedText Title { get; set; }
        [XmlElement("Situation")]
        public TranslatedText Situation { get; set; }

        [XmlArray("PhraseFuctions")]
        [XmlArrayItem("PhraseFuction", typeof(PhraseFunction))]
        public List<PhraseFunction> PhraseFuctions { get; set; }

        [XmlArray("Lines")]
        [XmlArrayItem("Line", typeof(Line))]
        public List<Line> Lines { get; set; }

        public IEnumerable<string> Phrases => Lines.Select(line => line.GetTextWithoutTagsWithBrackets());




        public IEnumerable<string> GetKeyAllWords() {
            return Lines.SelectMany(line => line.GetKeyWords());
        }

        public IEnumerable<int> GetRoleIds() {
            return Lines.Select(line => line.RoleId);
        }
    }

    public class PhraseFunction {
        [XmlElement("Id")]
        public virtual int Id { get; set; }
        [XmlElement("Function")]
        public TranslatedText Function { get; set; }
    }


    public class Line {

        [XmlElement("RoleId")]
        public int RoleId { get; set; }

        [XmlElement("LineText")]
        public string LineText { get; set; }



        public string GetTextWithoutTagsOrBrackets() {
            var textPattern = new Regex(@"<[^>]*>|\{|\}"); //anything between <> or '{' or '}' characters
            return textPattern.Replace(LineText, "");
        }

        public string GetTextWithoutTagsWithBrackets() {
            var textPattern = new Regex(@"<[^>]*>"); //anything between <>
            return textPattern.Replace(LineText, "");
        }

        public MatchCollection GetTaggedWordsMatches(int functionId) {
            var textPattern = new Regex($"<{functionId}>(.*?)<\\/{functionId}>");
            return textPattern.Matches(LineText);
        }

        public MatchCollection GetKeyWordsMatches() {
            var textPattern = new Regex(@"\{(.*?)\}");
            return textPattern.Matches(LineText);
        }

        public IEnumerable<string> GetKeyWords() {
            return GetKeyWordsMatches().Cast<Match>().Select(match => match.Value);
        }

    }
}
