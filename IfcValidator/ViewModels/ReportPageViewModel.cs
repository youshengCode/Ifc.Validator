using Caliburn.Micro;
using IfcValidator.Core.Models;
using IfcValidator.Models;
using IfcValidator.Services;
using System.Collections.Generic;
using System.Linq;

namespace IfcValidator.ViewModels
{
    public class ReportPageViewModel : Screen
    {
        #region Attributes
        private BindableCollection<ReportCard> _reports = new BindableCollection<ReportCard>();
        public BindableCollection<ReportCard> Reports
        {
            get { return _reports; }
            set { _reports = value; NotifyOfPropertyChange(() => Reports); }
        }

        private BindableCollection<NodeItem> _detailNodes = new BindableCollection<NodeItem>();
        public BindableCollection<NodeItem> DetailNodes
        {
            get { return _detailNodes; }
            set { _detailNodes = value; NotifyOfPropertyChange(() => DetailNodes); }
        }

        private string _reportTitle;
        public string ReportTitle
        {
            get { return _reportTitle; }
            set { _reportTitle = value; NotifyOfPropertyChange(() => ReportTitle); }
        }

        #endregion

        public ReportPageViewModel() { }

        public void LoadReport(BindableCollection<UserFile> files, BindableCollection<NodeItem> nodes)
        {
            Reports = new BindableCollection<ReportCard>(ReportService.LoadReport(files, nodes));
            LoadSelectedReport(Reports.First());
        }

        public void LoadSelectedReport(ReportCard reportCard)
        {
            ReportTitle = reportCard.FileName;
            DetailNodes = new BindableCollection<NodeItem>(reportCard.RelatedNodes);
        }

    }
}
