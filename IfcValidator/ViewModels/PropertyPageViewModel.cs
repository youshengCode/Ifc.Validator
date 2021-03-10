using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Caliburn.Micro;
using IfcValidator.Core.Models;
using IfcValidator.Helpers;
using IO.Swagger.Api;
using IO.Swagger.Model;

namespace IfcValidator.ViewModels
{
    public class PropertyPageViewModel : Screen
    {
        #region Properties prop
        private BindableCollection<NodeItem> _classes = new BindableCollection<NodeItem>();
        public BindableCollection<NodeItem> Classes
        {
            get { return _classes; }
            set { _classes = value; }
        }

        private BindableCollection<NodeItem> _selectedClasses = new BindableCollection<NodeItem>();
        public BindableCollection<NodeItem> SelectedClasses
        {
            get { return _selectedClasses; }
            set { _selectedClasses = value; }
        }

        private bool _hasSelection = false;
        public bool HasSelection
        {
            get { return _hasSelection; }
            set { _hasSelection = value; NotifyOfPropertyChange(() => HasSelection); }
        }
        #endregion

        #region Initiation
        public PropertyPageViewModel()
        {
            //string namespaceUrl = "http://identifier.buildingsmart.org/uri/buildingsmart/ifc-4.3/class/IfcBridge";
            string namespaceUrl = "http://identifier.buildingsmart.org/uri/buildingsmart/ifc-4.3/class/IfcWall";
            GetPropertiesInClassification(namespaceUrl);
        }
        #endregion

        private void GetPropertiesInClassification(string namespaceUrl, string languageCode = null, bool? includChild = false)
        {
            ClassificationContractV2 response = new ClassificationApi(LocalData.baseHttp).
                ApiClassificationV2Get(namespaceUrl, languageCode, includChild);
            _classes.Add(new NodeItem(response));
        }

        public void GetAllSelection(IList<NodeItem> selected)
        {
            SelectedClasses = new BindableCollection<NodeItem>();
            foreach (var item in selected)
            {
                if (item.Type == NodeItem.NodeItemType.Classification)
                    _selectedClasses.Add(item);
            }
            foreach (var item in _selectedClasses)
            {
                foreach (var prop in item.Children)
                    if (selected.Contains(prop))
                        selected.Remove(prop);
            }
            foreach (var item in selected)
            {
                IEnumerable<NodeItem> nodes = _classes.Where(o => o.Name == item.ParentName);
                if (nodes.Count() > 0)
                {
                    NodeItem node = nodes.First();
                    if (!_selectedClasses.Contains(node))
                    {
                        node.Children = null;
                        _selectedClasses.Add(node);
                    }
                }
            }
            foreach (var item in _selectedClasses)
            {
                IEnumerable<NodeItem> nodes = selected.Where(o => o.ParentName == item.Name);
                if (nodes.Count() > 0)
                    foreach (var node in nodes)
                        item.Children.Add(node);
            }
            if (_selectedClasses.Count > 0)
                HasSelection = true;
            else
                HasSelection = false;
            //foreach (var item in _selectedClasses)
            //    Debug.WriteLine(item.ToString());
        }
    }
}
