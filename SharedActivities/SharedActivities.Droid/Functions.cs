using System;
using System.Globalization;
using Java.Util;
using SharedActivities.Core;

namespace SharedActivities.Droid {
    /// <summary>
    /// Static functions for Android
    /// </summary>
    public static class Functions {


        public static CultureInfo GetLanguage(Locale locale) => new CultureInfo(locale.Language);

		public static Locale GetLocale(CultureInfo language) => new Locale(language.Name);

	}
}
