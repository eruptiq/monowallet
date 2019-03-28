using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
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
        private EditText MessageTextView { get; set; }
        private System.Timers.Timer Timer = new System.Timers.Timer(2000);
        private ConcurrentDictionary<string, Node> Nodes { get; set; } = new ConcurrentDictionary<string, Node>();

        public ObservableCollection<string> Messages { get; private set; }
        public PeerDiscoveryService PeerDiscoveryService { get; private set; }

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

                    RunOnUiThread(() => Messages.Add($"Discovered: {existingNode.Name}"));
                }

                existingNode.DiscoveredAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Messages.Add($"Exception: {ex.Message}"));
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
                        RunOnUiThread(() => Messages.Add($"Disconnected node: {removedNode.Name}"));
                    }
                }
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Messages.Add($"Exception: {ex.Message}"));
            }
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
                            RunOnUiThread(() => Messages.Add($"Disconnected node: {removedNode.Name}"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Messages.Add(ex.Message);
                }

                MessageTextView.Text = string.Empty;
            }
        }
    }
}

