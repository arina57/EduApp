using System;
using System.Globalization;
using SQLite;

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


        public static string ToOrdinalString(int num, GlobalEnums.Language language) {
            if (num <= 0 || language == GlobalEnums.Language.Japanese) {
                return num.ToString();
            }
            switch (num % 100) {
                case 11:
                case 12:
                case 13:
                    return num + "ᵗʰ";
            }

            switch (num % 10) {
                case 1:
                    return num + "ˢᵗ";
                case 2:
                    return num + "ⁿᵈ";
                case 3:
                    return num + "ʳᵈ";
                default:
                    return num + "ᵗʰ";
            }
        }

        /// <summary>
        /// Checks if a table exists in a SQLite database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool TableExists<T>(this SQLiteConnection connection) {
            const string cmdText = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
            var cmd = connection.CreateCommand(cmdText, typeof(T).Name);
            return cmd.ExecuteScalar<string>() != null;
        }

    }
}
