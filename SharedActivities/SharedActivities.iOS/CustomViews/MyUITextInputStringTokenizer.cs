using System;
using UIKit;

namespace SharedActivities.iOS.CustomViews {
    /// <summary>
    /// Hack class to make UITextInputStringTokenizer(IntPtr handle) public
    /// </summary>
    class MyUITextInputStringTokenizer : UITextInputStringTokenizer {

        public MyUITextInputStringTokenizer(IntPtr handle) : base(handle) {
        }
    }
}
