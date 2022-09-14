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
    public class SettlementTypeViewModel : ViewModelBase<SettlementType>
    {
        ICarShopDataService dataservice = null;

        public SettlementTypeViewModel(ICarShopDataService dataservice): base()
        {
            this.dataservice = dataservice;
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
        public string SettlementTypeDescription
        {
            get
            {
                return dataCashItem.SettlementTypeDescription;
            }
            set
            {
                if (dataCashItem.SettlementTypeDescription != value)
                {
                    dataCashItem.SettlementTypeDescription = value;
                    IsModified = true;
                    RaisePropertyChanged("SettlementTypeDescription");
                }
            }
        }
        public string SettlementTypeShortDescription
        {
            get
            {
                return dataCashItem.SettlementTypeShortDescription;
            }
            set
            {
                if (dataCashItem.SettlementTypeShortDescription != value)
                {
                    dataCashItem.SettlementTypeShortDescription = value;
                    IsModified = true;
                    RaisePropertyChanged("SettlementTypeShortDescription");
                }
            }
        }


        protected override void CopyData(SettlementType src, SettlementType dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.SettlementTypeId = src.SettlementTypeId;
            if (notify) RaisePropertyChanged("SettlementTypeId");
            dest.SettlementTypeDescription = src.SettlementTypeDescription;
            if (notify) RaisePropertyChanged("SettlementTypeDescription");
            dest.SettlementTypeShortDescription = src.SettlementTypeShortDescription;
            if (notify) RaisePropertyChanged("SettlementTypeShortDescription");
        }


        #region NewCommand
        protected override void OnDoNew()
        {
            SettlementTypeId = -1;
        }
        #endregion

        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.SettlementTypeDelete(CurrentItem.SettlementTypeId);
        }
        #endregion

        #region SaveCommand
        protected override SettlementType NewInstance()
        {
            return new SettlementType();
        }
        protected override SettlementType OnDoInser(SettlementType item)
        {
            return dataservice.SettlementTypeInsert(item);
        }
        protected override void OnDoUpdate(SettlementType item)
        {
            dataservice.SettlementTypeUpdate(item);
        }
        #endregion


        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.SettlementTypeIndexCount();
            Items = new ObservableCollection<SettlementType>(dataservice.SettlementTypeIndex(startVal, itemCountVal, sortColumnVal, ascendingVal));
            RaisePropertyChanged("Items");
        }
        #endregion

    }
}
