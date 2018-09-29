using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using Nethereum.UI.Core.Model;
using Nethereum.UI.Core.Services;
//using Nethereum.UI.Core.Services;
using System.Collections.Generic;
using System.Windows.Input;
using Monowallet.UI.Core.ViewModels.Base;

namespace Monowallet.UI.Core.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IHomeMenuService homeMenuService;
        private readonly IMvxNavigationService navigationService;

        private readonly IMvxMessenger messenger;

        private ShellMenuItem selectedMenuItem;




        public MenuViewModel(IHomeMenuService homeMenuService, IMvxNavigationService navigationService)
        {
            //this.messenger = messenger;
            this.homeMenuService = homeMenuService;
            this.navigationService = navigationService;
        }

        public List<ShellMenuItem> MenuItems { get; set; }

        public ShellMenuItem SelectedMenuItem
        {
            get { return selectedMenuItem; }
            set
            {
                if (selectedMenuItem != value)
                {
                    selectedMenuItem = value;
                    RaisePropertyChanged();

                    NavigateToSelectedMenuCommand.Execute(null);
                }
            }
        }



        public ICommand NavigateToSelectedMenuCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                    {
                        var vmType = SelectedMenuItem.PageViewModelType;
                        await navigationService.Navigate(vmType);

                    },
                    () => SelectedMenuItem != null);
            }
        }

        public override string Icon => "TODO: icon";

        public override string Title => "Trader Wallet";

        public override void Start()
        {
            base.Start();
            MenuItems = homeMenuService.GetMenuItems();
            SelectedMenuItem = MenuItems[0];
        }
    }
}