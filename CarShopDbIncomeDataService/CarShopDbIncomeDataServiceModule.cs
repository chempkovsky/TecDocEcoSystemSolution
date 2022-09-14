using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CarShopDataService.Interfaces;

namespace CarShopDbIncomeDataService
{
    
    [Module(ModuleName = "DbIncomeDataServiceModule")]
    public class CarShopDbIncomeDataServiceModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICarShopIncomeDataService, CarShopIncomeDBDataService>(); // "DBmapping", "Webmapping"   
        }
    }
}