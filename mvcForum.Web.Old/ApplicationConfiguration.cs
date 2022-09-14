// mvcForum.Web.ApplicationConfiguration
using ApplicationBoilerplate.DependencyInjection;
using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;
using MVCBootstrap.Web.Mvc.Interfaces;
using mvcForum.Core.Configuration;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Web;
using mvcForum.Web.Interfaces;
using MVCThemes.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace mvcForum.Web
{

    public class ApplicationConfiguration
    {
        public readonly IDependencyContainer container;

        private static bool initialized = false;

        private static object objectLock = new object();

        protected ApplicationConfiguration()
        {
        }

        private ApplicationConfiguration Configure()
        {
            MVCForumSection mVCForumSection = MVCForumSection.Get(ConfigurationManager.OpenExeConfiguration(""));
            if (mVCForumSection == null || (mVCForumSection != null && !mVCForumSection.ElementInformation.IsPresent))
            {
                throw new ConfigurationException("No MVC Forum configuration section found");
            }
            if (string.IsNullOrWhiteSpace(mVCForumSection.DependencyContainerBuilder.Type))
            {
                throw new ConfigurationException("No type found for dependencyContainerBuilder");
            }
            IDependencyContainer dependencyContainer = CreateContainer(mVCForumSection);
            ExecuteBuilder(mVCForumSection.DatabaseBuilder.Type, dependencyContainer, "databaseBuilder");
            ExecuteBuilder(mVCForumSection.StorageBuilder.Type, dependencyContainer, "storageBuilder");
            Bind(mVCForumSection.LoggingProviderComponent, dependencyContainer, typeof(ILogger));
            foreach (NamedComponent additionalBuilder in mVCForumSection.AdditionalBuilders)
            {
                ExecuteBuilder(additionalBuilder.Type, dependencyContainer, "additionalBuilder");
            }
            foreach (NamedComponent searchBuilder in mVCForumSection.SearchBuilders)
            {
                ExecuteBuilder(searchBuilder.Type, dependencyContainer, "searchBuilder");
            }
            Bind(mVCForumSection.ThemeProviderComponent, dependencyContainer, typeof(IThemeProvider));
            Bind(mVCForumSection.ThemeUrlProviderComponent, dependencyContainer, typeof(IThemeURLProvider));
            Bind(mVCForumSection.EventPublisherComponent, dependencyContainer, typeof(IEventPublisher));
            Bind(mVCForumSection.AsyncTaskComponent, dependencyContainer, typeof(IAsyncTask));
            Bind(mVCForumSection.UrlProviderComponent, dependencyContainer, typeof(IURLProvider));
            Bind(mVCForumSection.MailServiceComponent, dependencyContainer, typeof(IMailService));
            Bind(mVCForumSection.MembershipServiceComponent, dependencyContainer, typeof(mvcForum.Core.Interfaces.Services.IMembershipService));
            Bind(mVCForumSection.FormsAuthenticationComponent, dependencyContainer, typeof(IAuthenticationService));
            Bind(mVCForumSection.UserProviderComponent, dependencyContainer, typeof(IWebUserProvider));
            Bind(mVCForumSection.EventListenerComponents, dependencyContainer, typeof(IEventListener));
            Bind(mVCForumSection.ContentParserComponents, dependencyContainer, typeof(IContentParser));
            IWebUserProvider webUserProvider = (IWebUserProvider)DependencyResolver.Current.GetService(typeof(IWebUserProvider));
            return this;
        }

        private void Bind(NamedComponentsElementCollection components, IDependencyContainer container, Type bindTo)
        {
            foreach (NamedComponent component in components)
            {
                GetAssemblyAndType(component.Type, out string assembly, out string type);
                container.RegisterGeneric(bindTo, GetType(assembly, type));
            }
        }

        private void Bind(UniqueComponentElement component, IDependencyContainer container, Type bindTo)
        {
            if (string.IsNullOrWhiteSpace(component.Type))
            {
                throw new ArgumentException($"No configuration found for type {bindTo.Name}");
            }
            GetAssemblyAndType(component.Type, out string assembly, out string type);
            container.RegisterGeneric(bindTo, GetType(assembly, type));
        }

        private Type GetType(string assembly, string type)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly2 = (from a in assemblies
                                  where a.GetName().Name == assembly
                                  select a).FirstOrDefault();
            if (assembly2 == null)
            {
                throw new ArgumentException($"Assembly '{assembly}' not found.");
            }
            Type type2 = (from ty in assembly2.GetTypes()
                          where ty.FullName == type
                          select ty).FirstOrDefault();
            if (type2 == null)
            {
                throw new ArgumentException($"Type '{type}' not found.");
            }
            return type2;
        }

        private void ExecuteBuilder(string typeString, IDependencyContainer container, string builderName)
        {
            if (string.IsNullOrWhiteSpace(typeString))
            {
                throw new ConfigurationException($"No type found for {builderName}");
            }
            string assembly = string.Empty;
            string type = string.Empty;
            GetAssemblyAndType(typeString, out assembly, out type);
            Type type2 = GetType(assembly, type);
            IDependencyBuilder dependencyBuilder = (IDependencyBuilder)Activator.CreateInstance(type2, nonPublic: false);
            dependencyBuilder.Configure(container);
        }

        private IDependencyContainer CreateContainer(MVCForumSection config)
        {
            string assembly = string.Empty;
            string type = string.Empty;
            GetAssemblyAndType(config.DependencyContainerBuilder.Type, out assembly, out type);
            Type type2 = GetType(assembly, type);
            IDependencyContainer dependencyContainer = (IDependencyContainer)Activator.CreateInstance(type2, nonPublic: false);
            dependencyContainer.Configure();
            return dependencyContainer;
        }

        private void GetAssemblyAndType(string input, out string assembly, out string type)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException("input");
            }
            string[] array = input.Split(new char[1]
            {
            ','
            }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length != 2)
            {
                throw new ArgumentException("input");
            }
            assembly = array[1].Trim();
            type = array[0].Trim();
        }

        public static ApplicationConfiguration Initialize()
        {
            if (!initialized)
            {
                lock (objectLock)
                {
                    if (!initialized)
                    {
                        ApplicationConfiguration applicationConfiguration = new ApplicationConfiguration();
                        return applicationConfiguration.Configure();
                    }
                }
            }
            throw new ApplicationException("Already initialized");
        }
    }

}