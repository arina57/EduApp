using Foundation;
using System;
using UIKit;
using CrossLibrary.iOS.Views;
using SharedActivities.Core.ViewModels.Exercises.Results;
using SharedActivities.Core.ViewModels.Exercises;
using SharedActivities.iOS.Views.Exercises.OptionQuiz.OptionQuizResults;

namespace SharedActivities.iOS.Views.Exercises.GapFill {
    public partial class GapFillResultsView : CrossUIViewController<GapFillResultsViewModel> {


        public GapFillResultsView() {
        }

        public override void RefreshUILocale() {
            TitleLabel.Text = string.Empty; //Resx.String.Results;
            ResultsTable.ReloadData();
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            ResultsTable.RegisterNibForCellReuse(GapFillCellView.Nib, "ResultsReuseCell");
            ResultsTable.Source = new ResultsSource(ViewModel.GapFillViewModel);
        }

        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            ScrollView.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
            ResultsTable.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
        }


        private class ResultsSource : UITableViewSource {
            private GapFillViewModel viewModel;

            public ResultsSource(GapFillViewModel gapFillViewModel) {
                this.viewModel = gapFillViewModel;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
                tableView.DeselectRow(indexPath, false);
                var cell = tableView.CellAt(indexPath) as OptionQuizResultCell;
                cell.Expanded = !cell.Expanded;
                tableView.ReloadData();
                tableView.LayoutSubviews();
                cell.LayoutSubviews();

                //tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("ResultsReuseCell") as GapFillCellView;
                cell.Setup(viewModel, indexPath.Row);
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => viewModel.PhraseCount;
        }
    }
}