using System;
using CrossLibrary.iOS.Views;
using SharedActivities.Core.ViewModels.Exercises;
using UIKit;

namespace SharedActivities.iOS.Views.Exercises.OptionQuiz.BasicOptionQuiz {
    public partial class BasicOptionQuiz : CrossUIViewController<BasicOptionQuizViewModel> {

        public BasicOptionQuiz() {
        }


        public BasicOptionQuiz(IntPtr handle) : base(handle) {
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            //QuizView.Subviews.ReleaseChildren();
            ProgressView.ProgressTracker = ViewModel;
        }

        public override void DidReceiveMemoryWarning() {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void RefreshUILocale() {
        }
    }
}

