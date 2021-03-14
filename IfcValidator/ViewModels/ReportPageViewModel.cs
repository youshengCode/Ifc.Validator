using Caliburn.Micro;
using IfcValidator.Core.Models;
using IfcValidator.Models;
using IfcValidator.Services;
using System.Collections.Generic;

namespace IfcValidator.ViewModels
{
    public class ReportPageViewModel : Screen
    {
        #region Attributes
        private BindableCollection<ReportCard> _reports = new BindableCollection<ReportCard>();
        public BindableCollection<ReportCard> Reports
        {
            get { return _reports; }
            set { _reports = value; }
        }
        #endregion

        public ReportPageViewModel() { }

        public void LoadReport(BindableCollection<UserFile> files, BindableCollection<NodeItem> nodes)
        {
            _reports = new BindableCollection<ReportCard>(ReportService.LoadReport(files, nodes).Result);
        }

    }
}
