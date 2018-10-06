namespace Monowallet.UI.Core.ViewModels.Base
{
    public interface IBaseViewModel
    {
        string Icon { get; set; }
        bool IsBusy { get; set; }
        string Title { get; set; }
    }
}