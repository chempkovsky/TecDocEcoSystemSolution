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
    public class EnterpriseBranchWorkPlaceTDESViewModel : ViewModelSateBase<EnterpriseBranchWorkPlaceTDES>, INavigationAware, IRegionMemberLifetime
    {
        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;

        public Guid SearchEntGuid { get; set; }
        public string EntDescription { get; set; }
        public Guid SearchEntBranchGuid { get; set; }
        public string SearchEntBranchDescription { get; set; }


        public EnterpriseBranchWorkPlaceTDESViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "WorkPlaceGuid";

            EnterpriseTDESCommand = new DelegateCommand<object>(this.OnEnterpriseTDESCommand, e => false);
            EnterpriseBranchTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchTDESCommand, e => true);
        }

        protected override void CopyData(EnterpriseBranchWorkPlaceTDES src, EnterpriseBranchWorkPlaceTDES dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.EntGuid = src.EntGuid;
            if (notify) RaisePropertyChanged("EntGuid");
            dest.EntBranchGuid = src.EntBranchGuid;
            if (notify) RaisePropertyChanged("EntBranchGuid");
            dest.WorkPlaceGuid = src.WorkPlaceGuid;
            if (notify) RaisePropertyChanged("WorkPlaceGuid");
            dest.IsActive = src.IsActive;
            if (notify) RaisePropertyChanged("IsActive");
            dest.Description = src.Description;
            if (notify) RaisePropertyChanged("Description");
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
        public Guid WorkPlaceGuid
        {
            get
            {
                return dataCashItem.WorkPlaceGuid;
            }
            set
            {
                if (dataCashItem.WorkPlaceGuid != value)
                {
                    dataCashItem.WorkPlaceGuid = value;
                    IsModified = true;
                    RaisePropertyChanged("WorkPlaceGuid");
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
        public string Description
        {
            get
            {
                return dataCashItem.Description;
            }
            set
            {
                if (dataCashItem.Description != value)
                {
                    dataCashItem.Description = value;
                    IsModified = true;
                    RaisePropertyChanged("Description");
                }
            }
        }

        #region NewCommand
        protected override void OnDoNew()
        {
            WorkPlaceGuid = Guid.NewGuid();
            EntGuid = SearchEntGuid;
            EntBranchGuid = SearchEntBranchGuid;
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.EnterpriseBranchWorkPlaceTDESDelete(CurrentItem.WorkPlaceGuid);
        }
        #endregion
        #region SaveCommand
        protected override EnterpriseBranchWorkPlaceTDES NewInstance()
        {
            return new EnterpriseBranchWorkPlaceTDES();
        }
        protected override EnterpriseBranchWorkPlaceTDES OnDoInser(EnterpriseBranchWorkPlaceTDES item)
        {
            return dataservice.EnterpriseBranchWorkPlaceTDESInsert(item);
        }
        protected override void OnDoUpdate(EnterpriseBranchWorkPlaceTDES item)
        {
            dataservice.EnterpriseBranchWorkPlaceTDESUpdate(item);
        }
        #endregion

        #region searchStaff
        protected override Dictionary<string, string> GetSearchColumnList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("WorkPlaceGuid", displaynames["WorkPlaceGuid"]);
            result.Add("Description", displaynames["Description"]);
            return result;
        }
        #endregion
        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.EnterpriseBranchWorkPlaceTDESIndexCount(this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntBranchGuid);
            Items = new ObservableCollection<EnterpriseBranchWorkPlaceTDES>(dataservice.EnterpriseBranchWorkPlaceTDESIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntBranchGuid));
            RaisePropertyChanged("Items");
        }
        #endregion
        #region Autocomplete
        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.EnterpriseBranchWorkPlaceTDESIndex(0, 15, "WorkPlaceGuid", true, this.SearchColumn, filtercriteria, SearchEntGuid, SearchEntBranchGuid).ToList();
            foreach (var item in aList)
            {
                switch (SearchColumn)
                {
                    case "WorkPlaceGuid":
                        result.Add(new KeyValuePair<string, string>(item.WorkPlaceGuid.ToString(), item.WorkPlaceGuid + " : " + item.Description));
                        break;
                    case "Description":
                        result.Add(new KeyValuePair<string, string>(item.Description, item.WorkPlaceGuid + " : " + item.Description));
                        break;
                    default:
                        result.Add(new KeyValuePair<string, string>(item.WorkPlaceGuid.ToString(), item.WorkPlaceGuid + " : " + item.Description));
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
            string segWorkPlaceGuid = (string)navigationContext.Parameters["SearchWorkPlaceGuid"];

            Guid segEnt_Guid = Guid.Empty;
            Guid segEntBrch_Guid = Guid.Empty;
            Guid segWorkPlaceGuid_Guid = Guid.Empty;

            if (KeepAlive)
            {
                if ((!string.IsNullOrEmpty(segEnt)) && (!string.IsNullOrEmpty(segEntBrch)) && (!string.IsNullOrEmpty(segWorkPlaceGuid)))
                {
                    if ( Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segEntBrch, out segEntBrch_Guid) && Guid.TryParse(segWorkPlaceGuid, out segWorkPlaceGuid_Guid)) 
                    {
                        return (segEnt_Guid==SearchEntGuid) && (segEntBrch_Guid==SearchEntBranchGuid) && (segWorkPlaceGuid_Guid==WorkPlaceGuid);
                    } else {
                        return false;
                    }
                }
            }

            if ((SearchEntGuid==Guid.Empty) || (SearchEntBranchGuid==Guid.Empty))
                return true;
            else
            {
                if ( Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segEntBrch, out segEntBrch_Guid) ) 
                {
                    return (SearchEntGuid==segEnt_Guid) && (SearchEntBranchGuid==segEntBrch_Guid);
                } else {
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
    // EnterpriseBranchWorkPlaceTDES_INDEX
}
