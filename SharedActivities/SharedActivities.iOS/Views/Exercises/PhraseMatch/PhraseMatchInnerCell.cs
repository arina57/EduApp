using System;

using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    public partial class PhraseMatchInnerCell : UITableViewCell, IDraggableItem {
        public static readonly NSString Key = new NSString("PhraseMatchingPoolCellInnerCell");
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);
        private PhraseMatchViewModel viewModel;
        private int mainPhraseId;
        private int matchPhraseId;

        static PhraseMatchInnerCell() {
        }

        protected PhraseMatchInnerCell(IntPtr handle) : base(handle) {
            // Note: this .ctor should not contain any initialization logic.
        }


        public override void AwakeFromNib() {
            base.AwakeFromNib();
            //Label.Layer.BorderColor = UIColor.Black.CGColor;
            //Label.Layer.BorderWidth = 1;
        }

        public void Setup(PhraseMatchViewModel viewModel, int mainPhraseRow, int answerPosition) {
            this.viewModel = viewModel;
            this.mainPhraseId = viewModel.GetMainPhraseId(mainPhraseRow);
            this.matchPhraseId = viewModel.GetAnswerId(mainPhraseRow, answerPosition);
            Label.Text = viewModel.GetAnswerPhrase(mainPhraseRow, answerPosition);
        }

        public string Text => Label.Text;

        public void DroppedInMainPhrase(int mainPhraseId) {
            viewModel.SetAnswer(mainPhraseId, matchPhraseId);
        }

        public void DroppedInMatchPhases() {
            viewModel.RemoveAnswer(matchPhraseId);
        }
    }
}
