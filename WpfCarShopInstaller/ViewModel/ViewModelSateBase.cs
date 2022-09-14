using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WpfCarShopInstaller.ViewModel
{
    public class ViewModelSateBase<T> : ViewModelBase<T>
    {
        public ViewModelSateBase()
            : base()
        {
        }


        bool browserState = true;
        public bool BrowserState
        {
            get
            {
                return browserState;
            }
            set
            {
                if (browserState != value)
                {
                    browserState = value;
                    RaisePropertyChanged("BrowserState");
                }
            }
        }

        #region LocalDialog
        protected override void ShowLocalDialog(string title)
        {
            BrowserState = false;
        }
        #endregion

        #region UndoCommand
        protected override void OnUndo(object arg)
        {
            this.canUndo = false;
            try
            {
                if (IsUndoEnabled)
                {
                    CopyData(CurrentItem, dataCashItem, true);
                    IsModified = false;
                    IsNewState = false;
                }
                BrowserState = true;
            }
            finally
            {
                this.canUndo = true;
            }
        }
        #endregion

        #region SaveCommand
        protected override void OnSave(object arg)
        {
            this.canSave = false;
            try
            {
                if (SaveEnabled)
                {
                    if (Items == null)
                    {
                        Items = new ObservableCollection<T>();
                        RaisePropertyChanged("Items");
                    }
                    T item = default(T);
                    if (IsNewState)
                    {
                        item = NewInstance();
                        CopyData(dataCashItem, item, false);
                        item = OnDoInser(item);
                        Items.Add(item);
                    }
                    else
                    {
                        var index = Items.IndexOf(CurrentItem);
                        item = CurrentItem;
                        CopyData(dataCashItem, item, false);
                        OnDoUpdate(item);
                        Items.RemoveAt(index);
                        Items.Insert(index, item);
                    }
                    CurrentItem = item;
                    BrowserState = true;
                }
            }
            finally
            {
                this.canSave = true;
            }
        }
        #endregion


    }
}
