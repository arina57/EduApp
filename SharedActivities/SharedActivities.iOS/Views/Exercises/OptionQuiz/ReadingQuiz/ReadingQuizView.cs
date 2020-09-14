using System;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core.ViewModels;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.ReadingQuiz {
    public partial class ReadingQuizView : CrossUIViewController<ReadingOptionQuizViewModel> {

        public ReadingQuizView(IntPtr handle) : base(handle) {
        }

        public ReadingQuizView() {

        }

        public override void RefreshUILocale() {
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            ReadingTable.RegisterNibForCellReuse(ReadingQuizCellView.Nib, "ReadingQuizResuseCell");
            ReadingTable.Source = new ReadingQuizSource(this);
            ProgressView.ProgressTracker = ViewModel;

        }



        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            ReadingTable.ScrollRectToVisible(new CoreGraphics.CGRect(0, 0, 1, 1), false);
        }

        private class ReadingQuizSource : UITableViewSource {
            private ReadingQuizView readingQuiz;
            private ReadingOptionQuizViewModel Logic => readingQuiz.ViewModel;

            public ReadingQuizSource(ReadingQuizView readingQuiz) {
                this.readingQuiz = readingQuiz;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
                var cell = tableView.DequeueReusableCell("ReadingQuizResuseCell") as ReadingQuizCellView;
                cell.Setup(Logic, indexPath.Row);

                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section) => Logic.LineCount;

        }
    }
}