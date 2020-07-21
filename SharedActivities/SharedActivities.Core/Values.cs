using System;
using System.Globalization;
using SharedActivities.Core.Models.Database;

namespace SharedActivities.Core {
    public static class GlobalValues {
        public static CultureInfo Japanese => new CultureInfo("ja");
        public static CultureInfo English => new CultureInfo("en");

        private static Settings settings;
        public static Settings Settings {
            get {
                if (settings == null) {
                    settings = Settings.NewInstance();
                }
                return settings;
            }
        }
    }
}
