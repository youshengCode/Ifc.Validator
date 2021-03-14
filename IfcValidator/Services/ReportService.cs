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
                // Recreate the list for avoid the notifychange of NodeItem
                List<NodeItem> nodes = new List<NodeItem>();
                foreach (var node in selectedNodes)
                    nodes.Add(NodeItem.CopyNode(node));
                foreach (var node in nodes)
                    NodeItem.ExpandAll(node);
                string fullPath = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, file.Name);
                List<NodeItem> result = IfcPropertyAnalyse.PropertyAnalyse(fullPath, nodes);
                ReportCard report = new ReportCard(result, file.Name);
                reports.Add(report);
            }
            return reports;
        }
    }
}
