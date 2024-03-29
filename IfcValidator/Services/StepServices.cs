﻿using IfcValidator.Helpers;
using IfcValidator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfcValidator.Services
{
    public static class StepServices
    {
        public static List<Step> GenerateIfcValidatorSteps()
        {
            List<Step> steps = new List<Step>();
            steps.Add(new Step(1, ResourceExtensions.GetLocalized("ValidatorPage_Step1"), true));
            steps.Add(new Step(2, ResourceExtensions.GetLocalized("ValidatorPage_Step2")));
            steps.Add(new Step(3, ResourceExtensions.GetLocalized("ValidatorPage_Step3")));
            steps.Add(new Step(4, ResourceExtensions.GetLocalized("ValidatorPage_Step4")));
            return steps;
        }

        public static int GetCompletedStepCount(List<Step> steps)
        {
            int i = 0;
            foreach (var item in steps)
                if (item.IsCompleted)
                    i++;
            return i;
        }
        public static bool IsReacheLimit(List<Step> steps, bool isMaxLimit)
        {
            int i = GetCompletedStepCount(steps);
            if (isMaxLimit)
            {
                if (i == steps.Count)
                    return true;
                else
                    return false;
            }
            else
            {
                if (i == 1)
                    return true;
                else
                    return false;
            }
        }
        public static List<Step> MoveStep(List<Step> steps, bool isNext = true)
        {
            if (isNext)
            {
                if(!IsReacheLimit(steps, true))
                {
                    steps[GetCompletedStepCount(steps)].IsCompleted = true;
                }
            }
            else
            {
                if (!IsReacheLimit(steps, false))
                {
                    steps[GetCompletedStepCount(steps)-1].IsCompleted = false;
                }
            }
            return steps;
        }
    }
}
