using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IfcValidator.Core.Models;
using Xbim.Ifc;
using Serilog;
using Xbim.Common;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.Interfaces;
using System.Diagnostics;
using System.IO;

namespace IfcValidator.Core.Services
{
    public class IfcPropertyAnalyse
    {
        public static List<NodeItem> PropertyAnalyse(string filePath, List<NodeItem> nodes)
        {
            IfcStore.ModelProviderFactory.UseMemoryModelProvider();
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .CreateLogger();
            XbimLogging.LoggerFactory.AddSerilog();
            Debug.WriteLine($"{filePath} in analysing");

            using (var model = IfcStore.Open(filePath))
            {
                //Debug.WriteLine("IFC Schema", model.SchemaVersion.ToString());
                var products = model.Instances.OfType<IfcProduct>();
                foreach (var product in products)
                    AnalyseProperties(product, nodes);
            }
            //foreach (var item in nodes)
            //    Debug.WriteLine(item.ToString());
            return nodes;
        }

        private static void AnalyseProperties(IfcProduct ifcProduct, List<NodeItem> nodes)
        {
            string entityName = GetEntityNameFromType(ifcProduct.GetType().ToString());
            if (nodes.Any(n => n.Name == entityName))
            {
                NodeItem classNode = nodes.Where(n => n.Name == entityName && n.Type == NodeItem.NodeItemType.Classification).FirstOrDefault();
                classNode.ExistCount++;
                CountExistInPset(ifcProduct, classNode);
                CountExistInQto(ifcProduct, classNode);
            }
        }

        private static void CountExistInQto(IfcProduct ifcProduct, NodeItem classNode)
        {
            var ifcPropertySets = ifcProduct.IsDefinedBy
                .Where(r => r.RelatingPropertyDefinition is IIfcElementQuantity)
                .Select(r => ((IIfcElementQuantity)r.RelatingPropertyDefinition));
            foreach (var ifcPropertySet in ifcPropertySets)
            {
                if (classNode.Children.Any(ps => ps.Name == ifcPropertySet.Name))
                {
                    NodeItem propSetNode = classNode.Children.Where(ps => ps.Name == ifcPropertySet.Name).FirstOrDefault();
                    List<IIfcPhysicalQuantity> ifcProperties = new List<IIfcPhysicalQuantity>();
                    // Remove duplication
                    foreach (var ifcProperty in ifcPropertySet.Quantities)
                        if (!ifcProperties.Any(p => p.Name == ifcProperty.Name))
                            ifcProperties.Add(ifcProperty);
                    foreach (var ifcProperty in ifcProperties)
                    {
                        if (propSetNode.Children.Any(p => p.Name == ifcProperty.Name))
                        {
                            NodeItem propNode = propSetNode.Children.Where(p => p.Name == ifcProperty.Name).FirstOrDefault();
                            propNode.ExistCount++;
                        }
                    }
                }
            }
        }

        private static void CountExistInPset(IfcProduct ifcProduct, NodeItem classNode)
        {
            var ifcPropertySets = ifcProduct.IsDefinedBy
                .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                .Select(r => ((IIfcPropertySet)r.RelatingPropertyDefinition));
            foreach (var ifcPropertySet in ifcPropertySets)
            {
                if (classNode.Children.Any(ps => ps.Name == ifcPropertySet.Name))
                {
                    NodeItem propSetNode = classNode.Children.Where(ps => ps.Name == ifcPropertySet.Name).FirstOrDefault();
                    List<IIfcProperty> ifcProperties = new List<IIfcProperty>();
                    // Remove duplication
                    foreach (var ifcProperty in ifcPropertySet.HasProperties)
                        if (!ifcProperties.Any(p => p.Name == ifcProperty.Name))
                            ifcProperties.Add(ifcProperty);
                    foreach (var ifcProperty in ifcProperties)
                    {
                        if (propSetNode.Children.Any(p => p.Name == ifcProperty.Name))
                        {
                            NodeItem propNode = propSetNode.Children.Where(p => p.Name == ifcProperty.Name).FirstOrDefault();
                            propNode.ExistCount++;
                        }
                    }
                }
            }
        }

        private static string GetEntityNameFromType(string str)
        {
            Regex regex = new Regex(@"(?=.)\w+$");
            return regex.Match(str).Value;
        }
    }
}
