
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CrossLibrary.Droid.Views;
using EduApp.Core.ViewModels;

namespace EduApp.Droid.Views {
    public class MainView : PlatformCrossView<MainViewModel> {
        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.main_view, container, false);
            return view;
        }

        public override void OnResume() {
            base.OnResume();
            var activity = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity as ICrossActivity;
            activity.BackAction = CrossLibrary.Droid.Enums.ActivityBackAction.CloseActivity;
        }

        public override void OnPause() {
            base.OnPause();
            var activity = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity as ICrossActivity;
            activity.BackAction = CrossLibrary.Droid.Enums.ActivityBackAction.Normal;
        }
    }
}
