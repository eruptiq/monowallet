namespace Monowallet.Core.Services
{
    public interface IAccountKeySecureStorageService
    {
        string GetPrivateKey(string account);
    }
}