using Monowallet.Core.Model;
using System.Collections.Generic;
using Monowallet.UI.Core.ViewModels;

namespace Monowallet.UI.Core.Services
{
    public interface IAccountSummaryViewModelMapperService
    {
        List<AccountSummaryViewModel> Map(List<AccountInfo> accountsInfo);
    }
}