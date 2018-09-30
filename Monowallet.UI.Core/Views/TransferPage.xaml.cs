using Monowallet.UI.Core.Resources;
using Monowallet.UI.Core.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Xamarin.Forms.Xaml;

namespace Monowallet.UI.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxTabbedPagePresentation(Position = TabbedPosition.Tab, NoHistory = false, WrapInNavigationPage = true)]
    //[MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true)]
    public partial class TransferPage : MvxContentPage<TransferViewModel>
    {
        public TransferPage()
        {
            InitializeComponent();

        }

        protected override void OnViewModelSet()
        {
            this.ViewModel
                .ConfirmTransfer
                .RegisterHandler(
                    async interaction =>
                    {
                        var sendToken = await DisplayAlert(
                            Texts.ConfirmSendToken,
                             interaction.Input,
                            Texts.Yes,
                            Texts.No);

                        interaction.SetOutput(sendToken);
                    });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
