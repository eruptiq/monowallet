// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Monowallet.iOS.Chat
{
    [Register("MessageCell")]
    partial class MessageCell
    {
        [Outlet]
        UIKit.UILabel _label { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (_label != null)
            {
                _label.Dispose();
                _label = null;
            }
        }
    }
}