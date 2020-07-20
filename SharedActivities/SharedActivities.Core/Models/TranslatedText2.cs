using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SharedActivities.Core.Models {
    public class TranslatedText2 {
        public TranslatedText2() {
        }


        [XmlElement("Translation")]
        public virtual Dictionary<string, string> English { get; set; } = new Dictionary<string, string>();

    }
}
