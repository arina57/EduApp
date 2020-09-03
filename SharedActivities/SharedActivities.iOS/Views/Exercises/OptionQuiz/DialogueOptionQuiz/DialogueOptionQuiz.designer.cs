// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using SharedActivities.iOS.CustomViews;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.DialogueOptionQuiz {
    [Register("DialogueOptionQuiz")]
    partial class DialogueOptionQuiz {
        [Outlet]
        DiscreteProgressView ProgressView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CrossLibrary.iOS.Views.CrossContainerView ActivityTitleContainer { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UICollectionView CharacterView { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        CrossLibrary.iOS.Views.CrossContainerView QuestionContainer { get; set; }

        void ReleaseDesignerOutlets() {
            if (ActivityTitleContainer != null) {
                ActivityTitleContainer.Dispose();
                ActivityTitleContainer = null;
            }

            if (CharacterView != null) {
                CharacterView.Dispose();
                CharacterView = null;
            }

            if (QuestionContainer != null) {
                QuestionContainer.Dispose();
                QuestionContainer = null;
            }
        }
    }
}