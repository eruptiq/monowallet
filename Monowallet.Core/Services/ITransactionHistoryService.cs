using Monowallet.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monowallet.Core.Services
{
    public interface ITransactionHistoryService
    {
        Task AddTransaction(string transactionHash);
        List<TransactionModel> Transactions { get; set; }
    }
}
