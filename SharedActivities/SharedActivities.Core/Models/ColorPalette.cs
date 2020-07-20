using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using Xamarin.Essentials;

namespace SharedActivities.Core.Models {
    [XmlRoot("ColorPalettes")]
    public class ColorPalettes {
        [XmlElement("ColorPalette")]
        public List<ColorPalette> Items { get; set; }
    }

    public class ColorPalette {
        [XmlElement("Id")]
        public virtual int Id { get; set; }

        [XmlElement("VeryDark")]
        public string VeryDark { get; set; }
        [XmlElement("Dark")]
        public string Dark { get; set; }
        [XmlElement("Medium")]
        public string Medium { get; set; }
        [XmlElement("Light")]
        public string Light { get; set; }
        [XmlElement("VeryLight")]
        public string VeryLight { get; set; }


        public Color VeryDarkColor => ColorConverters.FromHex(VeryDark);
        public Color DarkColor => ColorConverters.FromHex(Dark);
        public Color MediumColor => ColorConverters.FromHex(Medium);
        public Color LightColor => ColorConverters.FromHex(Light);
        public Color VeryLightColor => ColorConverters.FromHex(VeryLight);

    }
}
