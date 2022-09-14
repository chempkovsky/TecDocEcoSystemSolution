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
    public class LookUpSoatoViewModel : ViewModelSateBase<Soato>, INavigationAware, IRegionMemberLifetime
    {


        ICarShopDataService dataservice = null;
        IRegionManager regionManager = null;

        List<KeyValuePair<string, string>> SearchBackParams;
        string SearchBackRegion = null;
        string SearchBackPage = null;

        public LookUpSoatoViewModel(ICarShopDataService dataservice, IRegionManager regionManager)
            : base()
        {
            this.dataservice = dataservice;
            this.regionManager = regionManager;
            this.searchColumn = "SoatoId";
            this.KeepAlive = false;
            SelectItemCommand = new DelegateCommand<object>(this.OnSelectItemCommand, e => true);
            GoBackCommand = new DelegateCommand<object>(this.OnGoBackCommand, e => true);
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
        public string SoatoSettlementName
        {
            get
            {
                return dataCashItem.SoatoSettlementName;
            }
            set
            {
                if (dataCashItem.SoatoSettlementName != value)
                {
                    dataCashItem.SoatoSettlementName = value;
                    IsModified = true;
                    RaisePropertyChanged("SoatoSettlementName");
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

        protected ObservableCollection<SettlementType> settlementTypes;
        public ObservableCollection<SettlementType> SettlementTypes
        {
            get
            {
                if (settlementTypes == null)
                {
                    settlementTypes = new ObservableCollection<SettlementType>(dataservice.SettlementTypeIndex(0, 300, "", true));
                }
                return settlementTypes;
            }
        }

        protected override void CopyData(Soato src, Soato dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.SoatoId = src.SoatoId;
            if (notify) RaisePropertyChanged("SoatoId");
            dest.SoatoSettlementName = src.SoatoSettlementName;
            if (notify) RaisePropertyChanged("SoatoSettlementName");
            dest.SettlementTypeId = src.SettlementTypeId;
            if (notify) RaisePropertyChanged("SettlementTypeId");
        }

        #region SaveCommand
        protected override Soato NewInstance()
        {
            return new Soato();
        }
        #endregion

        #region Autocomplete

        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
            var aList = dataservice.SoatoIndex(0, 15, "SoatoId", true, this.SearchColumn, filtercriteria).ToList();
            var keyIsId = "SoatoId".Equals(this.SearchColumn);
            foreach (var item in aList)
            {
                if (keyIsId)
                {
                    result.Add(new KeyValuePair<string, string>(item.SoatoId, item.SoatoId + ":" + item.SoatoSettlementName));
                }
                else
                {
                    result.Add(new KeyValuePair<string, string>(item.SoatoSettlementName, item.SoatoId + ":" + item.SoatoSettlementName));
                }
            }
            return result;
        }
        #endregion

        #region searchStaff
        protected override Dictionary<string, string> GetSearchColumnList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("SoatoId", displaynames["SoatoId"]);
            result.Add("SoatoSettlementName", displaynames["SoatoSettlementName"]);
            return result;
        }
        #endregion

        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.SoatoIndexCount(this.SearchColumn, SearchFilter);
            Items = new ObservableCollection<Soato>(dataservice.SoatoIndex(startVal, itemCountVal, sortColumnVal, ascendingVal, this.SearchColumn, SearchFilter));
            RaisePropertyChanged("Items");
        }
        #endregion

        #region IRegionMemberLifetime
        public bool KeepAlive { get; set; }
        #endregion

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true; // all a time TRUE? since it's a lookUp
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.SearchBackRegion = (string)navigationContext.Parameters["SearchBackRegion"];
            this.SearchBackPage = (string)navigationContext.Parameters["SearchBackPage"];
            if (SearchBackParams == null)
            {
                SearchBackParams = new List<KeyValuePair<string, string>>();
            }
            SearchBackParams.Clear();
            foreach (var item in navigationContext.Parameters)
            {
                SearchBackParams.Add(
                    new KeyValuePair<string, string>( item.Key, (string)item.Value )
                    );
            }
            KeepAlive = false;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion

        #region GoBackCommand
        public ICommand GoBackCommand { get; set; }
        protected void OnGoBackCommand(object arg)
        {
            KeepAlive = false;
            NavigationParameters query = new NavigationParameters();//UriQuery query = new UriQuery();
            foreach (var item in SearchBackParams) {
                query.Add(item.Key, item.Value);
            }
            regionManager.RequestNavigate(SearchBackRegion, new Uri(SearchBackPage + query.ToString(), UriKind.Relative));
        }
        #endregion

        #region SelectItemCommand
        public ICommand SelectItemCommand { get; set; }
        protected void OnSelectItemCommand(object arg)
        {
            KeepAlive = false;
            NavigationParameters query = new NavigationParameters();//UriQuery query = new UriQuery();
            foreach (var item in SearchBackParams)
            {
                query.Add(item.Key, item.Value);
            }
            query.Add("SoatoId", SoatoId);
            regionManager.RequestNavigate(SearchBackRegion, new Uri(SearchBackPage + query.ToString(), UriKind.Relative));
        }
        #endregion

    }
}
