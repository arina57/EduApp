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

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.OptionQuizResults {
    [Register("OptionQuizResultAnswerOptionCellView")]
    partial class OptionQuizResultAnswerOptionCellView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel AnswerOptionLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView ImageView { get; set; }

        void ReleaseDesignerOutlets() {
            if (AnswerOptionLabel != null) {
                AnswerOptionLabel.Dispose();
                AnswerOptionLabel = null;
            }

            if (ImageView != null) {
                ImageView.Dispose();
                ImageView = null;
            }
        }
    }
}