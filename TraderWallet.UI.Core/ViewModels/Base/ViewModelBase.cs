using Nethereum.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TraderWallet.UI.Core.ViewModels.Base
{
    public abstract class ViewModelBase : BaseViewModel
    {
        public abstract new string Icon { get; }

        public abstract new string Title { get; }
    }
}
