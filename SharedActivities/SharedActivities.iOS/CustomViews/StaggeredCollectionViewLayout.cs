using System;
using System.Collections.Generic;
using CoreGraphics;
using CrossLibrary;
using Foundation;
using UIKit;

namespace SharedActivities.iOS.CustomViews {


    public interface LayoutDelegate {
        float HeightForItem(UICollectionViewLayout collectionViewLayout, NSIndexPath indexPath);
    }

    [Register("StaggeredCollectionView")]
    public class StaggeredCollectionViewLayout : UICollectionViewLayout {
        public int NumberOfColumns { get; set; } = 2;
        public float CellPadding { get; set; } = 6;
        public Func<UICollectionView, NSIndexPath, nfloat, nfloat> HeightForItem { get; set; }


        private List<UICollectionViewLayoutAttributes> cache = new List<UICollectionViewLayoutAttributes>();
        private nfloat ContentHeight { get; set; } = 0;
        private nfloat ContentWidth {
            get {
                if (this.CollectionView == null) {
                    return 0;
                }
                var insets = CollectionView.ContentInset;
                return CollectionView.Bounds.Width - insets.Left + insets.Right;
            }
        }
        public nfloat ColumnWidth => ContentWidth / NumberOfColumns;
        public override CGSize CollectionViewContentSize => new CGSize(ContentWidth, ContentHeight);



        int column = 0;
        public override void PrepareLayout() {
            base.PrepareLayout();


            //|| cache.Count > 0
            if (CollectionView == null) {
                return;
            }


            var xOffset = new List<nfloat>();

            for (int columnNumber = 0; columnNumber < NumberOfColumns; columnNumber++) {
                xOffset.Add(columnNumber * ColumnWidth);
            }


            var yOffset = CommonFunctions.PopulateList<nfloat>(NumberOfColumns, 0);

            for (int item = 0; item < CollectionView.NumberOfItemsInSection(0); item++) {
                var indexPath = NSIndexPath.FromItemSection(item, 0);

                var itemHeight = HeightForItem != null ? HeightForItem(CollectionView, indexPath, ColumnWidth) : 0;

                var height = CellPadding * 2 + itemHeight;
                var frame = new CGRect(xOffset[column], yOffset[column], ColumnWidth, height);
                var insetFrame = frame.Inset(CellPadding, CellPadding);

                var attributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
                if (insetFrame != CGRect.Null) {
                    attributes.Frame = insetFrame;
                    cache.Add(attributes);

                    ContentHeight = Math.Max((float)ContentHeight, (float)frame.GetMaxY());
                    yOffset[column] += height;

                    column = column < (NumberOfColumns - 1) ? column + 1 : 0;
                }

            }
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect) {
            var visibleLayoutAttributes = new List<UICollectionViewLayoutAttributes>();


            foreach (var attribute in cache) {
                if (attribute.Frame.IntersectsWith(rect)) {
                    visibleLayoutAttributes.Add(attribute);
                }
            }

            return visibleLayoutAttributes.ToArray();
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath) {
            return cache[(int)indexPath.Item];
        }
    }
}
