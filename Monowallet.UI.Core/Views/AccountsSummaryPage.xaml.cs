using Monowallet.UI.Core.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Monowallet.UI.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true)]
    public partial class AccountsSummaryPage : MvxContentPage<AccountsSummaryViewModel>
    {
        public AccountsSummaryPage()
        {
            InitializeComponent();
        }

    }
}
