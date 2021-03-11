using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace IfcValidator.Core.Helpers
{
    public static class Json
    {
        public static async Task<T> ToObjectAsync<T>(string value)
        {
            return await Task.Run<T>(() =>
            {
                return JsonConvert.DeserializeObject<T>(value);
            });
        }

        public static async Task<string> StringifyAsync(object value)
        {
            return await Task.Run<string>(() =>
            {
                return JsonConvert.SerializeObject(value);
            });
        }

        public static T DeserializeJsonFile<T>(string fileFullPath)
        {
            try
            {
                string context = File.ReadAllText(fileFullPath);
                return JsonConvert.DeserializeObject<T>(context);
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
        }
    }
}
