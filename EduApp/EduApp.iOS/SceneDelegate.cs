using System;
using CrossLibrary.Core;
using CrossLibrary.iOS;
using EduApp.Core.ViewModels;
using Foundation;
using UIKit;

namespace NewSingleViewTemplate {
    [Register("SceneDelegate")]
    public class SceneDelegate : CrossSceneDelegate {

        
        protected override CrossApp CrossApp => new EduApp.Core.CrossApp();

    }
}
