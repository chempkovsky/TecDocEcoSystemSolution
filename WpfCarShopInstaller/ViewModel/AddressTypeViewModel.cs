using CarShopDataService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TecDocEcoSystemDbClassLibrary;
using CarShopDataService.Interfaces;

namespace WpfCarShopInstaller.ViewModel
{
    public class AddressTypeViewModel : ViewModelBase<AddressType>
    {
        ICarShopDataService dataservice = null;

        public AddressTypeViewModel(ICarShopDataService dataservice)
            : base()
        {
            this.dataservice = dataservice;
        }
        protected override void CopyData(AddressType src, AddressType dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.AddressTypeId = src.AddressTypeId;
            if (notify) RaisePropertyChanged("AddressTypeId");
            dest.AddressTypeDescription = src.AddressTypeDescription;
            if (notify) RaisePropertyChanged("AddressTypeDescription");
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
        public string AddressTypeDescription
        {
            get
            {
                return dataCashItem.AddressTypeDescription;
            }
            set
            {
                if (dataCashItem.AddressTypeDescription != value)
                {
                    dataCashItem.AddressTypeDescription = value;
                    IsModified = true;
                    RaisePropertyChanged("AddressTypeDescription");
                }
            }
        }

        #region NewCommand
        protected override void OnDoNew()
        {
            AddressTypeId = -1;
        }
        #endregion

        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.AddressTypeDelete(CurrentItem.AddressTypeId);
        }
        #endregion

        #region SaveCommand
        protected override AddressType NewInstance()
        {
            return new AddressType();
        }
        protected override AddressType OnDoInser(AddressType item)
        {
            return dataservice.AddressTypeInsert(item);
        }
        protected override void OnDoUpdate(AddressType item)
        {
            dataservice.AddressTypeUpdate(item);
        }
        #endregion

        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.AddressTypeIndexCount();
            Items = new ObservableCollection<AddressType>(dataservice.AddressTypeIndex(startVal, itemCountVal, sortColumnVal, ascendingVal));
            RaisePropertyChanged("Items");
        }

    }
}
