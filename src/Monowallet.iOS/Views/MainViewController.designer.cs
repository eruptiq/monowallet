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
		UIKit.UITextField _messageTextField { get; set; }

		[Outlet]
		UIKit.UIScrollView _scrollView { get; set; }

		[Outlet]
		UIKit.UITableView _tableView { get; set; }

		[Action ("OnSend:")]
		partial void OnSend (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_messageTextField != null) {
				_messageTextField.Dispose ();
				_messageTextField = null;
			}

			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}

			if (_scrollView != null) {
				_scrollView.Dispose ();
				_scrollView = null;
			}
		}
	}
}
