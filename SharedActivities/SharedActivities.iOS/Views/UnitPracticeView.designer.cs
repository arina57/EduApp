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
    [Register("UnitPracticeView")]
    partial class UnitPracticeView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ButtonHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton DoneButton { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UICollectionView PageSelector { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView PageSelectorLeft { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView PageSelectorRight { get; set; }

        void ReleaseDesignerOutlets() {
            if (ButtonHeightConstraint != null) {
                ButtonHeightConstraint.Dispose();
                ButtonHeightConstraint = null;
            }

            if (DoneButton != null) {
                DoneButton.Dispose();
                DoneButton = null;
            }

            if (PageSelector != null) {
                PageSelector.Dispose();
                PageSelector = null;
            }

            if (PageSelectorLeft != null) {
                PageSelectorLeft.Dispose();
                PageSelectorLeft = null;
            }

            if (PageSelectorRight != null) {
                PageSelectorRight.Dispose();
                PageSelectorRight = null;
            }
        }
    }
}
