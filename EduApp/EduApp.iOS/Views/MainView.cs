using System;
using CrossLibrary.iOS.Views;
using EduApp.Core.ViewModels;
using UIKit;

namespace EduApp.iOS.Views {
    public partial class MainView : PlatformCrossView<MainViewModel> {
        public MainView() {
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            BindText(Button, vm => vm.ButtonText);
            BindClick(Button, vm => vm.Button_Clicked);
        }

        public override void DidReceiveMemoryWarning() {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

