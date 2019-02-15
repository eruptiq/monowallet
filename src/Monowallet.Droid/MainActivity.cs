using System;
using System.Net.Sockets;
using System.Text;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Monowallet.Droid
{
    [Activity(Label = "Monowallet.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private EditText _iptext;
        private EditText _log;

        const int Port = 4321;
        const string IP = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.myButton);
            _iptext = FindViewById<EditText>(Resource.Id.ip_text);
            _log = FindViewById<EditText>(Resource.Id.log);

            _iptext.Text = IP;

            button.Click += OnSendMessage;
        }

        private void OnSendMessage(object sender, EventArgs e)
        {
            SendMessage("Aloha");
        }

        public void SendMessage(string textToSend = "")
        {
            try
            {
                var tcpClient = new TcpClient(IP, Port);
                using (var networkStream = tcpClient.GetStream())
                {
                    var bytesToSend = Encoding.ASCII.GetBytes(textToSend);
                    networkStream.Write(bytesToSend, 0, bytesToSend.Length);
                }

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                _log.Text = _log.Text + "\r\n" + ex.Message;
            }
        }
    }
}

