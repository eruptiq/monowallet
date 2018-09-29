using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Monowallet.UI.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Monowallet.UI.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Master, WrapInNavigationPage = false, NoHistory = true)]
    public partial class MenuPage : MvxContentPage<MenuViewModel>
    {
        public MenuPage()
        {

            InitializeComponent();

            //if (!App.IsWindows10)
            //{
            //    BackgroundColor = Color.FromHex("#03A9F4");
            //   // ListViewMenu.BackgroundColor = Color.FromHex("#F5F5F5");
            //}

#if __IOS__
            if(Parent is MasterDetailPage master)
                master.IsGestureEnabled = false;
#endif
        }


        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.SelectedMenuItem))
                {
                    if (Parent is MasterDetailPage master)
                    {
                        //master.MasterBehavior = MasterBehavior.Popover;
                        master.IsPresented = !master.IsPresented;
                    }
                }
            };
        }
    }

}

