// MVCBootstrap.Web.Mvc.Controllers.AsyncController
using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;
using MVCBootstrap.Web.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Controllers
{

    public class AsyncController : Controller
    {
        private readonly ILogger log;

        public AsyncController(ILogger log)
        {
            this.log = log;
        }

        [OnlyLocalCalls]
        public string Execute(string listener, string data, string type, string assembly)
        {
            log.Log(EventType.Info, $"ASync task arrived, listener: {listener}, type: {type}, assembly: {assembly}");
            try
            {
                Assembly assembly2 = Assembly.Load(assembly);
                Type type2 = assembly2.GetType(type);
                TextReader reader = new StringReader(data);
                JsonSerializer jsonSerializer = new JsonSerializer();
                object payload = jsonSerializer.Deserialize(new JsonTextReader(reader), type2);
                IEnumerable<IEventListener> services = DependencyResolver.Current.GetServices<IEventListener>();
                foreach (IEventListener item in services)
                {
                    if (item.GetType().FullName == listener)
                    {
                        item.Handle(payload);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log(EventType.Error, "Async handling", ex);
            }
            return "";
        }
    }

}
