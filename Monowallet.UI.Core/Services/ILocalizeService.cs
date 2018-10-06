using System.Globalization;

namespace Monowallet.UI.Core.Services
{

    public interface ILocalizeService
    {
        CultureInfo GetCurrentCultureInfo();
    }    
}
