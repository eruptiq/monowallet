using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Monowallet.UI.Core.ViewModels;

namespace Monowallet.UI.Core.Views
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