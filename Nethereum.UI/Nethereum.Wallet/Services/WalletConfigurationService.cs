namespace Nethereum.Wallet.Services
{
    public class WalletConfigurationService : IWalletConfigurationService
    {
        //defaulting to the rinkeby testnet

        public string ClientUrl { get; set; } = "https://ropsten.infura.io/";
        public bool IsConfigured()
        {
            return !string.IsNullOrEmpty(ClientUrl);
        }

        public Web3.Web3 GetReadOnlyWeb3()
        {
            return new Web3.Web3(ClientUrl);
        }
    }
}
