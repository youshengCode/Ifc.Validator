using IfcValidator.Core.Models;
using IfcValidator.Helpers;
using IfcValidator.Models;
using IfcValidator.ViewModels;
using System;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace IfcValidator.Views
{
    public sealed partial class InputFilePage : Page
    {
        public InputFilePage(bool isSingleInput = false)
        {
            this.InitializeComponent();
            ViewModel.IsSingleInput = isSingleInput;
            DataContext = ViewModel;
        }

        public InputFilePageViewModel ViewModel { get; set; } = new InputFilePageViewModel();

        private void OnFileDragOver(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = ResourceExtensions.GetLocalized("Tool_OnFileDragOver");
            e.DragUIOverride.IsGlyphVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsCaptionVisible = true;
            this.DragDropPanel.Opacity = 0.5;
        }

        private void OnFileDragLeave(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            this.DragDropPanel.Opacity = 1;
        }

        private async void OnFileDrop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    if (ViewModel.IsSingleInput)
                    {
                        if(ViewModel.InputFiles.Count == 0)
                            AddStorageFile(items.OfType<StorageFile>().First());
                    }
                    else
                        foreach (var inputFile in items.OfType<StorageFile>())
                            AddStorageFile(inputFile);
                }
            }
        }

        private async void AddFileToInput_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.IsSingleInput)
            {
                if(ViewModel.InputFiles.Count < 1)
                {
                    var inputFile = await MarsFileManager.FilePicker();
                    if (inputFile != null)
                        AddStorageFile(inputFile);
                }
            }
            else
            {
                var inputFiles = await MarsFileManager.MultipleFilesPicker();
                if (inputFiles != null)
                    foreach (var file in inputFiles)
                        AddStorageFile(file);
            }
        }

        private void ClearAllFiles_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.RemoveAllFiles();
        }

        private async void AddStorageFile(StorageFile inputFile)
        {
            await MarsFileManager.CopyFile(inputFile);
            Windows.Storage.FileProperties.BasicProperties basicProperties = await inputFile.GetBasicPropertiesAsync();
            UserFile file = new UserFile(inputFile, basicProperties.Size, basicProperties.DateModified.DateTime);
            if (LocalData.AccepteFormat.Contains(Path.GetExtension(file.Name)))
                ViewModel.AddFile(file);
        }
    }
}
