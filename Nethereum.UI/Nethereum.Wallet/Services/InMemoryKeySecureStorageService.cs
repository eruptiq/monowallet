using System.Collections.Generic;

namespace Nethereum.Wallet.Services
{
    public class InMemoryKeySecureStorageService : IAccountKeySecureStorageService
    {
        private Dictionary<string, string> MockSecureStorage { get; set; }

        public InMemoryKeySecureStorageService()
        {
            MockSecureStorage = new Dictionary<string, string>();
            MockSecureStorage.Add("0xcfbe2714fcc08efe20aa401549753ef15af3a2cb".ToLower(), "0x2bd5ccdcac5288e3b5a61e804529d9deefab8a58bf5e33feef5b5f016c737fa2");
        }

        public string GetPrivateKey(string account)
        {
            if (MockSecureStorage.ContainsKey(account.ToLower()))
            {
                return MockSecureStorage[account.ToLower()];
            }

            return null;
        }
    }
}
