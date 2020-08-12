using System;
using Airbnb.Lottie;
using CrossLibrary.iOS.Views;
using Foundation;
using SharedActivities.Core;
using SharedActivities.Core.ViewModels;
using UIKit;
using Xamarin.Essentials;

namespace SharedActivities.iOS.Views {
    public partial class UnitPracticeView : CrossUIViewController<UnitPracticeViewModel> {
        private UICollectionViewFlowLayout collectionViewFlowControl;
        private LOTAnimationView leftImage;
        private LOTAnimationView rightImage;

        public UnitPracticeView(IntPtr handle) : base(handle) {

        }

        public UnitPracticeView() {

        }



        public override void RefreshUILocale() {
            PageSelector.ReloadData();
            this.View.LayoutSubviews();
            ButtonHeightConstraint.Constant = ViewModel.ShowDoneButton ? 30 : 0;
            DoneButton.SetTitle(ViewModel.DoneButtonText, UIControlState.Normal);
        }
        public override void ViewDidLayoutSubviews() {
            base.ViewDidLayoutSubviews();
            if (ViewModel.PageCount > 0) {
                collectionViewFlowControl.ItemSize = new CoreGraphics.CGSize(PageSelector.Bounds.Width / ViewModel.PageCount, PageSelector.Bounds.Height);
            }
        }


        public override void ViewDidLoad() {
            base.ViewDidLoad();

            collectionViewFlowControl = new UICollectionViewFlowLayout();
            collectionViewFlowControl.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            collectionViewFlowControl.MinimumInteritemSpacing = 0;
            collectionViewFlowControl.MinimumLineSpacing = 0;
            PageSelector.CollectionViewLayout = collectionViewFlowControl;
            PageSelector.RegisterNibForCell(UnitPracticeCellView.Nib, "UnitPracticeReuseCell");
            PageSelector.Source = new PageSelectorSource(this);



            DoneButton.TouchUpInside += ResetButton_TouchUpInside;
            DoneButton.BackgroundColor = GlobalColorPalette.Medium.ToPlatformColor();
            DoneButton.Font = UIFont.FromName("ChaletComprime-CologneSeventy", DoneButton.Font.PointSize);
            DoneButton.SetTitleColor(GlobalColorPalette.White.ToPlatformColor(), UIControlState.Normal);
            DoneButton.AddShadow();

            //leftImage = Functions.LottieFromString(Resx.Lottie.UnitPractice_navbarrobotL, GlobalColorPalette.VeryDark);
            //rightImage = Functions.LottieFromString(Resx.Lottie.UnitPractice_navbarrobotR, GlobalColorPalette.VeryDark);
            //PageSelectorLeft.Subviews.ReleaseChildren();
            //PageSelectorLeft.Subviews.ReleaseChildren();
            //PageSelectorLeft.AddSubview(leftImage);
            //PageSelectorRight.AddSubview(rightImage);
            //leftImage.FillParentContraints();
            //rightImage.FillParentContraints();


        }

        private async void ResetButton_TouchUpInside(object sender, EventArgs e) {
            await ViewModel.DoneButtonPressed();
        }

        public override void ViewWillUnload() {
            DoneButton.TouchUpInside -= ResetButton_TouchUpInside;
            leftImage.RemoveFromSuperview();
            leftImage.Dispose();
            leftImage = null;
            rightImage.RemoveFromSuperview();
            rightImage.Dispose();
            rightImage = null;
        }

        private class PageSelectorSource : UICollectionViewSource {
            private UnitPracticeView unitPractice;
            UnitPracticeViewModel ViewModel => unitPractice.ViewModel;

            public PageSelectorSource(UnitPracticeView unitPractice) {
                this.unitPractice = unitPractice;
            }

            public override nint NumberOfSections(UICollectionView collectionView) => 1;
            public override nint GetItemsCount(UICollectionView collectionView, nint section) => ViewModel.PageCount;

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) {
                var position = indexPath.Row;
                var cell = collectionView.DequeueReusableCell("UnitPracticeReuseCell", indexPath) as UnitPracticeCellView;
                cell.LastPostion = position;
                if (cell.GestureRecognizers == null || cell.GestureRecognizers.Length < 1) {
                    var tapRecog = new UITapGestureRecognizer(async () => {
                        await ViewModel.ChangePage(cell.LastPostion);
                    });
                    cell.AddGestureRecognizer(tapRecog);
                }

                cell.PageNumber = position + 1;
                if (ViewModel.CurrentPage == position) {
                    cell.BackgroundColor = GlobalColorPalette.VeryLight.ToPlatformColor();
                } else {
                    cell.BackgroundColor = UIColor.Clear;
                }

                cell.ScoreText = ViewModel.ScoreText(position);
                cell.SetImageJson(ViewModel.GetDoneImage(position));
                return cell;

            }
        }
    }
}

