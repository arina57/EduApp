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

namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    [Register("PhraseMatchDragView")]
    partial class PhraseMatchDragView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        DashedBorderView BorderView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel TextLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (BorderView != null) {
                BorderView.Dispose();
                BorderView = null;
            }

            if (TextLabel != null) {
                TextLabel.Dispose();
                TextLabel = null;
            }
        }
    }
}