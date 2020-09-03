using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core.Models;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.WordWeb {
    public partial class WordWeb : CrossUIViewController<WordWebViewModel> {

        HashSet<WordWebRightCell> matchCells = new HashSet<WordWebRightCell>();
        HashSet<WordWebDotCell> mainPhraseDotCell = new HashSet<WordWebDotCell>();

        public WordWeb(IntPtr handle) : base(handle) {

        }

        public WordWeb() {
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            
            LeftTable.RegisterNibForCellReuse(WordWebLeftCell.Nib, "LeftCell");
            RightTable.RegisterNibForCellReuse(WordWebRightCell.Nib, "RightCell");

            LeftTable.Source = new MainPhraseTableSource(this);
            RightTable.Source = new MatchPhraseTableSource(this);

        }

        private void DonePressed() {
            ViewModel.Finish();
        }


        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            LineDrawingView.Lines = ViewModel.GetLinesForAnswers();
            LineDrawingView.Refresh();
        }

        public override void ViewDidLayoutSubviews() {
            base.ViewDidLayoutSubviews();
            if (ViewModel != null) {
                //resize the tables rows so they all fit in the table without scrolling
                if (ViewModel.MainPhraseCount > 0) {
                    LeftTable.RowHeight = LeftTable.Bounds.Height / ViewModel.MainPhraseCount;
                    LeftTable.LayoutIfNeeded();
                }
                if (ViewModel.MatchCount > 0) {
                    RightTable.RowHeight = RightTable.Bounds.Height / ViewModel.MatchCount;
                    RightTable.LayoutIfNeeded();
                }
            }
            ReloadLines();
        }

        public override void RefreshUILocale() {
            ReloadLines();
            //LayoutIfNeeded();
        }

        private void ReloadLines() {
            LeftTable.ReloadData();
            RightTable.ReloadData();
            LineDrawingView.Lines = ViewModel.GetLinesForAnswers();
            LineDrawingView.Refresh();
        }

        private class MainPhraseTableSource : UITableViewSource {
            private WordWeb wordWeb;
            WordWebViewModel ViewModel => wordWeb.ViewModel;

            public MainPhraseTableSource(WordWeb phraseMatchingPoolLineMatch) {
                this.wordWeb = phraseMatchingPoolLineMatch;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("LeftCell") as WordWebLeftCell;
                cell.Setup(ViewModel,
                           indexPath.Row,
                           wordWeb.MainPhrase_Dragged,
                           wordWeb.mainPhraseDotCell,
                           wordWeb.LineDrawingView);
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => ViewModel.MainPhraseCount;
        }



        private class MatchPhraseTableSource : UITableViewSource {
            private WordWeb phraseMatchingPoolLineMatch;
            WordWebViewModel Logic => phraseMatchingPoolLineMatch.ViewModel;

            public MatchPhraseTableSource(WordWeb phraseMatchingPoolLineMatch) {
                this.phraseMatchingPoolLineMatch = phraseMatchingPoolLineMatch;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("RightCell") as WordWebRightCell;
                if (cell.Position == -1) {
                    //if the cell hasn't been used before add the gesture recogniser
                    var gesture = new UILongPressGestureRecognizer();
                    gesture.AddTarget(() => phraseMatchingPoolLineMatch.Match_Dragged(gesture, cell));
                    gesture.MinimumPressDuration = 0;
                    cell.AddGestureRecognizer(gesture);
                    //Add the cell to the hash set of cells
                    phraseMatchingPoolLineMatch.matchCells.Add(cell);
                }
                //Setup the cell
                cell.Setup(Logic, indexPath.Row, phraseMatchingPoolLineMatch.LineDrawingView);
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => Logic.MatchCount;
        }




        /// <summary>
        /// Drag started from main phrase cell
        /// </summary>
        /// <param name="gesture"></param>
        /// <param name="cell"></param>
        /// <param name="dotCell"></param>
        private void MainPhrase_Dragged(UILongPressGestureRecognizer gesture, WordWebLeftCell cell, WordWebDotCell dotCell) {
            //get the touch location
            var touchPoint = gesture.LocationInView(this.LineDrawingView);
            switch (gesture.State) {
                case UIGestureRecognizerState.Began:
                    //if it's the start of the drag
                    //set the start of the drag in logic
                    //ViewModel.DragFromMainPhraseStarted(cell.Position, dotCell.Position, dotCell.ConvertRectToView(dotCell.DotContainer.Frame, this.LineDrawingView).ToFRect().Center);
                    ViewModel.DragFromMainPhraseStarted(cell.Position, dotCell.Position);
                    //draw the line
                    LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, touchPoint.ToFPoint(), dotCell.Color, 3);
                    break;
                case UIGestureRecognizerState.Changed:
                    //check when the position is changed, if it's within a cell draw the end line to the dot of that cell
                    var enteredMatchCell = matchCells.FirstOrDefault(matchCell => matchCell.ConvertRectToView(matchCell.Bounds, this.LineDrawingView).Contains(touchPoint));
                    if (enteredMatchCell != null) {
                        var matchRect = enteredMatchCell.ConvertRectToView(enteredMatchCell.TouchView.Frame, this.LineDrawingView);
                        LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, matchRect.ToFRect().Center, dotCell.Color, 3);
                    } else { //if not draw in to the touch position
                        LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, touchPoint.ToFPoint(), dotCell.Color, 3);
                    }
                    break;
                case UIGestureRecognizerState.Ended:
                    //when the drag ended
                    //check if it's in any of the cells
                    var endMatchCell = matchCells.FirstOrDefault(matchCell => matchCell.ConvertRectToView(matchCell.Bounds, this.LineDrawingView).Contains(touchPoint));
                    if (endMatchCell != null) {
                        var matchCenter = endMatchCell.ConvertRectToView(endMatchCell.TouchView.Frame, this.LineDrawingView).ToFRect().Center;
                        ViewModel.DroppedInMatchPhrase(endMatchCell.Position);
                    }
                    //remove the dragged line
                    LineDrawingView.Line = null;
                    //reload the tables
                    LeftTable.ReloadData();
                    RightTable.ReloadData();
                    break;
            }
            LineDrawingView.Lines = ViewModel.GetLinesForAnswers();
            LineDrawingView.Refresh();
        }


        /// <summary>
        /// Drag started from Match cell
        /// </summary>
        /// <param name="gesture"></param>
        /// <param name="cell"></param>
        private void Match_Dragged(UILongPressGestureRecognizer gesture, WordWebRightCell cell) {
            //find where the touch was
            var touchPoint = gesture.LocationInView(this.LineDrawingView);
            switch (gesture.State) {
                case UIGestureRecognizerState.Began:
                    //set the start of the drag in logic
                    ViewModel.DragFromMatchPhraseStarted(cell.Position);
                    //draw the line
                    LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, touchPoint.ToFPoint(), Color.Black, 3);
                    break;
                case UIGestureRecognizerState.Changed:
                    //check when the position is changed, if it's within a cell draw the end line to the dot of that cell
                    var enteredMainDot = mainPhraseDotCell.FirstOrDefault(dotCell => dotCell.ConvertRectToView(dotCell.Bounds, this.LineDrawingView).Contains(touchPoint));
                    if (enteredMainDot != null) {
                        var dotCenter = enteredMainDot.ConvertRectToView(enteredMainDot.DotContainer.Frame, this.LineDrawingView).ToFRect().Center;
                        LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, dotCenter, enteredMainDot.Color, 3);
                    } else { //if not draw in to the touch position
                        LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, touchPoint.ToFPoint(), Color.Black, 3);
                    }
                    break;
                case UIGestureRecognizerState.Ended:
                    //when the drag ended
                    //check if it's in any of the cells
                    var endMainDot = mainPhraseDotCell.FirstOrDefault(dotCell => dotCell.ConvertRectToView(dotCell.Bounds, this.LineDrawingView).Contains(touchPoint));
                    if (endMainDot != null) {
                        var dotCenter = endMainDot.ConvertRectToView(endMainDot.DotContainer.Frame, this.LineDrawingView).ToFRect().Center;
                        LineDrawingView.Line = new ColoredLine(ViewModel.LineStart, dotCenter, endMainDot.Color, 3);
                        ViewModel.DropppedInMainPhrase(endMainDot.MainPhrasePosition, endMainDot.Position);
                    }
                    //remove the dragged line
                    LineDrawingView.Line = null;
                    //reload the tables
                    LeftTable.ReloadData();
                    RightTable.ReloadData();
                    break;
            }
            LineDrawingView.Lines = ViewModel.GetLinesForAnswers();
            LineDrawingView.Refresh();
        }




    }
}