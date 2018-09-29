using System;
using System.Collections.Generic;
using System.Text;
using Monowallet.UI.Core.Resources;
using Monowallet.UI.Core.ViewModels.Base;

namespace Monowallet.UI.Core.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        public override string Icon => nameof(Texts.Account);

        public override string Title => Texts.Account;
    }
}
