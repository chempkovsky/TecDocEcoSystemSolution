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
    public class EnterpriseAddressTDESViewModel : ViewModelSateBase<EnterpriseAddressTDES>, INavigationAware, IRegionMemberLifetime
    {
        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;

        public Guid SearchEntGuid { get; set; }
        public string EntDescription { get; set; }
        protected Guid SearchAddressGuid;

        public EnterpriseAddressTDESViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "AddressGuid";

            EnterpriseTDESCommand = new DelegateCommand<object>(this.OnEnterpriseTDESCommand, e => true);
            SelectSoatoCommand = new DelegateCommand<object>(this.OnSelectSoatoCommand, e => true);
        }

        protected List<AddressType> addressTypes;
        public List<AddressType> AddressTypes
        {
            get
            {
                if (addressTypes == null)
                {
                    addressTypes = dataservice.AddressTypeIndex(0, 300, "", true).ToList();
                }
                return addressTypes;
            }
        }

        protected List<Country> country;
        public List<Country> Country
        {
            get
            {
                if (country == null)
                {
                    country = dataservice.CountryIndex(0, 300, "", true).ToList();
                }
                return country;
            }
        }

        protected List<SettlementType> settlementType;
        public List<SettlementType> SettlementType
        {
            get
            {
                if (settlementType == null)
                {
                    settlementType = dataservice.SettlementTypeIndex(0, 300, "", true).ToList();
                }
                return settlementType;
            }
        }

        protected List<StreetType> streetType;
        public List<StreetType> StreetType
        {
            get
            {
                if (streetType == null)
                {
                    streetType = dataservice.StreetTypeIndex(0, 300, "", true).ToList();
                }
                return streetType;
            }
        }

        protected override void CopyData(EnterpriseAddressTDES src, EnterpriseAddressTDES dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.EntGuid = src.EntGuid;
            if (notify) RaisePropertyChanged("EntGuid");
            dest.AddressGuid = src.AddressGuid;
            if (notify) RaisePropertyChanged("AddressGuid");
            dest.AddressTypeId = src.AddressTypeId;
            if (notify) RaisePropertyChanged("AddressTypeId");
            dest.CountryIso = src.CountryIso;
            if (notify) RaisePropertyChanged("CountryIso");
            dest.AddressRegion = src.AddressRegion;
            if (notify) RaisePropertyChanged("AddressRegion");
            dest.AddressRegion = src.AddressRegion;
            if (notify) RaisePropertyChanged("AddressRegion");
            dest.AddressDistrict = src.AddressDistrict;
            if (notify) RaisePropertyChanged("AddressDistrict");
            dest.AddressRural = src.AddressRural;
            if (notify) RaisePropertyChanged("AddressRural");
            dest.SettlementTypeId = src.SettlementTypeId;
            if (notify) RaisePropertyChanged("SettlementTypeId");
            dest.AddressSettlementName = src.AddressSettlementName;
            if (notify) RaisePropertyChanged("AddressSettlementName");
            dest.SoatoId = src.SoatoId;
            if (notify) RaisePropertyChanged("SoatoId");
            dest.StreetTypeId = src.StreetTypeId;
            if (notify) RaisePropertyChanged("StreetTypeId");
            dest.AddressStreetName = src.AddressStreetName;
            if (notify) RaisePropertyChanged("AddressStreetName");
            dest.AddressHouse = src.AddressHouse;
            if (notify) RaisePropertyChanged("AddressHouse");
            dest.AddressBuilding = src.AddressBuilding;
            if (notify) RaisePropertyChanged("AddressBuilding");
            dest.AddressOffice = src.AddressOffice;
            if (notify) RaisePropertyChanged("AddressOffice");
            dest.AddressPostCode = src.AddressPostCode;
            if (notify) RaisePropertyChanged("AddressPostCode");
            dest.AddressValidFrom = src.AddressValidFrom;
            if (notify) RaisePropertyChanged("AddressValidFrom");
            dest.AddressValidTo = src.AddressValidTo;
            if (notify) RaisePropertyChanged("AddressValidTo");
            dest.IsActive = src.IsActive;
            if (notify) RaisePropertyChanged("IsActive");
            dest.IsVisible = src.IsVisible;
            if (notify) RaisePropertyChanged("IsVisible");
            dest.AddressLongitude = src.AddressLongitude;
            if (notify) RaisePropertyChanged("AddressLongitude");
            dest.AddressLatitude = src.AddressLatitude;
            if (notify) RaisePropertyChanged("AddressLatitude");
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
        public Guid AddressGuid
        {
            get
            {
                return dataCashItem.AddressGuid;
            }
            set
            {
                if (dataCashItem.AddressGuid != value)
                {
                    dataCashItem.AddressGuid = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressGuid");
                }
            }
        }
        public int AddressTypeId
        {
            get
            {
                return dataCashItem.AddressTypeId;
            }
            set
            {
                if (dataCashItem.AddressTypeId != value)
                {
                    dataCashItem.AddressTypeId = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressTypeId");
                }
            }
        }
        public int CountryIso
        {
            get
            {
                return dataCashItem.CountryIso;
            }
            set
            {
                if (dataCashItem.CountryIso != value)
                {
                    dataCashItem.CountryIso = value;
                    IsModified = true;
                    RaisePropertyChanged("CountryIso");
                }
            }
        }
        public string AddressRegion
        {
            get
            {
                return dataCashItem.AddressRegion;
            }
            set
            {
                if (dataCashItem.AddressRegion != value)
                {
                    dataCashItem.AddressRegion = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressRegion");
                }
            }
        }
        public string AddressDistrict
        {
            get
            {
                return dataCashItem.AddressDistrict;
            }
            set
            {
                if (dataCashItem.AddressDistrict != value)
                {
                    dataCashItem.AddressDistrict = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressDistrict");
                }
            }
        }
        public string AddressRural
        {
            get
            {
                return dataCashItem.AddressRural;
            }
            set
            {
                if (dataCashItem.AddressRural != value)
                {
                    dataCashItem.AddressRural = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressRural");
                }
            }
        }
        public int SettlementTypeId
        {
            get
            {
                return dataCashItem.SettlementTypeId;
            }
            set
            {
                if (dataCashItem.SettlementTypeId != value)
                {
                    dataCashItem.SettlementTypeId = value;
                    IsModified = true;
                    RaisePropertyChanged("SettlementTypeId");
                }
            }
        }
        public string AddressSettlementName
        {
            get
            {
                return dataCashItem.AddressSettlementName;
            }
            set
            {
                if (dataCashItem.AddressSettlementName != value)
                {
                    dataCashItem.AddressSettlementName = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressSettlementName");
                }
            }
        }
        public string SoatoId
        {
            get
            {
                return dataCashItem.SoatoId;
            }
            set
            {
                if (dataCashItem.SoatoId != value)
                {
                    dataCashItem.SoatoId = value;
                    IsModified = true;
                    RaisePropertyChanged("SoatoId");
                }
            }
        }
        public int StreetTypeId
        {
            get
            {
                return dataCashItem.StreetTypeId;
            }
            set
            {
                if (dataCashItem.StreetTypeId != value)
                {
                    dataCashItem.StreetTypeId = value;
                    IsModified = true;
                    RaisePropertyChanged("StreetTypeId");
                }
            }
        }
        public string AddressStreetName
        {
            get
            {
                return dataCashItem.AddressStreetName;
            }
            set
            {
                if (dataCashItem.AddressStreetName != value)
                {
                    dataCashItem.AddressStreetName = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressStreetName");
                }
            }
        }
        public string AddressHouse
        {
            get
            {
                return dataCashItem.AddressHouse;
            }
            set
            {
                if (dataCashItem.AddressHouse != value)
                {
                    dataCashItem.AddressHouse = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressHouse");
                }
            }
        }
        public string AddressBuilding
        {
            get
            {
                return dataCashItem.AddressBuilding;
            }
            set
            {
                if (dataCashItem.AddressBuilding != value)
                {
                    dataCashItem.AddressBuilding = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressBuilding");
                }
            }
        }
        public string AddressOffice
        {
            get
            {
                return dataCashItem.AddressOffice;
            }
            set
            {
                if (dataCashItem.AddressOffice != value)
                {
                    dataCashItem.AddressOffice = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressOffice");
                }
            }
        }
        public string AddressPostCode
        {
            get
            {
                return dataCashItem.AddressPostCode;
            }
            set
            {
                if (dataCashItem.AddressPostCode != value)
                {
                    dataCashItem.AddressPostCode = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressPostCode");
                }
            }
        }
        public DateTime AddressValidFrom
        {
            get
            {
                return dataCashItem.AddressValidFrom;
            }
            set
            {
                if (dataCashItem.AddressValidFrom != value)
                {
                    dataCashItem.AddressValidFrom = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressValidFrom");
                }
            }
        }
        public DateTime AddressValidTo
        {
            get
            {
                return dataCashItem.AddressValidTo;
            }
            set
            {
                if (dataCashItem.AddressValidTo != value)
                {
                    dataCashItem.AddressValidTo = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressValidTo");
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
        public double AddressLongitude
        {
            get
            {
                return dataCashItem.AddressLongitude;
            }
            set
            {
                if (dataCashItem.AddressLongitude != value)
                {
                    dataCashItem.AddressLongitude = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressLongitude");
                }
            }
        }
        public double AddressLatitude
        {
            get
            {
                return dataCashItem.AddressLatitude;
            }
            set
            {
                if (dataCashItem.AddressLatitude != value)
                {
                    dataCashItem.AddressLatitude = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressLatitude");
                }
            }
        }

        #region NewCommand
        protected override void OnDoNew()
        {
            AddressGuid = Guid.NewGuid();
            EntGuid = SearchEntGuid;
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.EnterpriseAddressTDESDelete(CurrentItem.AddressGuid);
        }
        #endregion
        #region SaveCommand
        protected override EnterpriseAddressTDES NewInstance()
        {
            return new EnterpriseAddressTDES();
        }
        protected override EnterpriseAddressTDES OnDoInser(EnterpriseAddressTDES item)
        {
            return dataservice.EnterpriseAddressTDESInsert(item);
        }
        protected override void OnDoUpdate(EnterpriseAddressTDES item)
        {
            dataservice.EnterpriseAddressTDESUpdate(item);
        }
        #endregion


        #region searchStaff
        protected override Dictionary<string, string> GetSearchColumnList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("AddressGuid", displaynames["AddressGuid"]);
            result.Add("CountryIso", displaynames["CountryIso"]);
            result.Add("AddressRegion", displaynames["AddressRegion"]);
            result.Add("AddressDistrict", displaynames["AddressDistrict"]);
            result.Add("AddressRural", displaynames["AddressRural"]);
            result.Add("AddressSettlementName", displaynames["AddressSettlementName"]);
            result.Add("SoatoId", displaynames["SoatoId"]);
            result.Add("AddressStreetName", displaynames["AddressStreetName"]);
            result.Add("AddressHouse", displaynames["AddressHouse"]);
            result.Add("AddressBuilding", displaynames["AddressBuilding"]);
            result.Add("AddressOffice", displaynames["AddressOffice"]);
            result.Add("AddressPostCode", displaynames["AddressPostCode"]);
            result.Add("AddressLongitude", displaynames["AddressLongitude"]);
            result.Add("AddressLatitude", displaynames["AddressLatitude"]);
            return result;
        }
        #endregion
        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.EnterpriseAddressTDESIndexCount(this.SearchColumn, SearchFilter, SearchEntGuid);
            Items = new ObservableCollection<EnterpriseAddressTDES>(dataservice.EnterpriseAddressTDESIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter, SearchEntGuid));
            RaisePropertyChanged("Items");
        }
        #endregion
        #region Autocomplete
        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.EnterpriseAddressTDESIndex(0, 15, "AddressGuid", true, this.SearchColumn, filtercriteria, SearchEntGuid).ToList();
            foreach (var item in aList)
            {
                switch (SearchColumn)
                {
                    case "AddressGuid":
                        result.Add(new KeyValuePair<string, string>(item.AddressGuid.ToString(), item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "CountryIso":
                        result.Add(new KeyValuePair<string, string>(item.CountryIso.ToString(), item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressRegion":
                        result.Add(new KeyValuePair<string, string>(item.AddressRegion, item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressDistrict":
                        result.Add(new KeyValuePair<string, string>(item.AddressDistrict, item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressRural":
                        result.Add(new KeyValuePair<string, string>(item.AddressRural, item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressSettlementName":
                        result.Add(new KeyValuePair<string, string>(item.AddressSettlementName, item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "SoatoId":
                        result.Add(new KeyValuePair<string, string>(item.SoatoId, item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressHouse":
                        result.Add(new KeyValuePair<string, string>(item.AddressHouse, item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressBuilding":
                        result.Add(new KeyValuePair<string, string>(item.AddressBuilding, item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressOffice":
                        result.Add(new KeyValuePair<string, string>(item.AddressOffice, item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressPostCode":
                        result.Add(new KeyValuePair<string, string>(item.AddressPostCode, item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressLongitude":
                        result.Add(new KeyValuePair<string, string>(item.AddressLongitude.ToString(), item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    case "AddressLatitude":
                        result.Add(new KeyValuePair<string, string>(item.AddressLatitude.ToString(), item.AddressStreetName + " : " + item.AddressSettlementName));
                        break;
                    default:
                        result.Add(new KeyValuePair<string, string>(item.AddressGuid.ToString(), item.AddressStreetName + " : " + item.AddressSettlementName));
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
            string segAddressGuid = (string)navigationContext.Parameters["SearchAddressGuid"];

            Guid segEnt_Guid = Guid.Empty;
            Guid segAddressGuid_Guid = Guid.Empty;


            if (KeepAlive)
            {
                if ((!string.IsNullOrEmpty(segEnt)) && (!string.IsNullOrEmpty(segAddressGuid)))
                {
                    if ( Guid.TryParse(segEnt, out segEnt_Guid) && Guid.TryParse(segAddressGuid, out segAddressGuid_Guid) ) 
                        return (segEnt_Guid==SearchEntGuid) && (segAddressGuid_Guid==SearchAddressGuid);
                    else 
                        return false;
                }
            }

            if (SearchEntGuid == Guid.Empty)
                return true;
            else
            {
                if  (Guid.TryParse(segEnt, out segEnt_Guid) )
                return segEnt_Guid == SearchEntGuid;
                else
                return false;
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
            }
            else
            {
                DefineSoatoDetails((string)navigationContext.Parameters["SoatoId"] );
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
        #region SelectSoatoCommand
        public ICommand SelectSoatoCommand { get; set; }
        protected void OnSelectSoatoCommand(object arg)
        {
            KeepAlive = true;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();
            query.Add("SearchEntGuid", this.SearchEntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);

            this.SearchAddressGuid = Guid.NewGuid();
            query.Add("SearchAddressGuid", this.SearchAddressGuid.ToString());
            query.Add("SearchBackRegion", "EnterpriseNavigationContentRegion");
            query.Add("SearchBackPage", "EnterpriseAddressTDESView");

            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("LookUpSoatoView" + query.ToString(), UriKind.Relative));
        }
        #endregion

        protected void DefineSoatoDetails(string aSoatoId)
        {
            if (string.IsNullOrEmpty(aSoatoId)) return;
            if (aSoatoId.Length != 10) return;
            Soato st1 =  dataservice.SoatoDetail(aSoatoId);
            SoatoId = st1.SoatoId;
            SettlementTypeId = st1.SettlementTypeId;
            AddressSettlementName = st1.SoatoSettlementName;
            if (!aSoatoId.EndsWith("000"))
            {
                aSoatoId = aSoatoId.Substring(0, 7) + "000";
                st1 = dataservice.SoatoDetail(aSoatoId);
                this.AddressRural = st1.SoatoSettlementName;
            }
            else
            {
                this.AddressRural = "";
            }
            if (!aSoatoId.EndsWith("000000"))
            {
                aSoatoId = aSoatoId.Substring(0, 4) + "000000";
                st1 = dataservice.SoatoDetail(aSoatoId);
                this.AddressDistrict = st1.SoatoSettlementName;
            }
            else
            {
                this.AddressDistrict = "";
            }
            if (!aSoatoId.EndsWith("000000000"))
            {
                aSoatoId = aSoatoId.Substring(0, 1) + "000000000";
                st1 = dataservice.SoatoDetail(aSoatoId);
                this.AddressRegion = st1.SoatoSettlementName;
            }
            else
            {
                this.AddressRegion = "";
            }
        }

    }
}
