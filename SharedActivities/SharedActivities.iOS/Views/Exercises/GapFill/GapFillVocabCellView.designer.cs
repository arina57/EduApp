// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System.CodeDom.Compiler;
using Foundation;

namespace SharedActivities.iOS.Views.Exercises.GapFill {
    [Register("GapFillVocabCellView")]
    partial class GapFillVocabCellView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel VocabLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (VocabLabel != null) {
                VocabLabel.Dispose();
                VocabLabel = null;
            }
        }
    }
}
