using WpfCarShop.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using System;

namespace WpfCarShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;

//            Bootstrapper bootstrapper = new Bootstrapper();
//            bootstrapper.Run();
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;
            string aMessage = ex.Message;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                aMessage = aMessage + ex.Message;
            }

            MessageBox.Show(ex.Message);
            Environment.Exit(1);
        }


    }
}
