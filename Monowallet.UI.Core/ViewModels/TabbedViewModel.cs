using MvvmCross.Navigation;
using Monowallet.UI.Core.ViewModels.Base;

namespace Monowallet.UI.Core.ViewModels
{
    public class TabbedViewModel : ViewModelBase
    {
        public override string Icon => "";

        public override string Title => "Tabbed"; //TODO:

        private readonly IMvxNavigationService navigationService;
        public TabbedViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
        /*
                public override void ViewAppearing()
                {
                    base.ViewAppearing();

                    MvxNotifyTask.Create(async () => await this.InitializeViewModels());
                }

                private async Task InitializeViewModels()
                {
                    //await navigationService.Navigate<MenuViewModel>();
                    await navigationService.Navigate<TransferViewModel>();
                }
                */
    }
}