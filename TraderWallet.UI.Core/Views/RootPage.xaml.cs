using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using TraderWallet.UI.Core.ViewModels;

namespace TraderWallet.UI.Core.Views
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Root, WrapInNavigationPage = false)]
    public partial class RootPage : MvxMasterDetailPage<RootViewModel>
    {
        public RootPage()
        {
            InitializeComponent();
        }
    }
}