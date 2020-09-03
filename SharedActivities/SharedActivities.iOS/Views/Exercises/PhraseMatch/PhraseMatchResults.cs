using System;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises.Results;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    public partial class PhraseMatchResults : CrossUIViewController<PhraseMatchResultsViewModel> {


        public PhraseMatchResults() {
        }

        public override void RefreshUILocale() {
            TitleLabel.Text = string.Empty; //Resx.String.Results;
            ResultsTable.ReloadData();
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            ResultsTable.RegisterNibForCellReuse(PhraseMatchMainCell.Nib, "PhraseMatchResultsCell");
            ResultsTable.Source = new ResultsSource(this);
        }



        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            ScrollView.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
            ResultsTable.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
        }

        private class ResultsSource : UITableViewSource {
            private PhraseMatchResults resultExplaination;
            private PhraseMatchResultsViewModel ViewModel => resultExplaination.ViewModel;
            public ResultsSource(PhraseMatchResults resultExplaination) {
                this.resultExplaination = resultExplaination;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("PhraseMatchResultsCell") as PhraseMatchMainCell;
                cell.Setup(ViewModel.PhraseMatchingPoolViewModel, indexPath.Row);
                //cell.Expanded = false;
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => ViewModel.PhraseMatchingPoolViewModel.TotalNumberOfQuestions;
        }
    }
}