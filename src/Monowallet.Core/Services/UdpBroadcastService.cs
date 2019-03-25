using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Monowallet.Core.Models;

namespace Monowallet.Core.Services
{
    public class UdpBroadcastService : IUdpBroadcastService
    {
        private readonly IPAddress Localhost = IPAddress.Parse("127.0.0.1");
        private const int BroadcastPort = 50001;

        private readonly List<UnicastIPAddressInformation> UnicastAdressesInfo;
        private readonly List<string> Adresses;
        private readonly List<IPEndPoint> BroadcastAddresses;

        private readonly UdpClient Listener;
        private readonly UdpClient Sender;

        public UdpBroadcastService()
        {
            UnicastAdressesInfo =
                NetworkInterface.GetAllNetworkInterfaces()
                                .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                                .Where(a => !a.Address.Equals(Localhost))
                                .ToList();

            Adresses = UnicastAdressesInfo.Select(a => a.Address.ToString()).ToList();

            BroadcastAddresses = UnicastAdressesInfo
                .Select(a => new IPEndPoint(GetBroadcastAddress(a), BroadcastPort))
                .ToList();

            Listener = new UdpClient();
            Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Listener.Client.Bind(new IPEndPoint(IPAddress.Any, BroadcastPort));

            Sender = new UdpClient
            {
                EnableBroadcast = true
            };

            Sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public async Task<Broadcast> ReceiveAsync()
        {
            var request = await Listener.ReceiveAsync();
            var ipAddress = request.RemoteEndPoint.Address.ToString();

            var data = "";
            if (request.Buffer?.Length > 0)
            {
                data = Encoding.UTF8.GetString(request.Buffer);
            }

            return new Broadcast(ipAddress, data, Adresses.Contains(ipAddress));
        }

        public async Task SendAsync(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);

            foreach (var endpoint in BroadcastAddresses)
            {
                try
                {
                    await Sender.SendAsync(bytes, bytes.Length, endpoint);
                }
                catch (SocketException)
                {
                    // some shit happened
                }
            }
        }

        public void Dispose()
        {
            Listener.Close();
            Sender.Close();
        }

        private IPAddress GetBroadcastAddress(UnicastIPAddressInformation addressInformation)
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
