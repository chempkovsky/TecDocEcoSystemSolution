using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CarShopDataService.Interfaces;

namespace CarShopDBArticleDataService
{
    [Module(ModuleName = "DBArticleDataServiceModule")]
    public class CarShopDBArticleDataServiceModule : IModule
    {

        // prism 4 implementation
        //private readonly IUnityContainer container;
        //public DBArticleDataServiceModule(IUnityContainer container)
        //{
        //    this.container = container;
        //}


        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //
            // http://msdn.microsoft.com/en-us/library/ff647854.aspx#lifetime_register
            //
            // Register a default (un-named) type mapping with a transient lifetime: 
            // this.container.Resolve<ICarShopDataService>(); return a new instance of MyRealObject
            
            // prism 4 implementation
            //containerRegistry.RegisterType<ICarShopArticleDataService, CarShopArticleDBDataService>(); // "DBmapping", "Webmapping"


            // singleton lifetime
            // this.container.RegisterType<ICarShopDataService, CarShopDBDataService>(new ContainerControlledLifetimeManager());
            // new instance every tyme
            // this.container.RegisterType<ICarShopDataService, CarShopDBDataService>(new ExternallyControlledLifetimeManager());
            // Specify a default type mapping with an per thread lifetime
            // this.container.RegisterType<ICarShopDataService, CarShopDBDataService>(new PerThreadLifetimeManager());
            // MessageBox.Show("DBDataServiceModule done");


            containerRegistry.Register<ICarShopArticleDataService, CarShopArticleDBDataService>();
            
        }
    }
}