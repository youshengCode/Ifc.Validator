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
            set { _steps = value; NotifyOfPropertyChange(() => Steps); }
        }
        private bool _isLastButtonEnable = false;
        public bool IsLastButtonEnable
        {
            get { return _isLastButtonEnable; }
            set { _isLastButtonEnable = value; NotifyOfPropertyChange(() => IsLastButtonEnable); }
        }
        private bool _isNextButtonEnable = false;
        public bool IsNextButtonEnable
        {
            get { return _isNextButtonEnable; }
            set { _isNextButtonEnable = value; NotifyOfPropertyChange(() => IsNextButtonEnable); }
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
            CheckEnable();
        }
        private void CheckEnable()
        {
            if (StepServices.IsReacheLimit(_steps, true))
            {
                IsLastButtonEnable = true;
                IsNextButtonEnable = false;
            }
            else if (StepServices.IsReacheLimit(_steps, false))
            {
                IsLastButtonEnable = false;
                IsNextButtonEnable = true;
            }
            else
            {
                IsLastButtonEnable = true;
                IsNextButtonEnable = true;
            }
        }
        #endregion

        public List<Step> MoveSteps(bool isNext = true)
        {
            _steps = StepServices.MoveStep(_steps, isNext);
            CheckEnable();
            return Steps;
        }
    }
}
