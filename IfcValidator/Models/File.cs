using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IfcValidator.Helpers;
using Windows.Storage;

namespace IfcValidator.Models
{
    public class UserFile
    {
        public UserFile(string path)
        {
            Name = Path.GetFileNameWithoutExtension(path);
            CreatedTime = File.GetCreationTime(path);
            DisplayTime = CreatedTime.ToLongDateString();
            FilePath = path;
            Size = FileHelper.GetFileSize(path);
        }
        public UserFile(StorageFile storageFile, ulong size, DateTime modifiedTime, string token = null)
        {
            StorageFile = storageFile;
            Name = storageFile.Name;
            FilePath = storageFile.Path;
            Token = token;
            Size = FileHelper.ConvertFileSize(size);
            ModifiedTime = modifiedTime;
            DisplayTime = string.Format("{0} {1}", ModifiedTime.ToShortDateString(), ModifiedTime.ToShortTimeString());
        }
        public string Name { get; set; }
        private DateTime CreatedTime { get; set; }
        public string DisplayTime { get; set; }
        public string Size { get; set; }
        public string FilePath { get; set; }
        public string Token { get; set; }
        public StorageFile StorageFile { get; set; }
        private DateTime ModifiedTime { get; set; }
    }

}
