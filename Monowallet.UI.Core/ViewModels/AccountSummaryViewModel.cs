using Monowallet.UI.Core.ViewModels.Base;

namespace Monowallet.UI.Core.ViewModels
{
    public class AccountSummaryViewModel : ViewModelBase
    {
        public string Address { get; set; }

        public string ImgUrl { get; set; }

        public string BalanceSummary { get; set; }
    }
}