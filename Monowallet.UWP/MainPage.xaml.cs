#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion


using Syncfusion.ListView.XForms.UWP;
using Syncfusion.SfDataGrid.XForms.UWP;
using Syncfusion.SfPullToRefresh.XForms.UWP;
using Syncfusion.XForms.UWP.PopupLayout;

namespace Monowallet.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            SfPopupLayoutRenderer.Init();

            SfListViewRenderer.Init();

            SfPullToRefreshRenderer.Init();

            SfDataGridRenderer.Init();

            //LoadApplication(new Monowallet.FormsApp());
        }
    }
}
