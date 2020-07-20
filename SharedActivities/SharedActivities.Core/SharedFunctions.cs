using System;
using System.Globalization;

namespace SharedActivities.Core {
    public static class SharedFunctions {

        public static GlobalEnums.Language GetUILanguage() {
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName) {
                case "ja":
                    return GlobalEnums.Language.Japanese;
                default:
                    return GlobalEnums.Language.English;
            }
        }

    }
}
