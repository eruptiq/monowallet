using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Monowallet.Core.Models;
using Monowallet.Core.Services.Interfaces;

namespace Monowallet.Core.Services
{
    public class UdpBroadcastConnection : IUdpBroadcastConnection, IDisposable
    {
        private readonly string Name;
        private readonly IPAddress Localhost = IPAddress.Parse("127.0.0.1");
        private const int BroadcastPort = 50001;

        public List<UnicastIPAddressInformation> UnicastAdressesInfo { get; private set; }
        public List<string> Adresses { get; private set; }
        public List<IPEndPoint> BroadcastAddresses { get; private set; }

        public UdpClient Listener { get; private set; }
        public UdpClient Sender { get; private set; }

        public UdpBroadcastConnection(string name)
        {
            Name = name;

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

        public async Task<Node> ListenAsync()
        {
            var request = await Listener.ReceiveAsync();
            var ip = request.RemoteEndPoint.Address.ToString();

            var name = "";
            if (request.Buffer?.Length > 0)
            {
                name = Encoding.UTF8.GetString(request.Buffer);
            }

            return new Node
            {
                Address = ip,
                IsSelf = Adresses.Contains(ip),
                Name = name
            };
        }

        public async Task SendAsync()
        {
            var bytes = Encoding.UTF8.GetBytes(Name);

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
