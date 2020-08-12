using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SharedActivities.iOS.CustomViews {
    [Register("ContentSizedCollectionView")]
    public class ContentSizedCollectionView : UICollectionView {

        public ContentSizedCollectionView(IntPtr handle) : base(handle) {
        }

        public override CGSize ContentSize {
            get => base.ContentSize;
            set {
                base.ContentSize = value;
                InvalidateIntrinsicContentSize();
            }
        }

        public override CGSize IntrinsicContentSize {
            get {
                LayoutIfNeeded();
                return new CGSize(UIView.NoIntrinsicMetric, ContentSize.Height);
            }
        }
    }
}
