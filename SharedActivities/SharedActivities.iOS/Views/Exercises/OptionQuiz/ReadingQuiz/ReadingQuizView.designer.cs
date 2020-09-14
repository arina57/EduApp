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

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.ReadingQuiz {
    [Register("ReadingQuizView")]
    partial class ReadingQuizView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CrossLibrary.iOS.Views.CrossContainerView ActivityTitleContainer { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CustomViews.DiscreteProgressView ProgressView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CrossLibrary.iOS.Views.CrossContainerView QuizView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITableView ReadingTable { get; set; }

        void ReleaseDesignerOutlets() {
            if (ActivityTitleContainer != null) {
                ActivityTitleContainer.Dispose();
                ActivityTitleContainer = null;
            }

            if (ProgressView != null) {
                ProgressView.Dispose();
                ProgressView = null;
            }

            if (QuizView != null) {
                QuizView.Dispose();
                QuizView = null;
            }

            if (ReadingTable != null) {
                ReadingTable.Dispose();
                ReadingTable = null;
            }
        }
    }
}