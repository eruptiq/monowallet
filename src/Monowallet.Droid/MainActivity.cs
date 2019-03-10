using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Monowallet.Core.Models;
using Monowallet.Core.Services;
using Monowallet.Droid.Chat;

namespace Monowallet.Droid
{
    [Activity(Label = "Monowallet.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public EditText MessageTextView { get; private set; }

        public ObservableCollection<string> Messages { get; private set; }

        public List<Node> Nodes { get; set; } = new List<Node>(100);

        private SemaphoreSlim __nodessemaphore__ = new SemaphoreSlim(1);

        public UdpBroadcastConnection BroadcastConnection { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var sendButton = FindViewById<Button>(Resource.Id.send_button);
            MessageTextView = FindViewById<EditText>(Resource.Id.message_text_view);
            var messagesRecyclerView = FindViewById<RecyclerView>(Resource.Id.messages_recycler_view);

            Messages = new ObservableCollection<string>();
            messagesRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            messagesRecyclerView.SetAdapter(new MessagesAdapter(messagesRecyclerView, Messages));

            sendButton.Click += OnSend;

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
                        if (!Nodes.Any(n => n.Address == node.Address))
                        {
                            Nodes.Add(node);
                        }
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
        }

        private void OnSend(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageTextView.Text))
            {
                MessageTextView.Text = string.Empty;
            }
        }
    }
}

