using CarShopDataService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TecDocEcoSystemDbClassLibrary;
using CarShopDataService.Interfaces;
using Prism.Regions;
using Prism.Commands;

namespace WpfCarShopInstaller.ViewModel
{
    public class EnterpriseTDESViewModel : ViewModelSateBase<EnterpriseTDES>, INavigationAware, IRegionMemberLifetime
    {
        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;

        public EnterpriseTDESViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "EntGuid";

            EnterpriseUserTDESCommand = new DelegateCommand<object>(this.OnEnterpriseUserTDESCommand, e => true);
            EnterpriseBranchTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchTDESCommand, e => true);
            EnterpriseAddressTDESCommand = new DelegateCommand<object>(this.OnEnterpriseAddressTDESCommand, e => true);
            LoadCategoryTreeViewCommand = new DelegateCommand<object>(this.OnLoadCategoryTreeViewCommand, e => true);
        }

        protected List<EnterpriseTecDocSrcTypeTDES> enterprisetecdocsrctype = null;
        public List<EnterpriseTecDocSrcTypeTDES> EnterpriseTecDocSrcType
        {
            get
            {
                if (enterprisetecdocsrctype == null)
                {
                    enterprisetecdocsrctype = // new ObservableCollection<BranchType>();
                        dataservice.EnterpriseTecDocSrcTypeTDESIndex(0, 300, "", true).ToList();
                }
                return enterprisetecdocsrctype;
            }
        }


        protected override void CopyData(EnterpriseTDES src, EnterpriseTDES dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.EntGuid = src.EntGuid;
            if (notify) RaisePropertyChanged("EntGuid");
            dest.EntDescription = src.EntDescription;
            if (notify) RaisePropertyChanged("EntDescription");
            dest.IsActive = src.IsActive;
            if (notify) RaisePropertyChanged("IsActive");
            dest.ArticleCatalog = src.ArticleCatalog;
            if (notify) RaisePropertyChanged("ArticleCatalog");
            dest.TecDocSrcTypeId = src.TecDocSrcTypeId;
            if (notify) RaisePropertyChanged("TecDocSrcTypeId");
        }

        public Guid EntGuid
        {
            get
            {
                return dataCashItem.EntGuid;
            }
            set
            {
                if (dataCashItem.EntGuid != value)
                {
                    dataCashItem.EntGuid = value;
                    IsModified = true;
                    RaisePropertyChanged("EntGuid");
                }
            }
        }
        public string EntDescription
        {
            get
            {
                return dataCashItem.EntDescription;
            }
            set
            {
                if (dataCashItem.EntDescription != value)
                {
                    dataCashItem.EntDescription = value;
                    IsModified = true;
                    RaisePropertyChanged("EntDescription");
                }
            }
        }
        public bool IsActive
        {
            get
            {
                return dataCashItem.IsActive;
            }
            set
            {
                if (dataCashItem.IsActive != value)
                {
                    dataCashItem.IsActive = value;
                    IsModified = true;
                    RaisePropertyChanged("IsActive");
                }
            }
        }
        public string ArticleCatalog
        {
            get
            {
                return dataCashItem.ArticleCatalog;
            }
            set
            {
                if (dataCashItem.ArticleCatalog != value)
                {
                    dataCashItem.ArticleCatalog = value;
                    IsModified = true;
                    RaisePropertyChanged("ArticleCatalog");
                }
            }
        }
        public int TecDocSrcTypeId
        {
            get
            {
                return dataCashItem.TecDocSrcTypeId;
            }
            set
            {
                if (dataCashItem.TecDocSrcTypeId != value)
                {
                    dataCashItem.TecDocSrcTypeId = value;
                    IsModified = true;
                    RaisePropertyChanged("TecDocSrcTypeId");
                }
            }
        }


        #region NewCommand
        protected override void OnDoNew()
        {
            EntGuid = Guid.NewGuid();
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.EnterpriseTDESDelete(CurrentItem.EntGuid);
        }
        #endregion
        #region SaveCommand
        protected override EnterpriseTDES NewInstance()
        {
            return new EnterpriseTDES();
        }
        protected override EnterpriseTDES OnDoInser(EnterpriseTDES item)
        {
            return dataservice.EnterpriseTDESInsert(item);
        }
        protected override void OnDoUpdate(EnterpriseTDES item)
        {
            dataservice.EnterpriseTDESUpdate(item);
        }
        #endregion
        #region searchStaff
        protected override Dictionary<string, string> GetSearchColumnList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("EntGuid", displaynames["EntGuid"]);
            result.Add("EntDescription", displaynames["EntDescription"]);
            return result;
        }
        #endregion
        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.EnterpriseTDESIndexCount(this.SearchColumn, SearchFilter);
            Items = new ObservableCollection<EnterpriseTDES>(dataservice.EnterpriseTDESIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter));
            RaisePropertyChanged("Items");
        }
        #endregion
        #region Autocomplete

        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.EnterpriseTDESIndex(0, 15, "EntGuid", true, this.SearchColumn, filtercriteria).ToList();
            var keyIsId = "EntGuid".Equals(this.SearchColumn);
            foreach (var item in aList)
            {
                if (keyIsId)
                {
                    result.Add(new KeyValuePair<string, string>(item.EntGuid.ToString(), item.EntGuid + " : " + item.EntDescription));
                }
                else
                {
                    result.Add(new KeyValuePair<string, string>(item.EntDescription, item.EntGuid + " : " + item.EntDescription));
                }
            }
            return result;
        }
        #endregion

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {

            if (EntGuid == Guid.Empty)
                return true;
            else
            {
                Guid x;
                if (Guid.TryParse((string)navigationContext.Parameters["SearchEntGuid"], out x))
                {
                    return EntGuid == x;
                }

                return false;

            }
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            KeepAlive = false;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion
        #region IRegionMemberLifetime
        public bool KeepAlive  { get; set; }
        #endregion
        #region EnterpriseUserTDESCommand
        public ICommand EnterpriseUserTDESCommand { get; set; }

        protected void OnEnterpriseUserTDESCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.EntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);

            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseUserTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

        
        #region EnterpriseBranchTDESCommand
        public ICommand EnterpriseBranchTDESCommand { get; set; }
        protected void OnEnterpriseBranchTDESCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.EntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);

            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseBranchTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

        
        #region EnterpriseAddressTDESCommand
        public ICommand EnterpriseAddressTDESCommand { get; set; }
        protected void OnEnterpriseAddressTDESCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.EntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);

            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseAddressTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

        #region LoadCategoryTreeViewCommand
        public ICommand LoadCategoryTreeViewCommand { get; set; }
        protected void OnLoadCategoryTreeViewCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.EntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            query.Add("EntContext", this.ArticleCatalog);

            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("LoadCategoryTreeView" + query.ToString(), UriKind.Relative));
        }

        #endregion
    }
}
