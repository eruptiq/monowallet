using MvvmCross.Navigation;
using Monowallet.UI.Core.ViewModels.Base;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Monowallet.UI.Core.ViewModels
{
    public class TabbedRootViewModel : ViewModelBase
    {
        public override string Icon => "";

        public override string Title => "Tabbed"; //TODO:

        private readonly IMvxNavigationService navigationService;
        public TabbedRootViewModel(IMvxNavigationService navigationService)
        {
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