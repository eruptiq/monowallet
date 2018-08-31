#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.ListView.XForms.UWP;
using Syncfusion.RangeNavigator.XForms.UWP;
using Syncfusion.SfAutoComplete.XForms.UWP;
using Syncfusion.SfBarcode.XForms.UWP;
using Syncfusion.SfBusyIndicator.XForms.UWP;
using Syncfusion.SfCalendar.XForms.UWP;
using Syncfusion.SfCarousel.XForms.UWP;
using Syncfusion.SfChart.XForms.UWP;
using Syncfusion.SfDataGrid.XForms.UWP;
using Syncfusion.SfDiagram.XForms.UWP;
using Syncfusion.SfGauge.XForms.UWP;
using Syncfusion.SfImageEditor.XForms.UWP;
using Syncfusion.SfKanban.XForms.UWP;
using Syncfusion.SfMaps.XForms.UWP;
using Syncfusion.SfNavigationDrawer.XForms.UWP;
using Syncfusion.SfNumericTextBox.XForms.UWP;
using Syncfusion.SfNumericUpDown.XForms.UWP;
using Syncfusion.SfPdfViewer.XForms.UWP;
using Syncfusion.SfPicker.XForms.UWP;
using Syncfusion.SfPullToRefresh.XForms.UWP;
using Syncfusion.SfRadialMenu.XForms.UWP;
using Syncfusion.SfRangeSlider.XForms.UWP;
using Syncfusion.SfRating.XForms.UWP;
using Syncfusion.SfRotator.XForms.UWP;
using Syncfusion.SfSchedule.XForms.UWP;
using Syncfusion.SfSparkline.XForms.UWP;
using Syncfusion.SfSunburstChart.XForms.UWP;
using Syncfusion.SfTreeMap.XForms.UWP;
//using Syncfusion.XForms.UWP.Buttons;

using Syncfusion.XForms.UWP.ComboBox;
using Syncfusion.XForms.UWP.DataForm;
using Syncfusion.XForms.UWP.MaskedEdit;
using Syncfusion.XForms.UWP.PopupLayout;
using Syncfusion.XForms.UWP.ProgressBar;
using Syncfusion.XForms.UWP.TabView;
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TraderWallet.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                //// Create a Frame to act as the navigation context and navigate to the first page
                //rootFrame = new Frame();

                //rootFrame.NavigationFailed += OnNavigationFailed;

                List<Assembly> assembliesToInclude = new List<Assembly>();

                //assembliesToInclude.Add(typeof(SfSegmentedControlRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfComboBoxRenderer).GetTypeInfo().Assembly);

                //assembliesToInclude.Add(typeof(SfRadioButtonRenderer).GetTypeInfo().Assembly);

                //assembliesToInclude.Add(typeof(SfCheckBoxRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfTabViewRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfDiagramRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfPopupLayoutRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfMaskedEditRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfDataFormRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfBarcodeRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfSparklineRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfRangeNavigatorRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfKanbanRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfListViewRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfPullToRefreshRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfTreeMapRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfMapsRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfRatingRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfRadialMenuRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfNumericUpDownRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfNumericTextBoxRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfNavigationDrawerRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfBusyIndicatorRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfAutoCompleteRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfRotatorRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfCarouselRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfCalendarRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfCircularProgressBarRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfLinearProgressRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfLinearGaugeRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfDigitalGaugeRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfGaugeRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfScheduleRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfRangeSliderRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfPdfDocumentViewRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfPickerRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfDataGridRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfImageEditorRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfSunburstChartRenderer).GetTypeInfo().Assembly);

                assembliesToInclude.Add(typeof(SfChartRenderer).GetTypeInfo().Assembly);

                Xamarin.Forms.Forms.Init(e, assembliesToInclude);

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                //// Place the frame in the current Window
                //Window.Current.Content = rootFrame;
            }

            //if (rootFrame.Content == null)
            //{
            //    // When the navigation stack isn't restored navigate to the first page,
            //    // configuring the new page by passing required information as a navigation
            //    // parameter
            //    rootFrame.Navigate(typeof(MainPage), e.Arguments);
            //}
            //// Ensure the current window is active
            //Window.Current.Activate();

            base.OnLaunched(e);
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
