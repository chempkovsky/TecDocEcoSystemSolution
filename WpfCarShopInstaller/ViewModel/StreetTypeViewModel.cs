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
    public class StreetTypeViewModel : ViewModelBase<StreetType>
    {
        ICarShopDataService dataservice = null;

        public StreetTypeViewModel(ICarShopDataService dataservice): base()
        {
            this.dataservice = dataservice;
        }


        protected override void CopyData(StreetType src, StreetType dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.StreetTypeId = src.StreetTypeId;
            if (notify) RaisePropertyChanged("StreetTypeId");
            dest.StreetTypeDescription = src.StreetTypeDescription;
            if (notify) RaisePropertyChanged("StreetTypeDescription");
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
        public string StreetTypeDescription
        {
            get
            {
                return dataCashItem.StreetTypeDescription;
            }
            set
            {
                if (dataCashItem.StreetTypeDescription != value)
                {
                    dataCashItem.StreetTypeDescription = value;
                    IsModified = true;
                    RaisePropertyChanged("StreetTypeDescription");
                }
            }
        }



        #region NewCommand
        protected override void OnDoNew()
        {
            StreetTypeId = -1;
        }
        #endregion

        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.StreetTypeDelete(CurrentItem.StreetTypeId);
        }
        #endregion

        #region SaveCommand
        protected override StreetType NewInstance()
        {
            return new StreetType();
        }
        protected override StreetType OnDoInser(StreetType item)
        {
            return dataservice.StreetTypeInsert(item);
        }
        protected override void OnDoUpdate(StreetType item)
        {
            dataservice.StreetTypeUpdate(item);
        }
        #endregion

        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.StreetTypeIndexCount();
            Items = new ObservableCollection<StreetType>(dataservice.StreetTypeIndex(startVal, itemCountVal, sortColumnVal, ascendingVal));
            RaisePropertyChanged("Items");
        }
        #endregion


    }
}
