// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System.CodeDom.Compiler;
using Foundation;

namespace SharedActivities.iOS.Views.Exercises.GapFill {
    [Register("GapFillCell")]
    partial class GapFillCell {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView FaceImageView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ImageWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel NameLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        SharedActivities.iOS.CustomViews.ReplaceableTextUITextView ReplaceableText { get; set; }

        void ReleaseDesignerOutlets() {
            if (FaceImageView != null) {
                FaceImageView.Dispose();
                FaceImageView = null;
            }

            if (ImageWidthConstraint != null) {
                ImageWidthConstraint.Dispose();
                ImageWidthConstraint = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose();
                NameLabel = null;
            }

            if (ReplaceableText != null) {
                ReplaceableText.Dispose();
                ReplaceableText = null;
            }
        }
    }
}
