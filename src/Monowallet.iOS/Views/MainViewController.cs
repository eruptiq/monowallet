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

        public List<Node> Nodes { get; set; } = new List<Node>(10);

        private SemaphoreSlim __nodessemaphore__ = new SemaphoreSlim(1);

        public UdpBroadcastConnection BroadcastConnection { get; private set; }

        private System.Timers.Timer Timer = new System.Timers.Timer(2000);

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

            BroadcastConnection = new UdpBroadcastConnection(DeviceInfo.Name);

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
                        var existingNode = Nodes.FirstOrDefault(n => n.Address == node.Address);
                        if (existingNode == null)
                        {
                            Nodes.Add(node);
                            existingNode = node;

                            InvokeOnMainThread(() => Messages.Add($"Discovered: {node.Name}"));
                        }

                        existingNode.DiscoveredAt = DateTime.UtcNow;
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

            Timer.Elapsed += OnCheckExpiredNodes;
            Timer.Start();
        }

        private async void OnCheckExpiredNodes(object sender, System.Timers.ElapsedEventArgs e)
        {
            await __nodessemaphore__.WaitAsync();

            try
            {
                var removed = Nodes.RemoveAll(
                    n => DateTime.UtcNow - n.DiscoveredAt > TimeSpan.FromMilliseconds(2000));

                if (removed > 0)
                {
                    InvokeOnMainThread(() => Messages.Add($"Removed: {removed} node(s); connection with: {Nodes.Count} node(s)"));
                }
            }
            catch (Exception ex)
            {
                InvokeOnMainThread(() => Messages.Add(ex.Message));
            }
            finally
            {
                __nodessemaphore__.Release();
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

                Task.Run(async () =>
                {
                    await __nodessemaphore__.WaitAsync();
                    try
                    {
                        foreach (var node in Nodes)
                        {
                            try
                            {
                                var connection = TCPConnection.GetConnection(new ConnectionInfo(node.Address, 49999));
                                connection.SendObject("Chat", message);
                            }
                            catch (DuplicateConnectionException)
                            {
                                // todo: establishing connection simultaneously
                            }
                            catch (ConnectionSetupException)
                            {
                                node.Broken = true;
                            }
                        }

                        Nodes.RemoveAll(n => n.Broken);
                    }
                    catch (Exception ex)
                    {
                        InvokeOnMainThread(() => Messages.Add(ex.Message));
                    }
                    finally
                    {
                        __nodessemaphore__.Release();
                    }
                });

                _messageTextField.Text = string.Empty;
            }
        }
    }
}

