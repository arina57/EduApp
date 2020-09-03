// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using SharedActivities.iOS.CustomViews;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.BasicOptionQuiz {
    [Register("BasicOptionQuiz")]
    partial class BasicOptionQuiz {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        DiscreteProgressView ProgressView { get; set; }

        void ReleaseDesignerOutlets() {
            if (ProgressView != null) {
                ProgressView.Dispose();
                ProgressView = null;
            }
        }
    }
}