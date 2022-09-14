using CarShopDataService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WpfCarShopInstaller.Properties;
using System.IO;
using System.Windows.Input;
using System.Windows.Threading;
using TecDocEcoSystemDbClassLibrary;
using CarShopDataService.Interfaces;
using Prism.Regions;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace WpfCarShopInstaller.ViewModel
{
    public class LoadCategoryTreeViewModel : INotifyPropertyChanged, IDataErrorInfo, INavigationAware, IRegionMemberLifetime
    {
        int SavedCount = 0;
        bool HasError = false;
        ICarShopDataService dataservice;
        ICarShopArticleDataService articledataservice;
        ITecDocContext tecdoccontext;
        IRegionManager regionManager;
        ICarShopMsTecDocDataService mstecdocdataservice;

        public LoadCategoryTreeViewModel(ICarShopDataService dataservice, ICarShopArticleDataService articledataservice, IRegionManager regionManager, ITecDocContext tecdoccontext, ICarShopMsTecDocDataService mstecdocdataservice)
        {
            this.dataservice = dataservice;
            this.articledataservice = articledataservice;
            this.regionManager = regionManager;
            this.tecdoccontext = tecdoccontext;
            this.mstecdocdataservice = mstecdocdataservice;

            UndoCommand = new DelegateCommand<object>(this.OnUndoCommand, e => true);
            PassengerCarsLoadCommand = new DelegateCommand<object>(this.OnPassengerCarsLoadCommand, e => true);
            CommercialVehiclesLoadCommand = new DelegateCommand<object>(this.OnCommercialVehiclesLoadCommand, e => true);
            MotorsLoadCommand = new DelegateCommand<object>(this.OnMotorsLoadCommand, e => true);
            AxlesLoadCommand = new DelegateCommand<object>(this.OnAxlesLoadCommand, e => true);
            UniversalsLoadCommand = new DelegateCommand<object>(this.OnUniversalsLoadCommand, e => true);
            ArticleCategoryItemLoadCommand = new DelegateCommand<object>(this.OnArticleCategoryItemLoadCommand, e => true);
            ArticleBrandLoadCommand = new DelegateCommand<object>(this.OnArticleBrandLoadCommand, e => true);
            ArticleLookUpLoadCommand = new DelegateCommand<object>(this.OnArticleLookUpLoadCommand, e => true);
            ArticleLookUpMaxCommand = new DelegateCommand<object>(this.OnArticleLookUpMaxCommand, e => true);
            ArticleCategoryMaxCommand = new DelegateCommand<object>(this.OnArticleCategoryMaxCommand, e => true);

            ArticleApplicLoadCommand = new DelegateCommand<object>(this.OnArticleApplicLoadCommand, e => true);
            ArticleApplicMaxCommand = new DelegateCommand<object>(this.OnArticleApplicMaxCommand, e => true);

            FuelLoadCommand = new DelegateCommand<object>(this.OnFuelLoadCommand, e => true);
            CarBrandLoadCommand = new DelegateCommand<object>(this.OnCarBrandLoadCommand, e => true);
            CarModelTypeLoadCommand = new DelegateCommand<object>(this.OnCarModelTypeLoadCommand, e => true);
            CarModelLoadCommand = new DelegateCommand<object>(this.OnCarModelLoadCommand, e => true);

            TecDocLoadCommand = new DelegateCommand<object>(this.OnTecDocLoadCommand, e => true);
            TecDocMaxCommand = new DelegateCommand<object>(this.OnTecDocMaxCommand, e => true);

            ArticleTecDocLoadCommand = new DelegateCommand<object>(this.OnArticleTecDocLoadCommand, e => true);
            ArticleTecDocMaxCommand = new DelegateCommand<object>(this.OnArticleTecDocMaxCommand, e => true);

            CategoryTecDocLoadCommand = new DelegateCommand<object>(this.OnCategoryTecDocLoadCommand, e => true);
            CategoryItemTecDocLoadCommand = new DelegateCommand<object>(this.OnCategoryItemTecDocLoadCommand, e => true);

            EnterpriseTDESCommand = new DelegateCommand<object>(this.OnEnterpriseTDESCommand, e => true);

            InformationDialog = new InteractionRequest<Notification>();
            ConfirmationDialog = new InteractionRequest<Confirmation>();

        }

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
        public string GetRangeArticleLookUp(int aFrom ,int aTil)
        {
            if (aFrom < 0)
                return Resources.LessThanZero;
            if (aTil < 0)
                return Resources.LessThanZero;
            if (aFrom > aTil)
                return Resources.UpperLessThanLower;
            return string.Empty;
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "PassengerCarsFile":
                        return GetFileNameError(PassengerCarsFile);
                    case "CommercialVehiclesFile":
                        return GetFileNameError(CommercialVehiclesFile);
                    case "MotorsFile":
                        return GetFileNameError(MotorsFile);
                    case "AxlesFile":
                        return GetFileNameError(AxlesFile);
                    case "UniversalsFile":
                        return GetFileNameError(UniversalsFile);

                    case "ArticleLookUpFrom":
                        return GetRangeArticleLookUp(ArticleLookUpFrom, ArticleLookUpTil);
                    case "ArticleLookUpTil":
                        return GetRangeArticleLookUp(ArticleLookUpFrom, ArticleLookUpTil);

                    case "ArticleCategoryFrom":
                        return GetRangeArticleLookUp(ArticleCategoryFrom, ArticleCategoryTil);
                    case "ArticleCategoryTil":
                        return GetRangeArticleLookUp(ArticleCategoryFrom, ArticleCategoryTil);


                    case "TecDocFrom":
                        return GetRangeArticleLookUp(TecDocFrom, TecDocTil);
                    case "TecDocTil":
                        return GetRangeArticleLookUp(TecDocFrom, TecDocTil);

                    case "ArticleApplicFrom":
                        return GetRangeArticleLookUp(ArticleApplicFrom, ArticleApplicTil);
                    case "ArticleApplic":
                        return GetRangeArticleLookUp(ArticleApplicFrom, ArticleApplicTil);

                    case "ArticleTecDocFrom":
                        return GetRangeArticleLookUp(ArticleTecDocFrom, ArticleTecDocTil);
                    case "ArticleTecDocTil":
                        return GetRangeArticleLookUp(ArticleTecDocFrom, ArticleTecDocTil);

                        


                }
                return string.Empty;
            }
        }
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


        public Guid EntGuid { get; set; }
        public string EntDescription { get; set; }
        public string ArticleContextName { get; set; }
        public string MsTecDocContextName { get; set; }
        string processtitle = "";
        public string ProcessTitle
        {
            get
            {
                return processtitle;
            }
            set
            {
                processtitle = value;
                RaisePropertyChanged("ProcessTitle");
            }
        }


        #region Reporting staff
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
        #endregion



        protected string doSubTreeLoadFileName = null;
        public delegate void AsyncDoSubTreeLoad();
        protected void DoSubTreeLoad()
        {
            EnterpriseCategoryTDES categ = new EnterpriseCategoryTDES();
            EnterpriseCategoryItemTDES categItem = new EnterpriseCategoryItemTDES();
            EnterpriseCategoryDescriptionTDES categDescr = new EnterpriseCategoryDescriptionTDES();
            EnterpriseCategoryItemDescriptionTDES categItemDescr = new EnterpriseCategoryItemDescriptionTDES();

            articledataservice.SetConnectionStringName(ArticleContextName);

            string textFile;
            int AllCount = 0;
            SavedCount = 0;
            char[] separator = new char[] { ';' };
            var _dispatcher = Dispatcher.CurrentDispatcher;
            using (TextReader reader = File.OpenText(doSubTreeLoadFileName))
            {
                int CategId = 0;
                int ParentId = 0;
                int CategItemId = 0;
                textFile = reader.ReadLine();
                while (textFile != null)
                {
                    AllCount++;
                    var parts = textFile.Split(separator);
                    if (parts.Count() > 5)
                    {
                        if (int.TryParse(parts[0], out CategId))
                        {
                            if (!int.TryParse(parts[1], out ParentId))
                            {
                                ParentId = 0;
                            }
                            if (!int.TryParse(parts[3], out CategItemId))
                            {
                                CategItemId = 0;
                            }
                        }
                    }
                    else
                    {
                        CategId = 0;
                    }
                    if ((CategId != 0) && (CategItemId != 0))
                    {
                        categ.CategoryId = CategId;
                        categ.CategoryParent = ParentId;
                        categ.CategoryDescription = parts[2];
                        categ.EntGuid = EntGuid;

                        categItem.CategoryId = CategId;
                        categItem.CategoryItemId = CategItemId;
                        categItem.EntGuid = EntGuid;

                        categItemDescr.EntCategoryItemDescriptionId = 0;
                        categItemDescr.EntCategoryItemDescription = parts[4];

                        categDescr.EntCategoryDescriptionId = 0;
                        categDescr.EntCategoryDescription = parts[5];

                        categItemDescr = articledataservice.EnterpriseCategoryItemDescriptionTDESInsert(categItemDescr);
                        categDescr = articledataservice.EnterpriseCategoryDescriptionTDESInsert(categDescr);
                        articledataservice.EnterpriseCategoryTDESInsertOrUpdate(categ);

                        categItem.EntCategoryDescriptionId = categDescr.EntCategoryDescriptionId;
                        categItem.EntCategoryItemDescriptionId = categItemDescr.EntCategoryItemDescriptionId;
                        articledataservice.EnterpriseCategoryItemTDESInsertOrUpdate(categItem);
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
            HasError = false;
        }


        #region PassengerCars
        protected string passengercarsFile = "PassengerCarsTree.csv";
        public bool PassengerCarsFileEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(passengercarsFile));
            }
        }
        public string PassengerCarsFile
        {
            get
            {
                return passengercarsFile;
            }
            set
            {
                passengercarsFile = value;
                RaisePropertyChanged("PassengerCarsFile");
                RaisePropertyChanged("PassengerCarsFileEnabled");

            }
        }
        public ICommand PassengerCarsLoadCommand { get; protected set; }
        protected void OnPassengerCarsLoadCommand(object arg)
        {
            ProcessTitle = Resources.PassengerCarsFile_TITLE ;
            HasError = true;

            if (!PassengerCarsFileEnabled)
            {
                ShowInformationDialog(GetFileNameError(PassengerCarsFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(PassengerCarsFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            doSubTreeLoadFileName = PassengerCarsFile;
            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoSubTreeLoad(this.DoSubTreeLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);


        }
        #endregion

        #region сommercialмehicles
        protected string сommercialмehiclesfile = "CommercialVehiclesTree.csv";
        public bool CommercialVehiclesFileEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(сommercialмehiclesfile));
            }
        }
        public string CommercialVehiclesFile
        {
            get
            {
                return сommercialмehiclesfile;
            }
            set
            {
                сommercialмehiclesfile = value;
                RaisePropertyChanged("CommercialVehiclesFile");
                RaisePropertyChanged("CommercialVehiclesFileEnabled");

            }
        }
        public ICommand CommercialVehiclesLoadCommand { get; protected set; }
        protected void OnCommercialVehiclesLoadCommand(object arg)
        {
            ProcessTitle = Resources.CommercialVehicles;
            HasError = true;
            if (!CommercialVehiclesFileEnabled)
            {
                ShowInformationDialog(GetFileNameError(PassengerCarsFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(CommercialVehiclesFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            doSubTreeLoadFileName = CommercialVehiclesFile;
            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoSubTreeLoad(this.DoSubTreeLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion

        #region motors
        protected string motorsfile = "MotorsTree.csv";
        public bool MotorsFileEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(motorsfile));
            }
        }
        public string MotorsFile
        {
            get
            {
                return motorsfile;
            }
            set
            {
                motorsfile = value;
                RaisePropertyChanged("MotorsFile");
                RaisePropertyChanged("MotorsFileEnabled");

            }
        }
        public ICommand MotorsLoadCommand { get; protected set; }
        protected void OnMotorsLoadCommand(object arg)
        {
            ProcessTitle = Resources.Motors;
            HasError = true;
            if (!MotorsFileEnabled)
            {
                ShowInformationDialog(GetFileNameError(PassengerCarsFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(MotorsFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            doSubTreeLoadFileName = MotorsFile;
            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoSubTreeLoad(this.DoSubTreeLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion

        #region Axles
        protected string axlesfile = "AxlesTree.csv";
        public bool AxlesFileEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(axlesfile));
            }
        }
        public string AxlesFile
        {
            get
            {
                return axlesfile;
            }
            set
            {
                axlesfile = value;
                RaisePropertyChanged("AxlesFile");
                RaisePropertyChanged("AxlesFileEnabled");

            }
        }
        public ICommand AxlesLoadCommand { get; protected set; }
        protected void OnAxlesLoadCommand(object arg)
        {
            ProcessTitle = Resources.Axles;
            HasError = true;

            if (!AxlesFileEnabled)
            {
                ShowInformationDialog(GetFileNameError(PassengerCarsFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(AxlesFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            doSubTreeLoadFileName = AxlesFile;
            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoSubTreeLoad(this.DoSubTreeLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion

        #region Universals
        protected string universalsfile = "UniversalsTree.csv";
        public bool UniversalsFileEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetFileNameError(universalsfile));
            }
        }
        public string UniversalsFile
        {
            get
            {
                return universalsfile;
            }
            set
            {
                universalsfile = value;
                RaisePropertyChanged("UniversalsFile");
                RaisePropertyChanged("UniversalsFileEnabled");

            }
        }
        public ICommand UniversalsLoadCommand { get; protected set; }
        protected void OnUniversalsLoadCommand(object arg)
        {
            ProcessTitle = Resources.Universals;
            HasError = true;

            if (!UniversalsFileEnabled)
            {
                ShowInformationDialog(GetFileNameError(PassengerCarsFile), Resources.Error_TITLE);
                return;
            }
            if (!File.Exists(UniversalsFile))
            {
                ShowInformationDialog(Resources.Error_FILENOTEXISTS, Resources.Error_TITLE);
                return;
            }
            doSubTreeLoadFileName = UniversalsFile;
            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoSubTreeLoad(this.DoSubTreeLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion

        #region EnterpriseArticleTDES
        public bool TecDocEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetRangeArticleLookUp(TecDocFrom, TecDocTil));
            }
        }
        int tecdocfrom = 0;
        public int TecDocFrom
        {
            get
            {
                return tecdocfrom;
            }
            set
            {
                tecdocfrom = value;
                RaisePropertyChanged("TecDocFrom");
                RaisePropertyChanged("TecDocTil");
                RaisePropertyChanged("TecDocEnabled");
            }
        }
        int tecdoctil = 0;
        public int TecDocTil
        {
            get
            {
                return tecdoctil;
            }
            set
            {
                tecdoctil = value;
                RaisePropertyChanged("TecDocTil");
                RaisePropertyChanged("TecDocFrom");
                RaisePropertyChanged("TecDocEnabled");
            }
        }
        public delegate void AsyncDoTecDocLoad();
        protected void DoTecDocLoad()
        {
            int stepCount = 50;
            int readTil = 0;

            EnterpriseArticleTDES art = new EnterpriseArticleTDES();
            EnterpriseArticleDescriptionTDES descr = new EnterpriseArticleDescriptionTDES();
            articledataservice.SetConnectionStringName(ArticleContextName);

            int DysplayCount = 0;
            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            if (TecDocFrom <= 0) TecDocFrom = 1;
            if (TecDocTil <= 0) TecDocTil = 1;
            
            while (TecDocFrom <= TecDocTil)
            {
                readTil = TecDocFrom + stepCount;
                if (readTil > TecDocTil) readTil = TecDocTil;

                object aReader = tecdoccontext.GetAllArticle_READER(16, 249, TecDocFrom, readTil);
                MODELTYPETREEITEMS_TD tditm = tecdoccontext.GetAllArticle_NEXT(aReader);

                while (tditm != null)
                {
                    AllCount++;

                    art.EntGuid = this.EntGuid;
                    art.ExternArticle = tditm.ART_ARTICLE_NR;
                    art.ExternArticleEAN = tditm.EAN_TEXT;
                    art.ExternBrandNic = tditm.SUP_TEXT;
                    descr.EntArticleDescription = tditm.MASTER_BEZ;
                    art.EntArticle = art.ExternArticle;
                    art.EntBrandNic = art.ExternBrandNic;


                    articledataservice.EnterpriseArticleTDESAdd(art, descr);


                    SavedCount++;
                    if (DysplayCount > 100)
                    {
                        DysplayCount = 0;
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
                    }
                    else DysplayCount++;

                    tditm = tecdoccontext.GetAllArticle_NEXT(aReader);
                    if (undoCommandValue)
                    {
                        tecdoccontext.DoCloseReader(aReader);
                        break;
                    }
                }
                if (undoCommandValue)
                {
                    break;
                }
                TecDocFrom = readTil + 1;
            }
            HasError = false;
        }
        public ICommand TecDocLoadCommand { get; protected set; }
        protected void OnTecDocLoadCommand(object arg)
        {
            ProcessTitle = Resources.ArticuleTree + " : " + TecDocFrom.ToString() + " : " + TecDocTil.ToString();
            HasError = true;

            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoTecDocLoad(this.DoTecDocLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        public ICommand TecDocMaxCommand { get; protected set; }
        protected void OnTecDocMaxCommand(object arg)
        {
            TecDocTil = tecdoccontext.GetAllArticleMax();
        }
        #endregion

        #region EnterpriseCarModelFuelTDES
        public delegate void AsyncDoFuelLoad();
        protected void DoFuelLoad()
        {

            List<FUEL_TD> fuelsTDs = tecdoccontext.GetFUELS(16);

            EnterpriseCarModelFuelTDES fuel = new EnterpriseCarModelFuelTDES();
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);

            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;

            fuel.FUELId = 0;
            fuel.FuelName = "Не определено";
            mstecdocdataservice.EnterpriseCarModelFuelTDESInsertOrUpdate(fuel);

            foreach (var itm in fuelsTDs)
            {
                AllCount++;

                fuel.FUELId = itm.DES_ID;
                fuel.FuelName = itm.TEX_TEXT;


                mstecdocdataservice.EnterpriseCarModelFuelTDESInsertOrUpdate(fuel);


                SavedCount++;
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
            HasError = false;
        }
        public ICommand FuelLoadCommand { get; protected set; }
        protected void OnFuelLoadCommand(object arg)
        {
            ProcessTitle = Resources.Fuel_Title;
            HasError = true;
            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoFuelLoad(this.DoFuelLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion

        #region EnterpriseCarBrandTDES
        public delegate void AsyncDoCarBrandLoad();
        protected void DoCarBrandLoad()
        {

            List<BRAND_TD> cbTDs = tecdoccontext.GetBRANDS();

            EnterpriseCarBrandTDES cb = new EnterpriseCarBrandTDES();
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);

            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            foreach (var itm in cbTDs)
            {
                AllCount++;

                cb.EnterpriseCarBrandId = itm.MFA_ID;
                cb.EnterpriseCarBrandName = itm.MFA_BRAND;


                mstecdocdataservice.EnterpriseCarBrandTDESInsertOrUpdate(cb);


                SavedCount++;
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
            HasError = false;
        }
        public ICommand CarBrandLoadCommand { get; protected set; }
        protected void OnCarBrandLoadCommand(object arg)
        {
            ProcessTitle = Resources.CarBrand_Title;
            HasError = true;

            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoCarBrandLoad(this.DoCarBrandLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion

        #region EnterpriseCarModelTypeTDES
        public delegate void AsyncDoCarModelTypeLoad();
        protected void DoCarModelTypeLoad()
        {

            List<BRAND_TD> cbTDs = tecdoccontext.GetBRANDS();

            EnterpriseCarModelTypeTDES cmt = new EnterpriseCarModelTypeTDES();
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);

            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            foreach (var itm in cbTDs)
            {
                List<MODEL_TD> cmtTDs = tecdoccontext.GetMODELS_EX(16, 249, itm.MFA_ID, null);
                foreach (var subItm in cmtTDs)
                {
                    AllCount++;

                    cmt.EnterpriseCarBrandId = itm.MFA_ID;
                    cmt.EnterpriseCarModelTypeId = subItm.MOD_ID;
                    cmt.EnterpriseCarModelTypeName = subItm.TEX_TEXT;


                    mstecdocdataservice.EnterpriseCarModelTypeTDESInsertOrUpdate(cmt);


                    SavedCount++;
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
                if (undoCommandValue)
                {
                    break;
                }
            }
            HasError = false;
        }
        public ICommand CarModelTypeLoadCommand { get; protected set; }
        protected void OnCarModelTypeLoadCommand(object arg)
        {
            ProcessTitle = Resources.CarModelType_Title;
            HasError = true;
            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoCarModelTypeLoad(this.DoCarModelTypeLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion

        #region EnterpriseCarModelTDES
        public delegate void AsyncDoCarModelLoad();
        protected void DoCarModelLoad()
        {
            ProcessTitle = Resources.CarModel_Title;

            List<BRAND_TD> cbTDs = tecdoccontext.GetBRANDS();

            EnterpriseCarModelTDES cm = new EnterpriseCarModelTDES();
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);

            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            foreach (var itm in cbTDs)
            {
                List<MODEL_TD> cmtTDs = tecdoccontext.GetMODELS_EX(16, 249, itm.MFA_ID, null);
                foreach (var subItm in cmtTDs)
                {
                    List<MODELTYPE_TD> modelTDs = tecdoccontext.GetMODELTYPES_EX(16, 249, subItm.MOD_ID, null, null);
                    foreach (var subsubItm in modelTDs)
                    {

                        AllCount++;

                        cm.EnterpriseCarBrandId = itm.MFA_ID;
                        cm.EnterpriseCarModelTypeId = subItm.MOD_ID;
                        cm.EnterpriseCarModelId = subsubItm.TYP_ID;
                        cm.EnterpriseCarModelName = subsubItm.TEX_TEXT;
                        if (cm.EnterpriseCarModelName.Length > 80) cm.EnterpriseCarModelName = cm.EnterpriseCarModelName.Substring(0, 80);
                        cm.EnterpriseCarModelBody = subsubItm.TYP_KV_BODY;
                        if (cm.EnterpriseCarModelBody == null) cm.EnterpriseCarModelBody = "НЕ ОПРДЕЛЕНО";
                        if (cm.EnterpriseCarModelBody.Length > 80) cm.EnterpriseCarModelBody = cm.EnterpriseCarModelBody.Substring(0, 80);
                        cm.EnterpriseCarModelProductDateStart = subsubItm.TYP_PCON_START;
                        cm.EnterpriseCarModelProductDateTil = subsubItm.TYP_PCON_END;
                        cm.EnterpriseCarModelPowerKW = subsubItm.TYP_KW_FROM;
                        cm.EnterpriseCarModelPowerHP = subsubItm.TYP_HP_FROM;
                        cm.EnterpriseCarModelEngCap = subsubItm.TYP_CCM;
                        cm.EnterpriseCarModelVALVES = subsubItm.TYP_VALVES;
                        cm.EnterpriseCarModelCYLINDERS = subsubItm.TYP_CYLINDERS;
                        cm.EnterpriseCarModelABS = subsubItm.TYP_KV_ABS;
                        cm.EnterpriseCarModelASR = subsubItm.TYP_KV_ASR;
                        cm.EnterpriseCarModelBrakeType = subsubItm.TYP_KV_BRAKE_TYPE;
                        cm.EnterpriseCarModelBrakeSys = subsubItm.TYP_KV_BRAKE_SYST;
                        cm.FUELId = subsubItm.FUEL_ID;
                        cm.EnterpriseCarModelFUELSUPPLY = subsubItm.TYP_KV_FUEL_SUPPLY;
                        cm.EnterpriseCarModelCATALYST = subsubItm.TYP_KV_CATALYST;
                        cm.EnterpriseCarModelTRANS = subsubItm.TYP_KV_TRANS;
                        cm.EnterpriseCarModelENGCODE = subsubItm.TYP_KV_ENGINE;


                        mstecdocdataservice.EnterpriseCarModelTDESInsertOrUpdate(cm);


                        SavedCount++;
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
                    if (undoCommandValue)
                    {
                        break;
                    }
                }
                if (undoCommandValue)
                {
                    break;
                }
            }
            HasError = false;
        }
        public ICommand CarModelLoadCommand { get; protected set; }
        protected void OnCarModelLoadCommand(object arg)
        {
            undoCommandValue = false;
            HasError = true;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoCarModelLoad(this.DoCarModelLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion

        #region EnterpriseArticleCategoryItemTDES
        public bool ArticleCategoryEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetRangeArticleLookUp(ArticleCategoryFrom, ArticleCategoryTil));
            }
        }
        int articlecategoryfrom = 0;
        public int ArticleCategoryFrom
        {
            get
            {
                return articlecategoryfrom;
            }
            set
            {
                articlecategoryfrom = value;
                RaisePropertyChanged("ArticleCategoryFrom");
                RaisePropertyChanged("ArticleCategoryTil");
                RaisePropertyChanged("ArticleCategoryEnabled");
            }
        }
        int articlecategorytil = 0;
        public int ArticleCategoryTil
        {
            get
            {
                return articlecategorytil;
            }
            set
            {
                articlecategorytil = value;
                RaisePropertyChanged("ArticleCategoryTil");
                RaisePropertyChanged("ArticleCategoryFrom");
                RaisePropertyChanged("ArticleCategoryEnabled");
            }
        }
        public delegate void AsyncDoArticleCategoryItemLoad();
        protected void DoArticleCategoryItemLoad()
        {

            int stepCount = 50;
            int readTil = 0;

            EnterpriseArticleCategoryItemTDES artCateg = new EnterpriseArticleCategoryItemTDES();
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);

            int DysplayCount = 0;
            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            if (ArticleCategoryFrom <= 0) ArticleCategoryFrom = 1;
            if (ArticleCategoryTil <= 0) ArticleCategoryTil = 1;
            while (ArticleCategoryFrom <= ArticleCategoryTil)
            {
                readTil = ArticleCategoryFrom + stepCount;
                if (readTil > ArticleCategoryTil) readTil = ArticleCategoryTil;


                Object reader = tecdoccontext.GetArticleGroup(ArticleCategoryFrom, readTil);
                ArticleGroup_TD artCategTDs = tecdoccontext.GetArticleGroup_NEXT(reader);
                while (artCategTDs != null)
                {
                    AllCount++;

                    artCateg.ArticleId = artCategTDs.ART_ID;
                    artCateg.CategoryItemId = artCategTDs.GROUP_ID;


                    mstecdocdataservice.EnterpriseArticleCategoryItemTDESInsertOrUpdate(artCateg);


                    SavedCount++;
                    if (DysplayCount > 100)
                    {
                        DysplayCount = 0;
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
                    }
                    else DysplayCount++;

                    if (undoCommandValue)
                    {
                        tecdoccontext.DoCloseReader(reader);
                        break;
                    }
                    artCategTDs = tecdoccontext.GetArticleGroup_NEXT(reader);
                }
                if (undoCommandValue)
                {
                    break;
                }
                ArticleCategoryFrom = readTil + 1;

            }
            HasError = false;
        }
        public ICommand ArticleCategoryItemLoadCommand { get; protected set; }
        protected void OnArticleCategoryItemLoadCommand(object arg)
        {
            ProcessTitle = Resources.Article2Category + " : " + ArticleCategoryFrom.ToString() + " : " + ArticleCategoryTil.ToString();
            HasError = true;

            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoArticleCategoryItemLoad(this.DoArticleCategoryItemLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        public ICommand ArticleCategoryMaxCommand { get; protected set; }
        protected void OnArticleCategoryMaxCommand(object arg)
        {
            ArticleCategoryTil = tecdoccontext.GetArticleGroupMax();
        }
        #endregion

        #region EnterpriseArticleBrandTDES
        public bool ArticleBrandEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetRangeArticleLookUp(ArticleBrandFrom, ArticleBrandTil));
            }
        }
        int articlebrandfrom = 0;
        public int ArticleBrandFrom
        {
            get
            {
                return articlebrandfrom;
            }
            set
            {
                articlebrandfrom = value;
                RaisePropertyChanged("ArticleBrandFrom");
                RaisePropertyChanged("ArticleBrandTil");
                RaisePropertyChanged("ArticleBrandEnabled");
            }
        }
        int articlebrandtil = 0;
        public int ArticleBrandTil
        {
            get
            {
                return articlebrandtil;
            }
            set
            {
                articlebrandtil = value;
                RaisePropertyChanged("ArticleBrandTil");
                RaisePropertyChanged("ArticleBrandFrom");
                RaisePropertyChanged("ArticleBrandEnabled");
            }
        }
        public delegate void AsyncDoArticleBrandLoad();
        protected void DoArticleBrandLoad()
        {
            Object reader = tecdoccontext.GetArticleBrand();

            EnterpriseArticleBrandTDES ab = new EnterpriseArticleBrandTDES();
            EnterpriseBrandTDES eb = new EnterpriseBrandTDES();
            eb.EntGuid = this.EntGuid;
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);
            articledataservice.SetConnectionStringName(ArticleContextName);

            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            BRAND_TD brandTDs = tecdoccontext.GetArticleBrand_NEXT(reader);
            while (brandTDs != null)
            {
                AllCount++;

                ab.ArticleBrandId = brandTDs.MFA_ID;
                ab.ArticleBrandNic = brandTDs.MFA_BRAND;

                eb.EntBrandNic = brandTDs.MFA_BRAND;
                eb.EntBrandDescription = brandTDs.MFA_BRAND;
                
                


                mstecdocdataservice.EnterpriseArticleBrandTDESInsertOrUpdate(ab);
                articledataservice.EnterpriseBrandTDESInsertOrUpdate(eb);


                SavedCount++;
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
                brandTDs = tecdoccontext.GetArticleBrand_NEXT(reader);
            }
            HasError = false;
        }
        public ICommand ArticleBrandLoadCommand { get; protected set; }
        protected void OnArticleBrandLoadCommand(object arg)
        {
            ProcessTitle = Resources.ArticleBrand;
            HasError = true;
            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoArticleBrandLoad(this.DoArticleBrandLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion

        #region EnterpriseArticleLookUpTDES
        public bool ArticleLookUpEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetRangeArticleLookUp(ArticleLookUpFrom, ArticleLookUpTil));
            }
        }
        int articlelookupfrom = 0;
        public int ArticleLookUpFrom { 
            get {
                return articlelookupfrom;
            }
            set {
                articlelookupfrom = value;
                RaisePropertyChanged("ArticleLookUpFrom");
                RaisePropertyChanged("ArticleLookUpTil");
                RaisePropertyChanged("ArticleLookUpEnabled");
            } 
        }
        int articlelookuptil = 0;
        public int ArticleLookUpTil
        {
            get
            {
                return articlelookuptil;
            }
            set
            {
                articlelookuptil = value;
                RaisePropertyChanged("ArticleLookUpTil");
                RaisePropertyChanged("ArticleLookUpFrom");
                RaisePropertyChanged("ArticleLookUpEnabled");
            }
        }
        public delegate void AsyncDoArticleLookUpLoad();
        protected void DoArticleLookUpLoad()
        {
            int stepCount = 100;
            int readTil = 0;

            EnterpriseArticleLookUpTDES al = new EnterpriseArticleLookUpTDES();
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);

            int DysplayCounter = 0;
            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            if (ArticleLookUpFrom <= 0) ArticleLookUpFrom = 1;
            if (ArticleLookUpTil <= 0) ArticleLookUpTil = 1;
            while (ArticleLookUpFrom <= ArticleLookUpTil)
            {
                readTil = ArticleLookUpFrom + stepCount;
                if (readTil > ArticleLookUpTil) readTil = ArticleLookUpTil;

                Object reader = tecdoccontext.GetArticleLookUp(ArticleLookUpFrom, readTil);
                ARTICLE_LOOKUP_TD alTDs = tecdoccontext.GetArticleLookUp_NEXT(reader);
                while (alTDs != null)
                {
                    AllCount++;

                    al.ArticleSearch = alTDs.ARL_SEARCH_NUMBER;
                    al.ArticleId = alTDs.ARL_ART_ID;
                    al.ArticleBrandId = alTDs.ARL_BRA_ID;
                    al.ArticleSearchKind = alTDs.ARL_KIND;
                    al.ArticleDysplay = alTDs.ARL_DISPLAY_NR;

                    mstecdocdataservice.EnterpriseArticleLookUpTDESInsertOrUpdate(al);


                    SavedCount++;
                    if (DysplayCounter > 100)
                    {
                        DysplayCounter = 0;
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
                    }
                    else DysplayCounter++;

                    if (undoCommandValue)
                    {
                        tecdoccontext.DoCloseReader(reader);
                        break;
                    }
                    alTDs = tecdoccontext.GetArticleLookUp_NEXT(reader);
                }
                if (undoCommandValue)
                {
                    break;
                }
                ArticleLookUpFrom = readTil + 1;

            }
            HasError = false;
        }
        public ICommand ArticleLookUpLoadCommand { get; protected set; }
        protected void OnArticleLookUpLoadCommand(object arg)
        {
            HasError = true;
            ProcessTitle = Resources.ArticleLookUp + " : " + ArticleLookUpFrom.ToString() + " : " + ArticleLookUpTil.ToString();

            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoArticleLookUpLoad(this.DoArticleLookUpLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        public ICommand ArticleLookUpMaxCommand { get; protected set; }
        protected void OnArticleLookUpMaxCommand(object arg)
        {
            ArticleLookUpTil = tecdoccontext.GetArticleLookUpMax();
        }
        #endregion

        #region EnterpriseArticleApplicTDES
        public bool ArticleApplicEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetRangeArticleLookUp(ArticleApplicFrom, ArticleApplicTil));
            }
        }
        int articleApplicfrom = 0;
        public int ArticleApplicFrom
        {
            get
            {
                return articleApplicfrom;
            }
            set
            {
                articleApplicfrom = value;
                RaisePropertyChanged("ArticleApplicFrom");
                RaisePropertyChanged("ArticleApplicTil");
                RaisePropertyChanged("ArticleApplicEnabled");
            }
        }
        int articleApplictil = 0;
        public int ArticleApplicTil
        {
            get
            {
                return articleApplictil;
            }
            set
            {
                articleApplictil = value;
                RaisePropertyChanged("ArticleApplicTil");
                RaisePropertyChanged("ArticleApplicFrom");
                RaisePropertyChanged("ArticleApplicEnabled");
            }
        }
        public delegate void AsyncDoArticleApplicLoad();
        protected void DoArticleApplicLoad()
        {
            int DysplayCounter = 0;
            int AllCount = 0;
            SavedCount = 0;
            EnterpriseArticleApplicTDES ap = new EnterpriseArticleApplicTDES();
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);
            var _dispatcher = Dispatcher.CurrentDispatcher;

            if (ArticleApplicFrom <= 0) ArticleApplicFrom = 1;
            if (ArticleApplicTil <= 0) ArticleApplicTil = tecdoccontext.GetArticleApplicMax();

            while (ArticleApplicFrom <= ArticleApplicTil)
            {
                Object reader = tecdoccontext.GetAllArticleApplic(16, 249, articleApplicfrom, articleApplicfrom);


                ArticleGroup_TD alTDs = tecdoccontext.GetAllArticleApplic_NEXT(reader);
                while (alTDs != null)
                {
                    AllCount++;

                    ap.ArticleId = alTDs.ART_ID;
                    ap.EnterpriseCarModelId = alTDs.GROUP_ID;


                    mstecdocdataservice.EnterpriseArticleApplicTDESInsertOrUpdate(ap);

                    SavedCount++;
                    if (DysplayCounter > 100)
                    {
                        DysplayCounter = 0;
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
                    }
                    else DysplayCounter++;

                    if (undoCommandValue)
                    {
                        tecdoccontext.DoCloseReader(reader);
                        break;
                    }
                    alTDs = tecdoccontext.GetAllArticleApplic_NEXT(reader);
                }
                if (undoCommandValue)
                {
                    break;
                }
                ArticleApplicFrom++;
            }
            HasError = false;
        }
        public ICommand ArticleApplicLoadCommand { get; protected set; }
        protected void OnArticleApplicLoadCommand(object arg)
        {
            HasError = true;
            ProcessTitle = Resources.ArticleApplic + " : " + ArticleApplicFrom.ToString() + " : " + ArticleApplicTil.ToString();

            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoArticleApplicLoad(this.DoArticleApplicLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        public ICommand ArticleApplicMaxCommand { get; protected set; }
        protected void OnArticleApplicMaxCommand(object arg)
        {
            ArticleApplicTil = tecdoccontext.GetArticleApplicMax();
        }
        #endregion

        #region EnterpriseArticleTecDocTDES
        public bool ArticleTecDocEnabled
        {
            get
            {
                return String.IsNullOrEmpty(GetRangeArticleLookUp(ArticleTecDocFrom, ArticleTecDocTil));
            }
        }
        int articletecdocfrom = 0;
        public int ArticleTecDocFrom
        {
            get
            {
                return articletecdocfrom;
            }
            set
            {
                articletecdocfrom = value;
                RaisePropertyChanged("ArticleTecDocFrom");
                RaisePropertyChanged("ArticleTecDocTil");
                RaisePropertyChanged("ArticleTecDocEnabled");
            }
        }
        int articletecdoctil = 0;
        public int ArticleTecDocTil
        {
            get
            {
                return articletecdoctil;
            }
            set
            {
                articletecdoctil = value;
                RaisePropertyChanged("ArticleTecDocTil");
                RaisePropertyChanged("ArticleTecDocFrom");
                RaisePropertyChanged("ArticleTecDocEnabled");
            }
        }
        public delegate void AsyncDoArticleTecDocLoad();
        protected void DoArticleTecDocLoad()
        {
            int stepCount = 50;
            int readTil = 0;
   
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);
            int DysplayCount = 0;
            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            EnterpriseArticleTecDocTDES art = new EnterpriseArticleTecDocTDES();
            EnterpriseArticleTecDocDescriptionTDES descr = new EnterpriseArticleTecDocDescriptionTDES();

            if (ArticleTecDocFrom <= 0) ArticleTecDocFrom = 1;
            if (ArticleTecDocTil <= 0) ArticleTecDocTil = 1;


            while (ArticleTecDocFrom <= ArticleTecDocTil)
            {

                readTil = ArticleTecDocFrom + stepCount;
                if (readTil > ArticleTecDocTil) readTil = ArticleTecDocTil;

                object aReader = tecdoccontext.GetAllArticle_READER(16, 249, ArticleTecDocFrom, readTil);
                MODELTYPETREEITEMS_TD tditm = tecdoccontext.GetAllArticle_NEXT(aReader);

                while (tditm != null)
                {
                    AllCount++;
                    art.ArticleId = tditm.ART_ID;
                    art.ExternArticle = tditm.ART_ARTICLE_NR;
                    art.ExternArticleEAN = tditm.EAN_TEXT;

                    art.ArticleBrandId = tditm.SUP_ID;
                    art.ExternBrandNic = tditm.SUP_TEXT;
                    descr.EntArticleDescription = tditm.MASTER_BEZ;


                    mstecdocdataservice.EnterpriseArticleTecDocTDESAdd(art, descr);


                    SavedCount++;
                    if (DysplayCount > 100)
                    {
                        DysplayCount = 0;
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
                    }
                    else DysplayCount++;

                    tditm = tecdoccontext.GetAllArticle_NEXT(aReader);
                    if (undoCommandValue)
                    {
                        tecdoccontext.DoCloseReader(aReader);
                        break;
                    }
                }
                if (undoCommandValue)
                {
                    break;
                }

                ArticleTecDocFrom = readTil + 1;


            }
            HasError = false;
        }
        public ICommand ArticleTecDocLoadCommand { get; protected set; }
        protected void OnArticleTecDocLoadCommand(object arg)
        {
            ProcessTitle = Resources.ArticleTecDoc + " : " + ArticleTecDocFrom.ToString() + " : " + ArticleTecDocTil.ToString();
            HasError = true;

            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoArticleTecDocLoad(this.DoArticleTecDocLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        public ICommand ArticleTecDocMaxCommand { get; protected set; }
        protected void OnArticleTecDocMaxCommand(object arg)
        {
            ArticleTecDocTil = tecdoccontext.GetAllArticleMax();
        }
        #endregion


        #region EnterpriseCategoryTecDocTDES
        public delegate void AsyncDoCategoryTecDocLoad();
        protected void DoCategoryTecDocLoad()
        {

            object aReader = tecdoccontext.GetAllMODELTYPESTREE(16);

            EnterpriseCategoryTecDocTDES categ = new EnterpriseCategoryTecDocTDES();
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);
            
            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            MODELTYPESTREE_PARENT_TD categTDs = tecdoccontext.GetAllMODELTYPESTREE_NEXT(aReader);
            while (categTDs != null)
            {
                AllCount++;

                categ.CategoryId = categTDs.STR_ID; ;
                categ.CategoryDescription = categTDs.TEX_TEXT;
                categ.CategoryParent = categTDs.PARENT_ID;


                mstecdocdataservice.EnterpriseCategoryTecDocTDESInsertOrUpdate(categ);


                SavedCount++;
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
                categTDs = tecdoccontext.GetAllMODELTYPESTREE_NEXT(aReader);
            }
            HasError = false;
        }
        public ICommand CategoryTecDocLoadCommand { get; protected set; }
        protected void OnCategoryTecDocLoadCommand(object arg)
        {
            ProcessTitle = Resources.CategotyTecDoc;
            HasError = true;

            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoCategoryTecDocLoad(this.DoCategoryTecDocLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion


        #region EnterpriseCategoryItemTecDocTDES
        public delegate void AsyncDoCategoryItemTecDocLoad();
        protected void DoCategoryItemTecDocLoad()
        {

            object aReader = tecdoccontext.GetALLMODELTYPETREEITEMS(16);

            EnterpriseCategoryItemTecDocDescriptionTDES descr = new EnterpriseCategoryItemTecDocDescriptionTDES();
            EnterpriseCategoryItemTecDocTDES categItem = new EnterpriseCategoryItemTecDocTDES();
            mstecdocdataservice.SetConnectionStringName(MsTecDocContextName);

            int AllCount = 0;
            SavedCount = 0;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            MODELTYPESTREE_PARENT_TD categTDs = tecdoccontext.GetALLMODELTYPETREEITEMS_NEXT(aReader);
            while (categTDs != null)
            {
                AllCount++;

                categItem.CategoryItemId = categTDs.STR_ID; ;
                categItem.CategoryId = categTDs.PARENT_ID;
                descr.EntCategoryItemDescription = categTDs.TEX_TEXT;

                mstecdocdataservice.EnterpriseCategoryItemTecDocTDESAdd(categItem, descr);
                

                SavedCount++;
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
                categTDs = tecdoccontext.GetALLMODELTYPETREEITEMS_NEXT(aReader);
            }
            HasError = false;
        }
        public ICommand CategoryItemTecDocLoadCommand { get; protected set; }
        protected void OnCategoryItemTecDocLoadCommand(object arg)
        {
            ProcessTitle = Resources.CategotyItemTecDoc;
            HasError = true;

            undoCommandValue = false;
            StartState = false;
            var _dispatcher = Dispatcher.CurrentDispatcher;
            var caller = new AsyncDoCategoryItemTecDocLoad(this.DoCategoryItemTecDocLoad);
            caller.BeginInvoke(e =>
            {
                if (_dispatcher.CheckAccess())
                {
                    StartState = true;
                    if (undoCommandValue)
                        ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                    else
                    {
                        if (HasError)
                        {
                            ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                        else
                        {
                            ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                        }
                    }
                }
                else
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        StartState = true;
                        if (undoCommandValue)
                            ShowInformationDialog(Resources.Error_ProcessIsBroken + " : " + SavedCount.ToString(), Resources.Error_TITLE);
                        else
                        {
                            if (HasError)
                            {
                                ShowInformationDialog(Resources.Error_ProcessWithException + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                            else
                            {
                                ShowInformationDialog(Resources.Info_OperationIsDone + " : " + SavedCount.ToString(), Resources.Info_TITLE);
                            }
                        }
                    }
                    ));
                }
            }, null);
        }
        #endregion



        #region StartState & UndoCommand
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

        #region IRegionMemberLifetime
        public bool KeepAlive { get; set; }
        #endregion

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // all a time true, since one window for any EntGuid
            return true;
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            Guid x;
            if (Guid.TryParse((string)navigationContext.Parameters["SearchEntGuid"], out x))
                this.EntGuid = x;
            else
                this.EntGuid = Guid.Empty;
            this.EntDescription = (string)navigationContext.Parameters["EntDescription"];
            this.ArticleContextName = (string)navigationContext.Parameters["EntContext"];
            KeepAlive = false;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion

        #region EnterpriseTDESCommand
        public ICommand EnterpriseTDESCommand { get; set; }
        protected void OnEnterpriseTDESCommand(object arg)
        {
            KeepAlive = false;

            //UriQuery query = new UriQuery();
            NavigationParameters query = new NavigationParameters();//
            query.Add("SearchEntGuid", this.EntGuid.ToString());
            query.Add("EntDescription", this.EntDescription);
            regionManager.RequestNavigate("EnterpriseNavigationContentRegion", new Uri("EnterpriseTDESView" + query.ToString(), UriKind.Relative));
        }
        #endregion

    }
}

// PassengerCarsFile_TITLE