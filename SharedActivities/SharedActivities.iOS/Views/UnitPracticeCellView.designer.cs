// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System.CodeDom.Compiler;
using Foundation;

namespace SharedActivities.iOS.Views {
    [Register("UnitPracticeCellView")]
    partial class UnitPracticeCellView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView DoneImage { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel PageNumberLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel ScoreLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (DoneImage != null) {
                DoneImage.Dispose();
                DoneImage = null;
            }

            if (PageNumberLabel != null) {
                PageNumberLabel.Dispose();
                PageNumberLabel = null;
            }

            if (ScoreLabel != null) {
                ScoreLabel.Dispose();
                ScoreLabel = null;
            }
        }
    }
}
