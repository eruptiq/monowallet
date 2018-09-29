using Nethereum.UI.Core.Model;
using Nethereum.UI.Core.Services;
using System.Collections.Generic;
using Monowallet.UI.Core.Resources;
using Monowallet.UI.Core.ViewModels;

namespace Monowallet.UI.Core.Services
{
    public class HomeMenuService : IHomeMenuService
    {
        public List<ShellMenuItem> GetMenuItems()
        {
            return new List<ShellMenuItem>
            {
                new ShellMenuItem {Title = Texts.Trade, PageViewModelType = typeof (TradeViewModel), Icon = "ethIcon.png"},
                new ShellMenuItem {Title = Texts.Transfer, PageViewModelType = typeof (TransferViewModel), Icon = "blog.png"},
                new ShellMenuItem {Title = Texts.Account, PageViewModelType = typeof (AccountViewModel), Icon = "ethIcon.png"},
            };
        }
    }
}