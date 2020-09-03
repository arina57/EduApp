using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SharedActivities.Core.Models.PhraseMatchingPoolModel {

    [Serializable]
    [XmlRoot("PhraseMatchExercises")]
    public class PhraseMatchExercises {

        [XmlElement("PhraseMatchExercise")]
        public List<PhraseMatchExercise> Items { get; set; }

        public PhraseMatchExercise this[int index] => Items[index];
        public int Count => Items.Count;
    }


    [Serializable]
    public class PhraseMatchExercise : IActivityDataModel {

        [XmlElement("ActivityData")]
        public IdentityModel ActivityData { get; set; }

        [XmlElement("IsOrdered")]
        public bool IsOrdered { get; set; }

        [XmlElement("Situation", IsNullable = true)]
        public TranslatedText Situation { get; set; }

        [XmlArray("PhraseSets")]
        [XmlArrayItem("PhraseSet", typeof(PhraseSet))]
        public List<PhraseSet> PhraseSets { get; set; }

        public IEnumerable<string> AllMainPhrases => PhraseSets.Select(phraseSet => phraseSet.MainPhrase);
        public IEnumerable<string> AllMatchingPhrases => PhraseSets.SelectMany(phraseSet => phraseSet.MatchingPhrases);
        public int MaxMatchesPerMainPhrase => PhraseSets.Max(phaseSet => phaseSet.MatchingPhrases.Count);
        public int MinMatchesPerMainPhrase => PhraseSets.Min(phaseSet => phaseSet.MatchingPhrases.Count);

        public bool MainAndMatchOneToOne => MaxMatchesPerMainPhrase == 1 && MinMatchesPerMainPhrase == 1;

    }

    [Serializable]
    public class PhraseSet {

        [XmlElement("MainPhrase")]
        public string MainPhrase { get; set; }

        [XmlArray("MatchingPhrases")]
        [XmlArrayItem("MatchingPhrase", typeof(string))]
        public List<string> MatchingPhrases { get; set; }

        [XmlElement("Explaination", IsNullable = true)]
        public TranslatedText Explaination { get; set; }
    }
}
