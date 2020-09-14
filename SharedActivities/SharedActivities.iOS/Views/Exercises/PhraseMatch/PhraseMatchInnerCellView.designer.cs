// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System.CodeDom.Compiler;
using Foundation;

namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    [Register("PhraseMatchInnerCellView")]
    partial class PhraseMatchInnerCellView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel Label { get; set; }

        void ReleaseDesignerOutlets() {
            if (Label != null) {
                Label.Dispose();
                Label = null;
            }
        }
    }
}