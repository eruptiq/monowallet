using MvvmCross.Navigation;
using Monowallet.UI.Core.ViewModels.Base;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Monowallet.UI.Core.ViewModels
{
    public class TabbedRootViewModel : ViewModelBase
    {
        private readonly IMvxNavigationService navigationService;
        public TabbedRootViewModel(IMvxNavigationService navigationService)
        {
            Icon = "TODO: icon";
            Title = "TODO: Tabbed Root";

            this.navigationService = navigationService;
        }

        public async override void ViewAppearing()
        {
            base.ViewAppearing();

            await MvxNotifyTask.Create(async () => await this.InitializeViewModels()).Task;
        }

        private async Task InitializeViewModels()
        {
            //await navigationService.Navigate<MenuViewModel>();
            await navigationService.Navigate<TransferViewModel>();
        }
    }
}