using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using TraderWallet.UI.Core.Resources;
using TraderWallet.UI.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace TraderWallet.UI.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true)]
    public partial class TradePage : MvxContentPage<TradeViewModel>
    {
        public TradePage()
        {
            InitializeComponent();

        }

        protected override void OnViewModelSet()
        {
            //this.ViewModel
            //    .ConfirmTransfer
            //    .RegisterHandler(
            //        async interaction =>
            //        {
            //            var sendToken = await DisplayAlert(
            //                Texts.ConfirmSendToken,
            //                 interaction.Input,
            //                Texts.Yes,
            //                Texts.No);

            //            interaction.SetOutput(sendToken);
            //        });
        }
    }
}
