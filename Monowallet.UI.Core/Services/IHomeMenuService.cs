using Monowallet.UI.Core.Model;
using System.Collections.Generic;

namespace Monowallet.UI.Core.Services
{
    public interface IHomeMenuService
    {
        List<ShellMenuItem> GetMenuItems();
    }
}