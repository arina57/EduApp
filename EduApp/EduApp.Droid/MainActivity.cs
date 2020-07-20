using System;
using Android.App;
using Android.OS;
using Android.Runtime;

using AndroidX.AppCompat.App;
using Android.Views;
using Android.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Content;
using Android.Util;
using EduApp.Core.ViewModels;

namespace EduApp.Droid {
    [Activity(Label = "@string/app_name", Theme = "@style/splashScreenTheme", MainLauncher = true)]
    public class MainActivity : CrossLibrary.Droid.Views.CrossActivity {


        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetTheme(Resource.Style.AppTheme_NoActionBar);
            SetContentView(Resource.Layout.activity_main);

              
        }





        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
