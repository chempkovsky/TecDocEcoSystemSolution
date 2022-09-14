using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CarShopDataService.Interfaces;

namespace CarShopDbMsTecDocDataService
{
    [Module(ModuleName = "DbMsTecDocDataServiceModule")]
    public class CarShopDbMsTecDocDataServiceModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICarShopMsTecDocDataService, CarShopMsTecDocDBDataService>(); // "DBmapping", "Webmapping"   
        }
    }
}