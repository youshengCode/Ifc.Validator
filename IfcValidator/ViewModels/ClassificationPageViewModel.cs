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
    public class ClassificationPageViewModel : Screen
    {
        #region Domain prop
        private Dictionary<string, DisplayDomain> DomainDic = new Dictionary<string, DisplayDomain>();

        private BindableCollection<string> _domains = new BindableCollection<string>();
        public BindableCollection<string> Domains
        {
            get { return _domains; }
            set { _domains = value; }
        }
        private string _selectedDomain = null;
        public string SelectedDomain
        {
            get { return _selectedDomain; }
            set { _selectedDomain = value; }
        }
        #endregion

        #region Language prop
        private Dictionary<string, string> LanguageDic = new Dictionary<string, string>();

        private BindableCollection<string> _languages = new BindableCollection<string>();
        public BindableCollection<string> Languages
        {
            get { return _languages; }
            set { _languages = value; }
        }
        private string _defaultLanguage = null;
        public string DefaultLanguage
        {
            get { return _defaultLanguage; }
            set { _defaultLanguage = value; NotifyOfPropertyChange(() => DefaultLanguage); }
        }
        private string _selectedLanguage = null;
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { _selectedLanguage = value; }
        }
        #endregion

        #region Classification prop
        private BindableCollection<ClassificationSearchResultContractV2> _classifications = new BindableCollection<ClassificationSearchResultContractV2>();
        public BindableCollection<ClassificationSearchResultContractV2> Classifications
        {
            get { return _classifications; }
            set { _classifications = value; }
        }
        private string _classificationNotice = null;
        public string ClassificationNotice
        {
            get { return _classificationNotice; }
            set { _classificationNotice = value; NotifyOfPropertyChange(() => ClassificationNotice); }
        }
        private string _searchText = null;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; NotifyOfPropertyChange(() => SearchText); }
        }
        private BindableCollection<ClassificationSearchResultContractV2> _selectedClasses= new BindableCollection<ClassificationSearchResultContractV2>();
        public BindableCollection<ClassificationSearchResultContractV2> SelectedClasses
        {
            get { return _selectedClasses; }
            set { _selectedClasses = value; NotifyOfPropertyChange(() => SelectedClasses); }
        }
        private string _selectedNotice = null;
        public string SelectedNotice
        {
            get { return _selectedNotice; }
            set { _selectedNotice = value; NotifyOfPropertyChange(() => SelectedNotice); }
        }
        private bool _hasSelection = false;
        public bool HasSelection
        {
            get { return _hasSelection; }
            set { _hasSelection = value; NotifyOfPropertyChange(() => HasSelection); }
        }

        #endregion

        #region Initiation
        public ClassificationPageViewModel()
        {
            LoadDomain();
            LoadLanguages();
        }
        private void LoadDomain()
        {
            List<DomainContractV2> allDomains = new DomainApi(LocalData.baseHttp).ApiDomainV2Get();
            List<string> allDisplayNames = new List<string>();
            foreach (var item in allDomains)
            {
                DisplayDomain displayDomain = new DisplayDomain(item);
                DomainDic.Add(displayDomain.DisplayName, displayDomain);
                allDisplayNames.Add(displayDomain.DisplayName);
            }
            _domains = new BindableCollection<string>(allDisplayNames);
        }
        private void LoadLanguages()
        {
            var response = new LanguageApi(LocalData.baseHttp).ApiLanguageV1Get();
            List<string> languages = new List<string>();
            languages.Add(ResourceExtensions.GetLocalized("ClassificationPage_AllLanguage"));
            foreach (var item in response)
            {
                LanguageDic.Add(item.IsoCode, item.Name);
                languages.Add($"{item.Name} ({item.IsoCode})");
            }
            _languages = new BindableCollection<string>(languages);
        }
        #endregion

        #region Load Classification
        public BindableCollection<ClassificationSearchResultContractV2> LoadClassification(string languageCode = null, string searchText = null)
        {
            if (!string.IsNullOrEmpty(SelectedDomain))
            {
                _classifications = new BindableCollection<ClassificationSearchResultContractV2>();
                string domainNamespaceUrl = DomainDic[SelectedDomain].Domain.NamespaceUri;
                var response = new SearchListOpenApi(LocalData.baseHttp).
                    ApiSearchListOpenV2Get(domainNamespaceUrl, searchText, languageCode, null);
                UpdateClassificationNotice(response.NumberOfClassificationsFound);
                foreach (var item in response.Domains)
                {
                    _classifications.AddRange(item.Classifications);
                }
                //Debug.WriteLine(_classifications.Count);
                return _classifications;
            }
            return null;
        }
        public void SelecteClasses(IList<ClassificationSearchResultContractV2> objects)
        {
            foreach (var item in objects)
                if (!_selectedClasses.Contains(item))
                    _selectedClasses.Add(item);
            UpdateSelecedClassesNotice();
        }
        public void RemoveSelecteClasses(IList<ClassificationSearchResultContractV2> objects)
        {
            foreach (var item in objects)
                if (_selectedClasses.Contains(item))
                    _selectedClasses.Remove(item);
            UpdateSelecedClassesNotice();
        }
        public void RemoveAllClasses()
        {
            _selectedClasses.Clear();
            UpdateSelecedClassesNotice();
        }
        public string GetSelectLanguageCode()
        {
            if (_selectedLanguage == null)
                return null;
            else if( _selectedLanguage.Equals(ResourceExtensions.GetLocalized("ClassificationPage_AllLanguage")))
                return null;
            else
            {
                Regex regex = new Regex(@"(?is)(?<=\()(.*)(?=\))");
                return regex.Match(_selectedLanguage).Value;
            }
        }
        public void UpdateDefaultLanguage()
        {
            if (!string.IsNullOrEmpty(SelectedDomain))
            {
                string languangeCode = DomainDic[SelectedDomain].Domain.DefaultLanguageCode;
                DefaultLanguage = $"{ResourceExtensions.GetLocalized("ClassificationPage_DefaultLanguage")}: {LanguageDic[languangeCode]}";
            }
        }
        private void UpdateClassificationNotice(int? number)
        {
            if(number != null)
            {
                if (number >= 200)
                    ClassificationNotice = ResourceExtensions.GetLocalized("ClassificationPage_MoreThan200");
                else if (number == 0)
                    ClassificationNotice = ResourceExtensions.GetLocalized("ClassificationPage_NoResult");
                else
                    ClassificationNotice = $"{ResourceExtensions.GetLocalized("ClassificationPage_Total")}: {number}";
            }
        }
        private void UpdateSelecedClassesNotice()
        {
            if (_selectedClasses.Count > 0)
            {
                SelectedNotice = $"{ResourceExtensions.GetLocalized("ClassificationPage_Selected")}: {_selectedClasses.Count}";
                HasSelection = true;
            }
            else
            {
                SelectedNotice = null;
                HasSelection = false;
            }
        }
        #endregion
    }
}
