using IfcValidator.ViewModels;
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
    public sealed partial class ClassificationPage : Page
    {
        public ClassificationPageViewModel ViewModel { get; set; } = new ClassificationPageViewModel();

        public ClassificationPage()
        {
            InitializeComponent();
        }

        private void DomainSelectChange(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateDefaultLanguage();
            DragDropListView1.ItemsSource =
                ViewModel.LoadClassification();
        }

        private void LanguageSelectChange(object sender, SelectionChangedEventArgs e)
        {
            DragDropListView1.ItemsSource =
                ViewModel.LoadClassification(ViewModel.GetSelectLanguageCode(), ViewModel.SearchText);
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            DragDropListView1.ItemsSource =
                ViewModel.LoadClassification(ViewModel.GetSelectLanguageCode(), ViewModel.SearchText);
        }

        private void SearchKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
                DragDropListView1.ItemsSource =
                    ViewModel.LoadClassification(ViewModel.GetSelectLanguageCode(), SearchInput.Text);
        }

        private void ListViewSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            List<ClassificationSearchResultContractV2> listItems = new List<ClassificationSearchResultContractV2>();
            foreach (ClassificationSearchResultContractV2 item in DragDropListView1.SelectedItems)
                listItems.Add(item);
            ViewModel.SelecteClasses(listItems);
        }

        private void RemoveSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            List<ClassificationSearchResultContractV2> listItems = new List<ClassificationSearchResultContractV2>();
            foreach (ClassificationSearchResultContractV2 item in DragDropListView2.SelectedItems)
                listItems.Add(item);
            ViewModel.RemoveSelecteClasses(listItems);
        }

        private void RemoveAllButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveAllClasses();
        }
    }
}
