using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace IfcValidator.Helpers
{
    public static class FileHelper
    {
        public static string GetFileSize(string filePath)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = new FileInfo(filePath).Length;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            string result = String.Format("{0:0.#} {1}", len, sizes[order]);
            return result;
        }

        public static string ConvertFileSize(ulong fileLength)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = fileLength;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            string result = String.Format("{0:0.#} {1}", len, sizes[order]);
            return result;
        }

    }
}
