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
            //Workspace1.Content = propertyPage;
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
                        Workspace1.Content = propertyPage;
                        //Set LanguageCode And Url
                    }
                    else
                        NoticeShow("No classification selected.");
                    break;
                case 2:
                    if (propertyPage.HasSelection())
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

        private void NoticeShow(string text)
        {
            InAppNotice.Content = text;
            InAppNotice.Show(2000);
        }

        private void LastBtn_Click(object sender, RoutedEventArgs e)
        {
            StepControl.Content = new StepUserControl(ViewModel.MoveSteps(false));
            Workspace1.Content = classificationPage;
        }

        private int WhichStep(bool classDone = false, bool propDone = false, bool inputFileDone = false, bool importDome = false)
        {
            if ((classDone && propDone && inputFileDone) || (importDome && inputFileDone))
            {
                return 4;
            }
            else if ((classDone && propDone) || (importDome))
            {
                return 3;
            }
            else if (classDone && !propDone)
            {
                return 2;
            }
            else if (!classDone)
            {
                return 1;
            }
            else { return 0; }
        }
    }
}
