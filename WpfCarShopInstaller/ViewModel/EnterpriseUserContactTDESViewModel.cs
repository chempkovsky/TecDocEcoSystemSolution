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
    public class EnterpriseUserContactTDESViewModel : ViewModelSateBase<EnterpriseUserContactTDES>, INavigationAware, IRegionMemberLifetime
    {
        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;

        public Guid SearchEntGuid { get; set; }
        public string EntDescription { get; set; }
        public string SearchEntUserNic { get; set; }
        public string SearchLastName { get; set; }


        public EnterpriseUserContactTDESViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "ContactGuid";

            EnterpriseTDESCommand = new DelegateCommand<object>(this.OnEnterpriseTDESCommand, e => false);
            EnterpriseUserTDESCommand = new DelegateCommand<object>(this.OnEnterpriseUserTDESCommand, e => true);
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

        protected override void CopyData(EnterpriseUserContactTDES src, EnterpriseUserContactTDES dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.EntGuid = src.EntGuid;
            if (notify) RaisePropertyChanged("EntGuid");
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
            EntUserNic = SearchEntUserNic;
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.EnterpriseUserContactTDESDelete(CurrentItem.ContactGuid);
        }
        #endregion
        #region SaveCommand
        protected override EnterpriseUserContactTDES NewInstance()
        {
            return new EnterpriseUserContactTDES();
        }
        protected override EnterpriseUserContactTDES OnDoInser(EnterpriseUserContactTDES item)
        {
            return dataservice.EnterpriseUserContactTDESInsert(item);
        }
        protected override void OnDoUpdate(EnterpriseUserContactTDES item)
        {
            dataservice.EnterpriseUserContactTDESUpdate(item);
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
            totalItemsVal = dataservice.EnterpriseUserContactTDESIndexCount(this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntUserNic);
            Items = new ObservableCollection<EnterpriseUserContactTDES>(dataservice.EnterpriseUserContactTDESIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter, SearchEntGuid, SearchEntUserNic));
            RaisePropertyChanged("Items");
        }
        #endregion
        #region Autocomplete
        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.EnterpriseUserContactTDESIndex(0, 15, "ContactGuid", true, this.SearchColumn, filtercriteria, SearchEntGuid, SearchEntUserNic).ToList();
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
            if (SearchEntGuid == Guid.Empty)
                return true;
            else
            {
                Guid segEnt_Guid = Guid.Empty;
                string segEnt = (string)navigationContext.Parameters["SearchEntGuid"];
                if (Guid.TryParse(segEnt, out segEnt_Guid))
                {
                    if (SearchEntGuid == segEnt_Guid)
                    {
                        if (!string.IsNullOrEmpty(SearchEntUserNic))
                        {
                            string segEntUsr = (string)navigationContext.Parameters["SearchEntUserNic"];
                            return SearchEntUserNic.Equals(segEntUsr);
                        }
                    }
                }
                return false;
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
            this.SearchEntUserNic = (string)navigationContext.Parameters["SearchEntUserNic"];
            this.SearchLastName = (string)navigationContext.Parameters["SearchLastName"];

            KeepAlive = false;
            RaisePropertyChanged("EntDescription");
            RaisePropertyChanged("SearchLastName");
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
        #region EnterpriseUserTDESCommand
        public ICommand EnterpriseUserTDESCommand { get; set; }
        protected void OnEnterpriseUserTDESCommand(object arg)
        {
            KeepAlive = false;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            query.Add("SearchEntUserNic", this.SearchEntUserNic);
            query.Add("SearchLastName", this.SearchLastName);


            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseUserTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

        
    }
}
