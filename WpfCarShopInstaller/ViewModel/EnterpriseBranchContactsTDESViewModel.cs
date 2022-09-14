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
    public class EnterpriseBranchContactsTDESViewModel : ViewModelSateBase<EnterpriseBranchContactsTDES>, INavigationAware, IRegionMemberLifetime
    {
        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;

        public Guid SearchEntGuid { get; set; }
        public string EntDescription { get; set; }
        public Guid SearchEntBranchGuid { get; set; }
        public string SearchEntBranchDescription { get; set; }


        public EnterpriseBranchContactsTDESViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "ContactGuid";

            EnterpriseTDESCommand = new DelegateCommand<object>(this.OnEnterpriseTDESCommand, e => false);
            EnterpriseBranchTDESCommand = new DelegateCommand<object>(this.OnEnterpriseBranchTDESCommand, e => true);
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

        protected override void CopyData(EnterpriseBranchContactsTDES src, EnterpriseBranchContactsTDES dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.EntGuid = src.EntGuid;
            if (notify) RaisePropertyChanged("EntGuid");
            dest.EntBranchGuid = src.EntBranchGuid;
            if (notify) RaisePropertyChanged("EntBranchGuid");
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
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.EnterpriseBranchContactsTDESDelete(CurrentItem.ContactGuid);
        }
        #endregion
        #region SaveCommand
        protected override EnterpriseBranchContactsTDES NewInstance()
        {
            return new EnterpriseBranchContactsTDES();
        }
        protected override EnterpriseBranchContactsTDES OnDoInser(EnterpriseBranchContactsTDES item)
        {
            return dataservice.EnterpriseBranchContactsTDESInsert(item);
        }
        protected override void OnDoUpdate(EnterpriseBranchContactsTDES item)
        {
            dataservice.EnterpriseBranchContactsTDESUpdate(item);
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
            totalItemsVal = dataservice.EnterpriseBranchContactsTDESIndexCount(this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntBranchGuid);
            Items = new ObservableCollection<EnterpriseBranchContactsTDES>(dataservice.EnterpriseBranchContactsTDESIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntBranchGuid));
            RaisePropertyChanged("Items");
        }
        #endregion
        #region Autocomplete
        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.EnterpriseBranchContactsTDESIndex(0, 15, "ContactGuid", true, this.SearchColumn, filtercriteria, SearchEntGuid, SearchEntBranchGuid).ToList();
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
            string segContactGuid = (string)navigationContext.Parameters["SearchContactGuid"];

            Guid segEnt_Guid = Guid.Empty;
            Guid segEntBrch_Guid = Guid.Empty;
            Guid segContactGuid_Guid = Guid.Empty;


            if (KeepAlive)
            {
                if ((!string.IsNullOrEmpty(segEnt)) && (!string.IsNullOrEmpty(segEntBrch)) && (!string.IsNullOrEmpty(segContactGuid)))
                {
                    if (Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segEntBrch, out segEntBrch_Guid) && Guid.TryParse(segContactGuid, out segContactGuid_Guid))
                        return (SearchEntGuid == segEnt_Guid) && (segEntBrch_Guid == SearchEntBranchGuid) && (segContactGuid_Guid == ContactGuid);
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

        // EnterpriseBranchContactsTDES_INDEX
    }
}
