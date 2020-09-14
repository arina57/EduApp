// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System.CodeDom.Compiler;
using Foundation;

namespace SharedActivities.iOS.Views.Exercises.WordWeb {
    [Register("WordWebLeftCellView")]
    partial class WordWebLeftCellView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView CorrectView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UITableView DotsTable { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel PhraseLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (CorrectView != null) {
                CorrectView.Dispose();
                CorrectView = null;
            }

            if (DotsTable != null) {
                DotsTable.Dispose();
                DotsTable = null;
            }

            if (PhraseLabel != null) {
                PhraseLabel.Dispose();
                PhraseLabel = null;
            }
        }
    }
}