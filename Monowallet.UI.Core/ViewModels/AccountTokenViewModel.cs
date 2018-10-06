using Monowallet.UI.Core.ViewModels.Base;

namespace Monowallet.UI.Core.ViewModels
{
    public class AccountTokenViewModel: ViewModelBase
    {
        public string Symbol { get; set; }

        public decimal Balance { get; set; }

        public string TokenName { get; set; }

        public string TokenImgUrl { get; set; }
    }
}