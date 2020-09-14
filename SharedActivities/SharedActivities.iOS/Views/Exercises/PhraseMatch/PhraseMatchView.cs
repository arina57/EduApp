using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.iOS.CustomViews;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    public partial class PhraseMatchView : CrossUIViewController<PhraseMatchViewModel> {
        private StaggeredCollectionViewLayout collectionViewLayout;
        //private readonly UILabel draglabel = new UILabel();
        //private readonly DashedBorderView dragView = new DashedBorderView();
        private readonly PhraseMatchDragView dragView = Functions.LoadViewFromXib<PhraseMatchDragView>();
        HashSet<PhraseMatchMainCellView> mainPhraseCells = new HashSet<PhraseMatchMainCellView>();


        public PhraseMatchView(IntPtr handle) : base(handle) {
        }

        public PhraseMatchView() {
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            //CreateIn<ActivityTitle>(HeadingView, ViewModel);
            this.View.AddSubview(dragView);



            collectionViewLayout = new StaggeredCollectionViewLayout();
            MatchPhraseOptions.CollectionViewLayout = collectionViewLayout;
            collectionViewLayout.HeightForItem = GetHeightForItem;

            MatchPhraseOptions.Source = new MatchPhraseOptionSource(this);
            MatchPhraseOptions.RegisterNibForCell(PhraseMatchMatchCellView.Nib, "MatchPhraseResuseCell");

            //matchPhraseOptionsFlowControl.ItemSize = new CoreGraphics.CGSize(MatchPhraseOptions.Bounds.Width / 2.5, matchPhraseOptionsFlowControl.ItemSize.Height);

            MainPhraseOptions.RegisterNibForCellReuse(PhraseMatchMainCellView.Nib, "MainPhraseResuseCell");

            MainPhraseOptions.Source = new MainPhraseOptionSource(this);


            //MainContainer.AddDashedBoarder(UIColor.Black, 1);GetHeightForItem
            MatchContainer.AddShadow();
        }


        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            MatchPhraseOptions.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
            MainPhraseOptions.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
        }


        private nfloat GetHeightForItem(UICollectionView collectionView, NSIndexPath indexPath, nfloat columnWidth) {
            var text = ViewModel.GetUnusedMatchPhrase(indexPath.Row);
            var height = Functions.GetHeigthWithText(text, UIFont.SystemFontOfSize(17), columnWidth - 20) + 40;

            return height;
        }

        private void DonePressed() {
            ViewModel.Finish();
        }



        public override void ViewDidAppear(bool animated) {
            base.ViewDidAppear(animated);
            RefreshUILocale();

        }

        public override void RefreshUILocale() {
            MainPhraseOptions.ReloadData();
            MatchPhraseOptions.ReloadData();
            MainPhraseOptions.LayoutSubviews();
            MatchPhraseOptions.LayoutSubviews();
        }

        private void WasDragged(UIPanGestureRecognizer gesture) {
            var translation = gesture.TranslationInView(this.View);
            var draggedItem = gesture.View as IDraggableItem;

            var center = new CGPoint(dragView.Center.X + translation.X, dragView.Center.Y + translation.Y);
            switch (gesture.State) {
                case UIGestureRecognizerState.Possible:
                    break;
                case UIGestureRecognizerState.Began:
                    gesture.View.Hidden = true;
                    dragView.Label.Text = draggedItem.Text;
                    dragView.Label.ResizeFont(8, 17);

                    dragView.Hidden = false;

                    dragView.Frame = View.ConvertRectFromView(gesture.View.Frame, gesture.View.Superview);

                    break;
                case UIGestureRecognizerState.Changed:
                    dragView.Center = center;
                    gesture.SetTranslation(CGPoint.Empty, this.View);
                    foreach (var enteredCell in mainPhraseCells) {
                        if (enteredCell.Frame.Contains(MainPhraseOptions.ConvertPointFromView(center, this.View))) {
                            enteredCell.BackgroundColor = GlobalColorPalette.Light.ToPlatformColor();
                        } else {
                            enteredCell.BackgroundColor = UIColor.Clear;
                        }
                    }
                    if (MatchContainer.Frame.Contains(center)) {
                        MatchPhraseOptions.BackgroundColor = GlobalColorPalette.Light.ToPlatformColor();
                    } else {
                        MatchPhraseOptions.BackgroundColor = UIColor.Clear;
                    }

                    break;
                case UIGestureRecognizerState.Ended:
                    gesture.View.Hidden = false;
                    dragView.Hidden = true;
                    foreach (var enteredCell in mainPhraseCells) {
                        if (enteredCell.Frame.Contains(MainPhraseOptions.ConvertPointFromView(center, this.View))) {
                            draggedItem.DroppedInMainPhrase(enteredCell.MainPhraseId);
                        }
                        enteredCell.BackgroundColor = UIColor.Clear;
                    }
                    if (MatchContainer.Frame.Contains(center)) {
                        draggedItem.DroppedInMatchPhases();
                    }
                    MatchPhraseOptions.BackgroundColor = UIColor.Clear;

                    MainPhraseOptions.ReloadData();
                    MatchPhraseOptions.ReloadData();
                    MainPhraseOptions.SetNeedsLayout();
                    MatchPhraseOptions.SetNeedsLayout();
                    break;
                case UIGestureRecognizerState.Cancelled:
                    break;
                case UIGestureRecognizerState.Failed:
                    break;
            }
        }

        private class MatchPhraseOptionSource : UICollectionViewSource {
            private PhraseMatchView phraseMatchingPool;
            private PhraseMatchViewModel ViewModel => phraseMatchingPool.ViewModel;
            public MatchPhraseOptionSource(PhraseMatchView phraseMatchingPool) {
                this.phraseMatchingPool = phraseMatchingPool;
            }


            public override nint NumberOfSections(UICollectionView collectionView) => 1;
            public override nint GetItemsCount(UICollectionView collectionView, nint section) => ViewModel.AvailableMatchCount;

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) {
                var cell = collectionView.DequeueReusableCell("MatchPhraseResuseCell", indexPath) as PhraseMatchMatchCellView;
                if (cell.GestureRecognizers == null || !cell.GestureRecognizers.Any()) {
                    cell.AddGestureRecognizer(new UIPanGestureRecognizer(phraseMatchingPool.WasDragged));

                }
                cell.Setup(ViewModel, indexPath.Row);
                collectionView.LayoutIfNeeded();
                return cell;
            }


        }

        private class MainPhraseOptionSource : UITableViewSource {
            private PhraseMatchView phraseMatchingPool;
            private PhraseMatchViewModel ViewModel => phraseMatchingPool.ViewModel;
            public MainPhraseOptionSource(PhraseMatchView phraseMatchingPool) {
                this.phraseMatchingPool = phraseMatchingPool;
            }



            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("MainPhraseResuseCell") as PhraseMatchMainCellView;
                phraseMatchingPool.mainPhraseCells.Add(cell);
                cell.Setup(ViewModel, indexPath.Row, phraseMatchingPool.WasDragged);
                //tableView.LayoutIfNeeded();
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => ViewModel.MainPhraseCount;
        }
    }
}