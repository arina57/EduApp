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
    [Register("OptionQuizView")]
    partial class OptionQuizView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView ImageFrame { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        SharedActivities.iOS.CustomViews.ContentSizedCollectionView OptionCollection { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel QuestionLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (ImageFrame != null) {
                ImageFrame.Dispose();
                ImageFrame = null;
            }

            if (OptionCollection != null) {
                OptionCollection.Dispose();
                OptionCollection = null;
            }

            if (QuestionLabel != null) {
                QuestionLabel.Dispose();
                QuestionLabel = null;
            }
        }
    }
}