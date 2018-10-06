using Monowallet.UI.Core.Resources;
using Monowallet.UI.Core.ViewModels.Base;

namespace Monowallet.UI.Core.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        public AccountViewModel()
        {
            Icon = nameof(Texts.Account);
            Title = Texts.Account;
        }
    }
}
