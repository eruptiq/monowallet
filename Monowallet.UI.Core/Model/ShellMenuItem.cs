using System;

namespace Monowallet.UI.Core.Model
{
    public class ShellMenuItem : BaseModel
    {
        public string Icon { get; set; }

        public Type PageViewModelType { get; set; }
    }
}