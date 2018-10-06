using Monowallet.UI.Core.Model;
using Monowallet.UI.Core.Services;
using Monowallet.UI.Core.ViewModels.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using System.Collections.Generic;
using System.Windows.Input;

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
            Icon = "TODO: icon";
            Title = "Trader Wallet";

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

        public override void Start()
        {
            base.Start();
            MenuItems = homeMenuService.GetMenuItems();
            SelectedMenuItem = MenuItems[0];
        }
    }
}