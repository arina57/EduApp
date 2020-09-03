using System;
using System.Collections.Generic;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.Core.ViewModels.Exercises.Results;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.WordWeb {
    public partial class WordWebResults : CrossUIViewController<WordWebResultsViewModel> {

        HashSet<WordWebRightCell> matchCells = new HashSet<WordWebRightCell>();
        HashSet<WordWebDotCell> mainPhraseDotCell = new HashSet<WordWebDotCell>();
        public WordWebResults() {
        }

        public override void RefreshUILocale() {
            TitleLabel.Text = string.Empty; //Resx.String.Results;
            LeftTable.ReloadData();
            RightTable.ReloadData();
            LineDrawingView.Lines = ViewModel.WordWebViewModel.GetLinesForAnswers();
            LineDrawingView.Refresh();
        }

        public override void ViewDidLayoutSubviews() {
            base.ViewDidLayoutSubviews();
            if (ViewModel != null) {
                var largerContentHeight = (float)Math.Max(LeftTable.ContentSize.Height, RightTable.ContentSize.Height);
                TableHeight.Constant = largerContentHeight;

                //resize the tables rows so they all fit in the table without scrolling
                if (ViewModel.WordWebViewModel.MainPhraseCount > 0) {
                    LeftTable.RowHeight = largerContentHeight / ViewModel.WordWebViewModel.MainPhraseCount;
                    LeftTable.LayoutIfNeeded();
                }
                if (ViewModel.WordWebViewModel.MatchCount > 0) {
                    RightTable.RowHeight = largerContentHeight / ViewModel.WordWebViewModel.MatchCount;
                    RightTable.LayoutIfNeeded();
                }
                RefreshUILocale();
            }
        }

        public override void ViewDidAppear(bool animated) {
            base.ViewDidAppear(animated);
            View.LayoutSubviews();
            ViewDidLayoutSubviews();
        }

        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            ScrollView.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
        }



        public override void ViewDidLoad() {
            base.ViewDidLoad();
            LeftTable.RegisterNibForCellReuse(WordWebLeftCell.Nib, "LeftCellReuse");
            RightTable.RegisterNibForCellReuse(WordWebRightCell.Nib, "RightCellReuse");

            LeftTable.Source = new MainPhraseTableSource(this);
            RightTable.Source = new MatchPhraseTableSource(this);
        }


        private class MainPhraseTableSource : UITableViewSource {
            private WordWebResults phraseMatchingPoolLineMatch;
            WordWebViewModel ViewModel => phraseMatchingPoolLineMatch.ViewModel.WordWebViewModel;

            public MainPhraseTableSource(WordWebResults phraseMatchingPoolLineMatch) {
                this.phraseMatchingPoolLineMatch = phraseMatchingPoolLineMatch;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("LeftCellReuse") as WordWebLeftCell;
                cell.Setup(ViewModel, indexPath.Row, null, phraseMatchingPoolLineMatch.mainPhraseDotCell, phraseMatchingPoolLineMatch.LineDrawingView);
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => ViewModel.MainPhraseCount;
        }



        private class MatchPhraseTableSource : UITableViewSource {
            private WordWebResults phraseMatchingPoolLineMatch;
            WordWebViewModel Logic => phraseMatchingPoolLineMatch.ViewModel.WordWebViewModel;

            public MatchPhraseTableSource(WordWebResults phraseMatchingPoolLineMatch) {
                this.phraseMatchingPoolLineMatch = phraseMatchingPoolLineMatch;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("RightCellReuse") as WordWebRightCell;
                if (cell.Position == -1) {
                    //Add the cell to the hash set of cells
                    phraseMatchingPoolLineMatch.matchCells.Add(cell);
                }
                //Setup the cell
                cell.Setup(Logic, indexPath.Row, phraseMatchingPoolLineMatch.LineDrawingView);
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => Logic.MatchCount;
        }


    }
}