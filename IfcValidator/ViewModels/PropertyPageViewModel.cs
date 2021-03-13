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
        public PropertyPageViewModel() { }
        public void GetAllProperties(BindableCollection<ClassificationSearchResultContractV2> selectedClasses, string languageCode = null)
        {
            List<NodeItem> newNodes = new List<NodeItem>();
            foreach (var item in selectedClasses)
                if (!_classes.Any(o => o.Name == item.Name))
                    newNodes.Add(GetPropertiesInClassification(item.NamespaceUri, languageCode));
            foreach (var item in newNodes)
                if (!_classes.Any(o => o.Name == item.Name))
                    _classes.Add(item);
            List<NodeItem> removeList = new List<NodeItem>();
            foreach (var item in _classes)
                if (!selectedClasses.Any(o => o.Name == item.Name))
                    removeList.Add(item);
            foreach (var item in removeList)
                _classes.Remove(item);
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
            List<NodeItem> removeList = new List<NodeItem>();
            foreach (var item in selected)
                if (!_classes.Any(o => o.Name == item.ClassificationName))
                    removeList.Add(item);
            foreach (var item in removeList)
                selected.Remove(item);
            SelectedClasses.Clear();
            if (selected.Count > 0)
            {
                BindableCollection<NodeItem> newNodes = new BindableCollection<NodeItem>(
                    NodeItem.RestructureFlatNodes(_classes.ToList(), selected));
                SelectedClasses = newNodes;
            }
            UpdatePropNotice(_selectedClasses);
        }
        private void UpdatePropNotice(BindableCollection<NodeItem> newNodes)
        {
            if (newNodes.Count > 0)
            {
                int classCount = 0; string classText;
                int propCount = 0; string propText;
                foreach (var item in newNodes)
                {
                    if (item.Type == NodeItem.NodeItemType.Classification)
                    {
                        classCount++;
                        foreach (var propSet in item.Children)
                            foreach (var prop in propSet.Children)
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
