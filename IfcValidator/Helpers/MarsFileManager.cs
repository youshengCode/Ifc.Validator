using IfcValidator.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;

namespace IfcValidator.Helpers
{
    public class MarsFileManager
    {
        #region Pickers
        public async static Task<bool> FolderPicker(string token)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");
            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, folder);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async static Task<StorageFolder> StorageFolderPicker(string token)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            //folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");
            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, folder);
                return folder;
            }
            else
            {
                return null;
            }
        }

        public async static Task<StorageFile> FilePicker()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            //openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            foreach(string format in LocalData.AccepteFormat)
            {
                openPicker.FileTypeFilter.Add(format);
            }
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                return file;
            }
            else { return null; }
        }

        public async static Task<List<StorageFile>> MultipleFilesPicker()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            foreach (string format in LocalData.AccepteFormat)
            {
                openPicker.FileTypeFilter.Add(format);
            }
            var files = await openPicker.PickMultipleFilesAsync();
            if (files != null)
            {
                if (files.Count > 0) { return files.ToList<StorageFile>(); }
                else { return null; }
            }
            else { return null; }
        }
        #endregion

        #region Save and delete storage file
        public async static Task SaveFileToLocal(string fileName, string token)
        {
            Windows.Storage.StorageFolder newFolder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
            Windows.Storage.StorageFile newFile = await newFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalCacheFolder;
            Windows.Storage.StorageFile tragetFile = await storageFolder.GetFileAsync(fileName);

            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(tragetFile);
            await Windows.Storage.FileIO.WriteBufferAsync(newFile, buffer);

            #region ReadWriteStream
            //string contentText = null;
            //using (var stream = await tragetFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
            //{
            //    ulong size = stream.Size;
            //    var inputStream = stream.GetInputStreamAt(0);
            //    var dataReader = new Windows.Storage.Streams.DataReader(inputStream);
            //    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
            //    contentText = dataReader.ReadString(numBytesLoaded);
            //}
            //using (var stream = await newFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
            //{
            //    var outputStream = stream.GetOutputStreamAt(0);
            //    var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream);
            //    dataWriter.WriteString(contentText);
            //    await dataWriter.StoreAsync();
            //    await outputStream.FlushAsync();
            //}
            #endregion
        }

        public async static Task SaveFileToOutputFolder(string fileName)
        {
            Windows.Storage.StorageFolder newFolder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFolderAsync(LocalData.OutputToken);
            Windows.Storage.StorageFile newFile = await newFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalCacheFolder;
            Windows.Storage.StorageFile tragetFile = await storageFolder.GetFileAsync(fileName);

            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(tragetFile);
            await Windows.Storage.FileIO.WriteBufferAsync(newFile, buffer);
        }
        #endregion

        #region Send Email
        public async static Task SendEmail(string emailAddress = null, string subject = null, string messageBody = null, List<StorageFile> attachmentFiles = null)
        {
            Windows.ApplicationModel.Email.EmailMessage emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            if(emailAddress != null)
            { emailMessage.To.Add(new Windows.ApplicationModel.Email.EmailRecipient(emailAddress)); }
            if (subject != null)
            { emailMessage.Subject = subject; }
            if (messageBody != null)
            { emailMessage.Body = messageBody; }
            if (attachmentFiles != null)
            {
                foreach(StorageFile attachmentFile in attachmentFiles)
                {
                    var stream = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromFile(attachmentFile);
                    var attachment = new Windows.ApplicationModel.Email.EmailAttachment(attachmentFile.Name, stream);
                    emailMessage.Attachments.Add(attachment);
                }
            }
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
        #endregion

        public static void Dump(object obj)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(obj));
        }

        public static async void SaveJsonFile(object obj, string fileNameWithExt)
        {
            string json = JsonConvert.SerializeObject(obj);
            //Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileNameWithExt, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        public static async Task<T> DeserializeFileAsync<T>(string fileNameWithExt)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileNameWithExt);
                string context = await FileIO.ReadTextAsync(localFile);
                return JsonConvert.DeserializeObject<T>(context);
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
        }

        public static async Task<bool> CheckFileExist(string fileNameWithExt)
        {
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileNameWithExt) != null)
            { return true; }
            else { return false; }
        }
    }
}
