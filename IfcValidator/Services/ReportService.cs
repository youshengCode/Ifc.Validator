using Caliburn.Micro;
using IfcValidator.Core.Models;
using IfcValidator.Core.Services;
using IfcValidator.Helpers;
using IfcValidator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace IfcValidator.Services
{
    public class ReportService
    {
        public static List<ReportCard> LoadReport(BindableCollection<UserFile> InputFiles, BindableCollection<NodeItem> selectedNodes)
        {
            List<ReportCard> reports = new List<ReportCard>();
            foreach (var file in InputFiles)
            {
                string fullPath = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, file.Name);
                List<NodeItem> result = IfcPropertyAnalyse.PropertyAnalyse(fullPath, selectedNodes.ToList());
                ReportCard report = new ReportCard(result, file.Name);
                reports.Add(report);
            }
            return reports;
        }
    }
}
