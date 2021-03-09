﻿using IfcValidator.Models;
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
        public IfcValidatorPage()
        {
            InitializeComponent();
            StepControl.Content = new StepUserControl(ViewModel.Steps);
            Workspace1.Content = classificationPage;
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            StepControl.Content = new StepUserControl(ViewModel.MoveSteps());
            Workspace1.Content = null;
        }

        private void LastBtn_Click(object sender, RoutedEventArgs e)
        {
            StepControl.Content = new StepUserControl(ViewModel.MoveSteps(false));
            Workspace1.Content = classificationPage;
        }
    }
}
