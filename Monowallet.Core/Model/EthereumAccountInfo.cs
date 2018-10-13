using System;

namespace Monowallet.Core.Model
{
    public class EthereumAccountInfo : IAccountInfo
    {

        public EthereumAccountInfo()
        {
        }

        public EthereumAccountInfo(string address = null, string uniqueName = null)
        {
            if (string.IsNullOrWhiteSpace(address) && string.IsNullOrWhiteSpace(uniqueName))
            {
                throw new MonowalletException(MonowalletExceptionType.AllAccountInfoIdParametersAreVoid, nameof(address));
            }

            Address = address;
            UniqueName = uniqueName ?? address;
        }

        public string Address { get; }
        public string UniqueName { get; }
        public string GetUniquePublicKey()
        {
            return UniqueName ?? Address;
        }
    }
}
