using Caliburn.Micro;
using IfcValidator.Models;

namespace IfcValidator.ViewModels
{
    public class InputFilePageViewModel : Screen
    {
        #region fields
        private bool _dragDropVisible = true;
        private bool _fileInfoVisible = false;
        private bool _isSingleInput = false;
        private BindableCollection<UserFile> _inputFiles = new BindableCollection<UserFile>();

        public BindableCollection<UserFile> InputFiles
        {
            get { return _inputFiles; }
            set { _inputFiles = value; }
        }
        public bool DragDropVisible
        {
            get { return _dragDropVisible; }
            set { _dragDropVisible = value; NotifyOfPropertyChange(() => DragDropVisible); }
        }
        public bool FileInfoVisible
        {
            get { return _fileInfoVisible; }
            set { _fileInfoVisible = value; NotifyOfPropertyChange(() => FileInfoVisible); }
        }
        public bool IsSingleInput
        {
            get { return _isSingleInput; }
            set { _isSingleInput = value; }
        }
        #endregion

        public InputFilePageViewModel() { }

        public void AddFile(UserFile clickedItem)
        {
            bool isExist = false;
            foreach(UserFile inputFile in InputFiles)
                if (inputFile.Name == clickedItem.Name)
                    isExist = true;
            if (!isExist)
                InputFiles.Add(clickedItem);
            if (InputFiles.Count > 0)
                ShowFileInfoPanel();
        }

        public void RemoveFile(UserFile clickedItem)
        {
            if (clickedItem != null)
            {
                BindableCollection<UserFile> files = _inputFiles;
                foreach (UserFile inputFile in files)
                {
                    if(inputFile.Name == clickedItem.Name)
                    {
                        files.Remove(inputFile);
                        break;
                    }
                }
                InputFiles = files;
                if (files.Count == 0)
                    ShowDragDropPanel();
            }
        }

        public void RemoveAllFiles()
        {
            InputFiles.Clear();
            ShowDragDropPanel();
        }

        public void ShowFileInfoPanel()
        {
            DragDropVisible = false;
            FileInfoVisible = true;
        }

        public void ShowDragDropPanel()
        {
            DragDropVisible = true;
            FileInfoVisible = false;
        }
    }
}
