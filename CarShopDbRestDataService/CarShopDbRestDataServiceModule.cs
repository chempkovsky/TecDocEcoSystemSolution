using CarShopDataService.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CarShopDbRestDataService
{
    [Module(ModuleName = "DbRestDataServiceModule")]
    public class CarShopDbRestDataServiceModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICarShopRestDataService, CarShopRestDBDataService>(); // "DBmapping", "Webmapping"
        }
    }
}