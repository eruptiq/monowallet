using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Monowallet.Core.Model
{
    public class TransactionModel
    {
        public const string STATUS_INPROGRESS = "Pending";
        public const string STATUS_COMPLETED = "Completed";

        public void Initialise(Nethereum.RPC.Eth.DTOs.Transaction transaction)
        {
            this.TransactionHash = transaction.TransactionHash;
            this.BlockHash = transaction.BlockHash;
            this.Nonce = (ulong)transaction.Nonce.Value;
            this.From = transaction.From;
            this.To = transaction.To;
            this.Gas = (ulong)transaction.Gas.Value;
            this.GasPrice = (ulong)transaction.GasPrice.Value;
            this.Data = transaction.Input;

            if (transaction.Value != null) this.Amount = Web3.Convert.FromWei(transaction.Value.Value);
        }

        public string BlockHash { get; set; }

        public string TransactionHash { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public decimal Amount { get; set; }

        public ulong? Gas { get; set; }

        public string Data { get; set; }

        public ulong? GasPrice { get; set; }

        public ulong Nonce { get; set; }

        public string Status { get; set; }

    }
}
