using System;

using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.ReadingQuiz {
    public partial class ReadingQuizCellView : UITableViewCell {
        public static readonly NSString Key = new NSString(nameof(ReadingQuizCellView));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);


        protected ReadingQuizCellView(IntPtr handle) : base(handle) {
            // Note: this .ctor should not contain any initialization logic.
        }


        public void Setup(ReadingOptionQuizViewModel logic, int row) {
            LineLabel.Text = logic.LineText(row);
            NameLabel.Text = logic.RoleName(row);
            ImageView.Subviews.ReleaseChildren();
            ImageView.AddLottieToView(logic.GetRoleImageJson(row));
        }

    }
}
