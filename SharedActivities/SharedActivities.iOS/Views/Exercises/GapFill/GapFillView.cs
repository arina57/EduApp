using System;
using System.Linq;
using CoreGraphics;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.iOS.CustomViews;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS.Views.Exercises.GapFill {
    public partial class GapFillView : CrossUIViewController<GapFillViewModel> {
        private UICollectionViewFlowLayout vocabCollectionFlowControl;
        private readonly UILabel draglabel = new UILabel();
        public GapFillView(IntPtr handle) : base(handle) {
        }

        public GapFillView() {
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            //VocabCollection.RegisterClassForCell(typeof(VocabCollectionCell), "VocabCollectionCell");

            VocabCollection.RegisterNibForCell(GapFillVocabCellView.Nib, "VocabCollectionCell");

            VocabCollection.Source = new VocabCollectionSource(this);
            vocabCollectionFlowControl = new UICollectionViewFlowLayout();
            vocabCollectionFlowControl.MinimumInteritemSpacing = 0;
            vocabCollectionFlowControl.MinimumLineSpacing = 0;
            VocabCollection.CollectionViewLayout = vocabCollectionFlowControl;
            GapFillTable.RegisterNibForCellReuse(GapFillCellView.Nib, "GapFillCellView");
            GapFillTable.Source = new GapFillTableSource(this);
            //VocabCollection.AddShadow();
            this.View.AddSubview(draglabel);
            draglabel.Hidden = true;
            draglabel.Text = "Test";
            draglabel.TextColor = UIColor.White;

            draglabel.BackgroundColor = GlobalColorPalette.Light.WithAlpha(128).ToPlatformColor();

            draglabel.TextAlignment = UITextAlignment.Center;
            vocabCollectionFlowControl.ItemSize = new CoreGraphics.CGSize(VocabCollection.Bounds.Width / 3.05, vocabCollectionFlowControl.ItemSize.Height);

        }

        public override void ViewDidLayoutSubviews() {
            base.ViewDidLayoutSubviews();

        }

        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            VocabCollection.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
            GapFillTable.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
        }

        private void DonePressed() {
            ViewModel.Finish();
        }

        public override void RefreshUILocale() {
            VocabCollection.ReloadData();
            GapFillTable.ReloadData();
        }



        private class VocabCollectionSource : UICollectionViewSource {
            private GapFillView unorderedGapFill;
            private GapFillViewModel ViewModel => unorderedGapFill.ViewModel;
            public VocabCollectionSource(GapFillView unorderedGapFill) {
                this.unorderedGapFill = unorderedGapFill;
            }

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) {
                var cell = collectionView.DequeueReusableCell("VocabCollectionCell", indexPath) as GapFillVocabCellView;
                if (cell.GestureRecognizers == null || !cell.GestureRecognizers.Any()) { // if the cell has never been used
                    cell.AddGestureRecognizer(new UIPanGestureRecognizer(unorderedGapFill.VocabWasDragged));
                }
                cell.Position = indexPath.Row;

                cell.Text = ViewModel.GetUnusedTagString(indexPath.Row);
                return cell;
            }

            public override nint GetItemsCount(UICollectionView collectionView, nint section) => ViewModel.UnusedTagCount;

        }

        private void VocabWasDragged(UIPanGestureRecognizer gesture) {
            var draggedItem = gesture.View as GapFillVocabCellView;
            var dragPoint = gesture.LocationInView(this.View);
            switch (gesture.State) {
                case UIGestureRecognizerState.Possible:
                    break;
                case UIGestureRecognizerState.Began:
                    gesture.View.Hidden = true;
                    draglabel.Text = ViewModel.GetUnusedTagString(draggedItem.Position);
                    draglabel.Hidden = false;
                    //draglabel.Frame = ConvertRectFromView(draggedItem.VocabLabel.Frame, draggedItem.Superview);
                    draglabel.Center = dragPoint;
                    draglabel.SizeToFit();
                    break;
                case UIGestureRecognizerState.Changed:
                    draglabel.Center = dragPoint;
                    gesture.SetTranslation(CGPoint.Empty, this.View);
                    FindIfInTable(dragPoint);
                    break;
                case UIGestureRecognizerState.Ended:
                    FindIfInTable(ViewModel.GetUnusedTagId(draggedItem.Position), dragPoint);
                    gesture.View.Hidden = false;
                    draglabel.Hidden = true;
                    GapFillTable.ReloadData();
                    VocabCollection.ReloadData();
                    break;
                case UIGestureRecognizerState.Cancelled:
                    break;
                case UIGestureRecognizerState.Failed:
                    break;
            }
        }

        private TagFinder.TextLocation dragLocation;
        private int dragTag = -1;
        private void PhraseWasDragged(UIGestureRecognizer gesture) {
            var draggedItem = (ReplaceableTextUITextView)gesture.View;
            var dragPoint = gesture.LocationInView(this.View);
            switch (gesture.State) {
                case UIGestureRecognizerState.Possible:
                    break;
                case UIGestureRecognizerState.Began:
                    var pointInDraggedItem = this.View.ConvertPointToView(dragPoint, draggedItem);
                    dragLocation = draggedItem.GetTextLocation(pointInDraggedItem);
                    if (dragLocation != null && dragLocation.IsAMatch && draggedItem.Superview is GapFillCellView gapFillTableCell
                            && ViewModel.GetAnswerTagIndex(gapFillTableCell.Position, dragLocation.MatchNumber) != -1) {
                        draglabel.Hidden = false;
                        draglabel.Center = dragPoint;
                        draglabel.Text = ViewModel.GetAnswerTagString(gapFillTableCell.Position, dragLocation.MatchNumber);
                        draglabel.SizeToFit();
                        dragTag = ViewModel.GetAnswerTagIndex(gapFillTableCell.Position, dragLocation.MatchNumber);
                        ViewModel.RemoveAnswerAtPosition(gapFillTableCell.Position, dragLocation.MatchNumber);
                        GapFillTable.ReloadRows(new NSIndexPath[] { NSIndexPath.FromRowSection(gapFillTableCell.Position, 0) }, UITableViewRowAnimation.None);
                    }
                    break;
                case UIGestureRecognizerState.Changed:
                    if (dragLocation != null && dragLocation.IsAMatch) {
                        draglabel.Center = dragPoint;
                    } else {
                    }
                    FindIfInTable(dragPoint);
                    break;
                case UIGestureRecognizerState.Ended:
                    if (dragLocation != null && dragLocation.IsAMatch) {
                        var position = ((GapFillCellView)draggedItem.Superview).Position;
                        if (VocabCollection.Frame.Contains(dragPoint)) {
                            ViewModel.RemoveAnswerAtPosition(position, dragLocation.MatchNumber);
                        } else {
                            FindIfInTable(dragTag, dragPoint);
                        }
                    }
                    gesture.View.Hidden = false;
                    draglabel.Hidden = true;
                    dragLocation = null;
                    GapFillTable.ReloadData();
                    VocabCollection.ReloadData();
                    break;
                case UIGestureRecognizerState.Cancelled:
                    break;
                case UIGestureRecognizerState.Failed:
                    break;
            }
        }

        private void FindIfInTable(int tagId, CGPoint dragPoint) {
            if (GapFillTable.Frame.Contains(dragPoint)) {
                for (int i = 0; i < ViewModel.PhraseCount; i++) {
                    if (GapFillTable.CellAt(NSIndexPath.FromRowSection(i, 0)) is GapFillCellView cell
                        && cell.ReplaceableTextView.Frame.Contains(this.View.ConvertPointToView(dragPoint, cell))) {
                        var dropLocation = cell.ReplaceableTextView.GetTextLocation(this.View.ConvertPointToView(dragPoint, cell.ReplaceableTextView));
                        if (dropLocation != null && dropLocation.IsAMatch) {
                            ViewModel.SetAnswer(i, dropLocation.MatchNumber, tagId);
                        }
                        break;
                    }
                }
            }
        }


        private void FindIfInTable(CGPoint dragPoint) {
            for (int i = 0; i < ViewModel.PhraseCount; i++) {
                if (GapFillTable.CellAt(NSIndexPath.FromRowSection(i, 0)) is GapFillCellView cell) {
                    cell.ReplaceableTextView.RemoveLinkBackgroundColors();
                }
            }

            if (GapFillTable.Frame.Contains(dragPoint)) {
                for (int i = 0; i < ViewModel.PhraseCount; i++) {
                    if (GapFillTable.CellAt(NSIndexPath.FromRowSection(i, 0)) is GapFillCellView cell
                        && cell.ReplaceableTextView.Frame.Contains(this.View.ConvertPointToView(dragPoint, cell))) {
                        var dropLocation = cell.ReplaceableTextView.GetTextLocation(this.View.ConvertPointToView(dragPoint, cell.ReplaceableTextView));
                        if (dropLocation != null && dropLocation.IsAMatch) {
                            cell.ReplaceableTextView.SetLinkBackgroundColor(dropLocation.MatchNumber, ViewModel.HoveredLinkColor);

                        } else {
                            cell.ReplaceableTextView.RemoveLinkBackgroundColors();
                        }

                        //break;
                    }
                }
            }
        }




        private class GapFillTableSource : UITableViewSource {
            private GapFillView unorderedGapFill;
            private GapFillViewModel ViewModel => unorderedGapFill.ViewModel;

            public GapFillTableSource(GapFillView unorderedGapFill) {
                this.unorderedGapFill = unorderedGapFill;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("GapFillCellView", indexPath) as GapFillCellView;
                if (cell.Position == -1) { // if the cell has never been used
                    var gesture = new UIPanGestureRecognizer();
                    //gesture.MinimumPressDuration = 0.1f;
                    gesture.AddTarget(() => {
                        unorderedGapFill.PhraseWasDragged(gesture);
                    });
                    gesture.CancelsTouchesInView = false;

                    cell.ReplaceableTextView.AddGestureRecognizer(gesture);
                }
                cell.Setup(ViewModel, indexPath.Row);
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => ViewModel.PhraseCount;
        }


    }
}