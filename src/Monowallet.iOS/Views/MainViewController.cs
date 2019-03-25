using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using Foundation;
using Monowallet.Core.Models;
using Monowallet.Core.Services;
using Monowallet.iOS.Chat;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using UIKit;
using Xamarin.Essentials;

namespace Monowallet.iOS.Views
{
    public partial class MainViewController : UIViewController, IUITextFieldDelegate
    {
        private System.Timers.Timer Timer = new System.Timers.Timer(2000);
        private ConcurrentDictionary<string, Node> Nodes { get; set; } = new ConcurrentDictionary<string, Node>();

        public ObservableCollection<string> Messages { get; private set; }
        public PeerDiscoveryService PeerDiscoveryService { get; private set; }

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

            _tableView.RowHeight = UITableView.AutomaticDimension;
            _tableView.EstimatedRowHeight = UITableView.AutomaticDimension;

            _tableView.RegisterNibForCellReuse(MessageCell.Nib, MessageCell.Key);
            _tableView.Source = new MessagesTableSource(_tableView, Messages);
            _tableView.ReloadData();

            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Chat", HandleChatConnection);
            Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, 49999));

            PeerDiscoveryService = new PeerDiscoveryService(new UdpBroadcastService());

            PeerDiscoveryService.StartListening(OnPeerDiscovered);
            PeerDiscoveryService.StartSending(DeviceInfo.Name);

            Timer.Elapsed += CheckExpiredNodes;
            Timer.Start();
        }

        private void OnPeerDiscovered(Broadcast broadcast)
        {
            try
            {
                if (!Nodes.TryGetValue(broadcast.Address, out Node existingNode))
                {
                    existingNode = new Node
                    {
                        Address = broadcast.Address,
                        Name = broadcast.Data
                    };

                    Nodes.TryAdd(existingNode.Address, existingNode);

                    InvokeOnMainThread(() => Messages.Add($"Discovered: {existingNode.Name}"));
                }

                existingNode.DiscoveredAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                InvokeOnMainThread(() => Messages.Add($"Exception: {ex.Message}"));
            }
        }

        private void CheckExpiredNodes(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var expiredNodes = Nodes.Where(
                    p => DateTime.UtcNow - p.Value.DiscoveredAt > TimeSpan.FromMilliseconds(2000));

                foreach (var node in expiredNodes)
                {
                    if (Nodes.TryRemove(node.Key, out Node removedNode))
                    {
                        InvokeOnMainThread(() => Messages.Add($"Disconnected node: {removedNode.Name}"));
                    }
                }
            }
            catch (Exception ex)
            {
                InvokeOnMainThread(() => Messages.Add($"Exception: {ex.Message}"));
            }
        }

        private void HandleChatConnection(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            InvokeOnMainThread(() => Messages.Add($"{incomingObject}"));
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
                var message = _messageTextField.Text;

                try
                {
                    foreach (var node in Nodes)
                    {
                        try
                        {
                            var connection = TCPConnection.GetConnection(new ConnectionInfo(node.Key, 49999));
                            connection.SendObject("Chat", message);
                        }
                        catch (DuplicateConnectionException)
                        {
                            // todo: establishing connection simultaneously
                        }
                        catch (ConnectionSetupException)
                        {
                            node.Value.Broken = true;
                        }
                    }

                    var brokenNodes = Nodes.Where(n => n.Value.Broken);
                    foreach (var node in brokenNodes)
                    {
                        if (Nodes.TryRemove(node.Key, out Node removedNode))
                        {
                            InvokeOnMainThread(() => Messages.Add($"Disconnected node: {removedNode.Name}"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Messages.Add(ex.Message);
                }

                _messageTextField.Text = string.Empty;
            }
        }
    }
}

