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
    [Register("PhraseMatch")]
    partial class PhraseMatch {

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CrossLibrary.iOS.Views.CrossContainerView HeadingView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        DashedBorderView MainPhraseContainer { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITableView MainPhraseOptions { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView MatchContainer { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        ContentSizedCollectionView MatchPhraseOptions { get; set; }

        void ReleaseDesignerOutlets() {

            if (HeadingView != null) {
                HeadingView.Dispose();
                HeadingView = null;
            }

            if (MainPhraseContainer != null) {
                MainPhraseContainer.Dispose();
                MainPhraseContainer = null;
            }

            if (MainPhraseOptions != null) {
                MainPhraseOptions.Dispose();
                MainPhraseOptions = null;
            }

            if (MatchContainer != null) {
                MatchContainer.Dispose();
                MatchContainer = null;
            }

            if (MatchPhraseOptions != null) {
                MatchPhraseOptions.Dispose();
                MatchPhraseOptions = null;
            }
        }
    }
}