using Nethereum.JsonRpc.Client;
using Nethereum.Web3;

namespace Monowallet.Core.Services
{
    public interface IWalletConfigurationService
    {
        string ClientUrl { get; set; }

        bool IsConfigured();
        Web3 GetReadOnlyWeb3();
    }
}