using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Monowallet.UI.Core.ViewModels;

namespace Monowallet.UI.Core.Views
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Root, WrapInNavigationPage = true, NoHistory =true)]
    public partial class MasterDetailRootPage : MvxMasterDetailPage<MasterDetailRootViewModel>
    {
        public MasterDetailRootPage()
        {
            InitializeComponent();
        }
    }
}