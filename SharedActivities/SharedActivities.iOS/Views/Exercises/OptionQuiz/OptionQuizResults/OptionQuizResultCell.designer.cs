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
    [Register("OptionQuizResultCell")]
    partial class OptionQuizResultCell {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView AnswerCheckImageView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel AnswerLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView Divider3 { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView ExpandImageView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel ExplanationLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel ExplanationTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint HeightContraint { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CustomViews.ContentSizedTableView PossibleAnswersTable { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel QuestionLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (AnswerCheckImageView != null) {
                AnswerCheckImageView.Dispose();
                AnswerCheckImageView = null;
            }

            if (AnswerLabel != null) {
                AnswerLabel.Dispose();
                AnswerLabel = null;
            }

            if (Divider3 != null) {
                Divider3.Dispose();
                Divider3 = null;
            }

            if (ExpandImageView != null) {
                ExpandImageView.Dispose();
                ExpandImageView = null;
            }

            if (ExplanationLabel != null) {
                ExplanationLabel.Dispose();
                ExplanationLabel = null;
            }

            if (ExplanationTitleLabel != null) {
                ExplanationTitleLabel.Dispose();
                ExplanationTitleLabel = null;
            }

            if (HeightContraint != null) {
                HeightContraint.Dispose();
                HeightContraint = null;
            }

            if (PossibleAnswersTable != null) {
                PossibleAnswersTable.Dispose();
                PossibleAnswersTable = null;
            }

            if (QuestionLabel != null) {
                QuestionLabel.Dispose();
                QuestionLabel = null;
            }
        }
    }
}