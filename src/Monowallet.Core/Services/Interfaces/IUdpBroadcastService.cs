using System;
using System.Threading.Tasks;
using Monowallet.Core.Models;

namespace Monowallet.Core.Services
{
    public interface IUdpBroadcastService : IDisposable
    {
        Task<Broadcast> ReceiveAsync();

        Task SendAsync(string data);
    }
}
