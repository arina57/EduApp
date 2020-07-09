using System;

using UIKit;

namespace EduApp.iOS.Views {
    public partial class MainView : UIViewController {
        public MainView() : base("MainView", null) {
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning() {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

