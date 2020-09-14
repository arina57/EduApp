using System;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    public partial class PhraseMatchMatchCellView : UICollectionViewCell, IDraggableItem {
        public static readonly NSString Key = new NSString(nameof(PhraseMatchMatchCellView));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);


  

        protected PhraseMatchMatchCellView(IntPtr handle) : base(handle) {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib() {
            base.AwakeFromNib();
        }

        public void Setup(PhraseMatchViewModel viewModel, int postion) {
            this.viewModel = viewModel;
            this.matchId = viewModel.GetUnusedMatchId(postion);
            Label.Text = viewModel.GetUnusedMatchPhrase(postion);
        }

        public void DroppedInMainPhrase(int mainPhraseId) {
            viewModel.SetAnswer(mainPhraseId, matchId);
        }

        public void DroppedInMatchPhases() {
            viewModel.RemoveAnswer(matchId);
        }

        public string Text => Label.Text;

        private PhraseMatchViewModel viewModel;
        private int matchId;

    }
}
