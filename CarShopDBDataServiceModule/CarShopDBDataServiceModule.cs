using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CarShopDataService.Interfaces;
using System;

namespace CarShopDBDataServiceModule
{
    [Module(ModuleName = "DBDataServiceModule")]
    public class CarShopDBDataServiceModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICarShopDataService, CarShopDBDataService>(); // "DBmapping", "Webmapping"   
        }
    }
}