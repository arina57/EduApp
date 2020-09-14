using System;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises.Results;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    public partial class PhraseMatchResultsView : CrossUIViewController<PhraseMatchResultsViewModel> {


        public PhraseMatchResultsView() {
        }

        public override void RefreshUILocale() {
            TitleLabel.Text = string.Empty; //Resx.String.Results;
            ResultsTable.ReloadData();
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            ResultsTable.RegisterNibForCellReuse(PhraseMatchMainCellView.Nib, "PhraseMatchResultsCell");
            ResultsTable.Source = new ResultsSource(this);
        }



        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            ScrollView.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
            ResultsTable.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
        }

        private class ResultsSource : UITableViewSource {
            private PhraseMatchResultsView resultExplaination;
            private PhraseMatchResultsViewModel ViewModel => resultExplaination.ViewModel;
            public ResultsSource(PhraseMatchResultsView resultExplaination) {
                this.resultExplaination = resultExplaination;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("PhraseMatchResultsCell") as PhraseMatchMainCellView;
                cell.Setup(ViewModel.PhraseMatchViewModel, indexPath.Row);
                //cell.Expanded = false;
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => ViewModel.PhraseMatchViewModel.TotalNumberOfQuestions;
        }
    }
}