using System;
using EduApp.Core.ViewModels;

namespace EduApp.Core {
    public class CrossApp : CrossLibrary.Core.CrossApp {
        public CrossApp() {
        }

        public override void AppLoaded() {
            base.AppLoaded();
            var main = new MainViewModel();
            main.Show();
        }
    }
}
