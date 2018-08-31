using MvvmCross;
using MvvmCross.Forms.Platforms.Uap.Core;
using TraderWallet.UI.Core;

namespace TraderWallet.UWP
{
    public class Setup : MvxFormsWindowsSetup<UI.Core.App, FormsApp>
    {

        protected override void InitializeIoC()
        {
            base.InitializeIoC();
            //TraderWallet.UI.Core.App.IsWindows10 = true;
            //Mvx.RegisterSingleton<IWalletConfigurationService>(new WalletConfigurationService());
        }
    }
}