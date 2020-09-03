using System;

using Foundation;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.DialogueOptionQuiz {
    public partial class DialogueOptionQuizCell : UICollectionViewCell {
        public static readonly NSString Key = new NSString("DialogueOptionQuizCell");
        public static readonly UINib Nib;

        static DialogueOptionQuizCell() {
            Nib = UINib.FromName("DialogueOptionQuizCell", NSBundle.MainBundle);
        }

        protected DialogueOptionQuizCell(IntPtr handle) : base(handle) {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
