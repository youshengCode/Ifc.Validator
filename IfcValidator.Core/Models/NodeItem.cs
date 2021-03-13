using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace IfcValidator.Core.Models
{
    public class NodeItem : INotifyPropertyChanged
    {
        #region Constructor
        public NodeItem() { }
        public NodeItem(ClassificationContractV2 classEntity)
        {
            ClassEntity = classEntity;
            Name = classEntity.Name;
            Type = NodeItemType.Classification;
            GetChildrenNode();
            GetRefIfcEntity();
            ParentName = null;
            ClassificationName = classEntity.Name;
        }
        #endregion

        #region Attributes
        public string Name { get; set; }
        public string RefIfcEntity { get; set; }
        public string ParentName { get; set; }
        public string ClassificationName { get; set; }
        public int ExistCount { get; set; } = 0;

        public NodeItemType Type { get; set; }
        public enum NodeItemType { Classification, PropertySet, Property };
        public ClassificationContractV2 ClassEntity { get; set; }

        private ObservableCollection<NodeItem> _children;
        public ObservableCollection<NodeItem> Children
        {
            get
            {
                if (_children == null)
                    _children = new ObservableCollection<NodeItem>();
                return _children;
            }
            set { _children = value; }
        }

        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }
        #endregion

        #region Private Methode
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private static NodeItem CreatePropSetNode(string name, string parentName, string classificationName)
        {
            NodeItem node = new NodeItem();
            node.Name = name;
            node.Type = NodeItemType.PropertySet;
            node.ParentName = parentName;
            node.ClassificationName = classificationName;
            node.IsExpanded = false;
            return node;
        }
        private static NodeItem CreatePropNode(string name, string parentName, string classificationName)
        {
            NodeItem node = new NodeItem();
            node.Name = name;
            node.Type = NodeItemType.Property;
            node.ParentName = parentName;
            node.ClassificationName = classificationName;
            return node;
        }
        private static NodeItem CopyNode(NodeItem node, bool withChildren = false)
        {
            NodeItem newNode = new NodeItem();
            newNode.Name = node.Name;
            newNode.Type = node.Type;
            newNode.RefIfcEntity = node.RefIfcEntity;
            newNode.ParentName = node.ParentName;
            newNode.ClassificationName = node.ClassificationName;
            newNode.IsExpanded = node.IsExpanded;
            newNode.IsSelected = node.IsSelected;
            newNode.ExistCount = node.ExistCount;
            newNode.ClassEntity = node.ClassEntity;
            if (withChildren)
                newNode.Children = node.Children;
            return newNode;
        }
        private void GetChildrenNode()
        {
            if (ClassEntity.ClassificationProperties != null)
            {
                List<NodeItem> propSetNodes = new List<NodeItem>();
                foreach (var item in ClassEntity.ClassificationProperties)
                {
                    if (!string.IsNullOrEmpty(item.PropertySet))
                    {
                        NodeItem newPropSet = CreatePropSetNode(item.PropertySet, Name, Name);
                        if (!Children.Any(o => o.Name == newPropSet.Name))
                            Children.Add(newPropSet);
                        NodeItem propSet = Children.Where(o => o.Name == item.PropertySet).FirstOrDefault();
                        if (propSet != null)
                            propSet.Children.Add(CreatePropNode(item.Name, propSet.Name, Name));
                    }
                    else
                    {
                        NodeItem newPropSet = CreatePropSetNode(LocalData.UndefinedPset, Name, Name);
                        if (!Children.Any(o => o.Name == newPropSet.Name))
                            Children.Add(newPropSet);
                        NodeItem propSet = Children.Where(o => o.Name == LocalData.UndefinedPset).FirstOrDefault();
                        if (propSet != null)
                            propSet.Children.Add(CreatePropNode(item.Name, propSet.Name, Name));
                    }
                }
            }
        }
        private void GetRefIfcEntity()
        {
            string entities = null;
            if (ClassEntity.RelatedIfcEntityNames != null)
                foreach (var item in ClassEntity.RelatedIfcEntityNames)
                {
                    entities += $" {item}";
                }
            if (!string.IsNullOrEmpty(entities))
                RefIfcEntity = $"Related: {entities}";
        }
        #endregion

        #region Public Methode
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"{Name}  {Type}").Append("{\n");
            foreach (var item in Children)
            {
                sb.Append($"  {item.Name}  {item.Type}").Append("\n");
            }
            sb.Append("}\n");
            return sb.ToString();
        }
        public static List<NodeItem> RestructureFlatNodes(List<NodeItem> allNodes, IList<NodeItem> selected)
        {
            List<NodeItem> restructedClasses = new List<NodeItem>();
            List<NodeItem> restructedPropSet = new List<NodeItem>();
            // Classification selected - clear all subnodes
            foreach (var item in selected)
            {
                if (item.Type == NodeItemType.Classification)
                    restructedClasses.Add(item);
            }
            foreach (var item in restructedClasses)
            {
                foreach (var node in selected.Where(x => x.Type == NodeItemType.Classification).ToList())
                    selected.Remove(node);
                foreach (var node in selected.Where(x => x.ClassificationName == item.Name).ToList())
                    selected.Remove(node);
            }
            // PropertySet selected - clear all subnodes
            foreach (var item in selected)
            {
                if (item.Type == NodeItemType.PropertySet)
                    restructedPropSet.Add(item);
            }
            foreach (var item in restructedPropSet)
            {
                foreach (var node in selected.Where(x => x.Type == NodeItemType.PropertySet).ToList())
                    selected.Remove(node);
                foreach (var node in selected.Where(x => x.ClassificationName == item.ClassificationName && x.ParentName == item.Name).ToList())
                    selected.Remove(node);
            }
            // Add Property in PropertySet
            foreach (var prop in selected)
            {
                if (prop.Type == NodeItemType.Property)
                {
                    foreach (var node in allNodes)
                    {
                        NodeItem propSetNode = node.Children.Where(ps => ps.Name == prop.ParentName && ps.ClassificationName == prop.ClassificationName).FirstOrDefault();
                        if (propSetNode != null)
                            if (!restructedPropSet.Any(ps => ps.Name == propSetNode.Name && ps.ClassificationName == propSetNode.ClassificationName))
                            {
                                restructedPropSet.Add(CopyNode(propSetNode));
                            }
                    }
                    NodeItem newPropSetNode = restructedPropSet.Where(ps => ps.Name == prop.ParentName && ps.ClassificationName == prop.ClassificationName).FirstOrDefault();
                    if (newPropSetNode != null)
                        newPropSetNode.Children.Add(prop);
                }
            }
            // Add PropertySet in Classifications
            foreach (var item in restructedPropSet)
            {
                NodeItem classNode = allNodes.Where(x => x.Name == item.ClassificationName).FirstOrDefault();
                if (classNode != null)
                    if (!restructedClasses.Any(x => x.Name == classNode.Name))
                    {
                        restructedClasses.Add(CopyNode(classNode));
                    }
                NodeItem newClassNode = restructedClasses.Where(x => x.Name == item.ClassificationName).FirstOrDefault();
                if (newClassNode != null) 
                    newClassNode.Children.Add(item);
            }
            return restructedClasses;
        }
        #endregion
    }
}
