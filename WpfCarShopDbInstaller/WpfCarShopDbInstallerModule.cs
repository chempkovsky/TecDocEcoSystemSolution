using WpfCarShopDbInstaller.Properties;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;
using System.Windows.Controls;
using Unity;
using WpfCarShopDbInstaller.View;
using WpfCarShopDbInstaller.ViewModel;
using System.Linq;

namespace WpfCarShopDbInstaller
{
    public class WpfCarShopDbInstallerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

            var aRegionManager = containerProvider.Resolve<IRegionManager>();
            var aContainer = containerProvider.Resolve<IUnityContainer>();


            // ==2==
            MenuItem subItem = new MenuItem();
            subItem.Header = Resources.DbCreateTitle; // "Создание баз данных";


            // ==3==
            var subsubItem = new MenuItem()
            {
                Header = Resources.CreateDbCommand
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                DbCreaterViewControl atv = aContainer.Resolve<DbCreaterViewControl>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);



            aRegionManager.RegisterViewWithRegion("CarShopMainMenuRegion", () => subItem);

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // == 1 ==
            containerRegistry.Register<DbCreaterViewModel>();
            containerRegistry.Register<DbCreaterViewControl>();
            
        }

        IRegionManager regionManager = null;
        IUnityContainer container = null;

        public WpfCarShopDbInstallerModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        public void Initialize()
        {

            // == 1 ==
            this.container.RegisterType<DbCreaterViewModel>();
            this.container.RegisterType<DbCreaterViewControl>();

            // ==2==
            MenuItem subItem = new MenuItem();
            subItem.Header = "Создание баз данных";


            // ==3==
            var subsubItem = new MenuItem()
            {
                Header = Resources.CreateDbCommand
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                DbCreaterViewControl atv = this.container.Resolve<DbCreaterViewControl>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);

            this.regionManager.RegisterViewWithRegion("CarShopMainMenuRegion", () => subItem);
        }

    }
}