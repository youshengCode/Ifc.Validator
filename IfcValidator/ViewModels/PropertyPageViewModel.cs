using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private BindableCollection<PropNode> _classes = new BindableCollection<PropNode>();
        public BindableCollection<PropNode> Classes
        {
            get { return _classes; }
            set { _classes = value; }
        }

        private BindableCollection<PropNode> _selectedClasses = new BindableCollection<PropNode>();
        public BindableCollection<PropNode> SelectedClasses
        {
            get { return _selectedClasses; }
            set { _selectedClasses = value; }
        }


        #region Initiation
        public PropertyPageViewModel()
        {
            //string namespaceUrl = "http://identifier.buildingsmart.org/uri/buildingsmart/ifc-4.3/class/IfcBridge";
            string namespaceUrl = "http://identifier.buildingsmart.org/uri/buildingsmart/ifc-4.3/class/IfcWall";
            LoadClassification(namespaceUrl);
        }
        #endregion

        private void LoadClassification(string namespaceUrl, string languageCode = null, bool? includChild = false)
        {
            ClassificationContractV2 response = new ClassificationApi(LocalData.baseHttp).
                ApiClassificationV2Get(namespaceUrl, languageCode, includChild);
            _classes.Add(new PropNode(response));
            //Debug.WriteLine(response.Name);
            //foreach (var item in response.ClassificationProperties)
            //    Debug.WriteLine(item.Name);
        }
    }
}
