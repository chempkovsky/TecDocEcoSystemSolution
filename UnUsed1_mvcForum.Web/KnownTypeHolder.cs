// mvcForum.Web.KnownTypeHolder
using ApplicationBoilerplate.Logging;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Abstractions.Interfaces.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace mvcForum.Web
{

    public class KnownTypeHolder : IKnownTypeHolder
    {
        private readonly ILogger logger;

        private List<IInstaller> installers = new List<IInstaller>();

        public virtual IEnumerable<IInstaller> Installers => installers;

        public KnownTypeHolder(ILogger logger)
        {
            this.logger = logger;
            ShadowCopy();
        }

        protected virtual void ShadowCopy()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                logger.Log(EventType.Info, $"About to probe {assembly.FullName}");
                ProbeAssembly(assembly);
            }
        }

        protected virtual void ProbeAssembly(Assembly ass)
        {
            try
            {
                Type[] types = ass.GetTypes();
                foreach (Type type in types)
                {
                    if (type.GetInterface("IBootStrapper") != null)
                    {
                        logger.Log(EventType.Info, $"Found a bootstrapper {type}");
                    }
                    if (type.GetInterface("IInstaller") != null)
                    {
                        logger.Log(EventType.Info, $"Found an installer {type}");
                        installers.Add((IInstaller)Activator.CreateInstance(type));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(EventType.Error, string.Format("ProbeAssembly failed, {1}", ass, ass.FullName), ex);
            }
        }
    }

}