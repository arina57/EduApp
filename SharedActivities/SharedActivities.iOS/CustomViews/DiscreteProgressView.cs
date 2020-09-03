using System;
using Airbnb.Lottie;
using CoreGraphics;
using CrossLibrary.iOS;
using Foundation;
using SharedActivities.Core.CrossPlatformInterfaces;
using UIKit;
using static SharedActivities.Core.GlobalEnums;

namespace SharedActivities.iOS.CustomViews {
    [Register("DiscreteProgressView")]
    public class DiscreteProgressView : UICollectionView {

        private UICollectionViewFlowLayout collectionViewFlowControl;

        private IDiscreteProgressTracker discreteProgressTracker;
        private DiscreteProgressSource discreteProgressSource;

        public IDiscreteProgressTracker ProgressTracker {
            get => discreteProgressTracker;
            set {
                if (discreteProgressTracker != null) {
                    discreteProgressTracker.ProgressChanged -= StateChanged;
                    discreteProgressTracker.QuizReset -= DiscreteProgressTracker_QuizReset;
                }
                discreteProgressTracker = value;
                discreteProgressTracker.ProgressChanged += StateChanged;
                discreteProgressTracker.QuizReset += DiscreteProgressTracker_QuizReset;
                ResizeItems();
            }
        }

        private void DiscreteProgressTracker_QuizReset(object sender, EventArgs e) {
            ReloadData();
        }

        private void StateChanged(object sender, EventArgs e) {
            //this.ReloadData();
            if (ProgressTracker.CurrentQuestionNumber < ProgressTracker.TotalNumberOfQuestions) {
                discreteProgressSource.Animate = true;
                this.ReloadItems(new NSIndexPath[] { NSIndexPath.FromRowSection(ProgressTracker.CurrentQuestionNumber - 1, 0) });
                this.ReloadItems(new NSIndexPath[] { NSIndexPath.FromRowSection(ProgressTracker.CurrentQuestionNumber, 0) });
            }


        }

        public override void ReloadData() {
            discreteProgressSource.Animate = false;
            base.ReloadData();
        }

        protected internal DiscreteProgressView(IntPtr handle) : base(handle) {
        }

        public override void AwakeFromNib() {
            base.AwakeFromNib();
            collectionViewFlowControl = new UICollectionViewFlowLayout();
            collectionViewFlowControl.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            collectionViewFlowControl.MinimumInteritemSpacing = 0;
            collectionViewFlowControl.MinimumLineSpacing = 0;
            //((UICollectionViewFlowLayout)PageSelector.CollectionViewLayout).MinimumLineSpacing = 1000f;
            this.CollectionViewLayout = collectionViewFlowControl;
            RegisterClassForCell(typeof(DiscreteProgressViewCell), "DiscreteProgressReuse");
            discreteProgressSource = new DiscreteProgressSource(this);
            this.Source = discreteProgressSource;
            ResizeItems();
        }


        public override void LayoutSubviews() {
            base.LayoutSubviews();
            ResizeItems();
        }

        private void ResizeItems() {
            if (ProgressTracker != null) {
                var maxItemWidth = this.Bounds.Width / ProgressTracker.TotalNumberOfQuestions;
                var itemWidth = Math.Min(maxItemWidth, collectionViewFlowControl.ItemSize.Height);
                collectionViewFlowControl.ItemSize = new CoreGraphics.CGSize(itemWidth, collectionViewFlowControl.ItemSize.Height);
                this.Bounds = new CGRect(0, 0, itemWidth * ProgressTracker.TotalNumberOfQuestions, this.Bounds.Height);
                //SizeToFit();
            }

        }

        private class DiscreteProgressSource : UICollectionViewSource {

            private DiscreteProgressView discreteProgressView;
            IDiscreteProgressTracker progressTracker => discreteProgressView.ProgressTracker;

            public bool Animate { get; set; }

            public DiscreteProgressSource(DiscreteProgressView discreteProgressView) {
                this.discreteProgressView = discreteProgressView;
            }

            public override nint NumberOfSections(UICollectionView collectionView) => 1;
            public override nint GetItemsCount(UICollectionView collectionView, nint section) => progressTracker == null ? 0 : progressTracker.TotalNumberOfQuestions;

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) {
                var collectionCell = collectionView.DequeueReusableCell("DiscreteProgressReuse", indexPath) as DiscreteProgressViewCell;
                collectionCell.SetCurrentQuestion(progressTracker.CurrentQuestionNumber == indexPath.Row, Animate);
                collectionCell.SetProgressState(progressTracker.AnswerProgress(indexPath.Row), Animate);
                return collectionCell;
            }
        }

        private class DiscreteProgressViewCell : UICollectionViewCell {
            private LOTAnimationView image;
            //private bool _currentQuestion;

            protected internal DiscreteProgressViewCell(IntPtr handle) : base(handle) {
                image = Functions.LottieFromString(Core.Resx.Lottie.marubatsu);
                this.AddSubview(image);
                image.ContentMode = UIViewContentMode.ScaleAspectFit;
                image.FillParentContraints();
                image.AnimationProgress = 0.5f;
            }

            public float GetProgresForState(ProgressState progressState) =>
                progressState == ProgressState.NotDone ? 0.5f : progressState == ProgressState.Correct ? 0f : 1f;

            public void SetProgressState(ProgressState progressState, bool animate) {
                if (animate) {
                    image.PlayFromProgress(0.5f, GetProgresForState(progressState), null);
                } else {
                    image.AnimationProgress = GetProgresForState(progressState);
                }

            }
            public void SetCurrentQuestion(bool currentQuestion, bool animate) {
                var scale = currentQuestion ? 0.97f : 0.8f; //some reason clips at 1.0f, this is probably not best way to do this
                if (animate) {
                    //This doesnt seem to be working
                    var scaleTransform = CGAffineTransform.MakeScale(scale, scale);
                    UIView.Animate(0.5, () => {
                        image.Transform = scaleTransform;
                        image.LayoutIfNeeded();
                    });
                } else {
                    image.Transform = CGAffineTransform.MakeScale(scale, scale);
                    image.LayoutIfNeeded();
                }
            }

            //public bool CurrentQuestion {
            //    get => _currentQuestion;
            //    set { 
            //        _currentQuestion = value;
            //        var scale = value ? 0.97f : 0.8f; //some reason clips at 1.0f, this is probably not best way to do this
            //        UIView.Animate(0.5, () => {
            //            image.Transform = CGAffineTransform.MakeScale(scale, scale);
            //        });
            //    }
            //}

            public override void PrepareForReuse() {
                base.PrepareForReuse();
                image.AnimationProgress = 0.5f;
            }

        }
    }
}
