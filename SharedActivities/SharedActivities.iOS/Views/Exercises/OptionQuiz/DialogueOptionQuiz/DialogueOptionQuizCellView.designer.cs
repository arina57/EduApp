// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System.CodeDom.Compiler;
using Foundation;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.DialogueOptionQuiz {
    [Register("DialogueOptionQuizCellView")]
    partial class DialogueOptionQuizCellView {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIView CharacterImageView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel CharacterNameLabel { get; set; }

        void ReleaseDesignerOutlets() {
            if (CharacterImageView != null) {
                CharacterImageView.Dispose();
                CharacterImageView = null;
            }

            if (CharacterNameLabel != null) {
                CharacterNameLabel.Dispose();
                CharacterNameLabel = null;
            }
        }
    }
}