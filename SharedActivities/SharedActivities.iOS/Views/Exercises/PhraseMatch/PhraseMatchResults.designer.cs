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
    [Register("PhraseMatchResults")]
    partial class PhraseMatchResults {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        ContentSizedTableView ResultsTable { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CrossLibrary.iOS.Views.CrossContainerView ScoreViewContainer { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (ResultsTable != null) {
                ResultsTable.Dispose();
                ResultsTable = null;
            }

            if (ScoreViewContainer != null) {
                ScoreViewContainer.Dispose();
                ScoreViewContainer = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose();
                ScrollView = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose();
                TitleLabel = null;
            }
        }
    }
}