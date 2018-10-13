using Monowallet.Core.Services;
using Nethereum.Contracts.CQS;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Threading.Tasks;

namespace Monowallet.Core.Services.Implementation
{
    public class TransactionSenderService : ITransactionSenderService
    {
        private readonly IWalletConfigurationService walletConfigurationService;
        private readonly IAccountKeySecureStorageService accountKeySecureStorageService;

        public TransactionSenderService(IWalletConfigurationService walletConfigurationService,
            IAccountKeySecureStorageService accountKeySecureStorageService)
        {
            this.walletConfigurationService = walletConfigurationService;
            this.accountKeySecureStorageService = accountKeySecureStorageService;
        }

        public Task<string> SendTransactionAsync<TFunctionMessage>(TFunctionMessage functionMessage, string contractAddress) where TFunctionMessage : ContractMessage
        {
            var web3 = GetWeb3(functionMessage.FromAddress);
            return web3.Eth.GetContractHandler(contractAddress).SendRequestAsync(functionMessage);
        }

        public Task<string> SendTransactionAsync(TransactionInput transactionInput)
        {
            var web3 = GetWeb3(transactionInput.From);
            return web3.TransactionManager.SendTransactionAsync(transactionInput);
        }

        private Web3 GetWeb3(string accountAddress)
        {
            var privateKey = accountKeySecureStorageService.GetPrivateKey(accountAddress);
            if (privateKey == null) throw new Exception("Account not configured for signing transactions");
            //todo chainId
            return new Web3(new Account(privateKey), walletConfigurationService.ClientUrl);
        }
    }
}