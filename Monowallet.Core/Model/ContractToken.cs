namespace Monowallet.Core.Model
{
    public class ContractToken: Token
    {
        /// <summary>
        /// Token contract address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// True if it's main network currency, like Ethereum and not token.
        /// </summary>
        public bool IsMainCurrency => string.IsNullOrWhiteSpace(Address);
    }
}