// MVCBootstrap.ApplicationInitializer
using ApplicationBoilerplate.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVCBootstrap
{

    public class ApplicationInitializer
    {
        private static object initLock = new object();

        private static bool initialized = false;

        public ApplicationInitializer(IDependencyContainer container, IDependencyBuilder[] builders)
        {
            if (!initialized)
            {
                lock (initLock)
                {
                    if (!initialized)
                    {
                        container.Configure();
                        ValidateRequirements(builders);
                        foreach (IDependencyBuilder dependencyBuilder in builders)
                        {
                            dependencyBuilder.Configure(container);
                        }
                        initialized = true;
                    }
                }
            }
        }

        private void ValidateRequirements(IDependencyBuilder[] builders)
        {
            List<ApplicationRequirement> list = new List<ApplicationRequirement>();
            foreach (IDependencyBuilder dependencyBuilder in builders)
            {
                dependencyBuilder.ValidateRequirements(list);
            }
            bool flag = false;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ApplicationRequirement item in list)
            {
                if (item.Level == RequirementLevel.Fatal)
                {
                    flag = true;
                }
                stringBuilder.AppendLine(item.Feedback);
            }
            if (flag)
            {
                string str = "One or more fatal errors found during initialization, please look at the trace for details (visit: /trace.axd).";
                str = "Tracing is disabled on this web application, please enable it by adding this to the web.config file:\r\n<configuration>\r\n\t<system.web>\r\n\t\t<trace enabled=\"true\"/>\r\n\t</system.web>\r\n</configuration>\r\n\r\n" + str + Environment.NewLine + stringBuilder.ToString();
                throw new ApplicationException(str);
            }
        }
    }

}
