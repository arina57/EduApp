using System;
using System.Drawing;
using EduApp.Core.ViewModels;

namespace EduApp.Core {
    public class CrossApp : CrossLibrary.Core.CrossApp {
        public CrossApp() {
        }

        public override void AppLoaded() {
            base.AppLoaded();
            SharedActivities.Core.GlobalColorPalette.VeryDark = Xamarin.Essentials.ColorConverters.FromHex("#253237");
            SharedActivities.Core.GlobalColorPalette.Dark = Xamarin.Essentials.ColorConverters.FromHex("#5c6b73");
            SharedActivities.Core.GlobalColorPalette.Medium = Xamarin.Essentials.ColorConverters.FromHex("#9db4c0");
            SharedActivities.Core.GlobalColorPalette.Light = Xamarin.Essentials.ColorConverters.FromHex("#c2dfe3");
            SharedActivities.Core.GlobalColorPalette.VeryLight = Xamarin.Essentials.ColorConverters.FromHex("#e0fbfc");


            var main = new MainViewModel();
            main.Show(false);
        }
    }
}
