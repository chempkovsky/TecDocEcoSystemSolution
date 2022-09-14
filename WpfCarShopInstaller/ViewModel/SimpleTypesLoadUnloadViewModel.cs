using CarShopDataService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TecDocEcoSystemDbClassLibrary;
using WpfCarShopInstaller.Properties;
using System.Windows.Threading;
using CarShopDataService.Interfaces;
using Prism.Interactivity.InteractionRequest;
using Prism.Commands;
using System.Web.Script.Serialization;

namespace WpfCarShopInstaller.ViewModel
{
    public class SimpleTypesLoadUnloadViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        ICarShopDataService dataservice;

        public SimpleTypesLoadUnloadViewModel(ICarShopDataService dataservice)
        {
            this.dataservice = dataservice;

            InformationDialog = new InteractionRequest<Notification>();
            ConfirmationDialog = new InteractionRequest<Confirmation>();

            UndoCommand = new DelegateCommand<object>(this.OnUndoCommand, e => true);

            AddressTypeLoadCommand = new DelegateCommand<object>(this.OnAddressTypeLoadCommand, e => true);
            AddressTypeSaveCommand = new DelegateCommand<object>(this.OnAddressTypeSaveCommand, e => true);

            BranchTypeLoadCommand = new DelegateCommand<object>(this.OnBranchTypeLoadCommand, e => true);
            BranchTypeSaveCommand = new DelegateCommand<object>(this.OnBranchTypeSaveCommand, e => true);

            ContactTypeLoadCommand = new DelegateCommand<object>(this.OnContactTypeLoadCommand, e => true);
            ContactTypeSaveCommand = new DelegateCommand<object>(this.OnContactTypeSaveCommand, e => true);

            CountryLoadCommand = new DelegateCommand<object>(this.OnCountryLoadCommand, e => true);
            CountrySaveCommand = new DelegateCommand<object>(this.OnCountrySaveCommand, e => true);

            CurrencyLoadCommand = new DelegateCommand<object>(this.OnCurrencyLoadCommand, e => true);
            CurrencySaveCommand = new DelegateCommand<object>(this.OnCurrencySaveCommand, e => true);

            SettlementTypeLoadCommand = new DelegateCommand<object>(this.OnSettlementTypeLoadCommand, e => true);
            SettlementTypeSaveCommand = new DelegateCommand<object>(this.OnSettlementTypeSaveCommand, e => true);

            StreetTypeLoadCommand = new DelegateCommand<object>(this.OnStreetTypeLoadCommand, e => true);
            StreetTypeSaveCommand = new DelegateCommand<object>(this.OnStreetTypeSaveCommand, e => true);

            SoatoLoadCommand = new DelegateCommand<object>(this.OnSoatoLoadCommand, e => true);
            SoatoSaveCommand = new DelegateCommand<object>(this.OnSoatoSaveCommand, e => true);


            EnterpriseTecDocSrcTypeTDESLoadCommand = new DelegateCommand<object>(this.OnEnterpriseTecDocSrcTypeTDESLoadCommand, e => true);
            EnterpriseTecDocSrcTypeTDESSaveCommand = new DelegateCommand<object>(this.OnEnterpriseTecDocSrcTypeTDESSaveCommand, e => true);

        }

        int loadedCount;
        public int LoadedCount
        {
            get
            {
                return loadedCount;
            }
            set
            {
                loadedCount = value;
                RaisePropertyChanged("LoadedCount");
            }
        }
        int readCount;
        public int ReadCount
        {
            get
            {
                return readCount;
            }
            set
            {
                readCount = value;
                RaisePropertyChanged("ReadCount");
            }
        }


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
        public string GetFileNameError(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return Resources.Error_FileEmpty;
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
                return Resources.Error_FileIncorrect;
            return string.Empty;
        }
        string IDataErrorInfo.this[string columnName]
        {
            get {
                switch (columnName)
                {
                    case "BranchTypeFile":
                        return GetFileNameError(BranchTypeFile);
                    case "ContactTypeFile":
                        return GetFileNameError(ContactTypeFile);
                    case "CountryFile":
                        return GetFileNameError(CountryFile);
                    case "CurrencyFile":
                        return GetFileNameError(CurrencyFile);
                    case "SettlementTypeFile":
                        return GetFileNameError(SettlementTypeFile);
                    case "StreetTypeFile":
                        return GetFileNameError(StreetTypeFile);
                    case "SoatoFile":
                        return GetFileNameError(SoatoFile);
                    case "AddressTypeFile":
                        return GetFileNameError(AddressTypeFile);
                    case "EnterpriseTecDocSrcTypeTDESFile":
                        return GetFileNameError(EnterpriseTecDocSrcTypeTDESFile);

                }
                return string.Empty;
            }
        }
        #endregion

        #region BranchType
        public bool BranchTypeEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(branchTypeFile));
            }
        }
        string branchTypeFile = "BranchType.txt";
        public string BranchTypeFile {
            get
            {
                return branchTypeFile;
            }
            set
            {
                branchTypeFile = value;
                RaisePropertyChanged("BranchTypeFile");
                RaisePropertyChanged("BranchTypeEnabled");
            }
        }
        public ICommand BranchTypeLoadCommand { get; protected set; }
        protected void OnBranchTypeLoadCommand(object arg)
        {
            if (!BranchTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(BranchTypeFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(BranchTypeFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            DoBranchTypeLoad();
        }
        public ICommand BranchTypeSaveCommand { get; protected set; }
        protected void OnBranchTypeSaveCommand(object arg)
        {
            if (!BranchTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(BranchTypeFile), Resources.Error_TITLE);
                return;
            }
            if (File.Exists(BranchTypeFile))
            {
                this.ConfirmationDialog.Raise(
                    new Confirmation { Content = Resources.Error_FILEREWRITE, Title = Resources.Confirmation_TITLE },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            DoBranchTypeSave();
                        }
                    });
                return;
            }
            else
            {
                DoBranchTypeSave();
            }
        }
        protected void DoBranchTypeLoad()
        {
            string textFile;
            using (TextReader reader = File.OpenText(BranchTypeFile))
            {
                textFile = reader.ReadToEnd();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Items = serializer.Deserialize<List<BranchType>>(textFile);
            foreach (var item in Items)
            {
                dataservice.BranchTypeInsertOrUpdate(item);
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        protected void DoBranchTypeSave()
        {
            var Items = dataservice.BranchTypeIndex(0, 400, "", true).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(Items);
            using (TextWriter writer = File.CreateText(BranchTypeFile))
            {
                writer.Write(serializer.Serialize(Items));
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        #endregion

        #region ContactType
        public bool ContactTypeEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(contactTypeFile));
            }
        }
        string contactTypeFile = "ContactType.txt";
        public string ContactTypeFile {
            get
            {
                return contactTypeFile;
            }
            set
            {
                contactTypeFile = value;
                RaisePropertyChanged("ContactTypeFile");
                RaisePropertyChanged("ContactTypeEnabled");
            }
        }
        public ICommand ContactTypeLoadCommand { get; protected set; }
        public ICommand ContactTypeSaveCommand { get; protected set; }
        protected void OnContactTypeLoadCommand(object arg)
        {
            if (!ContactTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(ContactTypeFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(ContactTypeFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            DoContactTypeLoad();
        }
        protected void OnContactTypeSaveCommand(object arg)
        {
            if (!ContactTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(ContactTypeFile), Resources.Error_TITLE);
                return;
            }
            if (File.Exists(ContactTypeFile))
            {
                this.ConfirmationDialog.Raise(
                    new Confirmation { Content = Resources.Error_FILEREWRITE, Title = Resources.Confirmation_TITLE },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            DoContactTypeSave();
                        }
                    });
                return;
            }
            else
            {
                DoContactTypeSave();
            }
        }
        protected void DoContactTypeLoad()
        {
            string textFile;
            using (TextReader reader = File.OpenText(ContactTypeFile))
            {
                textFile = reader.ReadToEnd();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Items = serializer.Deserialize<List<ContactType>>(textFile);
            foreach (var item in Items)
            {
                dataservice.ContactTypeInsertOrUpdate(item);
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        protected void DoContactTypeSave()
        {

            var Items = dataservice.ContactTypeIndex(0, 400, "", true).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(Items);
            using (TextWriter writer = File.CreateText(ContactTypeFile))
            {
                writer.Write(serializer.Serialize(Items));
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        #endregion

        #region Country
        public bool CountryEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(CountryFile));
            }
        }
        string countryFile = "Country.txt";
        public string CountryFile
        {
            get
            {
                return countryFile;
            }
            set
            {
                countryFile = value;
                RaisePropertyChanged("CountryFile");
                RaisePropertyChanged("CountryEnabled");
            }
        }
        public ICommand CountryLoadCommand { get; protected set; }
        public ICommand CountrySaveCommand { get; protected set; }
        protected void OnCountryLoadCommand(object arg)
        {
            if (!CountryEnabled)
            {
                ShowInformationDialog(GetFileNameError(CountryFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(CountryFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            undoCommandValue = false;
            StartState = false;

            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoSoatoLoad(this.DoCountryLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken, Resources.Error_TITLE);
                    else
                        ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken, Resources.Error_TITLE);
                        else
                            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
                    }
                    ));
                }
            }, null);
        }
        protected void OnCountrySaveCommand(object arg)
        {
            if (!CountryEnabled)
            {
                ShowInformationDialog(GetFileNameError(CountryFile), Resources.Error_TITLE);
                return;
            }
            if (File.Exists(CountryFile))
            {
                this.ConfirmationDialog.Raise(
                    new Confirmation { Content = Resources.Error_FILEREWRITE, Title = Resources.Confirmation_TITLE },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            DoCountrySave();
                        }
                    });
                return;
            }
            else
            {
                DoCountrySave();
            }
        }
        protected void DoCountryLoad()
        {
            int SavedCount = 0;
            int AllCount = 0;
            string textFile;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            if (_dispatcher.CheckAccess())
            {
                LoadedCount = SavedCount;
                ReadCount = AllCount;
            }
            else
            {
                _dispatcher.Invoke(new Action(() =>
                {
                    LoadedCount = SavedCount;
                    ReadCount = AllCount;
                }
                ));
            }

            using (TextReader reader = File.OpenText(CountryFile))
            {
                textFile = reader.ReadToEnd();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Items = serializer.Deserialize<List<Country>>(textFile);
            foreach (var item in Items)
            {
                dataservice.CountryInsertOrUpdate(item);
                SavedCount++;
                AllCount++;
                if (_dispatcher.CheckAccess())
                {
                    LoadedCount = SavedCount;
                    ReadCount = AllCount;
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        LoadedCount = SavedCount;
                        ReadCount = AllCount;
                    }
                    ));
                }
                if (undoCommandValue)
                {
                    break;
                }
            }
//            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        protected void DoCountrySave()
        {

            var Items = dataservice.CountryIndex(0, 400, "", true).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(Items);
            using (TextWriter writer = File.CreateText(CountryFile))
            {
                writer.Write(serializer.Serialize(Items));
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        #endregion

        #region Currency
        public bool CurrencyEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(CurrencyFile));
            }
        }
        string currencyFile = "Currency.txt";
        public string CurrencyFile
        {
            get
            {
                return currencyFile;
            }
            set
            {
                currencyFile = value;
                RaisePropertyChanged("CurrencyFile");
                RaisePropertyChanged("CurrencyEnabled");
            }
        }
        public ICommand CurrencyLoadCommand { get; protected set; }
        public ICommand CurrencySaveCommand { get; protected set; }
        protected void OnCurrencyLoadCommand(object arg)
        {
            if (!CurrencyEnabled)
            {
                ShowInformationDialog(GetFileNameError(CurrencyFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(CurrencyFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            undoCommandValue = false;
            StartState = false;

            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoSoatoLoad(this.DoCurrencyLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken, Resources.Error_TITLE);
                    else
                        ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken, Resources.Error_TITLE);
                        else
                            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
                    }
                    ));
                }
            }, null);
        }
        protected void OnCurrencySaveCommand(object arg)
        {
            if (!CurrencyEnabled)
            {
                ShowInformationDialog(GetFileNameError(CurrencyFile), Resources.Error_TITLE);
                return;
            }
            if (File.Exists(CurrencyFile))
            {
                this.ConfirmationDialog.Raise(
                    new Confirmation { Content = Resources.Error_FILEREWRITE, Title = Resources.Confirmation_TITLE },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            DoCurrencySave();
                        }
                    });
                return;
            }
            else
            {
                DoCurrencySave();
            }
        }
        protected void DoCurrencyLoad()
        {
            int SavedCount = 0;
            int AllCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            if (_dispatcher.CheckAccess())
            {
                LoadedCount = SavedCount;
                ReadCount = AllCount;
            }
            else
            {
                _dispatcher.Invoke(new Action(() =>
                {
                    LoadedCount = SavedCount;
                    ReadCount = AllCount;
                }
                ));
            }


            string textFile;
            using (TextReader reader = File.OpenText(CurrencyFile))
            {
                textFile = reader.ReadToEnd();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Items = serializer.Deserialize<List<Currency>>(textFile);
            foreach (var item in Items)
            {
                dataservice.CurrencyInsertOrUpdate(item);
                SavedCount++;
                AllCount++;
                if (_dispatcher.CheckAccess())
                {
                    LoadedCount = SavedCount;
                    ReadCount = AllCount;
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        LoadedCount = SavedCount;
                        ReadCount = AllCount;
                    }
                    ));
                }
                if (undoCommandValue)
                {
                    break;
                }
            }
            // ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        protected void DoCurrencySave()
        {

            var Items = dataservice.CurrencyIndex(0, 400, "", true).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(Items);
            using (TextWriter writer = File.CreateText(CurrencyFile))
            {
                writer.Write(serializer.Serialize(Items));
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        #endregion

        #region SettlementType
        public bool SettlementTypeEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(SettlementTypeFile));
            }
        }
        string settlementTypeFile = "SettlementType.txt";
        public string SettlementTypeFile
        {
            get
            {
                return settlementTypeFile;
            }
            set
            {
                settlementTypeFile = value;
                RaisePropertyChanged("SettlementTypeFile");
                RaisePropertyChanged("SettlementTypeEnabled");
            }
        }
        public ICommand SettlementTypeLoadCommand { get; protected set; }
        public ICommand SettlementTypeSaveCommand { get; protected set; }
        protected void OnSettlementTypeLoadCommand(object arg)
        {
            if (!SettlementTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(SettlementTypeFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(SettlementTypeFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            DoSettlementTypeLoad();
        }
        protected void OnSettlementTypeSaveCommand(object arg)
        {
            if (!SettlementTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(SettlementTypeFile), Resources.Error_TITLE);
                return;
            }
            if (File.Exists(SettlementTypeFile))
            {
                this.ConfirmationDialog.Raise(
                    new Confirmation { Content = Resources.Error_FILEREWRITE, Title = Resources.Confirmation_TITLE },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            DoSettlementTypeSave();
                        }
                    });
                return;
            }
            else
            {
                DoSettlementTypeSave();
            }
        }
        protected void DoSettlementTypeLoad()
        {
            string textFile;
            using (TextReader reader = File.OpenText(SettlementTypeFile))
            {
                textFile = reader.ReadToEnd();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Items = serializer.Deserialize<List<SettlementType>>(textFile);
            foreach (var item in Items)
            {
                dataservice.SettlementTypeInsertOrUpdate(item);
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        protected void DoSettlementTypeSave()
        {

            var Items = dataservice.SettlementTypeIndex(0, 400, "", true).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(Items);
            using (TextWriter writer = File.CreateText(SettlementTypeFile))
            {
                writer.Write(serializer.Serialize(Items));
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        #endregion

        #region StreetType
        public bool StreetTypeEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(StreetTypeFile));
            }
        }
        string streetTypeFile = "StreetType.txt";
        public string StreetTypeFile
        {
            get
            {
                return streetTypeFile;
            }
            set
            {
                streetTypeFile = value;
                RaisePropertyChanged("StreetTypeFile");
                RaisePropertyChanged("StreetTypeEnabled");
            }
        }
        public ICommand StreetTypeLoadCommand { get; protected set; }
        public ICommand StreetTypeSaveCommand { get; protected set; }
        protected void OnStreetTypeLoadCommand(object arg)
        {
            if (!StreetTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(StreetTypeFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(StreetTypeFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            DoStreetTypeLoad();
        }
        protected void OnStreetTypeSaveCommand(object arg)
        {
            if (!StreetTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(StreetTypeFile), Resources.Error_TITLE);
                return;
            }
            if (File.Exists(StreetTypeFile))
            {
                this.ConfirmationDialog.Raise(
                    new Confirmation { Content = Resources.Error_FILEREWRITE, Title = Resources.Confirmation_TITLE },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            DoStreetTypeSave();
                        }
                    });
                return;
            }
            else
            {
                DoStreetTypeSave();
            }
        }
        protected void DoStreetTypeLoad()
        {
            string textFile;
            using (TextReader reader = File.OpenText(StreetTypeFile))
            {
                textFile = reader.ReadToEnd();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Items = serializer.Deserialize<List<StreetType>>(textFile);
            foreach (var item in Items)
            {
                dataservice.StreetTypeInsertOrUpdate(item);
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        protected void DoStreetTypeSave()
        {

            var Items = dataservice.StreetTypeIndex(0, 400, "", true).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(Items);
            using (TextWriter writer = File.CreateText(StreetTypeFile))
            {
                writer.Write(serializer.Serialize(Items));
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        #endregion

        #region Soato
        public bool SoatoEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(SoatoFile));
            }
        }
        string soatoFile = "Soato.txt";
        public string SoatoFile
        {
            get
            {
                return soatoFile;
            }
            set
            {
                soatoFile = value;
                RaisePropertyChanged("SoatoFile");
                RaisePropertyChanged("SoatoEnabled");
            }
        }
        public ICommand SoatoLoadCommand { get; protected set; }
        public ICommand SoatoSaveCommand { get; protected set; }
        public delegate void AsyncDoSoatoLoad();
        protected void OnSoatoLoadCommand(object arg)
        {
            if (!SoatoEnabled)
            {
                ShowInformationDialog(GetFileNameError(SoatoFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(SoatoFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            undoCommandValue = false;
            StartState = false;

            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoSoatoLoad(this.DoSoatoLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken, Resources.Error_TITLE);
                    else
                        ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken, Resources.Error_TITLE);
                        else
                            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
                    }
                    ));
                }
            }, null);

            // DoSoatoLoad();
        }
        protected void OnSoatoSaveCommand(object arg)
        {
            if (!SoatoEnabled)
            {
                ShowInformationDialog(GetFileNameError(SoatoFile), Resources.Error_TITLE);
                return;
            }
            if (File.Exists(SoatoFile))
            {
                this.ConfirmationDialog.Raise(
                    new Confirmation { Content = Resources.Error_FILEREWRITE, Title = Resources.Confirmation_TITLE },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            DoSoatoSave();
                        }
                    });
                return;
            }
            else
            {
                DoSoatoSave();
            }
        }
        protected void DoSoatoLoad()
        {
            Soato soatoObj = new Soato();
            string textFile;
            char[] separator = new char[] { '=' };
            int AllCount = 0;
            int SavedCount = 0;

            var _dispatcher = Dispatcher.CurrentDispatcher;

            using (TextReader reader = File.OpenText(SoatoFile))
            {
                List<SettlementType> stps = dataservice.SettlementTypeIndex(0, 400, "", true).ToList();
                SettlementType stp = null;
                textFile = reader.ReadLine();
                while ( textFile != null )  {
                    AllCount++;
                    var parts = textFile.Split(separator);
                    if (parts.Count() > 1)
                    {
                        soatoObj.SoatoId = parts[0];
                        soatoObj.SoatoSettlementName = null;
                        soatoObj.SettlementTypeId = -1;


                        if (!string.IsNullOrEmpty(textFile))
                        {
                            textFile = parts[1];
                            stp = (from e in stps
                                  where textFile.StartsWith(e.SettlementTypeShortDescription)
                                  orderby e.SettlementTypeShortDescription.Length descending
                                  select e).FirstOrDefault();
                            if (stp != null) {
                                soatoObj.SettlementTypeId = stp.SettlementTypeId;
                                soatoObj.SoatoSettlementName = textFile.Substring(stp.SettlementTypeShortDescription.Length+1);
                            } else {
                                stp = (from e in stps
                                       where textFile.EndsWith(e.SettlementTypeShortDescription)
                                       orderby e.SettlementTypeShortDescription.Length descending
                                       select e).FirstOrDefault();
                                if (stp != null) {
                                    soatoObj.SettlementTypeId = stp.SettlementTypeId;
                                    soatoObj.SoatoSettlementName = textFile.Substring(0, textFile.Length - stp.SettlementTypeShortDescription.Length -1);
                                } 
                            }
                        }
                    }
                    if (soatoObj.SoatoSettlementName != null)
                    {
                        dataservice.SoatoInsertOrUpdate(soatoObj);
                        SavedCount++;
                    }

                    if (_dispatcher.CheckAccess())
                    {
                        LoadedCount = SavedCount;
                        ReadCount = AllCount;
                    }
                    else
                    {
                        _dispatcher.Invoke(new Action(() =>
                        {
                            LoadedCount = SavedCount;
                            ReadCount = AllCount;
                        }
                        ));
                    }


                    textFile = reader.ReadLine();

                    if (undoCommandValue)
                    {
                        break;
                    }
                }
            }
            // ShowInformationDialog(Resources.Info_OperationIsDone + ssss + "  " + SavedCount.ToString() + " / " + AllCount.ToString() + "  ", Resources.Info_TITLE);
        }
        protected void DoSoatoSave()
        {
            /*
            var Items = dataservice.SoatoIndex(0, 400, "", true).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(Items);
            using (TextWriter writer = File.CreateText(SoatoFile))
            {
                writer.Write(serializer.Serialize(Items));
            }
            */
            ShowInformationDialog(Resources.Operation_NotImplemented, Resources.Info_TITLE);
        }
        bool startState = true;
        public bool StartState
        {
            get
            {
                return startState;
            }
            set
            {
                if (startState != value)
                {
                    startState = value;
                    RaisePropertyChanged("StartState");
                }
            }
        }
        bool undoCommandValue = false;
        public ICommand UndoCommand { get; protected set; }
        protected void OnUndoCommand(object arg)
        {
            undoCommandValue = true;
        }
        #endregion

        #region AddressType
        public bool AddressTypeEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(AddressTypeFile));
            }
        }
        string addressTypeFile = "AddressType.txt";
        public string AddressTypeFile
        {
            get
            {
                return addressTypeFile;
            }
            set
            {
                addressTypeFile = value;
                RaisePropertyChanged("AddressTypeFile");
                RaisePropertyChanged("AddressTypeEnabled");
            }
        }
        public ICommand AddressTypeLoadCommand { get; protected set; }
        protected void OnAddressTypeLoadCommand(object arg)
        {
            if (!AddressTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(AddressTypeFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(AddressTypeFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            DoAddressTypeLoad();
        }
        public ICommand AddressTypeSaveCommand { get; protected set; }
        protected void OnAddressTypeSaveCommand(object arg)
        {
            if (!AddressTypeEnabled)
            {
                ShowInformationDialog(GetFileNameError(AddressTypeFile), Resources.Error_TITLE);
                return;
            }
            if (File.Exists(AddressTypeFile))
            {
                this.ConfirmationDialog.Raise(
                    new Confirmation { Content = Resources.Error_FILEREWRITE, Title = Resources.Confirmation_TITLE },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            DoAddressTypeSave();
                        }
                    });
                return;
            }
            else
            {
                DoAddressTypeSave();
            }
        }
        protected void DoAddressTypeLoad()
        {
            string textFile;
            using (TextReader reader = File.OpenText(AddressTypeFile))
            {
                textFile = reader.ReadToEnd();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Items = serializer.Deserialize<List<AddressType>>(textFile);
            foreach (var item in Items)
            {
                dataservice.AddressTypeInsertOrUpdate(item);
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        protected void DoAddressTypeSave()
        {
            var Items = dataservice.AddressTypeIndex(0, 400, "", true).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(Items);
            using (TextWriter writer = File.CreateText(AddressTypeFile))
            {
                writer.Write(serializer.Serialize(Items));
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        #endregion


        #region EnterpriseTecDocSrcTypeTDES
        public bool EnterpriseTecDocSrcTypeTDESEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(EnterpriseTecDocSrcTypeTDESFile));
            }
        }
        string enterpriseTecDocSrcTypeTDESFile = "TecDocSrcType.txt";
        public string EnterpriseTecDocSrcTypeTDESFile
        {
            get
            {
                return enterpriseTecDocSrcTypeTDESFile;
            }
            set
            {
                addressTypeFile = value;
                RaisePropertyChanged("EnterpriseTecDocSrcTypeTDESFile");
                RaisePropertyChanged("EnterpriseTecDocSrcTypeTDESEnabled");
            }
        }
        public ICommand EnterpriseTecDocSrcTypeTDESLoadCommand { get; protected set; }
        protected void OnEnterpriseTecDocSrcTypeTDESLoadCommand(object arg)
        {
            if (!EnterpriseTecDocSrcTypeTDESEnabled)
            {
                ShowInformationDialog(GetFileNameError(EnterpriseTecDocSrcTypeTDESFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(EnterpriseTecDocSrcTypeTDESFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            DoEnterpriseTecDocSrcTypeTDESLoad();
        }
        public ICommand EnterpriseTecDocSrcTypeTDESSaveCommand { get; protected set; }
        protected void OnEnterpriseTecDocSrcTypeTDESSaveCommand(object arg)
        {
            if (!EnterpriseTecDocSrcTypeTDESEnabled)
            {
                ShowInformationDialog(GetFileNameError(EnterpriseTecDocSrcTypeTDESFile), Resources.Error_TITLE);
                return;
            }
            if (File.Exists(EnterpriseTecDocSrcTypeTDESFile))
            {
                this.ConfirmationDialog.Raise(
                    new Confirmation { Content = Resources.Error_FILEREWRITE, Title = Resources.Confirmation_TITLE },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            DoEnterpriseTecDocSrcTypeTDESSave();
                        }
                    });
                return;
            }
            else
            {
                DoEnterpriseTecDocSrcTypeTDESSave();
            }
        }
        protected void DoEnterpriseTecDocSrcTypeTDESLoad()
        {
            string textFile;
            using (TextReader reader = File.OpenText(EnterpriseTecDocSrcTypeTDESFile))
            {
                textFile = reader.ReadToEnd();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var Items = serializer.Deserialize<List<EnterpriseTecDocSrcTypeTDES>>(textFile);
            foreach (var item in Items)
            {
                dataservice.EnterpriseTecDocSrcTypeTDESInsertOrUpdate(item);
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        protected void DoEnterpriseTecDocSrcTypeTDESSave()
        {
            var Items = dataservice.EnterpriseTecDocSrcTypeTDESIndex(0, 400, "", true).ToList();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(Items);
            using (TextWriter writer = File.CreateText(EnterpriseTecDocSrcTypeTDESFile))
            {
                writer.Write(serializer.Serialize(Items));
            }
            ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
        }
        #endregion


    }
}
