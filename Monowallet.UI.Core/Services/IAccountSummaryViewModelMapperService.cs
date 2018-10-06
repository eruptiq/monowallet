using System.Collections.Generic;
using Monowallet.UI.Core.ViewModels;
using Monowallet.Wallet.Model;

namespace Monowallet.UI.Core.Services
{
    public interface IAccountSummaryViewModelMapperService
    {
        List<AccountSummaryViewModel> Map(List<AccountInfo> accountsInfo);
    }
}