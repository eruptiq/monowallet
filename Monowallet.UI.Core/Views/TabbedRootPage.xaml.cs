using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Monowallet.UI.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace Monowallet.UI.Core.Views
{
     [MvxTabbedPagePresentation(
        Position = TabbedPosition.Root, WrapInNavigationPage = false, NoHistory = true)]
    public partial class TabbedRootPage : MvxTabbedPage<TabbedRootViewModel>
    {
        public TabbedRootPage()
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