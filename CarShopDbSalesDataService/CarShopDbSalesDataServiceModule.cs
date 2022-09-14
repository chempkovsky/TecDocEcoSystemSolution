using CarShopDataService.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CarShopDbSalesDataService
{
    
    [Module(ModuleName = "DbSalesDataServiceModule")]
    public class CarShopDbSalesDataServiceModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICarShopSalesDataService, CarShopSalesDBDataService>(); // "DBmapping", "Webmapping"
        }
    }
}