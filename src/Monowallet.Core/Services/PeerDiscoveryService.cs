using System;
using System.Threading.Tasks;
using Monowallet.Core.Models;

namespace Monowallet.Core.Services
{
    public class PeerDiscoveryService : IDisposable
    {
        private readonly IUdpBroadcastService _udpBroadcastService;

        private Action<Broadcast> _peerDiscovered;

        public PeerDiscoveryService(IUdpBroadcastService udpBroadcastService)
        {
            _udpBroadcastService = udpBroadcastService;
        }

        public void StartListening(Action<Broadcast> peerDiscovered)
        {
            _peerDiscovered = peerDiscovered;

            Task.Run(async () =>
            {
                while (true)
                {
                    var broadcast = await _udpBroadcastService.ReceiveAsync();
                    if (broadcast.FromSelf)
                    {
                        continue;
                    }

                    _peerDiscovered?.Invoke(broadcast);
                }
            });
        }

        public void StartSending(string deviceName)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await _udpBroadcastService.SendAsync(deviceName);
                    await Task.Delay(499);
                }
            });
        }

        public void Dispose()
        {
            _udpBroadcastService.Dispose();
        }
    }
}
