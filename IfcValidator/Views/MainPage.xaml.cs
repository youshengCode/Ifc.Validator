using IfcValidator.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IfcValidator.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += delegate (object sender, RoutedEventArgs e) { ExtendAcrylicIntoTitleBar(); };
            Workspace.Content = new IfcValidatorPage();
        }

        #region AppTitleBar Reset
        private void ExtendAcrylicIntoTitleBar()
        {
            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
        }
        #endregion

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
    }
}
