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
    public class BranchTypeViewModel : ViewModelBase<BranchType>
    {
        ICarShopDataService dataservice = null;

        public BranchTypeViewModel(ICarShopDataService dataservice): base()
        {
            this.dataservice = dataservice;
        }
        public int BranchTypeId
        {
            get
            {
                return dataCashItem.BranchTypeId;
            }
            set
            {
                if (dataCashItem.BranchTypeId != value)
                {
                    dataCashItem.BranchTypeId = value;
                    IsModified = true;
                    RaisePropertyChanged("BranchTypeId");
                }
            }
        }
        public string BranchTypeDescription
        {
            get
            {
                return dataCashItem.BranchTypeDescription;
            }
            set
            {
                if (dataCashItem.BranchTypeDescription != value)
                {
                    dataCashItem.BranchTypeDescription = value;
                    IsModified = true;
                    RaisePropertyChanged("BranchTypeDescription");
                }
            }
        }

        protected override void CopyData(BranchType src, BranchType dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.BranchTypeId = src.BranchTypeId;
            if (notify) RaisePropertyChanged("BranchTypeId");
            dest.BranchTypeDescription = src.BranchTypeDescription;
            if (notify) RaisePropertyChanged("BranchTypeDescription");
        }


        #region NewCommand
        protected override void OnDoNew()
        {
            BranchTypeId = -1;
        }
        #endregion

        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.BranchTypeDelete(CurrentItem.BranchTypeId);
        }
        #endregion


        #region SaveCommand
        protected override BranchType NewInstance()
        {
            return new BranchType();
        }
        protected override BranchType OnDoInser(BranchType item)
        {
            return dataservice.BranchTypeInsert(item);
        }
        protected override void OnDoUpdate(BranchType item)
        {
            dataservice.BranchTypeUpdate(item);
        }
        #endregion

        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.BranchTypeIndexCount();
            Items = new ObservableCollection<BranchType>(dataservice.BranchTypeIndex(startVal, itemCountVal, sortColumnVal, ascendingVal));
            RaisePropertyChanged("Items");
        }
        #endregion


    }
}
