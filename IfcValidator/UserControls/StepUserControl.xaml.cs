using IfcValidator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace IfcValidator.Views.UserControls
{
    public sealed partial class StepUserControl : UserControl
    {
        public List<Step> Steps
        {
            get { return (List<Step>)GetValue(StepsProperty); }
            set { SetValue(StepsProperty, value); }
        }
        public static readonly DependencyProperty StepsProperty =
            DependencyProperty.Register("Steps", typeof(List<Step>), typeof(StepUserControl), new PropertyMetadata(new List<Step>()));

        public StepUserControl(List<Step> steps)
        {
            Steps = steps;
            this.InitializeComponent();
        }

        public StepUserControl()
        {
            this.InitializeComponent();
        }
    }
}
