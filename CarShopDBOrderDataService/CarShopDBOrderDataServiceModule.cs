using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CarShopDataService.Interfaces;

namespace CarShopDBOrderDataService
{
    [Module(ModuleName = "DbOrderDataServiceModule")]
    public class CarShopDBOrderDataServiceModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICarShopOrderDataService, CarShopOrderDBDataService>(); // "DBmapping", "Webmapping"
        }
    }
}