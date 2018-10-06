namespace Monowallet.Wallet.Services
{
    public interface IAccountKeySecureStorageService
    {
        string GetPrivateKey(string account);
    }
}