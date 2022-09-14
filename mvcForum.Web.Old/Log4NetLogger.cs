// mvcForum.Web.Log4NetLogger
using ApplicationBoilerplate.Logging;
using System;

namespace mvcForum.Web
{

    public class Log4NetLogger : ILogger
    {
        public void Log(EventType type, string message)
        {
        }

        public void Log(EventType type, string message, Exception ex)
        {
        }
    }

}