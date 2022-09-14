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
    public class CountryViewModel : ViewModelBase<Country>
    {
        ICarShopDataService dataservice = null;

        public CountryViewModel(ICarShopDataService dataservice): base()
        {
            this.dataservice = dataservice;
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
        public string CountryCode2
        {
            get
            {
                return dataCashItem.CountryCode2;
            }
            set
            {
                if (dataCashItem.CountryCode2 != value)
                {
                    dataCashItem.CountryCode2 = value;
                    IsModified = true;
                    RaisePropertyChanged("CountryCode2");
                }
            }
        }
        public string CountryCode3
        {
            get
            {
                return dataCashItem.CountryCode3;
            }
            set
            {
                if (dataCashItem.CountryCode3 != value)
                {
                    dataCashItem.CountryCode3 = value;
                    IsModified = true;
                    RaisePropertyChanged("CountryCode3");
                }
            }
        }
        public string CountryName
        {
            get
            {
                return dataCashItem.CountryName;
            }
            set
            {
                if (dataCashItem.CountryName != value)
                {
                    dataCashItem.CountryName = value;
                    IsModified = true;
                    RaisePropertyChanged("CountryName");
                }
            }
        }
        public string CountryEngName
        {
            get
            {
                return dataCashItem.CountryEngName;
            }
            set
            {
                if (dataCashItem.CountryEngName != value)
                {
                    dataCashItem.CountryEngName = value;
                    IsModified = true;
                    RaisePropertyChanged("CountryEngName");
                }
            }
        }
        public string CountryCapital
        {
            get
            {
                return dataCashItem.CountryCapital;
            }
            set
            {
                if (dataCashItem.CountryCapital != value)
                {
                    dataCashItem.CountryCapital = value;
                    IsModified = true;
                    RaisePropertyChanged("CountryCapital");
                }
            }
        }
        public string CountryPhone
        {
            get
            {
                return dataCashItem.CountryPhone;
            }
            set
            {
                if (dataCashItem.CountryPhone != value)
                {
                    dataCashItem.CountryPhone = value;
                    IsModified = true;
                    RaisePropertyChanged("CountryPhone");
                }
            }
        }

        protected override void CopyData(Country src, Country dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.CountryIso = src.CountryIso;
            if (notify) RaisePropertyChanged("CountryIso");
            dest.CountryCode2 = src.CountryCode2;
            if (notify) RaisePropertyChanged("CountryCode2");
            dest.CountryCode3 = src.CountryCode3;
            if (notify) RaisePropertyChanged("CountryCode3");
            dest.CountryName = src.CountryName;
            if (notify) RaisePropertyChanged("CountryName");
            dest.CountryEngName = src.CountryEngName;
            if (notify) RaisePropertyChanged("CountryEngName");
            dest.CountryCapital = src.CountryCapital;
            if (notify) RaisePropertyChanged("CountryCapital");
            dest.CountryPhone = src.CountryPhone;
            if (notify) RaisePropertyChanged("CountryPhone");
        }



        #region NewCommand
        protected override void OnDoNew()
        {
            CountryIso = -1;
        }
        #endregion

        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.CountryDelete(CurrentItem.CountryIso);
        }
        #endregion
        #region SaveCommand
        protected override Country NewInstance()
        {
            return new Country();
        }
        protected override Country OnDoInser(Country item)
        {
            return dataservice.CountryInsert(item);
        }
        protected override void OnDoUpdate(Country item)
        {
            dataservice.CountryUpdate(item);
        }
        #endregion

        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.CountryIndexCount();
            Items = new ObservableCollection<Country>(dataservice.CountryIndex(startVal, itemCountVal, sortColumnVal, ascendingVal));
            RaisePropertyChanged("Items");
        }
        #endregion



    }
}
