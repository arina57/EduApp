using System;

using UIKit;

namespace SharedActivities.iOS.Views.Exercises.PhraseMatch {
    public partial class PhraseMatchDragView : UIView {

        public UILabel Label => TextLabel;


        public PhraseMatchDragView(IntPtr handle) : base(handle) {
        }


        public override void AwakeFromNib() {
            base.AwakeFromNib();
            this.Hidden = true;
        }

    }
}