// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Monowallet.iOS.Views
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        UIKit.UITextField _messageTextField { get; set; }


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
        }
    }
}