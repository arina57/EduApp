using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SharedActivities.Core.Models {
    [Serializable]
    public class Role {
        [XmlElement("Id")]
        public int Id { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Gender")]
        public int GenderId { get; set; }
        [XmlElement("Appearance")]
        public int Appearance { get; set; }
        public Gender Gender => (Gender)GenderId;



    }

    [Serializable]
    [XmlRoot("Roles")]
    public class Roles {
        [XmlElement("Role")]
        public List<Role> Items { get; set; }
    }

    public enum Gender {
        Male = 1,
        Female = 2,
        NonSpecificOrOther = 3
    }
}
