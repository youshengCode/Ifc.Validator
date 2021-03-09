using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IfcValidator.Core.Models
{
    public class PropNode
    {
        public PropNode(ClassificationContractV2 classification)
        {
            Classification = classification;
            Name = classification.Name;
            IsSelected = false;
            GetChildrenNode();
        }
        public PropNode(string name)
        {
            Name = name;
            IsSelected = false;
        }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public ClassificationContractV2 Classification { get; set; }
        public List<PropNode> Properties { get; set; }

        public void GetChildrenNode()
        {
            if (Properties == null)
                Properties = new List<PropNode>();
            foreach (var item in Classification.ClassificationProperties)
            {
                Properties.Add(new PropNode(item.Name));
            }
        }
    }
}
