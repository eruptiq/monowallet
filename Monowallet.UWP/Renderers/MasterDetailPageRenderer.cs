using Monowallet.UWP.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(CustomMasterDetailPageRenderer))]
namespace Monowallet.UWP.Renderers
{
    public class CustomMasterDetailPageRenderer : MasterDetailPageRenderer
    {
    }
}
