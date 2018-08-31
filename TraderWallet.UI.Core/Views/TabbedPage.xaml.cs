using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using TraderWallet.UI.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace TraderWallet.UI.Core.Views
{
    [MvxMasterDetailPagePresentation(
        Position = MasterDetailPosition.Detail, WrapInNavigationPage = false, NoHistory = true)]
    public partial class TabbedPage : MvxTabbedPage<TabbedViewModel>
    {
        public TabbedPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (Parent is MvxMasterDetailPage<RootViewModel> master)
            {
                master.MasterBehavior = MasterBehavior.Popover;
                master.IsPresented = false;
            }
            On<Windows>().SetToolbarPlacement(
                Xamarin.Forms.PlatformConfiguration.WindowsSpecific.ToolbarPlacement.Bottom);
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(
                Xamarin.Forms.PlatformConfiguration.AndroidSpecific.ToolbarPlacement.Bottom);
            base.OnAppearing();
        }
    }
}