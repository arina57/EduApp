using System;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.DialogueOptionQuiz {
    public partial class DialogueOptionQuiz : CrossUIViewController<DialogueOptionQuizViewModel> {
        public DialogueOptionQuiz() {
        }
        private UICollectionViewFlowLayout collectionViewFlowControl;



        public override void ViewDidLoad() {
            base.ViewDidLoad();
            ProgressView.ProgressTracker = ViewModel;

            collectionViewFlowControl = new UICollectionViewFlowLayout();
            collectionViewFlowControl.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            collectionViewFlowControl.MinimumInteritemSpacing = 0;
            collectionViewFlowControl.MinimumLineSpacing = 0;
            CharacterView.CollectionViewLayout = collectionViewFlowControl;
            CharacterView.RegisterNibForCell(UINib.FromName(DialogueOptionQuizCell.Key, null), "CharacterResuseCell");
            CharacterView.Source = new CharacterSource(this);
            ViewModel.ProgressChanged += ViewModel_ProgressChanged;
        }

        private void ViewModel_ProgressChanged(object sender, EventArgs e) {
            CharacterView.ReloadData();
        }

        public override void ViewDidUnload() {
            base.ViewDidUnload();
            ViewModel.ProgressChanged -= ViewModel_ProgressChanged;
        }




        public override void RefreshUILocale() {
        }

        private class CharacterSource : UICollectionViewSource {
            private DialogueOptionQuiz dialogueOptionQuiz;
            DialogueOptionQuizViewModel Logic => dialogueOptionQuiz.ViewModel;

            public CharacterSource(DialogueOptionQuiz dialogueOptionQuiz) {
                this.dialogueOptionQuiz = dialogueOptionQuiz;
            }


            public override nint NumberOfSections(UICollectionView collectionView) => 1;
            public override nint GetItemsCount(UICollectionView collectionView, nint section) => Logic.RoleCount;

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) {
                var cell = collectionView.DequeueReusableCell("CharacterResuseCell", indexPath) as DialogueOptionQuizCell;
                dialogueOptionQuiz.collectionViewFlowControl.ItemSize = new CoreGraphics.CGSize(collectionView.Bounds.Width / Logic.RoleCount, collectionView.Bounds.Height);
                cell.Setup(Logic, indexPath.Row);
                return cell;

            }
        }
    }
}

