// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace EduApp.iOS.Views
{
	[Register ("MainView")]
	partial class MainView
	{
		[Outlet]
		[GeneratedCode("iOS Designer", "1.0")]
		UIKit.UIButton Button { get; set; }
		void ReleaseDesignerOutlets ()
		{
			if (Button != null) {
				Button.Dispose();
				Button = null;
			}
		}
	}
}
