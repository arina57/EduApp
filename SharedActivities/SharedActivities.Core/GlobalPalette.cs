using System.Drawing;
using Xamarin.Essentials;

namespace SharedActivities.Core {
    public static class GlobalColorPalette {
        public static Color VeryDark { get; set; } = ColorConverters.FromHex("#262626");
        public static Color Dark { get; set; } = ColorConverters.FromHex("#4c4c4c");
        public static Color Medium { get; set; } = ColorConverters.FromHex("#6d6d6d");
        public static Color Light { get; set; } = ColorConverters.FromHex("#b7b7b7");
        public static Color VeryLight { get; set; } = ColorConverters.FromHex("#D099D6");
        public static Color White { get; set; } = Color.White;
        public static Color Black { get; set; } = Color.Black;

        public static void SetPalette(Models.ColorPalette colorPalette) {
            if (colorPalette != null) {
                VeryDark = colorPalette.VeryDarkColor;
                Dark = colorPalette.DarkColor;
                Medium = colorPalette.MediumColor;
                Light = colorPalette.LightColor;
                VeryLight = colorPalette.VeryLightColor;
            } else {
                VeryDark = ColorConverters.FromHex("#262626");
                Dark = ColorConverters.FromHex("#4c4c4c");
                Medium = ColorConverters.FromHex("#6d6d6d");
                Light = ColorConverters.FromHex("#b7b7b7");
                VeryLight = ColorConverters.FromHex("#d6d6d6");
            }
        }
    }
}
