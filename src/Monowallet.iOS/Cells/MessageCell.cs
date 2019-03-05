using System;

using Foundation;
using UIKit;

namespace Monowallet.iOS.Cells
{
    public partial class MessageCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MessageCell");
        public static readonly UINib Nib;

        public UILabel Label => _label;

        static MessageCell()
        {
            Nib = UINib.FromName("MessageCell", NSBundle.MainBundle);
        }

        protected MessageCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
