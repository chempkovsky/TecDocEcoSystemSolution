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
    public class ContactTypeViewModel : ViewModelBase<ContactType>
    {
        ICarShopDataService dataservice = null;

        public ContactTypeViewModel(ICarShopDataService dataservice): base()
        {
            this.dataservice = dataservice;
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
        public string ContactTypeDescription
        {
            get
            {
                return dataCashItem.ContactTypeDescription;
            }
            set
            {
                if (dataCashItem.ContactTypeDescription != value)
                {
                    dataCashItem.ContactTypeDescription = value;
                    IsModified = true;
                    RaisePropertyChanged("ContactTypeDescription");
                }
            }
        }

        protected override void CopyData(ContactType src, ContactType dest, bool notify)
        {
            if ((src == null) || (dest == null)) return;
            dest.ContactTypeId = src.ContactTypeId;
            if (notify) RaisePropertyChanged("ContactTypeId");
            dest.ContactTypeDescription = src.ContactTypeDescription;
            if (notify) RaisePropertyChanged("ContactTypeDescription");
        }


        #region NewCommand
        protected override void OnDoNew()
        {
            ContactTypeId = -1;
        }
        #endregion

        #region DeleteCommand
        protected override void OnDoDelete()
        {
            dataservice.ContactTypeDelete(CurrentItem.ContactTypeId);
        }
        #endregion

        #region SaveCommand
        protected override ContactType NewInstance()
        {
            return new ContactType();
        }
        protected override ContactType OnDoInser(ContactType item)
        {
            return dataservice.ContactTypeInsert(item);
        }
        protected override void OnDoUpdate(ContactType item)
        {
            dataservice.ContactTypeUpdate(item);
        }
        #endregion


        #region ReloadCommand
        protected override void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal, out int totalItemsVal)
        {
            totalItemsVal = dataservice.ContactTypeIndexCount();
            Items = new ObservableCollection<ContactType>(dataservice.ContactTypeIndex(startVal, itemCountVal, sortColumnVal, ascendingVal));
            RaisePropertyChanged("Items");
        }
        #endregion


    }
}
