// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SharedActivities.iOS.Views
{
    [Register ("PracticeHeadingView")]
    partial class PracticeHeadingView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ChapterNumberText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CompletedView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PerfectView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PointsImageFrame { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PointsText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SubtitleText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimesCompletedText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimesPerfectText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TitleBackgroundView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ChapterNumberText != null) {
                ChapterNumberText.Dispose ();
                ChapterNumberText = null;
            }

            if (CompletedView != null) {
                CompletedView.Dispose ();
                CompletedView = null;
            }

            if (PerfectView != null) {
                PerfectView.Dispose ();
                PerfectView = null;
            }

            if (PointsImageFrame != null) {
                PointsImageFrame.Dispose ();
                PointsImageFrame = null;
            }

            if (PointsText != null) {
                PointsText.Dispose ();
                PointsText = null;
            }

            if (SubtitleText != null) {
                SubtitleText.Dispose ();
                SubtitleText = null;
            }

            if (TimesCompletedText != null) {
                TimesCompletedText.Dispose ();
                TimesCompletedText = null;
            }

            if (TimesPerfectText != null) {
                TimesPerfectText.Dispose ();
                TimesPerfectText = null;
            }

            if (TitleBackgroundView != null) {
                TitleBackgroundView.Dispose ();
                TitleBackgroundView = null;
            }

            if (TitleText != null) {
                TitleText.Dispose ();
                TitleText = null;
            }
        }
    }
}