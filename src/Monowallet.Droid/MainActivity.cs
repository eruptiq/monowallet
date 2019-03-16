using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Monowallet.Core.Models;
using Monowallet.Core.Services;
using Monowallet.Droid.Chat;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using Xamarin.Essentials;

namespace Monowallet.Droid
{
    [Activity(Label = "Monowallet.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public EditText MessageTextView { get; private set; }

        public ObservableCollection<string> Messages { get; private set; }

        public List<Node> Nodes { get; set; } = new List<Node>(10);

        private SemaphoreSlim __nodessemaphore__ = new SemaphoreSlim(1);

        private System.Timers.Timer Timer = new System.Timers.Timer(2000);

        public UdpBroadcastConnection BroadcastConnection { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var sendButton = FindViewById<Button>(Resource.Id.send_button);
            MessageTextView = FindViewById<EditText>(Resource.Id.message_text_view);
            var messagesRecyclerView = FindViewById<RecyclerView>(Resource.Id.messages_recycler_view);

            Messages = new ObservableCollection<string>();
            messagesRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            messagesRecyclerView.SetAdapter(new MessagesAdapter(messagesRecyclerView, Messages));

            sendButton.Click += OnSend;

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
                        var existingNode = Nodes.FirstOrDefault(n => n.Address == node.Address);
                        if (existingNode == null)
                        {
                            var connection = TCPConnection.GetConnection(new ConnectionInfo(node.Address, 49999));
                            connection.SendObject("Handshake", DeviceInfo.Name);

                            Nodes.Add(node);

                            existingNode = node;
                        }

                        existingNode.DiscoveredAt = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        RunOnUiThread(() => Messages.Add($"Exception: {ex.Message}"));
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
                    RunOnUiThread(() => Messages.Add($"Removed: {removed} node(s); connection with: {Nodes.Count} node(s)"));
                }
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Messages.Add(ex.Message));
            }
            finally
            {
                __nodessemaphore__.Release();
            }
        }

        private void HandleHandshakeConnection(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            RunOnUiThread(() =>
                Messages.Add($"Connected to: {incomingObject} ({((IPEndPoint)connection.ConnectionInfo.RemoteEndPoint).Address})"));
        }

        private void HandleChatConnection(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            RunOnUiThread(() => Messages.Add($"{incomingObject}"));
        }

        private void OnSend(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageTextView.Text))
            {
                var message = MessageTextView.Text;

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
                            catch (ConnectionSetupException)
                            {
                                node.Broken = true;
                            }
                        }

                        Nodes.RemoveAll(n => n.Broken);
                    }
                    catch (Exception ex)
                    {
                        RunOnUiThread(() => Messages.Add(ex.Message));
                    }
                    finally
                    {
                        __nodessemaphore__.Release();
                    }
                });

                MessageTextView.Text = string.Empty;
            }
        }
    }
}

