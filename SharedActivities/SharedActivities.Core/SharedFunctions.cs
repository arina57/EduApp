using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using CrossLibrary.Dependency;
using SharedActivities.Core.CrossPlatformInterfaces;
using SQLite;

namespace SharedActivities.Core {
    public static class SharedFunctions {

        public static ISharedCrossFuctions SharedCrossFuctions => CrossViewDependencyService.Get<ISharedCrossFuctions>(CrossViewDependencyService.DependencyFetchTarget.GlobalInstance);



        public static T GetXmlRoot<T>(string xml) {
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xml)) {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static void SetLanguage(CultureInfo language) {
            SharedCrossFuctions.SetLanguage(language);
        }

        public static string ToOrdinalString(int num, CultureInfo language) {
            if (num <= 0 || language.SameLanguage(GlobalValues.Japanese)) {
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



        public static bool SameLanguage(this CultureInfo cultureInfo1, CultureInfo cultureInfo2)
            => cultureInfo1.TwoLetterISOLanguageName == cultureInfo2.TwoLetterISOLanguageName;

    }
}
