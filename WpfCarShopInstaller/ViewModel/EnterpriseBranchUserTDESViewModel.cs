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
    // EnterpriseUserTDES
    public class EnterpriseBranchUserTDESViewModel : ViewModelSateBase<EnterpriseBranchUserTDES>, INavigationAware, IRegionMemberLifetime
    {
        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;
        public Guid SearchEntBranchGuid { get; set; }
        public string SearchEntBranchDescription { get; set; }

        public Guid SearchEntGuid { get; set; }
        public string EntDescription { get; set; }

        public EnterpriseBranchUserTDESViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "EntUserNic";

            EnterpriseTDESCommand = new DelegateCommand<object>(this.OnEnterpriseTDESCommand, e => false);
            EnterpriseBranchTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchTDESCommand, e => true);
            EnterpriseBranchUserContactTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchUserContactTDESCommand, e => true);
        }

        protected override void CopyData(EnterpriseBranchUserTDES src, EnterpriseBranchUserTDES dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.EntGuid = src.EntGuid;
            if (notify) RaisePropertyChanged("EntGuid");
            dest.EntBranchGuid = src.EntBranchGuid;
            if (notify) RaisePropertyChanged("EntBranchGuid");
            dest.EntUserNic = src.EntUserNic;
            if (notify) RaisePropertyChanged("EntUserNic");
            dest.Password = src.Password;
            if (notify) RaisePropertyChanged("Password");
            dest.FirstName = src.FirstName;
            if (notify) RaisePropertyChanged("FirstName");
            dest.LastName = src.LastName;
            if (notify) RaisePropertyChanged("LastName");
            dest.MiddleName = src.MiddleName;
            if (notify) RaisePropertyChanged("MiddleName");
            dest.IsActive = src.IsActive;
            if (notify) RaisePropertyChanged("IsActive");
            dest.IsAdmin = src.IsAdmin;
            if (notify) RaisePropertyChanged("IsAdmin");
            dest.IsAudit = src.IsAudit;
            if (notify) RaisePropertyChanged("IsAudit");
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
        public string EntUserNic
        {
            get
            {
                return dataCashItem.EntUserNic;
            }
            set
            {
                if (dataCashItem.EntUserNic != value)
                {
                    dataCashItem.EntUserNic = value;
                    IsModified = true;
                    RaisePropertyChanged("EntUserNic");
                }
            }
        }
        public string Password
        {
            get
            {
                return dataCashItem.Password;
            }
            set
            {
                if (dataCashItem.Password != value)
                {
                    dataCashItem.Password = value;
                    IsModified = true;
                    RaisePropertyChanged("Password");
                }
            }
        }
        public string FirstName
        {
            get
            {
                return dataCashItem.FirstName;
            }
            set
            {
                if (dataCashItem.FirstName != value)
                {
                    dataCashItem.FirstName = value;
                    IsModified = true;
                    RaisePropertyChanged("FirstName");
                }
            }
        }
        public string LastName
        {
            get
            {
                return dataCashItem.LastName;
            }
            set
            {
                if (dataCashItem.LastName != value)
                {
                    dataCashItem.LastName = value;
                    IsModified = true;
                    RaisePropertyChanged("LastName");
                }
            }
        }
        public string MiddleName
        {
            get
            {
                return dataCashItem.MiddleName;
            }
            set
            {
                if (dataCashItem.MiddleName != value)
                {
                    dataCashItem.MiddleName = value;
                    IsModified = true;
                    RaisePropertyChanged("MiddleName");
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
        public bool IsAdmin
        {
            get
            {
                return dataCashItem.IsAdmin;
            }
            set
            {
                if (dataCashItem.IsAdmin != value)
                {
                    dataCashItem.IsAdmin = value;
                    IsModified = true;
                    RaisePropertyChanged("IsAdmin");
                }
            }
        }
        public bool IsAudit
        {
            get
            {
                return dataCashItem.IsAudit;
            }
            set
            {
                if (dataCashItem.IsAudit != value)
                {
                    dataCashItem.IsAudit = value;
                    IsModified = true;
                    RaisePropertyChanged("IsAudit");
                }
            }
        }

        #region NewCommand
        protected override void OnDoNew()
        {
            EntUserNic = Guid.NewGuid().ToString();
            EntGuid = SearchEntGuid;
            EntBranchGuid = SearchEntBranchGuid;
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.EnterpriseBranchUserTDESDelete(CurrentItem.EntUserNic);
        }
        #endregion
        #region SaveCommand
        protected override EnterpriseBranchUserTDES NewInstance()
        {
            return new EnterpriseBranchUserTDES();
        }
        protected override EnterpriseBranchUserTDES OnDoInser(EnterpriseBranchUserTDES item)
        {
            return dataservice.EnterpriseBranchUserTDESInsert(item);
        }
        protected override void OnDoUpdate(EnterpriseBranchUserTDES item)
        {
            dataservice.EnterpriseBranchUserTDESUpdate(item);
        }
        #endregion
        #region searchStaff
        protected override Dictionary<string, string> GetSearchColumnList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("EntUserNic", displaynames["EntUserNic"]);
            result.Add("FirstName", displaynames["FirstName"]);
            result.Add("LastName", displaynames["LastName"]);
            result.Add("MiddleName", displaynames["MiddleName"]);
            return result;
        }
        #endregion
        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.EnterpriseBranchUserTDESIndexCount(this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntBranchGuid);
            Items = new ObservableCollection<EnterpriseBranchUserTDES>(dataservice.EnterpriseBranchUserTDESIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntBranchGuid));
            RaisePropertyChanged("Items");
        }
        #endregion
        #region Autocomplete
        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.EnterpriseBranchUserTDESIndex(0, 15, "EntUserNic", true, this.SearchColumn, filtercriteria, SearchEntGuid, SearchEntBranchGuid).ToList();
            foreach (var item in aList)
            {
                switch (SearchColumn)
                {
                    case "EntUserNic":
                        result.Add(new KeyValuePair<string, string>(item.EntUserNic, item.EntUserNic + " : " + item.FirstName + " : " + item.LastName + " : " + item.MiddleName));
                        break;
                    case "FirstName":
                        result.Add(new KeyValuePair<string, string>(item.FirstName, item.EntUserNic + " : " + item.FirstName + " : " + item.LastName + " : " + item.MiddleName));
                        break;
                    case "LastName":
                        result.Add(new KeyValuePair<string, string>(item.LastName, item.EntUserNic + " : " + item.FirstName + " : " + item.LastName + " : " + item.MiddleName));
                        break;
                    case "MiddleName":
                        result.Add(new KeyValuePair<string, string>(item.MiddleName, item.EntUserNic + " : " + item.FirstName + " : " + item.LastName + " : " + item.MiddleName));
                        break;
                    default:
                        result.Add(new KeyValuePair<string, string>(item.EntUserNic, item.EntUserNic + " : " + item.FirstName + " : " + item.LastName + " : " + item.MiddleName));
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
            string segUsr = (string)navigationContext.Parameters["SearchEntUserNic"];

            Guid segEnt_Guid = Guid.Empty;
            Guid segEntBrch_Guid = Guid.Empty;


            if (KeepAlive)
            {
                if ((!string.IsNullOrEmpty(segEnt)) && (!string.IsNullOrEmpty(segEntBrch)) && (!string.IsNullOrEmpty(segUsr)))
                {

                    if (Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segEntBrch, out segEntBrch_Guid) )
                        return (SearchEntGuid == segEnt_Guid) && (segEntBrch_Guid == SearchEntBranchGuid) && segUsr.Equals(EntUserNic);
                    else
                        return false;

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
            RaisePropertyChanged("EntDescription");
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

        #region EnterpriseBranchUserContactTDESCommand
        public ICommand EnterpriseBranchUserContactTDESCommand { get; set; }
        protected void OnEnterpriseBranchUserContactTDESCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            query.Add("SearchEntBranchGuid", this.SearchEntBranchGuid.ToString());
            query.Add("SearchEntBranchDescription", this.SearchEntBranchDescription);
            query.Add("SearchEntUserNic", this.EntUserNic);
            query.Add("SearchLastName", this.EntUserNic + " : " + this.LastName);


            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseBranchUserContactTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion


    }
}
