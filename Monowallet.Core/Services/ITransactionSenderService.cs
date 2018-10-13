using System.Threading.Tasks;
using Nethereum.Contracts.CQS;
using Nethereum.RPC.Eth.DTOs;

namespace Monowallet.Core.Services
{
    public interface ITransactionSenderService
    {
        Task<string> SendTransactionAsync<TFunctionMessage>(TFunctionMessage functionMessage, string contractAddress) where TFunctionMessage : ContractMessage;
        Task<string> SendTransactionAsync(TransactionInput transactionInput);
    }
}