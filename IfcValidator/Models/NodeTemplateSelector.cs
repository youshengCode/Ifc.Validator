using IfcValidator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IfcValidator.Models
{
    public class NodeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ClassTemplate { get; set; }
        public DataTemplate PropSetTemplate { get; set; }
        public DataTemplate PropTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            var nodeItem = (NodeItem)item;
            switch (nodeItem.Type)
            {
                case NodeItem.NodeItemType.Classification:
                    return ClassTemplate;
                case NodeItem.NodeItemType.PropertySet:
                    return PropSetTemplate;
                case NodeItem.NodeItemType.Property:
                    return PropTemplate;
                default:
                    return null;
            }
        }
    }
}
