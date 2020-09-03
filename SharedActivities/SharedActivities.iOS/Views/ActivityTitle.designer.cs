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

namespace SharedActivities.iOS.Views {
    [Register("ActivityTitle")]
    partial class ActivityTitle {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView QuestionIconView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel SituationLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SubtitleGapConstraint { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SubtitleHeightContraint { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel SubTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TitleGapConstraint { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TitleHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (QuestionIconView != null) {
                QuestionIconView.Dispose();
                QuestionIconView = null;
            }

            if (SituationLabel != null) {
                SituationLabel.Dispose();
                SituationLabel = null;
            }

            if (SubtitleGapConstraint != null) {
                SubtitleGapConstraint.Dispose();
                SubtitleGapConstraint = null;
            }

            if (SubtitleHeightContraint != null) {
                SubtitleHeightContraint.Dispose();
                SubtitleHeightContraint = null;
            }

            if (SubTitleLabel != null) {
                SubTitleLabel.Dispose();
                SubTitleLabel = null;
            }

            if (TitleGapConstraint != null) {
                TitleGapConstraint.Dispose();
                TitleGapConstraint = null;
            }

            if (TitleHeightConstraint != null) {
                TitleHeightConstraint.Dispose();
                TitleHeightConstraint = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose();
                TitleLabel = null;
            }
        }
    }
}