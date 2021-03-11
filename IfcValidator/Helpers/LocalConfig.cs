using Caliburn.Micro;
using IfcValidator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfcValidator.Helpers
{
    public static class LocalConfig
    {
        public static void SaveConfigFile(object obj, string configFileName = null)
        {
            if (string.IsNullOrEmpty(configFileName))
                configFileName = LocalData.DefaultConfig + LocalData.JsonExt;
            MarsFileManager.SaveJsonFile(obj, configFileName);
        }
        public static async Task<BindableCollection<NodeItem>> LoadConfigFile(string configFileName = null)
        {
            if (string.IsNullOrEmpty(configFileName))
                configFileName = LocalData.DefaultConfig + LocalData.JsonExt;
            if (await MarsFileManager.CheckFileExist(configFileName))
                return await MarsFileManager.DeserializeFileAsync<BindableCollection<NodeItem>>(configFileName);
            else
                return null;
        }
    }
}
