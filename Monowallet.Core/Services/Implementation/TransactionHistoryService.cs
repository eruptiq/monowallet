using Monowallet.Core.Model;
using Monowallet.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Monowallet.Wallet.Services
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly IWalletConfigurationService _walletConfigurationService;
        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
        private readonly TimeSpan updateInterval = TimeSpan.FromMilliseconds(2000);
        private IDisposable timer;
        private readonly object receiptsCheckLock = new object();

        public TransactionHistoryService(IWalletConfigurationService walletConfigurationService)
        {
            _walletConfigurationService = walletConfigurationService;
            timer = new Timer(async _ => await CheckReceiptsAsync(), null, updateInterval, updateInterval);
        }

        public async Task AddTransaction(string transactionHash)
        {

            var web3 = _walletConfigurationService.GetReadOnlyWeb3();
            var transactionViewModel = new TransactionModel();
            var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transactionHash);
            transactionViewModel.Initialise(transaction);
            transactionViewModel.Status = TransactionModel.STATUS_INPROGRESS;
            lock (receiptsCheckLock)
            {
                Transactions.Add(transactionViewModel);
            }
        }

        public async Task CheckReceiptsAsync()
        {
            IEnumerable<TransactionModel> transactionsInProgress = new List<TransactionModel>();

            lock (receiptsCheckLock)
            {
                transactionsInProgress =
                    Transactions.Where(x => x.Status == TransactionModel.STATUS_INPROGRESS);
            }

            var web3 = _walletConfigurationService.GetReadOnlyWeb3();

            foreach (var transaction in transactionsInProgress)
            {
                var receipt = await web3.Eth.Transactions.GetTransactionReceipt
                    .SendRequestAsync(transaction.TransactionHash);

                if (receipt != null)
                {
                    transaction.BlockHash = receipt.BlockHash;
                    transaction.Status = TransactionModel.STATUS_COMPLETED;
                }
            }
        }
    }
}
