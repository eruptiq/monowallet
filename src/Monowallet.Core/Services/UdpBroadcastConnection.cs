using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Monowallet.Core.Services.Interfaces;

namespace Monowallet.Core.Services
{
    public class UdpBroadcastConnection : IUdpBroadcastConnection, IDisposable
    {
        private readonly IPAddress Localhost = IPAddress.Parse("127.0.0.1");
        private const int BroadcastPort = 50001;

        public UdpClient Listener { get; private set; }
        public UdpClient Sender { get; private set; }

        public UdpBroadcastConnection()
        {
            Listener = new UdpClient();
            Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Listener.Client.Bind(new IPEndPoint(IPAddress.Any, BroadcastPort));

            Sender = new UdpClient
            {
                EnableBroadcast = true
            };

            Sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public async Task<string> ListenAsync()
        {
            var request = await Listener.ReceiveAsync();
            var text = Encoding.ASCII.GetString(request.Buffer);

            return text;
        }

        public void Send(string text)
        {
            var message = Encoding.UTF8.GetBytes(text);

            var unicastAdressesInfo =
                NetworkInterface.GetAllNetworkInterfaces()
                                .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                                .Where(a => !a.Address.Equals(Localhost))
                                .ToList();

            foreach (var addressInfo in unicastAdressesInfo)
            {
                var endpoint = new IPEndPoint(GetBroadcastAddress(addressInfo), BroadcastPort);

                Sender.Send(message, message.Length, endpoint);
            }
        }

        public IPAddress GetBroadcastAddress(UnicastIPAddressInformation addressInformation)
        {
            var ipAdressBytes = addressInformation.Address.GetAddressBytes();
            var subnetMaskBytes = addressInformation.IPv4Mask.GetAddressBytes();

            var broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }

            return new IPAddress(broadcastAddress);
        }

        public void Dispose()
        {
            Listener.Close();
            Listener = null;

            Sender.Close();
            Sender = null;
        }
    }
}
