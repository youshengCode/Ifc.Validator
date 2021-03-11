using IfcValidator.Models;
using IfcValidator.Services;
using IfcValidator.ViewModels;
using IfcValidator.Views.UserControls;
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
        private ClassificationPage classificationPage = new ClassificationPage();
        private PropertyPage propertyPage = new PropertyPage();
        public IfcValidatorPage()
        {
            InitializeComponent();
            StepControl.Content = new StepUserControl(ViewModel.Steps);
            Workspace1.Content = classificationPage;
            //Workspace1.Content = propertyPage;
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            int step = StepServices.GetCompletedStepCount(ViewModel.Steps);
            switch (step)
            {
                case 1:
                    if (classificationPage.ViewModel.HasSelection)
                    {
                        StepControl.Content = new StepUserControl(ViewModel.MoveSteps());
                        propertyPage.ViewModel.GetAllProperties(classificationPage.ViewModel.SelectedClasses);
                        Workspace1.Content = propertyPage;
                    }
                    else
                        NoticeShow("No classification selected.");
                    break;
                case 2:
                    if (propertyPage.ViewModel.HasSelection)
                    {
                        StepControl.Content = new StepUserControl(ViewModel.MoveSteps());
                        Workspace1.Content = null;
                    }
                    else
                        NoticeShow("No property selected.");
                    break;
                default:
                    break;
            }
        }

        private void LastBtn_Click(object sender, RoutedEventArgs e)
        {
            StepControl.Content = new StepUserControl(ViewModel.MoveSteps(false));
            Workspace1.Content = classificationPage;
        }

        private void NoticeShow(string text)
        {
            InAppNotice.Content = text;
            InAppNotice.Show(2000);
        }
    }
}
