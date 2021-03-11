using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace IfcValidator.Core.Models
{
    public class NodeItem : INotifyPropertyChanged
    {
        public NodeItem() { }
        public NodeItem(ClassificationContractV2 classEntity)
        {
            ClassEntity = classEntity;
            Name = classEntity.Name;
            Type = NodeItemType.Classification;
            GetChildrenNode();
            GetRefIfcEntity();
            ParentName = null;
        }
        public NodeItem(string name, string parentName)
        {
            Name = name;
            Type = NodeItemType.Property;
            ParentName = parentName;
        }
        public string Name { get; set; }
        public string RefIfcEntity { get; set; }
        public string ParentName { get; set; }
        public NodeItemType Type { get; set; }
        public enum NodeItemType { Classification, Property };
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GetChildrenNode()
        {
            if (ClassEntity.ClassificationProperties != null)
                foreach (var item in ClassEntity.ClassificationProperties)
                    Children.Add(new NodeItem(item.Name, Name));
        }
        private void GetRefIfcEntity()
        {
            string entities = null;
            if (ClassEntity.RelatedIfcEntityNames != null)
                foreach (var item in ClassEntity.RelatedIfcEntityNames)
                {
                    entities += $" {item}";
                }
            if(!string.IsNullOrEmpty(entities))
                RefIfcEntity = $"Related: {entities}";
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"NodeItem ").Append(Name).Append("{\n");
            foreach (var item in Children)
            {
                sb.Append("  Props: ").Append(item.Name).Append("\n");
            }
            sb.Append("}\n");
            return sb.ToString();
        }
    }

}
