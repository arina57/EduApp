using System;
using System.Globalization;
using CrossLibrary.Dependency;
using SharedActivities.Core.CrossPlatformInterfaces;
using SharedActivities.iOS.CrossPlatformImplimentations;

[assembly: CrossDependency(typeof(SharedCrossFunctions))]
namespace SharedActivities.iOS.CrossPlatformImplimentations {
    public class SharedCrossFunctions : ISharedCrossFuctions {
        public void SetLanguage(CultureInfo language) {
            CultureInfo.DefaultThreadCurrentCulture = language;
            CultureInfo.DefaultThreadCurrentUICulture = language;
            //Add iOS locale settings
        }



    }
}
