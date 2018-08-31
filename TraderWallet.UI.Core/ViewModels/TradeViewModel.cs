using System;
using System.Collections.Generic;
using System.Text;
using TraderWallet.UI.Core.Resources;
using TraderWallet.UI.Core.ViewModels.Base;

namespace TraderWallet.UI.Core.ViewModels
{
    public class TradeViewModel : ViewModelBase
    {
        public override string Icon => nameof(Texts.Trade);

        public override string Title => Texts.Trade;
    }
}
