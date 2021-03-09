using IfcValidator.ViewModels;
using IO.Swagger.Model;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace IfcValidator.Views
{
    public sealed partial class PropertyPage : Page
    {
        public PropertyPageViewModel ViewModel { get; set; } = new PropertyPageViewModel();

        public PropertyPage()
        {
            InitializeComponent();
        }
    }
}
