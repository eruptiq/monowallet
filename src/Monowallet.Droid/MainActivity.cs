using Android.App;
using Android.Widget;
using Android.OS;
using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;

namespace Monowallet.Droid
{
    [Activity(Label = "Monowallet.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        //int count = 1;
        private EditText _iptext;
        private EditText _log;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);
            _iptext = FindViewById<EditText>(Resource.Id.ip_text);
            _log = FindViewById<EditText>(Resource.Id.log);

            _iptext.Text = "192.168.0.30";

            button.Click += OnSendMessage;
        }

        private void OnSendMessage(object sender, EventArgs e)
        {
            SendMessage();
        }

        public void SendMessage(string stringToSend = "")
        {
            //If we have tried to send a zero length string we just return
            //if (stringToSend.Trim() == "") return;

            //We may or may not have entered some server connection information
            ConnectionInfo serverConnectionInfo = null;
            try
            {
                serverConnectionInfo = new ConnectionInfo(_iptext.Text, 5656);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Failed to parse the server IP and port. Please ensure it is correct and try again");
                return;
            }

            //We wrap everything we want to send in the ChatMessage class we created
            var msg = "Aloha";

            //We add our own message to the message history in case it gets relayed back to us
            //lock (lastPeerMessageDict) lastPeerMessageDict[NetworkComms.NetworkIdentifier] = chatMessage;

            //We write our own message to the chatBox
            //AppendLineToChatHistory(chatMessage.SourceName + " - " + chatMessage.Message);

            //Clear the input box text
            //ClearInputLine();

            //If we provided server information we send to the server first
            if (serverConnectionInfo != null)
            {
                //We perform the send within a try catch to ensure the application continues to run if there is a problem.
                try
                {
                    TCPConnection.GetConnection(serverConnectionInfo).SendObject("msg", msg);
                }
                catch (CommsException ex)
                {
                    _log.Text = _log.Text + "/r/n" + ex.Message;
                    System.Diagnostics.Debug.WriteLine("Error: A communication error occurred while trying to send message to " + serverConnectionInfo + ". Please check settings and try again.");
                }
                catch (Exception ex)
                {
                    _log.Text = _log.Text + "/r/n" + ex.Message;
                    System.Diagnostics.Debug.WriteLine("Error: A general error occurred while trying to send message to " + serverConnectionInfo + ". Please check settings and try again.");
                }
            }

            //If we have any other connections we now send the message to those as well
            //This ensures that if we are the server everyone who is connected to us gets our message
            //We want a list of all established connections not including the server if set
            //List<ConnectionInfo> otherConnectionInfos;
            //if (serverConnectionInfo != null)
            //    otherConnectionInfos = (from current in NetworkComms.AllConnectionInfo() where current.RemoteEndPoint != serverConnectionInfo.RemoteEndPoint select current).ToList();
            //else
            //otherConnectionInfos = NetworkComms.AllConnectionInfo();

            //foreach (ConnectionInfo info in otherConnectionInfos)
            //{
            //    //We perform the send within a try catch to ensure the application continues to run if there is a problem.
            //    try
            //    {
            //        if (ConnectionType == ConnectionType.TCP)
            //            TCPConnection.GetConnection(info).SendObject("ChatMessage", chatMessage);
            //        else if (ConnectionType == ConnectionType.UDP)
            //            UDPConnection.GetConnection(info, UDPOptions.None).SendObject("ChatMessage", chatMessage);
            //        else
            //            throw new Exception("An invalid connectionType is set.");
            //    }
            //    catch (CommsException) { AppendLineToChatHistory("Error: A communication error occurred while trying to send message to " + info + ". Please check settings and try again."); }
            //    catch (Exception) { AppendLineToChatHistory("Error: A general error occurred while trying to send message to " + info + ". Please check settings and try again."); }
            //}

            //return;
        }
    }
}

