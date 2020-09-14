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

namespace SharedActivities.iOS.Views.Exercises.WordWeb {
    [Register("WordWebResultsView")]
    partial class WordWebResultsView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITableView LeftTable { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        LineDrawingView LineDrawingView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITableView RightTable { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CrossLibrary.iOS.Views.CrossContainerView ScoreViewContainer { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TableHeight { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (LeftTable != null) {
                LeftTable.Dispose();
                LeftTable = null;
            }

            if (LineDrawingView != null) {
                LineDrawingView.Dispose();
                LineDrawingView = null;
            }

            if (RightTable != null) {
                RightTable.Dispose();
                RightTable = null;
            }

            if (ScoreViewContainer != null) {
                ScoreViewContainer.Dispose();
                ScoreViewContainer = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose();
                ScrollView = null;
            }

            if (TableHeight != null) {
                TableHeight.Dispose();
                TableHeight = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose();
                TitleLabel = null;
            }
        }
    }
}
