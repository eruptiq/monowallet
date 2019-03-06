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
    public class UdpBroadcastConnection : IUdpBroadcastService
    {
        private readonly IPAddress Localhost = IPAddress.Parse("127.0.0.1");
        private const int BroadcastPort = 10000;

        public async Task<string> ListenAsync()
        {
            var server = new UdpClient();
            server.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            server.Client.Bind(new IPEndPoint(IPAddress.Any, BroadcastPort));

            var request = await server.ReceiveAsync();
            var text = Encoding.ASCII.GetString(request.Buffer);
            server.Close();

            return text;
        }

        public void Send(string text)
        {
            var message = Encoding.UTF8.GetBytes(text);

            var client = new UdpClient();

            var unicastAdressesInfo =
                NetworkInterface.GetAllNetworkInterfaces()
                                .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                                .Where(a => !a.Address.Equals(Localhost))
                                .ToList();

            foreach (var addressInfo in unicastAdressesInfo)
            {
                var endpoint = new IPEndPoint(GetBroadcastAddress(addressInfo), BroadcastPort);
                client.EnableBroadcast = true;
                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                var result = client.Send(message, message.Length, endpoint);
            }

            client.Close();
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
    }
}
