using System;
using System.Collections.Generic;
using System.Text;

namespace IfcValidator.Core.Models
{
    public static class LocalData
    {
        public static string baseHttp = "https://bs-dd-api-prototype.azurewebsites.net";
        public static string[] AccepteFormat = { ".ifc", ".ifczip", ".ifcxml" };
        public static string OutputToken = "userOutputStorageFolderToken";
        public static string JsonExt = ".json";
        public static string DefaultConfig = "DefaultConfig";

    }
}
