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
    [Register("PhraseMatchMainCell")]
    partial class PhraseMatchMainCell {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView CorrectImageContainer { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint CorrectImageWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel MainPhraseLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        ContentSizedTableView SelectedMatches { get; set; }

        void ReleaseDesignerOutlets() {
            if (CorrectImageContainer != null) {
                CorrectImageContainer.Dispose();
                CorrectImageContainer = null;
            }

            if (CorrectImageWidthConstraint != null) {
                CorrectImageWidthConstraint.Dispose();
                CorrectImageWidthConstraint = null;
            }

            if (MainPhraseLabel != null) {
                MainPhraseLabel.Dispose();
                MainPhraseLabel = null;
            }

            if (SelectedMatches != null) {
                SelectedMatches.Dispose();
                SelectedMatches = null;
            }
        }
    }
}