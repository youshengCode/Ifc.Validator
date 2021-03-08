using IfcValidator.ViewModels;
using IO.Swagger.Model;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IfcValidator.Views
{
    public sealed partial class IfcValidatorPage : Page
    {
        private IfcValidatorPageViewModel ViewModel { get; set; } = new IfcValidatorPageViewModel();

        public IfcValidatorPage()
        {
            InitializeComponent();
            Workspace1.Content = new ClassificationPage();
        }
    }
}
