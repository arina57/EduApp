using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SharedActivities.Core.Models {

    public class Activity {
        [XmlElement("Id")]
        public int Id { get; set; }


        [XmlElement("Name")]
        public TranslatedText Name { get; set; }


        [XmlElement("Description")]
        public TranslatedText Description { get; set; }
    }
    [Serializable]
    [XmlRoot("Activities")]
    public class Activities {
        [XmlElement("Activity")]
        public List<Activity> Items { get; set; }
        public Activity this[int index] => Items[index];
        public int Count => Items.Count;
    }
}
