using System;
using System.Drawing;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises.Results;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.OptionQuizResults {
    public partial class OptionQuizResultCell : UITableViewCell {
        public static readonly NSString Key = new NSString(nameof(OptionQuizResultCell));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);
        public OptionQuizResultCell(IntPtr handle) : base(handle) {
        }

        public OptionQuizResultCell() {
        }

        private PossibleAnswersSource possibleAnswersSource;

        public int QuestionNumber { get; internal set; }


        public override void AwakeFromNib() {
            base.AwakeFromNib();
            PossibleAnswersTable.RegisterNibForCellReuse(OptionQuizResultAnswerOptionCell.Nib, "OptionQuizResultAnswerOptionCellReuse");
            possibleAnswersSource = new PossibleAnswersSource();
            PossibleAnswersTable.Source = possibleAnswersSource;
            //HeightContraint.Active = false;
            SetExpandedImage();
            SelectionStyle = UITableViewCellSelectionStyle.None;
            Expanded = false;
        }

        public bool Expanded {
            get => !HeightContraint.Active;
            set {
                HeightContraint.Active = !value;
                PossibleAnswersTable.ReloadData();

                SetExpandedImage();
                PossibleAnswersTable.ReloadData();
            }
        }

        private void SetExpandedImage() {
            ExpandImageView.Subviews.ReleaseChildren();
            ExpandImageView.AddLottieToView(Expanded ? Core.Resx.Lottie.expand_less : Core.Resx.Lottie.expand_more);
        }

        internal void Setup(OptionQuizResultViewModel viewModel, int questionNumber) {
            QuestionNumber = questionNumber;

            AnswerCheckImageView.Subviews.ReleaseChildren();
            var image = AnswerCheckImageView.AddLottieToView(viewModel.OptionQuiz.QuestionAnsweredCorrectly(questionNumber) ? Core.Resx.Lottie.maru : Core.Resx.Lottie.batsu);
            image.ColorAll(viewModel.OptionQuiz.QuestionAnsweredCorrectly(questionNumber) ? Color.Green : Color.Red);

            AnswerLabel.AttributedText = Functions.GetAttributedStringFromBracketedPlainText(viewModel.OptionQuiz.GetEnteredAnswerText(questionNumber), AnswerLabel.Font.PointSize);
            ExplanationLabel.Text = viewModel.OptionQuiz.GetExplainationFor(questionNumber);
            QuestionLabel.Text = viewModel.OptionQuiz.GetQuestionText(questionNumber);
            ExplanationTitleLabel.Text = string.IsNullOrWhiteSpace(ExplanationLabel.Text) ? string.Empty : viewModel.ExplanationTitle;
            Divider3.Hidden = string.IsNullOrWhiteSpace(ExplanationLabel.Text);
            possibleAnswersSource.Setup(viewModel, questionNumber);
            PossibleAnswersTable.ReloadData();
        }

        public override void LayoutSubviews() {
            base.LayoutSubviews();
            PossibleAnswersTable.LayoutSubviews();
        }

        private class PossibleAnswersSource : UITableViewSource {
            private OptionQuizResultViewModel viewModel;
            private int questionNumber = -1;

            public void Setup(OptionQuizResultViewModel viewModel, int questionNumber) {
                this.viewModel = viewModel;
                this.questionNumber = questionNumber;
            }

            public PossibleAnswersSource() {
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("OptionQuizResultAnswerOptionCellReuse") as OptionQuizResultAnswerOptionCell;
                cell.Setup(viewModel, questionNumber, indexPath.Row);
                cell.LayoutSubviews();
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) {
                var rows = questionNumber < 0 ? 0 : viewModel.OptionQuiz.GetAnswerOptionCount(questionNumber);
                return rows;
            }
        }
    }
}