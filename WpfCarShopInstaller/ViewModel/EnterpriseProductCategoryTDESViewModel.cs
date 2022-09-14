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
    public class EnterpriseProductCategoryTDESViewModel : ViewModelSateBase<EnterpriseProductCategoryTDES>, INavigationAware, IRegionMemberLifetime
    {
        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;

        public Guid SearchEntGuid { get; set; }
        public string EntDescription { get; set; }
        public Guid SearchEntBranchGuid { get; set; }
        public string SearchEntBranchDescription { get; set; }

        public EnterpriseProductCategoryTDESViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "PCId";

            EnterpriseTDESCommand = new DelegateCommand<object>(this.OnEnterpriseTDESCommand, e => false);
            EnterpriseBranchTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchTDESCommand, e => true);
        }

        protected override void CopyData(EnterpriseProductCategoryTDES src, EnterpriseProductCategoryTDES dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.EntGuid = src.EntGuid;
            if (notify) RaisePropertyChanged("EntGuid");
            dest.EntBranchGuid = src.EntBranchGuid;
            if (notify) RaisePropertyChanged("EntBranchGuid");
            dest.PCId = src.PCId;
            if (notify) RaisePropertyChanged("PCId");
            dest.PCDescription = src.PCDescription;
            if (notify) RaisePropertyChanged("PCDescription");
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
        public int PCId
        {
            get
            {
                return dataCashItem.PCId;
            }
            set
            {
                if (dataCashItem.PCId != value)
                {
                    dataCashItem.PCId = value;
                    IsModified = true;
                    RaisePropertyChanged("PCId");
                }
            }
        }
        public string PCDescription
        {
            get
            {
                return dataCashItem.PCDescription;
            }
            set
            {
                if (dataCashItem.PCDescription != value)
                {
                    dataCashItem.PCDescription = value;
                    IsModified = true;
                    RaisePropertyChanged("PCDescription");
                }
            }
        }

        #region NewCommand
        protected override void OnDoNew()
        {
            PCId = -1;
            EntGuid = SearchEntGuid;
            EntBranchGuid = SearchEntBranchGuid;
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.EnterpriseProductCategoryTDESDelete(CurrentItem.PCId, CurrentItem.EntBranchGuid);
        }
        #endregion
        #region SaveCommand
        protected override EnterpriseProductCategoryTDES NewInstance()
        {
            return new EnterpriseProductCategoryTDES();
        }
        protected override EnterpriseProductCategoryTDES OnDoInser(EnterpriseProductCategoryTDES item)
        {
            return dataservice.EnterpriseProductCategoryTDESInsert(item);
        }
        protected override void OnDoUpdate(EnterpriseProductCategoryTDES item)
        {
            dataservice.EnterpriseProductCategoryTDESUpdate(item);
        }
        #endregion
        #region searchStaff
        protected override Dictionary<string, string> GetSearchColumnList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("PCId", displaynames["PCId"]);
            result.Add("PCDescription", displaynames["PCDescription"]);
            return result;
        }
        #endregion
        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.EnterpriseProductCategoryTDESIndexCount(this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntBranchGuid);
            Items = new ObservableCollection<EnterpriseProductCategoryTDES>(dataservice.EnterpriseProductCategoryTDESIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntBranchGuid));
            RaisePropertyChanged("Items");
        }
        #endregion
        #region Autocomplete
        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.EnterpriseProductCategoryTDESIndex(0, 15, "PCId", true, this.SearchColumn, filtercriteria, SearchEntGuid, SearchEntBranchGuid).ToList();
            foreach (var item in aList)
            {
                switch (SearchColumn)
                {
                    case "PCId":
                        result.Add(new KeyValuePair<string, string>(item.PCId.ToString(), item.PCId.ToString() + " : " + item.PCDescription));
                        break;
                    case "PCDescription":
                        result.Add(new KeyValuePair<string, string>(item.PCDescription, item.PCId.ToString() + " : " + item.PCDescription));
                        break;
                    default:
                        result.Add(new KeyValuePair<string, string>(item.PCId.ToString(), item.PCId.ToString() + " : " + item.PCDescription));
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
            string segPCId = (string)navigationContext.Parameters["SearchPCId"];

            Guid segEnt_Guid = Guid.Empty;
            Guid segEntBrch_Guid = Guid.Empty;


            if (KeepAlive)
            {
                if ((!string.IsNullOrEmpty(segEnt)) && (!string.IsNullOrEmpty(segEntBrch)) && (!string.IsNullOrEmpty(segPCId)))
                {
                    if (Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segEntBrch, out segEntBrch_Guid))
                    {
                        return (segEnt_Guid == SearchEntGuid) && (segEntBrch_Guid == SearchEntBranchGuid) && segPCId.Equals(PCId.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if ((SearchEntGuid == Guid.Empty) || (SearchEntBranchGuid == Guid.Empty))
                return true;
            else
            {
                if (Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segEntBrch, out segEntBrch_Guid))
                {
                    return (SearchEntGuid == segEnt_Guid) && (SearchEntBranchGuid == segEntBrch_Guid);
                }
                else
                {
                    return false;
                }
            }
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!KeepAlive)
            {
                Guid x;
                if (Guid.TryParse((string)navigationContext.Parameters["SearchEntGuid"], out x))
                    this.SearchEntGuid = x;
                else
                    this.SearchEntGuid = Guid.Empty;
                this.EntDescription = (string)navigationContext.Parameters["EntDescription"];
                if (Guid.TryParse((string)navigationContext.Parameters["SearchEntBranchGuid"], out x))
                    this.SearchEntBranchGuid = x;
                else
                    this.SearchEntBranchGuid = Guid.Empty;
                this.SearchEntBranchDescription = (string)navigationContext.Parameters["SearchEntBranchDescription"];
            }
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


            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion
        #region EnterpriseBranchTDESCommand
        public ICommand EnterpriseBranchTDESCommand { get; set; }
        protected void OnEnterpriseBranchTDESCommand(object arg)
        {
            KeepAlive = false;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            query.Add("SearchEntBranchGuid", this.SearchEntBranchGuid.ToString());
            query.Add("SearchEntBranchDescription", this.SearchEntBranchDescription);


            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseBranchTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

    }
}
