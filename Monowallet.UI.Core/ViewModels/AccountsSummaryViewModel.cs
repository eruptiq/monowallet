using Monowallet.Core.Services;
using Monowallet.UI.Core.Resources;
using Monowallet.UI.Core.Services;
using Monowallet.UI.Core.ViewModels.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Monowallet.UI.Core.ViewModels
{
    public class AccountsSummaryViewModel : ViewModelBase
    {
        private readonly IAccountSummaryViewModelMapperService accountSummaryViewModelMapperService;
        private readonly IMvxNavigationService navigationService;
        private readonly IEthWalletService walletService;

        public AccountsSummaryViewModel(IEthWalletService walletService,
            IAccountSummaryViewModelMapperService accountSummaryViewModelMapperService,
            IMvxNavigationService navigationService)
        {
            this.walletService = walletService;
            this.accountSummaryViewModelMapperService = accountSummaryViewModelMapperService;
            this.navigationService = navigationService;
            AccountsSummary = new ObservableCollection<AccountSummaryViewModel>();
            Title = Texts.Accounts;
            Icon = "blog.png";
        }
        public ObservableCollection<AccountSummaryViewModel> AccountsSummary { get; set; }

        private AccountSummaryViewModel selectedAccount;
        public AccountSummaryViewModel SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedAccount, value);
                if (selectedAccount != null)
                    ShowSelectedAccountCommand.Execute(null);
            }
        }

        public ICommand RefreshItemsCommand
        {
            get { return new MvxAsyncCommand(async () => await LoadData(true)); }
        }

        public ICommand LoadItemsCommand
        {
            get { return new MvxAsyncCommand(async () => await LoadData()); }
        }


        public ICommand ShowSelectedAccountCommand
        {
            get
            {
                return new MvxAsyncCommand(async () => await navigationService.Navigate<AccountBalanceSummaryViewModel, string>(SelectedAccount.Address));
            }
        }


        public override async void Start()
        {
            await LoadData();
            base.Start();
        }

        private async Task LoadData(bool forceRefresh = false)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;
            try
            {
                var walletSummary = await walletService.GetWalletSummary(forceRefresh);
                AccountsSummary.Clear();
                foreach (var accountSummary in accountSummaryViewModelMapperService.Map(walletSummary.AccountsInfo))
                {
                    AccountsSummary.Add(accountSummary);
                }
            }
            catch
            {
                error = true;
            }

            if (error)
            {
                var page = new ContentPage();
                await page.DisplayAlert("Error", "Unable to load accounts", "OK");
            }

            IsBusy = false;
        }
    }
}