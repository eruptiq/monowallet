using Monowallet.UI.Core.ViewModels;
using System.Collections.Generic;
using Monowallet.Wallet.Model;

namespace Monowallet.UI.Core.Services
{
    public interface IAccountTokenViewModelMapperService
    {
        List<AccountTokenViewModel> Map(List<AccountToken> accountTokens);
        List<AccountTokenViewModel> Map(EthAccountToken ethAccountToken, List<AccountToken> accountTokens);
    }
}