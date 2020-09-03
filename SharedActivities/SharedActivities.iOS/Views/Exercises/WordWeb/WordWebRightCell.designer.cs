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

namespace SharedActivities.iOS.Views.Exercises.WordWeb {
    [Register("WordWebRightCell")]
    partial class WordWebRightCell {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView DotView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel PhraseLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (DotView != null) {
                DotView.Dispose();
                DotView = null;
            }

            if (PhraseLabel != null) {
                PhraseLabel.Dispose();
                PhraseLabel = null;
            }
        }
    }
}