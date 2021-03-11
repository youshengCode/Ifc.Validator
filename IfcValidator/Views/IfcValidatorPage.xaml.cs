using Caliburn.Micro;
using IfcValidator.Core.Models;
using IfcValidator.Helpers;
using IfcValidator.Models;
using IfcValidator.Services;
using IfcValidator.ViewModels;
using IfcValidator.Views.UserControls;
using IO.Swagger.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IfcValidator.Views
{
    public sealed partial class IfcValidatorPage : Page
    {
        private IfcValidatorPageViewModel ViewModel { get; set; } = new IfcValidatorPageViewModel();
        private ClassificationPage classificationPage = new ClassificationPage();
        private PropertyPage propertyPage = new PropertyPage();
        private InputFilePage inputFilePage = new InputFilePage();
        public IfcValidatorPage()
        {
            InitializeComponent();
            StepControl.Content = new StepUserControl(ViewModel.Steps);
            Workspace1.Content = classificationPage;
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
                        propertyPage.UpdateSelection();
                        Workspace1.Content = propertyPage;
                    }
                    else
                        NoticeShow(ResourceExtensions.GetLocalized("ValidatorPage_Notice_NoClasses"));
                    break;
                case 2:
                    if (propertyPage.ViewModel.HasSelection)
                    {
                        StepControl.Content = new StepUserControl(ViewModel.MoveSteps());
                        LocalConfig.SaveConfigFile(propertyPage.ViewModel.SelectedClasses);
                        Workspace1.Content = inputFilePage;
                    }
                    else
                        NoticeShow(ResourceExtensions.GetLocalized("ValidatorPage_Notice_NoProps"));
                    break;
                case 3:
                    if (inputFilePage.ViewModel.InputFiles.Count > 0)
                    {
                        StepControl.Content = new StepUserControl(ViewModel.MoveSteps());
                        //TODO: ReportPage
                        Workspace1.Content = null;
                    }
                    else
                        NoticeShow(ResourceExtensions.GetLocalized("ValidatorPage_Notice_NoFile"));
                    break;
                default:
                    break;
            }
        }

        private void LastBtn_Click(object sender, RoutedEventArgs e)
        {
            int step = StepServices.GetCompletedStepCount(ViewModel.Steps);
            switch (step)
            {
                case 2:
                    StepControl.Content = new StepUserControl(ViewModel.MoveSteps(false));
                    Workspace1.Content = classificationPage;
                    break;
                case 3:
                    StepControl.Content = new StepUserControl(ViewModel.MoveSteps(false));
                    Workspace1.Content = propertyPage;
                    break;
                case 4:
                    StepControl.Content = new StepUserControl(ViewModel.MoveSteps(false));
                    Workspace1.Content = inputFilePage;
                    break;
                default:
                    break;
            }
        }

        private void NoticeShow(string text)
        {
            InAppNotice.Content = text;
            InAppNotice.Show(2000);
        }
    }
}
