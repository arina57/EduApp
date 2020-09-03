using System;
using System.Drawing;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises.Results;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.OptionQuizResults {
    public partial class OptionQuizResultAnswerOptionCell : UITableViewCell {
        public static readonly NSString Key = new NSString(nameof(OptionQuizResultAnswerOptionCell));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);
        public OptionQuizResultAnswerOptionCell(IntPtr handle) : base(handle) {
        }

        public OptionQuizResultAnswerOptionCell() : base() {
        }

        internal void Setup(OptionQuizResultViewModel viewModel, int questionNumber, int answerOption) {

            ImageView.Subviews.ReleaseChildren();
            var image = ImageView.AddLottieToView(viewModel.OptionQuiz.GetAnswerOptionCorrect(questionNumber, answerOption) ? Core.Resx.Lottie.maru : Core.Resx.Lottie.batsu);
            image.ColorAll(viewModel.OptionQuiz.GetAnswerOptionCorrect(questionNumber, answerOption) ? Color.Green : Color.Red);
            AnswerOptionLabel.TextColor = viewModel.OptionQuiz.GetAnswerOptionCorrect(questionNumber, answerOption) ? Color.Green.ToPlatformColor() : Color.Red.ToPlatformColor();
            AnswerOptionLabel.AttributedText = Functions.GetAttributedStringFromBracketedPlainText(viewModel.OptionQuiz.GetAnswerOptionText(questionNumber, answerOption), AnswerOptionLabel.Font.PointSize);
        }
    }
}