using System;
using System.Drawing;
using System.Linq;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    public partial class PhraseMatchMainCell : UITableViewCell {
        public static readonly NSString Key = new NSString("PhraseMatchingPoolMainPhraseCell");
        public static readonly UINib Nib;
        private Action<UIPanGestureRecognizer> dragActionHandler;
        private PhraseMatchViewModel logic;
        int position;
        public int MainPhraseId { get; private set; }

        static PhraseMatchMainCell() {
            Nib = UINib.FromName("PhraseMatchingPoolMainPhraseCell", NSBundle.MainBundle);
        }

        public override void AwakeFromNib() {
            base.AwakeFromNib();
            SelectedMatches.RegisterNibForCellReuse(PhraseMatchInnerCell.Nib, "InnerResuseCell");
            SelectedMatches.Source = new MatchesSource(this);
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        protected PhraseMatchMainCell(IntPtr handle) : base(handle) {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void Setup(PhraseMatchViewModel logic, int position, Action<UIPanGestureRecognizer> dragActionHandler = null) {
            this.dragActionHandler = dragActionHandler;
            this.logic = logic;
            this.position = position;
            MainPhraseLabel.Text = logic.GetMainPhrase(position);
            MainPhraseId = logic.GetMainPhraseId(position);
            SelectedMatches.ReloadData();
            SelectedMatches.SizeToFit();

            CorrectImageContainer.Subviews.ReleaseChildren();
            CorrectImageWidthConstraint.Active = logic.Finished;
            if (logic.Finished) {
                var correct = logic.ScoringViewModel.QuestionAnsweredCorrectly(position);
                var image = CorrectImageContainer.AddLottieToView(correct ? Core.Resx.Lottie.maru : Core.Resx.Lottie.batsu);
                image.ColorAll(correct ? Color.Green : Color.Red);
            }
        }

        private class MatchesSource : UITableViewSource {
            private PhraseMatchMainCell phraseMatchingPoolMainPhraseCell;
            PhraseMatchViewModel Logic => phraseMatchingPoolMainPhraseCell.logic;
            public MatchesSource(PhraseMatchMainCell phraseMatchingPoolMainPhraseCell) {

                this.phraseMatchingPoolMainPhraseCell = phraseMatchingPoolMainPhraseCell;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("InnerResuseCell") as PhraseMatchInnerCell;
                if ((cell.GestureRecognizers == null || !cell.GestureRecognizers.Any()) && phraseMatchingPoolMainPhraseCell.dragActionHandler != null) {
                    cell.AddGestureRecognizer(new UIPanGestureRecognizer(phraseMatchingPoolMainPhraseCell.dragActionHandler));
                }
                cell.Setup(Logic, phraseMatchingPoolMainPhraseCell.position, indexPath.Row);
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => Logic == null ? 0 : Logic.GetAnswerCount(phraseMatchingPoolMainPhraseCell.position);
        }
    }
}
