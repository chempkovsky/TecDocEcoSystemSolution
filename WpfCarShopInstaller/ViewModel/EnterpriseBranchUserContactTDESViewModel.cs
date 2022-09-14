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
    // EnterpriseUserContactTDES
    public class EnterpriseBranchUserContactTDESViewModel : ViewModelSateBase<EnterpriseBranchUserContactTDES>, INavigationAware, IRegionMemberLifetime
    {
        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;

        public Guid SearchEntGuid { get; set; }
        public string EntDescription { get; set; }
        public Guid SearchEntBranchGuid { get; set; }
        public string SearchEntBranchDescription { get; set; }
        public string SearchEntUserNic { get; set; }
        public string SearchLastName { get; set; }


        public EnterpriseBranchUserContactTDESViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "ContactGuid";

            EnterpriseTDESCommand = new DelegateCommand<object>(this.OnEnterpriseTDESCommand, e => false);
            EnterpriseBranchTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchTDESCommand, e => false);
            EnterpriseBranchUserTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchUserTDESCommand, e => true);
        }

        protected List<ContactType> contactTypes;
        public List<ContactType> ContactTypes
        {
            get
            {
                if (contactTypes == null)
                {
                    contactTypes = dataservice.ContactTypeIndex(0, 300, "", true).ToList();
                }
                return contactTypes;
            }
        }

        protected override void CopyData(EnterpriseBranchUserContactTDES src, EnterpriseBranchUserContactTDES dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.EntGuid = src.EntGuid;
            if (notify) RaisePropertyChanged("EntGuid");
            dest.EntBranchGuid = src.EntBranchGuid;
            if (notify) RaisePropertyChanged("EntBranchGuid");
            dest.EntUserNic = src.EntUserNic;
            if (notify) RaisePropertyChanged("EntUserNic");
            dest.ContactGuid = src.ContactGuid;
            if (notify) RaisePropertyChanged("ContactGuid");
            dest.Contact = src.Contact;
            if (notify) RaisePropertyChanged("Contact");
            dest.IsActive = src.IsActive;
            if (notify) RaisePropertyChanged("IsActive");
            dest.IsVisible = src.IsVisible;
            if (notify) RaisePropertyChanged("IsVisible");
            dest.Description = src.Description;
            if (notify) RaisePropertyChanged("Description");
            dest.ContactTypeId = src.ContactTypeId;
            if (notify) RaisePropertyChanged("ContactTypeId");
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
        public Guid ContactGuid
        {
            get
            {
                return dataCashItem.ContactGuid;
            }
            set
            {
                if (dataCashItem.ContactGuid != value)
                {
                    dataCashItem.ContactGuid = value;
                    IsModified = true;
                    RaisePropertyChanged("ContactGuid");
                }
            }
        }
        public string Contact
        {
            get
            {
                return dataCashItem.Contact;
            }
            set
            {
                if (dataCashItem.Contact != value)
                {
                    dataCashItem.Contact = value;
                    IsModified = true;
                    RaisePropertyChanged("Contact");
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
        public int ContactTypeId
        {
            get
            {
                return dataCashItem.ContactTypeId;
            }
            set
            {
                if (dataCashItem.ContactTypeId != value)
                {
                    dataCashItem.ContactTypeId = value;
                    IsModified = true;
                    RaisePropertyChanged("ContactTypeId");
                }
            }
        }

        #region NewCommand
        protected override void OnDoNew()
        {
            ContactGuid = Guid.NewGuid();
            EntGuid = SearchEntGuid;
            EntBranchGuid = SearchEntBranchGuid;
            EntUserNic = SearchEntUserNic;
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.EnterpriseBranchUserContactTDESDelete(CurrentItem.ContactGuid);
        }
        #endregion
        #region SaveCommand
        protected override EnterpriseBranchUserContactTDES NewInstance()
        {
            return new EnterpriseBranchUserContactTDES();
        }
        protected override EnterpriseBranchUserContactTDES OnDoInser(EnterpriseBranchUserContactTDES item)
        {
            return dataservice.EnterpriseBranchUserContactTDESInsert(item);
        }
        protected override void OnDoUpdate(EnterpriseBranchUserContactTDES item)
        {
            dataservice.EnterpriseBranchUserContactTDESUpdate(item);
        }
        #endregion

        #region searchStaff
        protected override Dictionary<string, string> GetSearchColumnList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("ContactGuid", displaynames["ContactGuid"]);
            result.Add("Contact", displaynames["Contact"]);
            result.Add("Description", displaynames["Description"]);
            return result;
        }
        #endregion
        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.EnterpriseBranchUserContactTDESIndexCount(this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntUserNic, SearchEntBranchGuid);
            Items = new ObservableCollection<EnterpriseBranchUserContactTDES>(dataservice.EnterpriseBranchUserContactTDESIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntUserNic, SearchEntBranchGuid));
            RaisePropertyChanged("Items");
        }
        #endregion
        #region Autocomplete
        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.EnterpriseBranchUserContactTDESIndex(0, 15, "ContactGuid", true, this.SearchColumn, filtercriteria, SearchEntGuid, SearchEntUserNic, SearchEntBranchGuid).ToList();
            foreach (var item in aList)
            {
                switch (SearchColumn)
                {
                    case "ContactGuid":
                        result.Add(new KeyValuePair<string, string>(item.ContactGuid.ToString(), item.ContactGuid + " : " + item.Contact));
                        break;
                    case "Contact":
                        result.Add(new KeyValuePair<string, string>(item.Contact, item.ContactGuid + " : " + item.Contact));
                        break;
                    case "Description":
                        result.Add(new KeyValuePair<string, string>(item.Description, item.ContactGuid + " : " + item.Description));
                        break;
                    default:
                        result.Add(new KeyValuePair<string, string>(item.ContactGuid.ToString(), item.ContactGuid + " : " + item.Contact));
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
            string segEntUsr = (string)navigationContext.Parameters["SearchEntUserNic"];
            string segContactGuid = (string)navigationContext.Parameters["ContactGuid"];

            Guid segEnt_Guid = Guid.Empty;
            Guid segEntBrch_Guid = Guid.Empty;
            Guid segContactGuid_Guid = Guid.Empty;


            if (KeepAlive)
            {
                if ((!string.IsNullOrEmpty(segEnt)) && (!string.IsNullOrEmpty(segEntBrch)) && (!string.IsNullOrEmpty(segEntUsr)) && (!string.IsNullOrEmpty(segContactGuid)))
                {
                    if (Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segEntBrch, out segEntBrch_Guid) && Guid.TryParse(segContactGuid, out segContactGuid_Guid))
                    {
                        return (segEnt_Guid == SearchEntGuid) && (segEntBrch_Guid == SearchEntBranchGuid) && segEntUsr.Equals(SearchEntUserNic) && (segContactGuid_Guid == ContactGuid);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if ((SearchEntGuid == Guid.Empty) || (SearchEntBranchGuid == Guid.Empty) || string.IsNullOrEmpty(SearchEntUserNic))
                return true;
            else
            {
                if (Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segEntBrch, out segEntBrch_Guid))
                {
                    return (SearchEntGuid == segEnt_Guid) && (SearchEntBranchGuid == segEntBrch_Guid) && SearchEntUserNic.Equals(segEntUsr);
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

                this.SearchEntUserNic = (string)navigationContext.Parameters["SearchEntUserNic"];
                this.SearchLastName = (string)navigationContext.Parameters["SearchLastName"];
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
            query.Add("EntDescription", this.EntDescription);
            query.Add("SearchEntBranchGuid", this.SearchEntBranchGuid.ToString());
            query.Add("SearchEntBranchDescription", this.SearchEntBranchDescription);
            query.Add("SearchEntUserNic", this.SearchEntUserNic);
            query.Add("SearchLastName", this.SearchLastName);


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
            query.Add("SearchEntUserNic", this.SearchEntUserNic);
            query.Add("SearchLastName", this.SearchLastName);


            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseBranchTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion
        #region EnterpriseBranchUserTDESCommand
        public ICommand EnterpriseBranchUserTDESCommand { get; set; }
        protected void OnEnterpriseBranchUserTDESCommand(object arg)
        {
            KeepAlive = false;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            query.Add("SearchEntBranchGuid", this.SearchEntBranchGuid.ToString());
            query.Add("SearchEntBranchDescription", this.SearchEntBranchDescription);
            query.Add("SearchEntUserNic", this.SearchEntUserNic);
            query.Add("SearchLastName", this.SearchLastName);
            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseBranchUserTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion
        
    }
}
