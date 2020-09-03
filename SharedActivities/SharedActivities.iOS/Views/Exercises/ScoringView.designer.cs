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

namespace SharedActivities.iOS.Views.Exercises {
    [Register("ScoringView")]
    partial class ScoringView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel CommentLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView ImageFrame { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel MultiplierLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel PerfectBonusLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel PointsLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CustomViews.CircularProgressBar ProgressBar { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel ScoreLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (CommentLabel != null) {
                CommentLabel.Dispose();
                CommentLabel = null;
            }

            if (ImageFrame != null) {
                ImageFrame.Dispose();
                ImageFrame = null;
            }

            if (MultiplierLabel != null) {
                MultiplierLabel.Dispose();
                MultiplierLabel = null;
            }

            if (PerfectBonusLabel != null) {
                PerfectBonusLabel.Dispose();
                PerfectBonusLabel = null;
            }

            if (PointsLabel != null) {
                PointsLabel.Dispose();
                PointsLabel = null;
            }

            if (ProgressBar != null) {
                ProgressBar.Dispose();
                ProgressBar = null;
            }

            if (ScoreLabel != null) {
                ScoreLabel.Dispose();
                ScoreLabel = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose();
                TitleLabel = null;
            }
        }
    }
}