using MvvmCross;
using MvvmCross.Forms.Platforms.Uap.Core;
using Monowallet.UI.Core;

namespace Monowallet.UWP
{
    public class Setup : MvxFormsWindowsSetup<UI.Core.App, FormsApp>
    {

        protected override void InitializeIoC()
        {
            base.InitializeIoC();
            //Monowallet.UI.Core.App.IsWindows10 = true;
            //Mvx.RegisterSingleton<IWalletConfigurationService>(new WalletConfigurationService());
        }
    }
}