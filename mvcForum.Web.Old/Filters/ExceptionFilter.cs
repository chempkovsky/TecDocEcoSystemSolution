// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Filters.ExceptionFilter
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.Logging;
using System.Web.Mvc;

namespace mvcForum.Web.Filters
{
  public class ExceptionFilter : IExceptionFilter
  {
    public void OnException(ExceptionContext filterContext)
    {
      DependencyResolver.Current.GetService<ILogger>().Log(EventType.Error, string.Format("Exception filter caught exception. The exception has been handled: {0}", (object) filterContext.ExceptionHandled), filterContext.Exception);
    }
  }
}
