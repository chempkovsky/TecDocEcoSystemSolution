using CarShopDataService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using TecDocEcoSystemDbClassLibrary;
using CarShopDataService.Interfaces;

// ListCollectionView

namespace WpfCarShopInstaller.ViewModel
{
    public class SoatoViewModel : ViewModelBase<Soato>
    {
        ICarShopDataService dataservice = null;
        public SoatoViewModel(ICarShopDataService dataservice): base()
        {
            this.dataservice = dataservice;
            this.searchColumn = "SoatoId";
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

        #region Autocomplete

        protected override List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {

            var result = base.AsyncGetAutocompleteItems(filtercriteria);
                //new Dictionary<string, string>();
            /*
                if (AutocompleteItems == null)
                {
                    AutocompleteItems = new Dictionary<string, string>();
                }
                else
                {
                    AutocompleteItems.Clear();
                }
             */ 
                var aList = dataservice.SoatoIndex(0, 15, "SoatoId", true, this.SearchColumn, filtercriteria).ToList();
                var keyIsId = "SoatoId".Equals(this.SearchColumn);
                foreach (var item in aList)
                {
                    if (keyIsId)
                    {
                        result.Add( new KeyValuePair<string, string>(  item.SoatoId, item.SoatoId + ":" + item.SoatoSettlementName ));
                    }
                    else
                    {
                        result.Add( new KeyValuePair<string, string>( item.SoatoSettlementName, item.SoatoId + ":" + item.SoatoSettlementName ));
                    }
                }
            return result;
            //    RaisePropertyChanged("AutocompleteItems");
        }
        #endregion


        #region NewCommand
        protected override void OnDoNew()
        {
            SoatoId = "000000000";
        }
        #endregion

        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.SoatoDelete(CurrentItem.SoatoId);
        }
        #endregion

        #region SaveCommand
        protected override Soato NewInstance()
        {
            return new Soato();
        }
        protected override Soato OnDoInser(Soato item)
        {
            return dataservice.SoatoInsert(item);
        }
        protected override void OnDoUpdate(Soato item)
        {
            dataservice.SoatoUpdate(item);
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


    }
}
