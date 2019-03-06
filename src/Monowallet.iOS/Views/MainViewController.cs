using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Foundation;
using Monowallet.Core.Services;
using Monowallet.iOS.Chat;
using UIKit;

namespace Monowallet.iOS.Views
{
    public partial class MainViewController : UIViewController, IUITextFieldDelegate
    {
        public ObservableCollection<string> Messages { get; private set; }

        public UdpBroadcastConnection BroadcastListener { get; private set; }

        public MainViewController(IntPtr handler) : base(handler)
        {
            Messages = new ObservableCollection<string>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UIKeyboard.Notifications.ObserveDidShow(OnKeyboardDidShow);
            UIKeyboard.Notifications.ObserveWillHide(OnKeyboardWillHide);

            _messageTextField.Delegate = this;

            _tableView.RegisterNibForCellReuse(MessageCell.Nib, MessageCell.Key);
            _tableView.Source = new MessagesTableSource(_tableView, Messages);
            _tableView.ReloadData();

            BroadcastListener = new UdpBroadcastConnection();

            Task.Run(async () =>
            {
                while (true)
                {
                    var message = await BroadcastListener.ListenAsync();
                    InvokeOnMainThread(() => Messages.Add(message));
                }
            });
        }

        private void OnKeyboardDidShow(object sender, UIKeyboardEventArgs e)
        {
            var contentInsets = new UIEdgeInsets(0, 0, e.FrameEnd.Height, 0);
            _scrollView.ContentInset = contentInsets;
            _scrollView.ScrollIndicatorInsets = contentInsets;
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            return true;
        }

        private void OnKeyboardWillHide(object sender, UIKeyboardEventArgs e)
        {
            _scrollView.ContentInset = UIEdgeInsets.Zero;
            _scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
        }

        partial void OnSend(NSObject sender)
        {
            if (!string.IsNullOrEmpty(_messageTextField.Text))
            {
                BroadcastListener.Send(_messageTextField.Text);
                _messageTextField.Text = string.Empty;
            }
        }
    }
}

