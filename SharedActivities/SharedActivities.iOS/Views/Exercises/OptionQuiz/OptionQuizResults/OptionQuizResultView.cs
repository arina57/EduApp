using System;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core.ViewModels.Exercises.Results;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.OptionQuizResults {
    public partial class OptionQuizResultView : CrossUIViewController<OptionQuizResultViewModel> {


        public OptionQuizResultView() {
        }

        public override void RefreshUILocale() {
            TitleLabel.Text = string.Empty; //Resx.String.Results;
            ResultsTable.ReloadData();
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            ResultsTable.RegisterNibForCellReuse(OptionQuizResultCellView.Nib, "ResultsReuseCell");
            ResultsTable.Source = new ResultsSource(this);
        }

        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            ScrollView.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
            ResultsTable.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
        }

        private class ResultsSource : UITableViewSource {
            private OptionQuizResultView resultExplaination;
            private OptionQuizResultViewModel ViewModel => resultExplaination.ViewModel;
            public ResultsSource(OptionQuizResultView resultExplaination) {
                this.resultExplaination = resultExplaination;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
                tableView.DeselectRow(indexPath, false);
                var cell = tableView.CellAt(indexPath) as OptionQuizResultCellView;
                cell.Expanded = !cell.Expanded;
                tableView.ReloadData();
                tableView.LayoutSubviews();
                cell.LayoutSubviews();

                //tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("ResultsReuseCell") as OptionQuizResultCellView;
                if (cell.QuestionNumber == -1) {

                }

                cell.Setup(ViewModel, indexPath.Row);
                //cell.Expanded = false;
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => ViewModel.OptionQuiz.TotalNumberOfQuestions;
        }
    }
}