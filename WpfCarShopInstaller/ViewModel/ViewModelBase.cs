using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfCarShopInstaller.Properties;
using WpfCarShopInstaller.Utility;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;


// ListCollectionView
// CollectionViewSource

//
// validation
// http://www.youtube.com/watch?v=OOHDie8BdGI
//

namespace WpfCarShopInstaller.ViewModel
{

    

    public class ViewModelBase<T> : INotifyPropertyChanged, IDataErrorInfo
    {

        public ViewModelBase()
        {
            dataCashItem = NewInstance();
            currentItem = default(T);

            ReloadCommand = new DelegateCommand<object>(this.OnReload, this.CanReload);
            SaveCommand = new DelegateCommand<object>(this.OnSave, this.CanSave);
            DeleteCommand = new DelegateCommand<object>(this.OnDelete, this.CanDelete);
            NewCommand = new DelegateCommand<object>(this.OnNew, this.CanNew);
            UndoCommand = new DelegateCommand<object>(this.OnUndo, this.CanUndo);
            EditCommand = new DelegateCommand<object>(this.OnEdit, e => true);

            FirstCommand = new DelegateCommand<object>(this.OnFirstCommand, e=>true);
            PreviousCommand = new DelegateCommand<object>(this.OnPreviousCommand, e => true);
            NextCommand = new DelegateCommand<object>(this.OnNextCommand, e => true);
            LastCommand = new DelegateCommand<object>(this.OnLastCommand, e => true);

            InformationDialog = new InteractionRequest<Notification>();
            ConfirmationDialog  = new InteractionRequest<Confirmation>();
            LocalDialog = new InteractionRequest<Notification>();


            ExecuteFilterAsyncCommand = new DelegateCommand<FilterAutoCompleteParameters>(this.DoExecuteFilterAsync, e => true);
            // AutocompleteItems = new Dictionary<string, string>();
            //_dispatcher = Dispatcher.CurrentDispatcher;
        }
        //public Dispatcher _dispatcher; //= Dispatcher.CurrentDispatcher;

        protected T dataCashItem;


        private static readonly Dictionary<string, Func<T, object>> propertyGetters =
                            typeof(T).GetProperties()
                            .Where(p => GetValidations(p).Length != 0)
                            .ToDictionary(p => p.Name, p => GetValueGetter(p));

        private static readonly Dictionary<string, ValidationAttribute[]> validators =
                            typeof(T).GetProperties()
                            .Where(p => GetValidations(p).Length != 0)
                            .ToDictionary(p => p.Name, p => GetValidations(p));

        protected static readonly Dictionary<string, string> displaynames =
                            typeof(T).GetProperties()
                            .Where(p => GetDisplayNames(p).Length != 0)
                            .ToDictionary(p => p.Name, p => (GetDisplayNames(p)[0]).GetName());


        private static DisplayAttribute[] GetDisplayNames(PropertyInfo property)
        {
            return (DisplayAttribute[])property.GetCustomAttributes(typeof(DisplayAttribute), true);
        }
        private static ValidationAttribute[] GetValidations(PropertyInfo property)
        {
            return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
        }
        private static Func<T, object> GetValueGetter(PropertyInfo property)
        {
            var instance = Expression.Parameter(typeof(T), "i");
            var cast = Expression.TypeAs(Expression.Property(instance, property), typeof(object));
            return (Func<T, object>)Expression.Lambda(cast, instance).Compile();
        }

        #region ObservableCollection<T>
        public ObservableCollection<T> Items { get; protected set; }
        #endregion

        protected virtual void CopyData(T src, T dest, bool notify)
        {
        }

        protected T currentItem;
        public T CurrentItem
        {
            get
            {
                return currentItem;
            }
            set
            {
                currentItem = value;
                CopyData(currentItem, dataCashItem, true);
                IsModified = false;
                IsNewState = false;
                RaisePropertyChanged("CurrentItem");
                RaisePropertyChanged("IsDeleteEnabled");
            }
        }

        public Dictionary<string, string> LabelDisplayNames
        {
            get { 
                 return displaynames;
            }
        }

        protected bool isModified = false;
        public bool IsModified
        {
            get
            {
                return isModified;
            }
            set
            {
                if (isModified != value)
                {
                    isModified = value;
                    RaisePropertyChanged("IsModified");
                }
                RaisePropertyChanged("SaveEnabled");
                RaisePropertyChanged("IsUndoEnabled");
            }
        }
        public bool SaveEnabled
        {
            get
            {
                if (IsModified)
                {
                    return IsCashItemValid;
                }
                return IsModified;
            }
        }
        protected bool isNewState = false;
        public bool IsNewState
        {
            get
            {
                return isNewState;
            }
            set
            {
                isNewState = value;
                RaisePropertyChanged("IsNewState");
                RaisePropertyChanged("SaveEnabled");
                RaisePropertyChanged("IsNewStateEnabled");
                RaisePropertyChanged("IsUndoEnabled");
            }
        }
        public bool IsNewStateEnabled
        {
            get
            {
                return (!IsNewState);
            }
        }
        public bool IsDeleteEnabled
        {
            get {
                return (currentItem != null);
            }
        }
        public bool IsUndoEnabled
        {
            get
            {
                return IsNewState || IsModified;
            }
        }


        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IDataErrorInfo
        string IDataErrorInfo.Error
        {
            get { throw new NotImplementedException(); }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (propertyGetters.ContainsKey(columnName))
                {
                    var value = propertyGetters[columnName](dataCashItem);
                    foreach (var iii in validators[columnName])
                    {
                        if (! ( iii.IsValid(value) ) )
                        {
                            string s = null;
                            var disp = displaynames[columnName];
                            if (string.IsNullOrEmpty(disp)) s = columnName; else s = disp;
                            return iii.FormatErrorMessage(s);
                        }
                    }
                }
                return string.Empty;
            }
        }
        #endregion

        #region IsCashItemValid
        public bool IsCashItemValid
        {
            get
            {
                foreach (var propGetter in propertyGetters)
                {
                    var value = propertyGetters[propGetter.Key](dataCashItem);
                    foreach (var iii in validators[propGetter.Key])
                    {
                        if (!(iii.IsValid(value))) return false;
                    }
                }
                return true;
            }
        }
        #endregion

        #region NewCommand

        protected virtual void OnDoNew() {
        }

        private bool canNew = true;
        public ICommand NewCommand { get; protected set; }
        protected void OnNew(object arg)
        {
            this.canNew = false;
            try
            {
                OnDoNew();
                IsNewState = true;
                ShowLocalDialog(Resources.NewCommand);
            }
            finally
            {
                this.canNew = true;
            }
        }
        protected bool CanNew(object arg)
        {
            return this.canNew;
        }
        #endregion

        #region DeleteCommand
        protected virtual void OnDoDelete()
        {
        }

        private bool canDelete = true;
        public ICommand DeleteCommand { get; protected set; }
        protected void OnDelete(object arg)
        {
            this.canDelete = false;
            try
            {

                if ((Items != null) && (CurrentItem != null))
                {
                    this.ConfirmationDialog.Raise(
                        new Confirmation { Content = Resources.ConfirmDelete, Title = Resources.ConfirmDelete },
                        confirmation => {
                            if(confirmation.Confirmed) {
                                var currIndex = Items.IndexOf(CurrentItem);
                                OnDoDelete();
                                Items.Remove(CurrentItem);
                                if (currIndex < Items.Count)
                                {
                                    CurrentItem = Items[currIndex];
                                }
                                else
                                {
                                    currIndex--;
                                    if ((currIndex < Items.Count) && (currIndex > -1))
                                        CurrentItem = Items[currIndex];
                                    else
                                        CurrentItem = default(T); // null;
                                }
                            }
                    });
                }
            }
            finally
            {
                this.canDelete = true;
            }
        }
        protected bool CanDelete(object arg)
        {
            return this.canDelete;
        }
        #endregion

        #region ReloadCommand
        private bool canReload = true;
        public ICommand ReloadCommand { get; protected set; }
        protected void OnReload(object arg)
        {
            this.canReload = false;
            try
            {
                CurrentItem = default(T);
                OnDoReload();
//                RaisePropertyChanged("Items");
            }
            finally
            {
                this.canReload = true;
            }
        }
        protected bool CanReload(object arg) { return this.canReload; }
        #endregion

        #region SaveCommand
        protected virtual T NewInstance()
        {
            return default(T);
        }
        protected virtual T OnDoInser(T item)
        {
            return default(T);
        }
        protected virtual void OnDoUpdate(T item)
        {
        }
        protected bool canSave = true;
        public ICommand SaveCommand { get; protected set; }
        protected virtual void OnSave(object arg)
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
                }
            }
            finally
            {
                this.canSave = true;
            }
        }
        protected bool CanSave(object arg) { return this.canSave; }
        #endregion

        #region UndoCommand
        protected bool canUndo = true;
        public ICommand UndoCommand { get; protected set; }
        protected virtual void OnUndo(object arg)
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
            }
            finally
            {
                this.canUndo = true;
            }
        }
        protected bool CanUndo(object arg) { return this.canUndo; }
        #endregion

        #region PaginationStaff
        public ICommand FirstCommand { get; protected set; }
        protected void OnFirstCommand(object arg)
        {
            try
            {
                start = 0;
                OnDoReload();
//                RaisePropertyChanged("Items");
            }
            finally
            {
            }
        }
        public bool CanFirstCommand{
        get{
            return start - itemCount >= 0 ? true : false;
        }
        }

        public ICommand PreviousCommand { get; protected set; }
        protected void OnPreviousCommand(object arg)
        {
            start -= itemCount;
            OnDoReload();
//            RaisePropertyChanged("Items");
        }
        public bool CanPreviousCommand
        {
            get
            {
                return start - itemCount >= 0 ? true : false;
            }
        }


        public ICommand NextCommand { get; protected set; }
        protected void OnNextCommand(object arg)
        {
            start += itemCount;
            OnDoReload();
//            RaisePropertyChanged("Items");
        }
        public bool CanNextCommand
        {
            get
            {
                return start + itemCount < totalItems ? true : false;
            }
        }

        public ICommand LastCommand { get; protected set; }
        protected void OnLastCommand(object arg)
        {
            try
            {
                start = (totalItems / itemCount - 1) * itemCount;
                start += totalItems % itemCount == 0 ? 0 : itemCount;
                OnDoReload();
//                RaisePropertyChanged("Items");
            }
            finally
            {
            }
        }
        public bool CanLastCommand
        {
            get
            {
                return start + itemCount < totalItems ? true : false;
            }
        }


        protected int start = 0;
        protected int itemCount = 4;
        protected string sortColumn = string.Empty;
        protected bool ascending = true;
        protected int totalItems = 0;

        /// <summary>
        /// Gets the index of the first item in the products list.
        /// </summary>        
        public int Start { get { return start + 1; } }

        /// <summary>
        /// Gets the index of the last item in the products list.
        /// </summary>
        public int End { get { return start + itemCount < totalItems ? start + itemCount : totalItems; } }

        /// <summary>
        /// The number of total items in the data store.
        /// </summary>
        public int TotalItems { get { return totalItems; } }

        /// <summary>
        /// Refreshes the list of products. Called by navigation commands.
        /// </summary>
        protected virtual void OnDoReload()
        {
            DoRefreshItems(start, itemCount, sortColumn, ascending,  out totalItems);

            RaisePropertyChanged("Start");
            RaisePropertyChanged("End");
            RaisePropertyChanged("TotalItems");

            RaisePropertyChanged("CanFirstCommand");
            RaisePropertyChanged("CanPreviousCommand");
            RaisePropertyChanged("CanNextCommand");
            RaisePropertyChanged("CanLastCommand");
            RaisePropertyChanged("Items");
            RaisePropertyChanged("IsPagingDisplay");
        }

        protected virtual void DoRefreshItems(int startVal, int itemCountVal, string sortColumnVal, bool ascendingVal,  out int totalItemsVal)
        {
            totalItemsVal = itemCountVal;
        }

        public bool IsPagingDisplay
        {
            get
            {
                return  (itemCount-start) < totalItems;
            }
        }
        

        #endregion

        #region Sorting Staff

        public void DoSort(string Column,  bool sortAscending) {
            bool callRefresh = (this.sortColumn != Column) || (this.ascending != sortAscending);
            this.sortColumn = Column;
            this.ascending = sortAscending;
            if (callRefresh && IsPagingDisplay)
                OnDoReload();
        }

        #endregion

        #region InformationDialog
        public InteractionRequest<Notification> InformationDialog { get; private set; }
        protected void ShowInformationDialog(string content, string title)
        {
            this.InformationDialog.Raise(new Notification { Content = content, Title = title }, dummy => { });
        }
        #endregion

        #region ConfirmationDialog
        public InteractionRequest<Confirmation> ConfirmationDialog { get; private set; }
        #endregion

        #region LocalDialog
        public InteractionRequest<Notification> LocalDialog { get; private set; }
        protected virtual void ShowLocalDialog(string title)
        {
            if (this.LocalDialog != null)
            {
                this.LocalDialog.Raise(new Notification { Content = this, Title = title }, dummy => { });
            }
        }

        #endregion

        #region EditCommand
        public ICommand EditCommand { get; protected set; }
        protected virtual void OnEdit(object arg)
        {
            ShowLocalDialog(Resources.EditCommand);
        }
        #endregion

        #region searchStaff
        protected string searchColumn;
        public string SearchColumn
        {
            get
            {
                return searchColumn;
            }
            set
            {
                if (searchColumn != value)
                {
                    searchColumn = value;
                    RaisePropertyChanged("SearchColumn");
                }
            }
        }

        Dictionary<string, string> searchColumnList = null;
        protected virtual Dictionary<string, string> GetSearchColumnList()
        {
            return null;
        }
        public Dictionary<string, string> SearchColumnList {
            get
            {
                if (searchColumnList == null)
                {
                    searchColumnList = GetSearchColumnList();
                }
                return searchColumnList;
            }
            set
            {
                searchColumnList = value;
                RaisePropertyChanged("SearchColumnList");
            }
        }

        protected string searchFilter=string.Empty;
        public string SearchFilter { 
            get { return searchFilter; }
            set {
                if (searchFilter != value)
                {
                    searchFilter = value;
                    RaisePropertyChanged("SearchFilter");
                }
            }
        }

        public List<KeyValuePair<string, string>> AutocompleteItems { get; set; }

        protected virtual List<KeyValuePair<string, string>> AsyncGetAutocompleteItems(string filtercriteria)
        {
            return new List<KeyValuePair<string, string>>();
        }
        public delegate List<KeyValuePair<string, string>> AsyncMethodCaller(string filtercriteria);

        private void DoExecuteFilterAsync(FilterAutoCompleteParameters facp)
        {
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncMethodCaller(this.AsyncGetAutocompleteItems);
            caller.BeginInvoke(facp.FilterCriteria, e =>
                {
                    var newAutocompleteItems = caller.EndInvoke(e);

                    if (_dispatcher.CheckAccess())
                    {
                        AutocompleteItems = newAutocompleteItems;
                        RaisePropertyChanged("AutocompleteItems");
                        facp.FilterComplete();
                    }
                    else
                    {
                        _dispatcher.Invoke(new Action(() =>
                            {
                                AutocompleteItems = newAutocompleteItems;
                                RaisePropertyChanged("AutocompleteItems");
                                facp.FilterComplete();
                            }
                        ));
                    }
                }, null);
        }


        public ICommand ExecuteFilterAsyncCommand { get; protected set; }
        #endregion

    }

    

}
/*
var dispatcher = System.Windows.Deployment.Current.Dispatcher;
if (dispatcher.CheckAccess())
{
QuestionnaireView.DataContext = questionnaire;
}
else
{
dispatcher.BeginInvoke(
() => { Questionnaire.DataContext = questionnaire; });
}
*/