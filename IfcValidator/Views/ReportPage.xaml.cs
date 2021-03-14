using IfcValidator.Models;
using IfcValidator.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace IfcValidator.Views
{
    public sealed partial class ReportPage : Page
    {
        public ReportPageViewModel ViewModel { get; set; } = new ReportPageViewModel();

        public ReportPage()
        {
            InitializeComponent();
        }

        private void reportList_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ReportCard item = (ReportCard)e.ClickedItem;
            ViewModel.LoadSelectedReport(item);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Export report data as csv
        }
    }
}
