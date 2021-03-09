using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Caliburn.Micro;
using IfcValidator.Core.Models;
using IfcValidator.Models;
using IfcValidator.Services;
using IfcValidator.Views.UserControls;
using IO.Swagger.Api;
using IO.Swagger.Model;

namespace IfcValidator.ViewModels
{
    public class IfcValidatorPageViewModel : Screen
    {
        #region StepControl props
        private List<Step> _steps = new List<Step>();
        public List<Step> Steps
        {
            get { return _steps; }
            set { _steps = value; }
        }
        private bool _showLastButton = false;
        public bool ShowLastButton
        {
            get { return _showLastButton; }
            set { _showLastButton = value; NotifyOfPropertyChange(() => ShowLastButton); }
        }
        private bool _showNextButton = false;
        public bool ShowNextButton
        {
            get { return _showNextButton; }
            set { _showNextButton = value; NotifyOfPropertyChange(() => ShowNextButton); }
        }
        #endregion

        #region Initiation
        public IfcValidatorPageViewModel()
        {
            LoadSteps();
        }
        private void LoadSteps()
        {
            _steps = StepServices.GenerateIfcValidatorSteps();
            CheckVisibilityForLastAndNextButton();
        }
        private void CheckVisibilityForLastAndNextButton()
        {
            if (StepServices.IsReacheLimit(_steps, true))
            {
                ShowLastButton = true;
                ShowNextButton = false;
            }
            else if (StepServices.IsReacheLimit(_steps, false))
            {
                ShowLastButton = false;
                ShowNextButton = true;
            }
            else
            {
                ShowLastButton = true;
                ShowNextButton = true;
            }
        }
        #endregion

        public List<Step> MoveSteps(bool isNext = true)
        {
            _steps = StepServices.MoveStep(_steps, isNext);
            CheckVisibilityForLastAndNextButton();
            return Steps;
        }
    }
}
