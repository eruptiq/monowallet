using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Foundation;
using Monowallet.Core.Models;
using Monowallet.Core.Services;
using Monowallet.iOS.Chat;
using UIKit;
using System.Threading;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet;
using System.Net;
using Xamarin.Essentials;

namespace Monowallet.iOS.Views
{
    public partial class MainViewController : UIViewController, IUITextFieldDelegate
    {
        public ObservableCollection<string> Messages { get; private set; }

        public Dictionary<Node, Connection> Nodes { get; set; } = new Dictionary<Node, Connection>(10);

        private SemaphoreSlim __nodessemaphore__ = new SemaphoreSlim(1);

        public UdpBroadcastConnection BroadcastConnection { get; private set; }

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

            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Handshake", HandleHandshakeConnection);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Chat", HandleChatConnection);
            Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, 49999));

            BroadcastConnection = new UdpBroadcastConnection();

            Task.Run(async () =>
            {
                while (true)
                {
                    var node = await BroadcastConnection.ListenAsync();
                    if (node.IsSelf)
                    {
                        continue;
                    }

                    await __nodessemaphore__.WaitAsync();

                    try
                    {
                        if (!Nodes.Any(n => n.Key.Address == node.Address))
                        {
                            var connection = TCPConnection.GetConnection(new ConnectionInfo(node.Address, 49999));
                            connection.SendObject("Handshake", DeviceInfo.Name);

                            Nodes.Add(node, connection);
                        }
                    }
                    catch (Exception ex)
                    {
                        InvokeOnMainThread(() => Messages.Add($"Exception: {ex.Message}"));
                    }
                    finally
                    {
                        __nodessemaphore__.Release();
                    }
                }
            });

            Task.Run(async () =>
            {
                while (true)
                {
                    await BroadcastConnection.SendAsync();
                    await Task.Delay(499);
                }
            });
        }

        private void HandleHandshakeConnection(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            InvokeOnMainThread(() => Messages.Add($"Connected to: {incomingObject}"));
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
                Nodes.First().Value.SendObject("Chat", _messageTextField.Text);
                _messageTextField.Text = string.Empty;
            }
        }
    }
}

