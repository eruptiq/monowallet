using Monowallet.Core.Model;
using Monowallet.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using MvvmCross.Commands;
using Monowallet.UI.Core.Services;
using Xamarin.Forms;
using Monowallet.UI.Core.ViewModels.Base;
using Monowallet.UI.Core.Resources;

namespace Monowallet.UI.Core.ViewModels
{
    public class AccountBalanceSummaryViewModel : ViewModelBase<string>
    {
        private readonly IEthWalletService walletService;
        private readonly IAccountTokenViewModelMapperService accountTokenViewModelMapperService;

        private AccountTokenViewModel selectedToken;

        private ObservableCollection<AccountTokenViewModel> tokenBalanceSummary;

        public AccountBalanceSummaryViewModel(IEthWalletService walletService,
            IAccountTokenViewModelMapperService accountTokenViewModelMapperService
        )
        {
            Title = Texts.AccountBalance;
            Icon = "blog.png";

            this.walletService = walletService;
            this.accountTokenViewModelMapperService = accountTokenViewModelMapperService;
            tokenBalanceSummary = new ObservableCollection<AccountTokenViewModel>();
        }



        public ObservableCollection<AccountTokenViewModel> TokensBalanceSummary { get; set; }

        public AccountTokenViewModel SelectedToken { get; set; }

        public ICommand LoadItemsCommand
        {
            get { return new MvxAsyncCommand(() => LoadData()); }
        }

        public ICommand RefreshItemsCommand
        {
            get { return new MvxAsyncCommand(() => LoadData(true)); }
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            if (TokensBalanceSummary == null || TokensBalanceSummary.Count == 0)
            {
                await LoadData();
            }

            Title = Parameter;
        }


        private async Task LoadData(bool forceRefresh = false)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;
            try
            {
                var walletSummary = await walletService.GetAccountInfo(Parameter, forceRefresh);
                
                TokensBalanceSummary.Clear();
                var ethToken = new EthToken();
                TokensBalanceSummary.Add(new AccountTokenViewModel()
                {
                    Balance = walletSummary.Eth.Balance,
                    Symbol = walletSummary.Eth.Symbol,
                    TokenImgUrl = ethToken.ImgUrl,
                    TokenName = ethToken.Name
                });

                foreach (var accountSummary in accountTokenViewModelMapperService.Map(
                    walletSummary.AccountTokens))
                {
                    TokensBalanceSummary.Add(accountSummary);
                }
            }
            catch
            {
                error = true;
            }

            if (error)
            {
                var page = new ContentPage();
                await page.DisplayAlert("Error", "Unable to load token summary", "OK");
            }

            IsBusy = false;
        }
    }
}