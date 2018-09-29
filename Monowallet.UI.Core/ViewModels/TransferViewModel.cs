using MvvmCross.Commands;
using MvvmCross.Navigation;

using Nethereum.Signer;
using Nethereum.StandardTokenEIP20.CQS;
using Nethereum.Util;
using Nethereum.Wallet.Model;
using Nethereum.Wallet.Services;
using Nethereum.Web3;
using ReactiveUI;

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Monowallet.UI.Core.Resources;
using Monowallet.UI.Core.ViewModels.Base;

using Xamarin.Forms;

namespace Monowallet.UI.Core.ViewModels
{
    public class TransferViewModel : ViewModelBase
    {
        public override string Icon => nameof(Texts.Transfer);

        public override string Title => Texts.Transfer;

        #region Nethereum
        private readonly IAccountRegistryService accountRegistryService;

        protected readonly Interaction<string, bool> confirmTransfer;
        private readonly IMvxNavigationService navigationService;
        private readonly ITokenRegistryService tokenRegistryService;
        private readonly ITransactionHistoryService transactionHistoryService;
        private readonly ITransactionSenderService transactionSenderService;
        private readonly IEthWalletService walletService;

        public TransferViewModel(IEthWalletService walletService,
            ITokenRegistryService tokenRegistryService,
            IAccountRegistryService accountRegistryService,
            ITransactionSenderService transactionSenderService,
            IMvxNavigationService navigationService,
            ITransactionHistoryService transactionHistoryService
        )
        {
            this.walletService = walletService;
            this.tokenRegistryService = tokenRegistryService;
            this.accountRegistryService = accountRegistryService;
            this.transactionSenderService = transactionSenderService;
            this.navigationService = navigationService;
            this.transactionHistoryService = transactionHistoryService;

            RegisteredAccounts = new ObservableCollection<string>();
            RegisteredTokens = new ObservableCollection<ContractToken>();
            confirmTransfer = new Interaction<string, bool>();

            SelectedToken = -1;
            SelectedAccountFrom = -1;
            GasPrice = Web3.Convert.FromWei(TransactionBase.DEFAULT_GAS_PRICE, UnitConversion.EthUnit.Gwei);


            this.WhenAnyValue(x => x.SelectedToken,
                x => x.SelectedAccountFrom, (selectedToken, selectedAccountFrom) => selectedToken != -1 &&
                                                                                    selectedAccountFrom != -1).Subscribe(async _ =>
                await RefreshTokenBalanceAsync());

            var canExecuteTransaction = this.WhenAnyValue(
                x => x.ToAddress,
                x => x.Amount,
                x => x.SelectedToken,
                x => x.SelectedAccountFrom,
                (addressTo, amount, selectedToken, selectedAccountFrom) =>
                    !string.IsNullOrEmpty(ToAddress) && //TODO: valid address
                    amount > 0 &&
                    selectedToken != -1 &&
                    selectedAccountFrom != -1);

            executeTransactionCommand = ReactiveCommand.CreateFromTask(ExecuteAsync, canExecuteTransaction);
        }

        public ObservableCollection<string> RegisteredAccounts { get; set; }

        public ObservableCollection<ContractToken> RegisteredTokens { get; set; }

        public int? SelectedToken { get; set; }

        public int? SelectedAccountFrom { get; set; }

        public Interaction<string, bool> ConfirmTransfer => confirmTransfer;

        public ICommand RefreshTokenBalanceCommand => new MvxAsyncCommand(async () => await RefreshTokenBalanceAsync());

        public string Symbol { get; set; }

        public string ToAddress { get; set; }

        public decimal Amount { get; set; }

        public ulong? Gas { get; set; }

        public decimal GasPrice { get; set; }

        public decimal CurrentTokenBalance { get; set; }

        protected ReactiveCommand<Unit, Unit> executeTransactionCommand;
        public ReactiveCommand<Unit, Unit> ExecuteTransactionCommand => this.executeTransactionCommand;

        public override async Task Initialize()
        {
            await base.Initialize();
            await LoadDataAsync();
        }

        private async Task RefreshTokenBalanceAsync()
        {
            if (SelectedToken != -1 && SelectedAccountFrom != -1)
                CurrentTokenBalance = await walletService.GetTokenBalance(RegisteredTokens[SelectedToken.Value],
                    RegisteredAccounts[SelectedAccountFrom.Value]);
        }

        private async Task LoadDataAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;
            try
            {
                RegisteredAccounts.Clear();
                accountRegistryService.GetRegisteredAccounts().ForEach(x => RegisteredAccounts.Add(x));

                tokenRegistryService.GetRegisteredTokens().ForEach(x => RegisteredTokens.Add(x));
            }
            catch
            {
                error = true;
            }

            if (error)
            {
                var page = new ContentPage();
                var result = page.DisplayAlert("Error", "Unable to load registered accounts or tokens", "OK");
            }

            IsBusy = false;
        }


        public string GetConfirmationMessage()
        {
            return
                $"Are you sure you want to make this transfer: \n\r To: {ToAddress} \n\r Token Amount: {Amount}";
        }

        private async Task ExecuteAsync()
        {

            var confirmed = await confirmTransfer.Handle(GetConfirmationMessage());
            if (confirmed)
            {
                var error = false;
                var exceptionMessage = string.Empty;

                var currentToken = RegisteredTokens[SelectedToken.Value];
                var currentAddres = RegisteredAccounts[SelectedAccountFrom.Value];

                try
                {
                    var transferFunction = new TransferFunction
                    {
                        Value = Web3.Convert.ToWei(Amount, currentToken.NumberOfDecimalPlaces),
                        FromAddress = currentAddres,
                        Gas = Gas,
                        GasPrice = Web3.Convert.ToWei(GasPrice, UnitConversion.EthUnit.Gwei),
                        To = ToAddress
                    };

                    var transactionHash = await
                        transactionSenderService.SendTransactionAsync(transferFunction, currentToken.Address);
                    await transactionHistoryService.AddTransaction(transactionHash);

                }
                catch (Exception ex)
                {
                    error = true;
                    exceptionMessage = ex.Message;
                }

                if (error)
                {
                    //todo rxui
                    var page = new ContentPage();
                    await page.DisplayAlert("Error", "Unable to transfer token :" + exceptionMessage, "OK");
                }

                IsBusy = false;
            }
        }
        #endregion
    }
}
