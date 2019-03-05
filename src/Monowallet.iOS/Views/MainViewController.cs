using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Foundation;
using Monowallet.Core.Services;
using Monowallet.iOS.Cells;
using Monowallet.iOS.Tables;
using UIKit;

namespace Monowallet.iOS.Views
{
    public partial class MainViewController : UIViewController
    {
        public ObservableCollection<string> Messages { get; private set; }

        public MainViewController(IntPtr handler) : base(handler)
        {
            Messages = new ObservableCollection<string>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _tableView.RegisterNibForCellReuse(MessageCell.Nib, MessageCell.Key);
            _tableView.Source = new MessagesTableSource(_tableView, Messages);
            _tableView.ReloadData();

            var broadcastListener = new UdpBroadcastService();
            Task.Run(async () =>
            {
                while (true)
                {
                    var message = await broadcastListener.ListenAsync();
                    InvokeOnMainThread(() => Messages.Add(message));
                }
            });
        }

        partial void OnSend(NSObject sender)
        {
            _messageTextField.Text = string.Empty;
        }
    }
}

