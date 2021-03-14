using IfcValidator.Core.Models;
using IfcValidator.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace IfcValidator.Models
{
    public class ReportCard
    {
        public ReportCard(string fileName, string originalInfo, string traitedInfo, string keyword, string keywordDescription)
        {
            FileName = fileName;
            OriginalInfo = originalInfo;
            TraitedInfo = traitedInfo;
            Keyword = keyword;
            KeywordDescription = keywordDescription;
        }
        public ReportCard(List<NodeItem> nodes, string fileName = null)
        {
            FileName = fileName;
            RelatedNodes = nodes;
            GetReport(nodes);
        }
        public string FileName { get; set; }
        public string OriginalInfo { get; set; }
        public string TraitedInfo { get; set; }
        public string Keyword { get; set; }
        public string KeywordDescription { get; set; }
        public List<NodeItem> RelatedNodes { get; set; }

        private void GetReport(List<NodeItem> nodes)
        {
            int entityFound = GetEntityCount(nodes);
            int realPropTotal = 0;
            int idealPropTotal = 0;
            decimal percentage;
            foreach (var node in nodes)
            {
                realPropTotal += GetRealPropCount(node);
                idealPropTotal += GetIdealPropCount(node);
            }
            if (realPropTotal != 0 && idealPropTotal != 0)
                percentage = Math.Round(((decimal)realPropTotal / idealPropTotal) * 100, 2);
            else
                percentage = 0;
            Keyword = $"{percentage}%";
            KeywordDescription = $"{ResourceExtensions.GetLocalized("ReportInfo_Presence")}";
            OriginalInfo = $"{entityFound} {ResourceExtensions.GetLocalized("ReportInfo_EntityFound")}";
            TraitedInfo = $"{realPropTotal} {ResourceExtensions.GetLocalized("ReportInfo_PropertyFound")}";
        }

        private int GetEntityCount(List<NodeItem> nodes)
        {
            int entityFound = 0;
            foreach (var node in nodes)
                entityFound += node.ExistCount;
            return entityFound;
        }
        private int GetRealPropCount(NodeItem classNode)
        {
            int propRealCount = 0;
            foreach (var propSetNode in classNode.Children)
                foreach (var propNode in propSetNode.Children)
                    propRealCount += propNode.ExistCount;
            return propRealCount;
        }
        private int GetIdealPropCount(NodeItem classNode)
        {
            int propIdealCount = 0;
            foreach (var propSetNode in classNode.Children)
                propIdealCount += propSetNode.Children.Count;
            return propIdealCount * classNode.ExistCount;
        }
    }
}
