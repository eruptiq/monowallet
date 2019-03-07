using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Monowallet.Core.Services;
using Monowallet.Droid.Chat;

namespace Monowallet.Droid
{
    [Activity(Label = "Monowallet.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public EditText MessageTextView { get; private set; }

        public ObservableCollection<string> Messages { get; private set; }

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
                    var message = await BroadcastConnection.ListenAsync();
                    RunOnUiThread(() => Messages.Add(message));
                }
            });
        }

        private void OnSend(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageTextView.Text))
            {
                BroadcastConnection.Send(MessageTextView.Text);
                MessageTextView.Text = string.Empty;
            }
        }
    }
}

