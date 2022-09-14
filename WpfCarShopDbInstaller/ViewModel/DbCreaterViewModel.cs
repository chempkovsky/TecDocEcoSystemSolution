using CarShopDataService;
using CarShopDataService.Interfaces;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WpfCarShopDbInstaller.Properties;

namespace WpfCarShopDbInstaller.ViewModel
{
    public class DbCreaterViewModel : INotifyPropertyChanged //, IDataErrorInfo
    {
        protected ICarShopArticleDataService articledataservice = null;
        protected ICarShopRestDataService restdataservice = null;
        protected ICarShopIncomeDataService incomedataservice = null;
        protected ICarShopSalesDataService salesdataservice = null;
        protected ICarShopDataService dataservice = null;
        protected ICarShopMsTecDocDataService mstecdocdataservice = null;
        protected ICarShopOrderDataService orderdataservice = null;

        protected string carShopArticleContextName = "CarShopArticleContext";

        public DbCreaterViewModel(ICarShopArticleDataService articledataservice, 
            ICarShopRestDataService restdataservice,
            ICarShopIncomeDataService incomedataservice,
            ICarShopSalesDataService salesdataservice,
            ICarShopDataService dataservice,
            ICarShopMsTecDocDataService mstecdocdataservice,
            ICarShopOrderDataService orderdataservice)
            : base()
        {
            this.articledataservice = articledataservice;
            this.restdataservice = restdataservice;
            this.incomedataservice = incomedataservice;
            this.salesdataservice = salesdataservice;
            this.dataservice = dataservice;
            this.mstecdocdataservice = mstecdocdataservice;
            this.orderdataservice = orderdataservice;


            CreateCarShopArticleCommand = new DelegateCommand<object>(this.OnCreateCarShopArticle, this.CanCreateCarShopArticle);
            CreateCarShopRestCommand = new DelegateCommand<object>(this.OnCreateCarShopRest, this.CanCreateCarShopRest);
            CreateCarShopIncomeCommand = new DelegateCommand<object>(this.OnCreateCarShopIncome, this.CanCreateCarShopIncome);
            CreateCarShopSalesCommand = new DelegateCommand<object>(this.OnCreateCarShopSales, this.CanCreateCarShopSales);
            CreateCarShopDataCommand = new DelegateCommand<object>(this.OnCreateCarShopData, this.CanCreateCarShopData);
            CreateCarShopMsTecDocCommand = new DelegateCommand<object>(this.OnCreateCarShopMsTecDoc, this.CanCreateCarShopMsTecDoc);
            CreateCarShopOrderCommand = new DelegateCommand<object>(this.OnCreateCarShopOrder, this.CanCreateCarShopOrder);

            InformationDialog = new InteractionRequest<Notification>();


        }

        #region CreateCarShopArticleContext
        public string CarShopArticleContextName
        {
            get
            {
                return carShopArticleContextName;
            }
            set
            {
                carShopArticleContextName = value;
                RaisePropertyChanged("CarShopArticleContextName");
            }
        }


        private bool canCreateCarShopArticle = true;
        public ICommand CreateCarShopArticleCommand { get; protected set; }
        protected void OnCreateCarShopArticle(object arg)
        {
            this.canCreateCarShopArticle = false;
            try
            {
                articledataservice.SetConnectionStringName(CarShopArticleContextName);
                articledataservice.DoCreateDb();
                ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
            }
            finally
            {
                this.canCreateCarShopArticle = true;
            }
        }
        protected bool CanCreateCarShopArticle(object arg)
        {
            return this.canCreateCarShopArticle;
        }
        #endregion

        #region CarShopRestContext
        protected string carShopRestContextName = "CarShopRestContext";
        private bool canCreateCarShopRest = true;
        public string CarShopRestContextName
        {
            get
            {
                return carShopRestContextName;
            }
            set
            {
                carShopRestContextName = value;
                RaisePropertyChanged("CarShopRestContextName");
            }
        }
        public ICommand CreateCarShopRestCommand { get; protected set; }
        protected bool CanCreateCarShopRest(object arg)
        {
            return this.canCreateCarShopRest;
        }
        protected void OnCreateCarShopRest(object arg)
        {
            this.canCreateCarShopRest = false;
            try
            {
                restdataservice.SetConnectionStringName(CarShopRestContextName);
                restdataservice.DoCreateDb();
                ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
            }
            finally
            {
                this.canCreateCarShopRest = true;
            }
        }

        #endregion

        #region CarShopIncomeContext
        protected string carShopIncomeContextName = "CarShopIncomeContext";
        private bool canCreateCarShopIncome = true;
        public string CarShopIncomeContextName
        {
            get
            {
                return carShopIncomeContextName;
            }
            set
            {
                carShopIncomeContextName = value;
                RaisePropertyChanged("CarShopIncomeContextName");
            }
        }
        public ICommand CreateCarShopIncomeCommand { get; protected set; }
        protected bool CanCreateCarShopIncome(object arg)
        {
            return this.canCreateCarShopIncome;
        }
        protected void OnCreateCarShopIncome(object arg)
        {
            this.canCreateCarShopIncome = false;
            try
            {
                incomedataservice.SetConnectionStringName(CarShopIncomeContextName);
                incomedataservice.DoCreateDb();
                ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
            }
            finally
            {
                this.canCreateCarShopIncome = true;
            }
        }

        #endregion

        #region CarShopSalesContext
        protected string carShopSalesContextName = "CarShopSalesContext";
        private bool canCreateCarShopSales = true;
        public string CarShopSalesContextName
        {
            get
            {
                return carShopSalesContextName;
            }
            set
            {
                carShopSalesContextName = value;
                RaisePropertyChanged("CarShopSalesContextName");
            }
        }
        public ICommand CreateCarShopSalesCommand { get; protected set; }
        protected bool CanCreateCarShopSales(object arg)
        {
            return this.canCreateCarShopSales;
        }
        protected void OnCreateCarShopSales(object arg)
        {
            this.canCreateCarShopSales = false;
            try
            {
                salesdataservice.SetConnectionStringName(CarShopSalesContextName);
                salesdataservice.DoCreateDb();
                ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
            }
            finally
            {
                this.canCreateCarShopSales = true;
            }
        }

        #endregion

        #region CarShopDataContext
        protected string carShopDataContextName = "CarShopContext";
        private bool canCreateCarShopData = true;
        public string CarShopDataContextName
        {
            get
            {
                return carShopDataContextName;
            }
            set
            {
                carShopDataContextName = value;
                RaisePropertyChanged("CarShopDataContextName");
            }
        }
        public ICommand CreateCarShopDataCommand { get; protected set; }
        protected bool CanCreateCarShopData(object arg)
        {
            return this.canCreateCarShopData;
        }
        protected void OnCreateCarShopData(object arg)
        {
            this.canCreateCarShopData = false;
            try
            {
                dataservice.SetConnectionStringName(CarShopDataContextName);
                dataservice.DoCreateDb();
                ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
            }
            finally
            {
                this.canCreateCarShopSales = true;
            }
        }

        #endregion

        #region CarShopMsTecDocContext
        protected string carShopMsTecDocContextName = "CarShopMsTecDocContext";
        private bool canCreateCarShopMsTecDoc = true;
        public string CarShopMsTecDocContextName
        {
            get
            {
                return carShopMsTecDocContextName;
            }
            set
            {
                carShopMsTecDocContextName = value;
                RaisePropertyChanged("CarShopMsTecDocContextName");
            }
        }
        public ICommand CreateCarShopMsTecDocCommand { get; protected set; }
        protected bool CanCreateCarShopMsTecDoc(object arg)
        {
            return this.canCreateCarShopMsTecDoc;
        }
        protected void OnCreateCarShopMsTecDoc(object arg)
        {
            this.canCreateCarShopMsTecDoc = false;
            try
            {
                mstecdocdataservice.SetConnectionStringName(CarShopMsTecDocContextName);
                mstecdocdataservice.DoCreateDb();
                ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
            }
            finally
            {
                this.canCreateCarShopMsTecDoc = true;
            }
        }
        #endregion


        #region CarShopOrderContext
        protected string carShopOrderContextName = "CarShopOrdersContext";
        private bool canCreateCarShopOrder = true;
        public string CarShopOrderContextName
        {
            get
            {
                return carShopOrderContextName;
            }
            set
            {
                carShopOrderContextName = value;
                RaisePropertyChanged("CarShopOrderContextName");
            }
        }
        public ICommand CreateCarShopOrderCommand { get; protected set; }
        protected bool CanCreateCarShopOrder(object arg)
        {
            return this.canCreateCarShopOrder;
        }
        protected void OnCreateCarShopOrder(object arg)
        {
            this.canCreateCarShopOrder = false;
            try
            {
                orderdataservice.SetConnectionStringName(CarShopOrderContextName);
                orderdataservice.DoCreateDb();
                ShowInformationDialog(Resources.Info_OperationIsDone, Resources.Info_TITLE);
            }
            finally
            {
                this.canCreateCarShopOrder = true;
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


    }
}
