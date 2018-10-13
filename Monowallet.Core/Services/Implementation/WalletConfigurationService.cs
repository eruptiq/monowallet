using Nethereum.Web3;

namespace Monowallet.Core.Services.Implementation
{
    public class WalletConfigurationService : IWalletConfigurationService
    {
        //defaulting to the rinkeby testnet

        public string ClientUrl { get; set; } = "https://rinkeby.infura.io/";
        public bool IsConfigured()
        {
            return !string.IsNullOrEmpty(ClientUrl);
        }

        public Web3 GetReadOnlyWeb3()
        {
            return new Web3(ClientUrl);
        }
    }
}
