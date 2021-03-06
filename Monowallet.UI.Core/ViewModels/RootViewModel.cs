﻿using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using Monowallet.UI.Core.ViewModels.Base;

namespace Monowallet.UI.Core.ViewModels
{
    public class RootViewModel : ViewModelBase
    {
        private readonly IMvxNavigationService navigationService;
        public RootViewModel(IMvxNavigationService navigationService)
        {
            Icon = "TODO: icon";
            Title = "TODO: Root";

            this.navigationService = navigationService;
        }

        public override void Start()
        {
            base.Start();
            //MvxNotifyTask.Create(async () => await this.InitializeViewModels());
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            MvxNotifyTask.Create(async () => await this.InitializeViewModels());
        }

        private async Task InitializeViewModels()
        {
            bool isDesktop = false;
            if (isDesktop)
            {
                //await navigationService.Navigate<MenuViewModel>();
                //await navigationService.Navigate<TradeViewModel>();
                await navigationService.Navigate<MasterDetailRootViewModel>();
            }
            else
            {
                /*
                if (Xamarin.Forms.Application.Current.MainPage is MasterDetailPage masterDetailPage)
                {
                    masterDetailPage.MasterBehavior = MasterBehavior.Popover;
                    masterDetailPage.IsPresented = false;
                }
                else if (Xamarin.Forms.Application.Current.MainPage is NavigationPage navigationPage && navigationPage.CurrentPage is MasterDetailPage nestedMasterDetail)
                {
                    nestedMasterDetail.MasterBehavior = MasterBehavior.Popover;
                    nestedMasterDetail.IsPresented = false;
                }
                */

                await navigationService.Navigate<TabbedRootViewModel>();
            }
        }
    }
}