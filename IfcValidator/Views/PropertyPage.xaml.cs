using Caliburn.Micro;
using IfcValidator.Core.Models;
using IfcValidator.ViewModels;
using IO.Swagger.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Windows.UI.Xaml;
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

        private void propTreeView_SelectionChanged(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewSelectionChangedEventArgs args)
        {
            UpdateSelection();
        }

        public void UpdateSelection()
        {
            IList<NodeItem> selected = new List<NodeItem>();
            if (propTreeView.SelectedItems != null)
                foreach (var item in propTreeView.SelectedItems)
                    if (item is NodeItem node)
                        selected.Add(node);
            ViewModel.GetAllSelection(selected);
        }
    }
}
