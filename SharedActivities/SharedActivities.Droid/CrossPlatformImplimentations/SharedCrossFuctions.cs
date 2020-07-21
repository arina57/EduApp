using System;
using System.Globalization;
using CrossLibrary.Dependency;
using Java.Util;
using SharedActivities.Droid.CrossPlatformImplimentations;

[assembly: CrossDependency(typeof(SharedCrossFuctions))]
namespace SharedActivities.Droid.CrossPlatformImplimentations {
    public class SharedCrossFuctions {
        public void SetLanguage(CultureInfo language) {
            CultureInfo.DefaultThreadCurrentCulture = language;
            CultureInfo.DefaultThreadCurrentUICulture = language;
            Locale.Default = Functions.GetLocale(language);
         
        }
    }
}
