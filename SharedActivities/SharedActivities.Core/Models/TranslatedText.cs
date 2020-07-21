using System;
using System.Globalization;
using System.Xml.Serialization;

namespace SharedActivities.Core.Models {
    public class TranslatedText {

        public TranslatedText() {
        }

        public TranslatedText(string english = "", string japanese = "") {
            English = english;
            Japanese = japanese;
        }


        [XmlElement("English")]
        public virtual string English { get; set; } = string.Empty;

        [XmlElement("Japanese")]
        public virtual string Japanese { get; set; } = string.Empty;

        public virtual string GetString(CultureInfo language) {
            if (language.SameLanguage(GlobalValues.Japanese) && !string.IsNullOrWhiteSpace(Japanese)) {
                return Japanese;
            } else {
                return English;
            }
        }
        public virtual string GetString() {
            return GetString(CultureInfo.DefaultThreadCurrentUICulture);
        }
    }
}
