using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using CrossLibrary.Core;
using CrossLibrary.Dependency;
using CrossLibrary.Droid.CrossPlatformImplimentations;
using EduApp.Core.ViewModels;

namespace EduApp.Droid {
    [Application]
    public class MainApplication : CrossLibrary.Droid.CrossMainApplication {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer) {
        }

        protected override CrossApp CrossApp => new Core.CrossApp();



    }
}