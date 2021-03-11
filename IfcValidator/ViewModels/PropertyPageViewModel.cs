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
            set { _classes = value; NotifyOfPropertyChange(() => Classes); }
        }

        private BindableCollection<NodeItem> _selectedClasses = new BindableCollection<NodeItem>();
        public BindableCollection<NodeItem> SelectedClasses
        {
            get { return _selectedClasses; }
            set { _selectedClasses = value; NotifyOfPropertyChange(() => SelectedClasses); }
        }
        private string _propertyNotice = null;
        public string PropertyNotice
        {
            get { return _propertyNotice; }
            set { _propertyNotice = value; NotifyOfPropertyChange(() => PropertyNotice); }
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

        }
        public void GetAllProperties(BindableCollection<ClassificationSearchResultContractV2> selectedClasses)
        {
            List<NodeItem> newNodes = new List<NodeItem>();
            foreach (var item in selectedClasses)
            {
                newNodes.Add(GetPropertiesInClassification(item.NamespaceUri));
            }
            foreach (var item in newNodes)
            {
                IEnumerable<NodeItem> nodes = _classes.Where(o => o.Name == item.Name);
                if (nodes.Count() == 0)
                    _classes.Add(item);
            }
            for (int i = 0; i < _classes.Count; i++)
            {
                var item = _classes[i];
                IEnumerable<NodeItem> nodes = newNodes.Where(o => o.Name == item.Name);
                if (nodes.Count() == 0)
                    _classes.RemoveAt(i);
            }
        }
        private NodeItem GetPropertiesInClassification(string namespaceUrl, string languageCode = null, bool? includChild = false)
        {
            ClassificationContractV2 response = new ClassificationApi(LocalData.baseHttp).
                ApiClassificationV2Get(namespaceUrl, languageCode, includChild);
            NodeItem node = new NodeItem(response);
            return node;
        }
        #endregion

        #region Selection
        public void GetAllSelection(IList<NodeItem> selected)
        {
            BindableCollection<NodeItem> newNodes = new BindableCollection<NodeItem>();
            SelectedClasses.Clear();
            if (selected.Count > 0)
            {
                foreach (var item in selected)
                {
                    IEnumerable<NodeItem> nodes = _classes.Where(o => o.Name == item.Name);
                    if (nodes.Count() != 0)
                        if (item.Type == NodeItem.NodeItemType.Classification)
                            newNodes.Add(item);
                }
                foreach (var item in newNodes)
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
                        if (!newNodes.Contains(node))
                        {
                            node.Children = null;
                            newNodes.Add(node);
                        }
                    }
                }
                foreach (var item in newNodes)
                {
                    IEnumerable<NodeItem> nodes = selected.Where(o => o.ParentName == item.Name);
                    if (nodes.Count() > 0)
                        foreach (var node in nodes)
                        {
                            node.Type = NodeItem.NodeItemType.Property;
                            item.Children.Add(node);
                        }
                }
            }
            UpdatePropNotice(newNodes);
            SelectedClasses = newNodes;
        }
        private void UpdatePropNotice(BindableCollection<NodeItem> newNodes)
        {
            if (newNodes.Count > 0)
            {
                int classCount = 0; string classText;
                int propCount = 0; string propText;
                foreach (var item in newNodes)
                {
                    //Debug.WriteLine(item.ToString());
                    if (item.Type == NodeItem.NodeItemType.Classification)
                    {
                        classCount++;
                        foreach (var prop in item.Children)
                            propCount++;
                    }
                }
                if (classCount > 1)
                    classText = $"{classCount} {ResourceExtensions.GetLocalized("PropertyNotice_ClassN")}";
                else
                    classText = $"{classCount} {ResourceExtensions.GetLocalized("PropertyNotice_Class1")}";
                if (propCount > 1)
                    propText = $"{propCount} {ResourceExtensions.GetLocalized("PropertyNotice_PropN")}";
                else
                    propText = $"{propCount} {ResourceExtensions.GetLocalized("PropertyNotice_Prop1")}";
                PropertyNotice = $"{ResourceExtensions.GetLocalized("PropertyNotice_Selected")} {propText} " +
                    $"{ResourceExtensions.GetLocalized("PropertyNotice_In")} {classText}";
                HasSelection = true;
            }
            else
            {
                PropertyNotice = null;
                HasSelection = false;
            }
        }
        #endregion 
    }
}
