// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Monowallet.iOS.Views
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		UIKit.UITextView log { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (log != null) {
				log.Dispose ();
				log = null;
			}
		}
	}
}
