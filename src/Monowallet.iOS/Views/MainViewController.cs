// Copyright © 2019 Bosch SoftTec GmbH
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UIKit;

namespace Monowallet.iOS.Views
{
    public partial class MainViewController : UIViewController
    {
        const int Port = 4321;
        const string IP = "";

        public MainViewController(IntPtr handler) : base(handler)
        {
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {
                var localAddress = IPAddress.Parse(IP);
                var listener = new TcpListener(localAddress, Port);
                listener.Start();

                while (true)
                {
                    var tcpClient = await listener.AcceptTcpClientAsync();
                    using (var networkStream = tcpClient.GetStream())
                    {
                        var buffer = new byte[tcpClient.ReceiveBufferSize];
                        var bytesRead = networkStream.Read(buffer, 0, tcpClient.ReceiveBufferSize);
                        var dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        HandleIncomingMessage(dataReceived);
                    }

                    tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                log.Text = log.Text + "\r\n" + ex.Message;
            }
        }

        private void HandleIncomingMessage(string incomingObject)
        {
            InvokeOnMainThread(() =>
            {
                log.Text = log.Text + "\r\n" + incomingObject;
            });
        }
    }
}

