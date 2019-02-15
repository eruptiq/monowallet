// Copyright © 2019 Bosch SoftTec GmbH
using System;

using UIKit;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System.Net;

namespace Monowallet.iOS.Views
{
    public partial class MainViewController : UIViewController
    {
        public MainViewController(IntPtr handler) : base(handler)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {
                NetworkComms.AppendGlobalIncomingPacketHandler<string>("msg", HandleIncomingMessage);
                NetworkComms.AppendGlobalConnectionCloseHandler(HandleConnectionClosed);
                Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, 5656));

                System.Diagnostics.Debug.WriteLine($"Listening for incoming TCP connections on:");
                foreach (IPEndPoint listenEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                    System.Diagnostics.Debug.WriteLine(listenEndPoint.Address + ":" + listenEndPoint.Port);
            }
            catch (Exception ex)
            {
                log.Text = log.Text + "\r\n" + ex.Message;
            }
        }

        private void HandleIncomingMessage(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            InvokeOnMainThread(() =>
            {
                log.Text = log.Text + "\r\n" + incomingObject;
            });
        }

        private void HandleConnectionClosed(Connection connection)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

