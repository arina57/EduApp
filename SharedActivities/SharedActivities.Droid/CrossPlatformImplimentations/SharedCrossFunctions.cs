using System;
using System.Globalization;
using CrossLibrary.Dependency;
using Java.Util;
using SharedActivities.Core.CrossPlatformInterfaces;
using SharedActivities.Droid.CrossPlatformImplimentations;

[assembly: CrossDependency(typeof(SharedCrossFunctions))]
namespace SharedActivities.Droid.CrossPlatformImplimentations {
    public class SharedCrossFunctions : ISharedCrossFuctions {
        public void SetLanguage(CultureInfo language) {
            CultureInfo.DefaultThreadCurrentCulture = language;
            CultureInfo.DefaultThreadCurrentUICulture = language;
            Locale.Default = Functions.GetLocale(language);
         
        }
    }
}
