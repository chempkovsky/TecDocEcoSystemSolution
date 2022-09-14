using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TecDocEcoSystemDbClassLibrary;
using Unity;
using WpfCarShopInstaller.Properties;
using WpfCarShopInstaller.View;
using WpfCarShopInstaller.ViewModel;

namespace WpfCarShopInstaller
{
    [Module(ModuleName = "WpfCarShopInstallerModule")]
    public class WpfCarShopInstallerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var aRegionManager = containerProvider.Resolve<IRegionManager>();
            var aContainer = containerProvider.Resolve<IUnityContainer>();

            MenuItem subItem = new MenuItem();
            subItem.Header = "Развертывание";

            ////////////////////AddressType////////////////////
            var subsubItem = new MenuItem()
            {
                Header = Resources.SimpleTypesLoadUnload_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                SimpleTypesLoadUnloadView atv = aContainer.Resolve<SimpleTypesLoadUnloadView>();
                // atv.ViewModel = this.container.Resolve<AddressTypeViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);

            ////////////////////AddressType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.AddressType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) => {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                AddressTypeView atv = aContainer.Resolve<AddressTypeView>();
                // atv.ViewModel = this.container.Resolve<AddressTypeViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////StreetType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.StreetType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                StreetTypeView atv = aContainer.Resolve<StreetTypeView>();
                //atv.ViewModel = this.container.Resolve<StreetTypeViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////SettlementType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.SettlementType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                SettlementTypeView atv = aContainer.Resolve<SettlementTypeView>();
                //atv.ViewModel = this.container.Resolve<SettlementTypeViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////ContactType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.ContactType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                ContactTypeView atv = aContainer.Resolve<ContactTypeView>();
                //atv.ViewModel = this.container.Resolve<ContactTypeViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////BranchType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.BranchType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                BranchTypeView atv = aContainer.Resolve<BranchTypeView>();
                //atv.ViewModel = this.container.Resolve<BranchTypeViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////Country////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.Country_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                CountryView atv = aContainer.Resolve<CountryView>();
                //atv.ViewModel = this.container.Resolve<CountryViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////Currency////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.Currency_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                CurrencyView atv = aContainer.Resolve<CurrencyView>();
                //atv.ViewModel = this.container.Resolve<CurrencyViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////Currency////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.Soato_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                SoatoView atv = aContainer.Resolve<SoatoView>();
                //atv.ViewModel = this.container.Resolve<CurrencyViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////EnterpriseNavigationView////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.EnterpriseNavigation_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = aRegionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                EnterpriseNavigationView atv = aContainer.Resolve<EnterpriseNavigationView>();
                //atv.ViewModel = this.container.Resolve<CurrencyViewModel>();
                aRegionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
                EnterpriseTDESView intV = aContainer.Resolve<EnterpriseTDESView>();
                aRegionManager.RegisterViewWithRegion("EnterpriseNavigationContentRegion", () => intV);
            };
            subItem.Items.Add(subsubItem);



            aRegionManager.RegisterViewWithRegion("CarShopMainMenuRegion", () => subItem);

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            this.container.RegisterType<ITecDocContext, TecDocContext>(); // "DBmapping", "Webmapping"


            containerRegistry.Register<SimpleTypesLoadUnloadViewModel>();
            containerRegistry.Register<SimpleTypesLoadUnloadView>();

            containerRegistry.Register<AddressTypeViewModel>(); // "DBmapping", "Webmapping"
            containerRegistry.Register<AddressTypeView>(); // "DBmapping", "Webmapping"

            containerRegistry.Register<StreetTypeViewModel>();
            containerRegistry.Register<StreetTypeView>();

            containerRegistry.Register<SettlementTypeViewModel>();
            containerRegistry.Register<SettlementTypeView>();

            containerRegistry.Register<ContactTypeViewModel>();
            containerRegistry.Register<ContactTypeView>();

            containerRegistry.Register<BranchTypeViewModel>();
            containerRegistry.Register<BranchTypeView>();

            containerRegistry.Register<CountryViewModel>();
            containerRegistry.Register<CountryView>();

            containerRegistry.Register<CurrencyViewModel>();
            containerRegistry.Register<CurrencyView>();

            containerRegistry.Register<SoatoViewModel>();
            containerRegistry.Register<SoatoView>();

            containerRegistry.Register<EnterpriseNavigationView>();
            containerRegistry.Register<EnterpriseTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseTDESView>("EnterpriseTDESView");

            containerRegistry.Register<EnterpriseUserTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseUserTDESView>("EnterpriseUserTDESView");

            containerRegistry.Register<EnterpriseBranchTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseBranchTDESView>("EnterpriseBranchTDESView");

            containerRegistry.Register<EnterpriseAddressTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseAddressTDESView>("EnterpriseAddressTDESView");

            containerRegistry.Register<EnterpriseUserContactTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseUserContactTDESView>("EnterpriseUserContactTDESView");

            containerRegistry.Register<EnterpriseBranchContactsTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseBranchContactsTDESView>("EnterpriseBranchContactsTDESView");

            containerRegistry.Register<EnterpriseBranchUserTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseBranchUserTDESView>("EnterpriseBranchUserTDESView");

            containerRegistry.Register<EnterpriseBranchUserContactTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseBranchUserContactTDESView>("EnterpriseBranchUserContactTDESView");

            containerRegistry.Register<EnterpriseBranchWorkPlaceTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseBranchWorkPlaceTDESView>("EnterpriseBranchWorkPlaceTDESView");

            containerRegistry.Register<EnterpriseProductCategoryTDESViewModel>();
            containerRegistry.Register<Object, EnterpriseProductCategoryTDESView>("EnterpriseProductCategoryTDESView");

            containerRegistry.Register<LookUpSoatoViewModel>();
            containerRegistry.Register<Object, LookUpSoatoView>("LookUpSoatoView");

            containerRegistry.Register<LoadCategoryTreeViewModel>();
            containerRegistry.Register<Object, LoadCategoryTreeView>("LoadCategoryTreeView");


        }
        IRegionManager regionManager = null;
        IUnityContainer container = null;

        public WpfCarShopInstallerModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.container = container;
        }

        public void Initialize()
        {
            this.container.RegisterType<ITecDocContext, TecDocContext>(); // "DBmapping", "Webmapping"


            this.container.RegisterType<SimpleTypesLoadUnloadViewModel>();
            this.container.RegisterType<SimpleTypesLoadUnloadView>();

            this.container.RegisterType<AddressTypeViewModel>(); // "DBmapping", "Webmapping"
            this.container.RegisterType<AddressTypeView>(); // "DBmapping", "Webmapping"

            this.container.RegisterType<StreetTypeViewModel>();
            this.container.RegisterType<StreetTypeView>();

            this.container.RegisterType<SettlementTypeViewModel>();
            this.container.RegisterType<SettlementTypeView>();

            this.container.RegisterType<ContactTypeViewModel>();
            this.container.RegisterType<ContactTypeView>();

            this.container.RegisterType<BranchTypeViewModel>();
            this.container.RegisterType<BranchTypeView>();

            this.container.RegisterType<CountryViewModel>();
            this.container.RegisterType<CountryView>();

            this.container.RegisterType<CurrencyViewModel>();
            this.container.RegisterType<CurrencyView>();

            this.container.RegisterType<SoatoViewModel>();
            this.container.RegisterType<SoatoView>();

            this.container.RegisterType<EnterpriseNavigationView>();
            this.container.RegisterType<EnterpriseTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseTDESView>("EnterpriseTDESView");

            this.container.RegisterType<EnterpriseUserTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseUserTDESView>("EnterpriseUserTDESView");

            this.container.RegisterType<EnterpriseBranchTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseBranchTDESView>("EnterpriseBranchTDESView");

            this.container.RegisterType<EnterpriseAddressTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseAddressTDESView>("EnterpriseAddressTDESView");

            this.container.RegisterType<EnterpriseUserContactTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseUserContactTDESView>("EnterpriseUserContactTDESView");

            this.container.RegisterType<EnterpriseBranchContactsTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseBranchContactsTDESView>("EnterpriseBranchContactsTDESView");

            this.container.RegisterType<EnterpriseBranchUserTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseBranchUserTDESView>("EnterpriseBranchUserTDESView");

            this.container.RegisterType<EnterpriseBranchUserContactTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseBranchUserContactTDESView>("EnterpriseBranchUserContactTDESView");

            this.container.RegisterType<EnterpriseBranchWorkPlaceTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseBranchWorkPlaceTDESView>("EnterpriseBranchWorkPlaceTDESView");

            this.container.RegisterType<EnterpriseProductCategoryTDESViewModel>();
            this.container.RegisterType<Object, EnterpriseProductCategoryTDESView>("EnterpriseProductCategoryTDESView");

            this.container.RegisterType<LookUpSoatoViewModel>();
            this.container.RegisterType<Object, LookUpSoatoView>("LookUpSoatoView");

            this.container.RegisterType<LoadCategoryTreeViewModel>();
            this.container.RegisterType<Object, LoadCategoryTreeView>("LoadCategoryTreeView");



            MenuItem subItem = new MenuItem();
            subItem.Header = "Развертывание";

            ////////////////////AddressType////////////////////
            var subsubItem = new MenuItem()
            {
                Header = Resources.SimpleTypesLoadUnload_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                SimpleTypesLoadUnloadView atv = this.container.Resolve<SimpleTypesLoadUnloadView>();
                // atv.ViewModel = this.container.Resolve<AddressTypeViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);

            ////////////////////AddressType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.AddressType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) => {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                AddressTypeView atv = this.container.Resolve<AddressTypeView>();
                // atv.ViewModel = this.container.Resolve<AddressTypeViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////StreetType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.StreetType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                StreetTypeView atv = this.container.Resolve<StreetTypeView>();
                //atv.ViewModel = this.container.Resolve<StreetTypeViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////SettlementType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.SettlementType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                SettlementTypeView atv = this.container.Resolve<SettlementTypeView>();
                //atv.ViewModel = this.container.Resolve<SettlementTypeViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////ContactType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.ContactType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                ContactTypeView atv = this.container.Resolve<ContactTypeView>();
                //atv.ViewModel = this.container.Resolve<ContactTypeViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////BranchType////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.BranchType_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                BranchTypeView atv = this.container.Resolve<BranchTypeView>();
                //atv.ViewModel = this.container.Resolve<BranchTypeViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////Country////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.Country_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                CountryView atv = this.container.Resolve<CountryView>();
                //atv.ViewModel = this.container.Resolve<CountryViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////Currency////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.Currency_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                CurrencyView atv = this.container.Resolve<CurrencyView>();
                //atv.ViewModel = this.container.Resolve<CurrencyViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////Currency////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.Soato_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                SoatoView atv = this.container.Resolve<SoatoView>();
                //atv.ViewModel = this.container.Resolve<CurrencyViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
            };
            subItem.Items.Add(subsubItem);
            ////////////////////EnterpriseNavigationView////////////////////
            subsubItem = new MenuItem()
            {
                Header = Resources.EnterpriseNavigation_INDEX
            };
            subsubItem.Click += (object sender, RoutedEventArgs e) =>
            {
                var region = this.regionManager.Regions["CarShopMainContentRegion"];
                while (region.Views.Count() > 0)
                {
                    region.Remove(region.Views.ElementAt(0));
                }
                EnterpriseNavigationView atv = this.container.Resolve<EnterpriseNavigationView>();
                //atv.ViewModel = this.container.Resolve<CurrencyViewModel>();
                this.regionManager.RegisterViewWithRegion("CarShopMainContentRegion", () => atv);
                EnterpriseTDESView intV = this.container.Resolve<EnterpriseTDESView>();
                this.regionManager.RegisterViewWithRegion("EnterpriseNavigationContentRegion", () => intV);
            };
            subItem.Items.Add(subsubItem);



            this.regionManager.RegisterViewWithRegion("CarShopMainMenuRegion", () => subItem);



        }

    }
}