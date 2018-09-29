using System;
using System.Collections.Generic;
using System.Text;
using Monowallet.UI.Core.Resources;
using Monowallet.UI.Core.ViewModels.Base;

namespace Monowallet.UI.Core.ViewModels
{
    public class TradeViewModel : ViewModelBase
    {
        public override string Icon => nameof(Texts.Trade);

        public override string Title => Texts.Trade;
    }
}
