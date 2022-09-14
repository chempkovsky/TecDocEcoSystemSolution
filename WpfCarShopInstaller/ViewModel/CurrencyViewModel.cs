using CarShopDataService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TecDocEcoSystemDbClassLibrary;
using CarShopDataService.Interfaces;

namespace WpfCarShopInstaller.ViewModel
{
    public class CurrencyViewModel : ViewModelBase<Currency>
    {
        ICarShopDataService dataservice = null;

        public CurrencyViewModel(ICarShopDataService dataservice): base()
        {
            this.dataservice = dataservice;
        }

        public int CurrencyIso
        {
            get
            {
                return dataCashItem.CurrencyIso;
            }
            set
            {
                if (dataCashItem.CurrencyIso != value)
                {
                    dataCashItem.CurrencyIso = value;
                    IsModified = true;
                    RaisePropertyChanged("CurrencyIso");
                }
            }
        }
        public string CurrencyCode3
        {
            get
            {
                return dataCashItem.CurrencyCode3;
            }
            set
            {
                if (dataCashItem.CurrencyCode3 != value)
                {
                    dataCashItem.CurrencyCode3 = value;
                    IsModified = true;
                    RaisePropertyChanged("CurrencyCode3");
                }
            }
        }
        public string CurrencyName
        {
            get
            {
                return dataCashItem.CurrencyName;
            }
            set
            {
                if (dataCashItem.CurrencyName != value)
                {
                    dataCashItem.CurrencyName = value;
                    IsModified = true;
                    RaisePropertyChanged("CurrencyName");
                }
            }
        }
        public int FractionalUnit
        {
            get
            {
                return dataCashItem.FractionalUnit;
            }
            set
            {
                if (dataCashItem.FractionalUnit != value)
                {
                    dataCashItem.FractionalUnit = value;
                    IsModified = true;
                    RaisePropertyChanged("FractionalUnit");
                }
            }
        }
        public string FractionalUnitName
        {
            get
            {
                return dataCashItem.FractionalUnitName;
            }
            set
            {
                if (dataCashItem.FractionalUnitName != value)
                {
                    dataCashItem.FractionalUnitName = value;
                    IsModified = true;
                    RaisePropertyChanged("FractionalUnitName");
                }
            }
        }
        public string IssuerName
        {
            get
            {
                return dataCashItem.IssuerName;
            }
            set
            {
                if (dataCashItem.IssuerName != value)
                {
                    dataCashItem.IssuerName = value;
                    IsModified = true;
                    RaisePropertyChanged("IssuerName");
                }
            }
        }
        public bool IsNational
        {
            get
            {
                return dataCashItem.IsNational;
            }
            set
            {
                if (dataCashItem.IsNational != value)
                {
                    dataCashItem.IsNational = value;
                    IsModified = true;
                    RaisePropertyChanged("IsNational");
                }
            }
        }
        public bool IsOperational
        {
            get
            {
                return dataCashItem.IsOperational;
            }
            set
            {
                if (dataCashItem.IsOperational != value)
                {
                    dataCashItem.IsOperational = value;
                    IsModified = true;
                    RaisePropertyChanged("IsOperational");
                }
            }
        }

        protected override void CopyData(Currency src, Currency dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.CurrencyIso = src.CurrencyIso;
            if (notify) RaisePropertyChanged("CurrencyIso");
            dest.CurrencyCode3 = src.CurrencyCode3;
            if (notify) RaisePropertyChanged("CurrencyCode3");
            dest.CurrencyName = src.CurrencyName;
            if (notify) RaisePropertyChanged("CurrencyName");
            dest.FractionalUnit = src.FractionalUnit;
            if (notify) RaisePropertyChanged("FractionalUnit");
            dest.FractionalUnitName = src.FractionalUnitName;
            if (notify) RaisePropertyChanged("FractionalUnitName");
            dest.IssuerName = src.IssuerName;
            if (notify) RaisePropertyChanged("IssuerName");
            dest.IsNational = src.IsNational;
            if (notify) RaisePropertyChanged("IsNational");
            dest.IsOperational = src.IsOperational;
            if (notify) RaisePropertyChanged("IsOperational");
        }


        #region NewCommand
        protected override void OnDoNew()
        {
            CurrencyIso = -1;
        }
        #endregion
        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.CurrencyDelete(CurrentItem.CurrencyIso);
        }
        #endregion
        #region SaveCommand
        protected override Currency NewInstance()
        {
            return new Currency();
        }
        protected override Currency OnDoInser(Currency item)
        {
            return dataservice.CurrencyInsert(item);
        }
        protected override void OnDoUpdate(Currency item)
        {
            dataservice.CurrencyUpdate(item);
        }
        #endregion
        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.CurrencyIndexCount();
            Items = new ObservableCollection<Currency>(dataservice.CurrencyIndex(startVal, itemCountVal, sortColumnVal, ascendingVal));
            RaisePropertyChanged("Items");
        }
        #endregion


    }
}
