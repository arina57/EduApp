// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System.CodeDom.Compiler;
using Foundation;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.ReadingQuiz {
    [Register("ReadingQuizCell")]
    partial class ReadingQuizCell {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView ImageView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel LineLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel NameLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (ImageView != null) {
                ImageView.Dispose();
                ImageView = null;
            }

            if (LineLabel != null) {
                LineLabel.Dispose();
                LineLabel = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose();
                NameLabel = null;
            }
        }
    }
}