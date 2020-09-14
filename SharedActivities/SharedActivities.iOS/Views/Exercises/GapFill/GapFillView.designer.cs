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

namespace SharedActivities.iOS.Views.Exercises.GapFill {
    [Register("GapFillView")]
    partial class GapFillView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITableView GapFillTable { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CrossLibrary.iOS.Views.CrossContainerView HeadingView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        SharedActivities.iOS.CustomViews.ContentSizedCollectionView VocabCollection { get; set; }

        void ReleaseDesignerOutlets() {
            if (GapFillTable != null) {
                GapFillTable.Dispose();
                GapFillTable = null;
            }

            if (HeadingView != null) {
                HeadingView.Dispose();
                HeadingView = null;
            }

            if (VocabCollection != null) {
                VocabCollection.Dispose();
                VocabCollection = null;
            }
        }
    }
}
