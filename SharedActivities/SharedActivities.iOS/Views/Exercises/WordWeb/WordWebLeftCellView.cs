using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.iOS.CustomViews;
using UIKit;
using static CrossLibrary.MathAndGeometry;

namespace SharedActivities.iOS.Views.Exercises.WordWeb {
    public partial class WordWebLeftCellView : UITableViewCell {

        public static readonly NSString Key = new NSString(nameof(WordWebLeftCellView));
        public static readonly UINib Nib = UINib.FromName(Key, NSBundle.MainBundle);
        private int numberOfMatches = 0;
        private Color dotColor;
        private Action<UILongPressGestureRecognizer, WordWebLeftCellView, WordWebDotCellView> dragAction;
        private LineDrawingView lineDrawingView;

        public int Position { get; private set; } = -1;
        public int MainPhraseId { get; private set; } = -1;

        private WordWebViewModel viewModel;
        HashSet<WordWebDotCellView> mainPhraseDotCell;
        public WordWebLeftCellView(IntPtr handle) : base(handle) {

        }


        public void Setup(WordWebViewModel viewModel,
                          int row,
                          Action<UILongPressGestureRecognizer, WordWebLeftCellView, WordWebDotCellView> dragAction,
                          HashSet<WordWebDotCellView> mainPhraseDotCell,
                          LineDrawingView lineDrawingView, bool userBorder = false) {
            this.viewModel = viewModel;
            this.mainPhraseDotCell = mainPhraseDotCell;
            this.dragAction = dragAction;
            this.lineDrawingView = lineDrawingView;
            if(userBorder) {
                this.Layer.BorderWidth = 1f;
                this.Layer.BorderColor = UIColor.DarkGray.CGColor;
            } else {
                this.Layer.BorderWidth = 0f;
                this.Layer.BorderColor = UIColor.Clear.CGColor;
            }
            
            Position = row;

            PhraseLabel.Text = viewModel.GetMainPhrase(row);
            numberOfMatches = viewModel.PossibleMatches(row);
            dotColor = viewModel.MainPhraseColor(row);

            DotsTable.RegisterNibForCellReuse(WordWebDotCellView.Nib, "DotCell");
            DotsTable.Source = new DotsTableSource(this);
            DotsTable.ReloadData();

            CorrectView.Subviews?.ReleaseChildren();
            if (viewModel.Finished) {
                var lottie = CorrectView.AddLottieToView(Core.Resx.Lottie.marubatsu);
                lottie.AnimationProgress = viewModel.QuestionAnsweredCorrectly(row) ? 0 : 1;
                lottie.Alpha = 0.5f;
            }
        }

        public override void LayoutSubviews() {
            base.LayoutSubviews();
            if (numberOfMatches > 0) {
                DotsTable.RowHeight = DotsTable.Bounds.Height / numberOfMatches;
            }

        }

        private class DotsTableSource : UITableViewSource {
            private WordWebLeftCellView leftCell;
            private WordWebViewModel ViewModel => leftCell.viewModel;
            public DotsTableSource(WordWebLeftCellView phraseMatchingPoolLineMatchLeftCell) {
                this.leftCell = phraseMatchingPoolLineMatchLeftCell;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("DotCell", indexPath) as WordWebDotCellView;
                if (cell.GestureRecognizers == null || cell.GestureRecognizers.Length == 0) {
                    var gesture = new UILongPressGestureRecognizer();
                    gesture.MinimumPressDuration = 0;
                    gesture.AddTarget(() => leftCell.dragAction(gesture, leftCell, cell));
                    cell.AddGestureRecognizer(gesture);
                }
                leftCell.mainPhraseDotCell.Add(cell);
                cell.Color = leftCell.dotColor;
                cell.Position = indexPath.Row;
                cell.MainPhrasePosition = leftCell.Position;
                var cellCenter = cell.DotFrame.ToFRect().Center;
                FPoint cellPoint = cell.ConvertPointToView(cellCenter.ToCGPoint(), leftCell.lineDrawingView).ToFPoint();
                ViewModel.SetLinePositionForMainPhrase(cell.MainPhrasePosition, cell.Position, cellPoint);


                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => leftCell.numberOfMatches;
        }
    }
}