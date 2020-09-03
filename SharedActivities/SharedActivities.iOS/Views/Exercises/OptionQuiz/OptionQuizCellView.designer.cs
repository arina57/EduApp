// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz {
    [Register("OptionQuizCellView")]
    partial class OptionQuizCellView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView BorderView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel OptionLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (BorderView != null) {
                BorderView.Dispose();
                BorderView = null;
            }

            if (OptionLabel != null) {
                OptionLabel.Dispose();
                OptionLabel = null;
            }
        }
    }
}