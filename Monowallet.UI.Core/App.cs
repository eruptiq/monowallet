using MvvmCross.IoC;
using MvvmCross.ViewModels;
using System.Reflection;
using Monowallet.UI.Core.ViewModels;
using Xamarin.Forms;
using Monowallet.UI.Core;

namespace Monowallet.UI.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            typeof(Nethereum.Wallet.Services.IEthWalletService).GetTypeInfo().Assembly.CreatableTypes()
           .EndingWith("Service")
           .AsInterfaces()
           .RegisterAsLazySingleton();

            //if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            //{
            //    Resources.AppResources.Culture = Mvx.Resolve<Services.ILocalizeService>().GetCurrentCultureInfo();
            //}

            // register the appstart object
            if (Device.RuntimePlatform == Device.macOS)
            {
                RegisterCustomAppStart<AppStart<MenuViewModel>>();
            }
            else
            {
                RegisterCustomAppStart<AppStart<RootViewModel>>();
                //RegisterCustomAppStart<AppStart<TransferViewModel>>();
                //RegisterCustomAppStart<AppStart<TradeViewModel>>();
            }
        }
    }
}
