#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Reflection;

using Syncfusion.SfDataGrid.XForms.UWP;

using Syncfusion.SfPullToRefresh.XForms.UWP;

using Syncfusion.ListView.XForms.UWP;

using Syncfusion.XForms.UWP.PopupLayout;

namespace TraderWallet.UWP
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

            LoadApplication(new TraderWallet.App());
        }
    }
}
