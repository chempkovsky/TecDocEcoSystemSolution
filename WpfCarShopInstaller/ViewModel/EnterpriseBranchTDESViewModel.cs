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

    public class EnterpriseBranchTDESViewModel : ViewModelSateBase<EnterpriseBranchTDES>, INavigationAware, IRegionMemberLifetime
    {
        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;

        public Guid SearchEntGuid { get; set; }
        public string EntDescription { get; set; }

        public EnterpriseBranchTDESViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "EntBranchGuid";

            EnterpriseTDESCommand = new DelegateCommand<object>(this.OnEnterpriseTDESCommand, e => true);
            EnterpriseBranchContactsTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchContactsTDESCommand, e => true);
            EnterpriseBranchUserTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchUserTDESCommand, e => true);
            EnterpriseBranchWorkPlaceTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchWorkPlaceTDESCommand, e => true);
            EnterpriseProductCategoryTDESCommand = new DelegateCommand<object>(this.OnEnterpriseProductCategoryTDESCommand, e => true);
        }

        protected List<BranchType> branchTypes;
        public List<BranchType> BranchTypes
        {
            get
            {
                if (branchTypes == null)
                {
                    branchTypes = // new ObservableCollection<BranchType>();
                        dataservice.BranchTypeIndex(0, 300, "", true).ToList();
                }
                return branchTypes;
            }
        }

        protected override void CopyData(EnterpriseBranchTDES src, EnterpriseBranchTDES dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.EntGuid = src.EntGuid;
            if (notify) RaisePropertyChanged("EntGuid");
            dest.EntBranchGuid = src.EntBranchGuid;
            if (notify) RaisePropertyChanged("EntBranchGuid");
            dest.EntBranchDescription = src.EntBranchDescription;
            if (notify) RaisePropertyChanged("EntBranchDescription");
            dest.IsActive = src.IsActive;
            if (notify) RaisePropertyChanged("IsActive");
            dest.IsVisible = src.IsVisible;
            if (notify) RaisePropertyChanged("IsVisible");
            dest.BranchTypeId = src.BranchTypeId;
            if (notify) RaisePropertyChanged("BranchTypeId");
            dest.TecDocCatalog = src.TecDocCatalog;
            if (notify) RaisePropertyChanged("TecDocCatalog");
            dest.SalesCatalog = src.SalesCatalog;
            if (notify) RaisePropertyChanged("SalesCatalog");
            dest.IncomeCatalog = src.IncomeCatalog;
            if (notify) RaisePropertyChanged("IncomeCatalog");
            dest.OrderCatalog = src.OrderCatalog;
            if (notify) RaisePropertyChanged("OrderCatalog");
            
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
        public Guid EntBranchGuid
        {
            get
            {
                return dataCashItem.EntBranchGuid;
            }
            set
            {
                if (dataCashItem.EntBranchGuid != value)
                {
                    dataCashItem.EntBranchGuid = value;
                    IsModified = true;
                    RaisePropertyChanged("EntBranchGuid");
                }
            }
        }
        public string EntBranchDescription
        {
            get
            {
                return dataCashItem.EntBranchDescription;
            }
            set
            {
                if (dataCashItem.EntBranchDescription != value)
                {
                    dataCashItem.EntBranchDescription = value;
                    IsModified = true;
                    RaisePropertyChanged("EntBranchDescription");
                }
            }
        }
        public string TecDocCatalog
        {
            get
            {
                return dataCashItem.TecDocCatalog;
            }
            set
            {
                if (dataCashItem.TecDocCatalog != value)
                {
                    dataCashItem.TecDocCatalog = value;
                    IsModified = true;
                    RaisePropertyChanged("TecDocCatalog");
                }
            }
        }
        public string SalesCatalog
        {
            get
            {
                return dataCashItem.SalesCatalog;
            }
            set
            {
                if (dataCashItem.SalesCatalog != value)
                {
                    dataCashItem.SalesCatalog = value;
                    IsModified = true;
                    RaisePropertyChanged("SalesCatalog");
                }
            }
        }
        public string IncomeCatalog
        {
            get
            {
                return dataCashItem.IncomeCatalog;
            }
            set
            {
                if (dataCashItem.IncomeCatalog != value)
                {
                    dataCashItem.IncomeCatalog = value;
                    IsModified = true;
                    RaisePropertyChanged("IncomeCatalog");
                }
            }
        }
        public string OrderCatalog
        {
            get
            {
                return dataCashItem.OrderCatalog;
            }
            set
            {
                if (dataCashItem.OrderCatalog != value)
                {
                    dataCashItem.OrderCatalog = value;
                    IsModified = true;
                    RaisePropertyChanged("OrderCatalog");
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
        public bool IsVisible
        {
            get
            {
                return dataCashItem.IsVisible;
            }
            set
            {
                if (dataCashItem.IsVisible != value)
                {
                    dataCashItem.IsVisible = value;
                    IsModified = true;
                    RaisePropertyChanged("IsVisible");
                }
            }
        }
        public int BranchTypeId
        {
            get
            {
                return dataCashItem.BranchTypeId;
            }
            set
            {
                if (dataCashItem.BranchTypeId != value)
                {
                    dataCashItem.BranchTypeId = value;
                    IsModified = true;
                    RaisePropertyChanged("BranchTypeId");
                }
            }
        }

        #region NewCommand
        protected override void OnDoNew()
        {
            EntBranchGuid = Guid.NewGuid();
            EntGuid = SearchEntGuid;
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.EnterpriseBranchTDESDelete(CurrentItem.EntBranchGuid);
        }
        #endregion
        #region SaveCommand
        protected override EnterpriseBranchTDES NewInstance()
        {
            return new EnterpriseBranchTDES();
        }
        protected override EnterpriseBranchTDES OnDoInser(EnterpriseBranchTDES item)
        {
            return dataservice.EnterpriseBranchTDESInsert(item);
        }
        protected override void OnDoUpdate(EnterpriseBranchTDES item)
        {
            dataservice.EnterpriseBranchTDESUpdate(item);
        }
        #endregion

        #region searchStaff
        protected override Dictionary<string, string> GetSearchColumnList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("EntBranchGuid", displaynames["EntBranchGuid"]);
            result.Add("EntBranchDescription", displaynames["EntBranchDescription"]);
            return result;
        }
        #endregion
        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.EnterpriseBranchTDESIndexCount(this.SearchColumn, SearchFilter, SearchEntGuid);
            Items = new ObservableCollection<EnterpriseBranchTDES>(dataservice.EnterpriseBranchTDESIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter, SearchEntGuid));
            RaisePropertyChanged("Items");
        }
        #endregion
        #region Autocomplete
        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.EnterpriseBranchTDESIndex(0, 15, "EntBranchGuid", true, this.SearchColumn, filtercriteria, SearchEntGuid).ToList();
            foreach (var item in aList)
            {
                switch (SearchColumn)
                {
                    case "EntBranchGuid":
                        result.Add(new KeyValuePair<string, string>(item.EntBranchGuid.ToString(), item.EntBranchGuid + " : " + item.EntBranchDescription ));
                        break;
                    case "EntBranchDescription":
                        result.Add(new KeyValuePair<string, string>(item.EntBranchDescription, item.EntBranchGuid + " : " + item.EntBranchDescription));
                        break;
                    default:
                        result.Add(new KeyValuePair<string, string>(item.EntBranchGuid.ToString(), item.EntBranchGuid + " : " + item.EntBranchDescription));
                        break;
                }
            }
            return result;
        }
        #endregion
        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            string segEnt = (string)navigationContext.Parameters["SearchEntGuid"];
            string segEntBrch = (string)navigationContext.Parameters["SearchEntBranchGuid"];

            Guid segEnt_Guid = Guid.Empty;
            Guid segEntBrch_Guid = Guid.Empty;


            if (KeepAlive)
            {
                if ((!string.IsNullOrEmpty(segEnt)) && (!string.IsNullOrEmpty(segEntBrch)))
                {
                    if (Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segEntBrch, out segEntBrch_Guid) )
                    {
                        return (segEnt_Guid == SearchEntGuid) && (segEntBrch_Guid == EntBranchGuid);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (SearchEntGuid == Guid.Empty)
                return true;
            else
            {
                if (Guid.TryParse(segEnt, out segEnt_Guid) )
                {
                    return (SearchEntGuid == segEnt_Guid) ;
                }
                else
                {
                    return false;
                }
            }


        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            Guid x;
            if (Guid.TryParse((string)navigationContext.Parameters["SearchEntGuid"], out x))
                this.SearchEntGuid = x;
            else
                this.SearchEntGuid = Guid.Empty;
            this.EntDescription = (string)navigationContext.Parameters["EntDescription"];
            KeepAlive = false;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion
        #region IRegionMemberLifetime
        public bool KeepAlive { get; set; }
        #endregion
        #region EnterpriseTDESCommand
        public ICommand EnterpriseTDESCommand { get; set; }
        protected void OnEnterpriseTDESCommand(object arg)
        {
            KeepAlive = false;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

        #region EnterpriseTDESCommand
        public ICommand EnterpriseBranchContactsTDESCommand { get; set; }
        protected void OnEnterpriseBranchContactsTDESCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            query.Add("SearchEntBranchGuid", this.EntBranchGuid.ToString());
            query.Add("SearchEntBranchDescription", this.EntBranchDescription);

            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseBranchContactsTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

        #region EnterpriseBranchUserTDESCommand
        public ICommand EnterpriseBranchUserTDESCommand { get; set; }
        protected void OnEnterpriseBranchUserTDESCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            query.Add("SearchEntBranchGuid", this.EntBranchGuid.ToString());
            query.Add("SearchEntBranchDescription", this.EntBranchDescription);

            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseBranchUserTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion


        #region EnterpriseBranchWorkPlaceTDESCommand
        public ICommand EnterpriseBranchWorkPlaceTDESCommand { get; set; }
        protected void OnEnterpriseBranchWorkPlaceTDESCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            query.Add("SearchEntBranchGuid", this.EntBranchGuid.ToString());
            query.Add("SearchEntBranchDescription", this.EntBranchDescription);

            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseBranchWorkPlaceTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

        #region EnterpriseProductCategoryTDESCommand
        public ICommand EnterpriseProductCategoryTDESCommand { get; set; }
        protected void OnEnterpriseProductCategoryTDESCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            query.Add("SearchEntBranchGuid", this.EntBranchGuid.ToString());
            query.Add("SearchEntBranchDescription", this.EntBranchDescription);

            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseProductCategoryTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

        
    }
}
