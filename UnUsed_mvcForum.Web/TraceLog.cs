// mvcForum.Web.TraceLog
using ApplicationBoilerplate.Logging;
using System;
using System.Web;

namespace mvcForum.Web
{

    public class TraceLog : ILogger
    {
        public void Log(EventType type, string message)
        {
            Log(type, type.ToString(), message, null);
        }

        public void Log(EventType type, string message, Exception ex)
        {
            Log(type, type.ToString(), message, ex);
        }

        private void Log(EventType type, string category, string message, Exception ex)
        {
            if (HttpContext.Current != null && HttpContext.Current.Trace.IsEnabled)
            {
                if (type == EventType.Error || type == EventType.Fatal || type == EventType.Warning)
                {
                    HttpContext.Current.Trace.Warn(category, message, ex);
                }
                else
                {
                    HttpContext.Current.Trace.Write(category, message, ex);
                }
            }
        }
    }

}