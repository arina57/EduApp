using System;
using System.Threading.Tasks;
using Airbnb.Lottie;
using CoreGraphics;
using Foundation;
using SharedLibrary.iOS;
using UIKit;
using CrossLibrary.iOS;
using CrossLibrary.iOS.Views;
using SharedActivities.Core.ViewModels.Exercises;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz {
    public partial class OptionQuizView : CrossUIViewController<OptionQuizViewModel> {

        private UICollectionViewFlowLayout collectionViewFlowControl;
        private LOTAnimationView marubatsu;



        public OptionQuizView() {
        }

        public override void AwakeFromNib() {
            base.AwakeFromNib();

        }


        private async Task AnimateImageIn(bool correct) {
            marubatsu.AnimationProgress = correct ? 0 : 1;
            await UIView.AnimateAsync(1, () => {
                OptionCollection.Alpha = 0;
                QuestionLabel.Alpha = 0;
                marubatsu.Transform = CGAffineTransform.MakeScale(1, 1);
                marubatsu.Alpha = 1;
            });
        }


        private async Task AnimateImageOut(bool correct) {
            marubatsu.AnimationProgress = correct ? 0 : 1;
            await UIView.AnimateAsync(1, () => {
                OptionCollection.Alpha = 1;
                QuestionLabel.Alpha = 1;
                marubatsu.Transform = CGAffineTransform.MakeScale(0.5f, 0.5f);
                marubatsu.Alpha = 0;
            });
        }


        public override void ViewDidLoad() {
            base.ViewDidLoad();
            marubatsu = Functions.LottieFromString(SharedActivities.Core.Resx.Lottie.marubatsu);
            ImageFrame.AddSubview(marubatsu);
            marubatsu.FillParentContraints();
            marubatsu.Transform = CGAffineTransform.MakeScale(0.5f, 0.5f);
            marubatsu.Alpha = 0.0f;
            marubatsu.UserInteractionEnabled = false;
            ImageFrame.UserInteractionEnabled = false;
            OptionCollection.RegisterNibForCell(UINib.FromName("OptionQuizCellView", null), "OptionResuseCell");
            OptionCollection.Source = new OptionCollectionSource(this);
            collectionViewFlowControl = new UICollectionViewFlowLayout();
            collectionViewFlowControl.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            collectionViewFlowControl.MinimumInteritemSpacing = 0;
            collectionViewFlowControl.MinimumLineSpacing = 0;
            OptionCollection.CollectionViewLayout = collectionViewFlowControl;
            RefreshUILocale();
        }

        public override void ViewDidLayoutSubviews() {
            base.ViewDidLayoutSubviews();
            QuestionLabel.LayoutIfNeeded();
            OptionCollection.LayoutIfNeeded();
            if (ViewModel.AnswerGrid) {
                collectionViewFlowControl.ItemSize = new CGSize(OptionCollection.Bounds.Width / 2, OptionCollection.Bounds.Height / ViewModel.CurrentQuestionAnswerOptionsCount * 2);
            } else {
                collectionViewFlowControl.ItemSize = new CGSize(OptionCollection.Bounds.Width, OptionCollection.Bounds.Height / ViewModel.CurrentQuestionAnswerOptionsCount);
            }
        }

        public override void RefreshUILocale() {
            if (ViewModel != null) {
                QuestionLabel.Text = ViewModel.CurrentQuestionText;
                QuestionLabel.Hidden = string.IsNullOrWhiteSpace(ViewModel.CurrentQuestionText);
                View.LayoutSubviews();
                OptionCollection.ReloadData();
            }
        }



        private class OptionCollectionSource : UICollectionViewSource {
            private OptionQuizView optionQuizView;
            private OptionQuizViewModel logic;


            public OptionCollectionSource(OptionQuizView optionQuizView) {
                this.optionQuizView = optionQuizView;
                logic = optionQuizView.ViewModel;
            }

            public override nint NumberOfSections(UICollectionView collectionView) => 1;
            public override nint GetItemsCount(UICollectionView collectionView, nint section) => logic.CurrentQuestionAnswerOptionsCount;
            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) {
                var position = indexPath.Row;
                var cell = collectionView.DequeueReusableCell("OptionResuseCell", indexPath) as OptionQuizCellView;
                cell.LastPostion = position;
                if (cell.GestureRecognizers == null || cell.GestureRecognizers.Length < 1) {
                    var tapRecog = new UITapGestureRecognizer(async () => {
                        if (logic.AcceptAnswers) {
                            logic.AcceptAnswers = false; //prevents more clicks while awaiting
                            var correct = logic.CheckAnswer(cell.LastPostion);
                            cell.BackgroundColor = correct ? UIColor.Green : UIColor.Red;
                            await optionQuizView.AnimateImageIn(correct);
                            logic.AcceptAnswers = true;
                            logic.SetAnswer(cell.LastPostion);
                            cell.BackgroundColor = UIColor.Clear;
                            optionQuizView.RefreshUILocale();
                            await optionQuizView.AnimateImageOut(correct);
                        }
                    });
                    cell.AddGestureRecognizer(tapRecog);
                }
                cell.OptionText = logic.GetAnswerOptionText(position);
                return cell;
            }
        }
    }
}