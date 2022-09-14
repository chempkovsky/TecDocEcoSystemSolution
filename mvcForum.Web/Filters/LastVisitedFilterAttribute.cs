// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Filters.LastVisitedFilterAttribute
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Logging;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using System;
using System.Web.Mvc;

namespace mvcForum.Web.Filters
{
  public class LastVisitedFilterAttribute : ActionFilterAttribute
  {
    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      base.OnResultExecuted(filterContext);
      using (IContext service = DependencyResolver.Current.GetService<IContext>())
      {
        if (filterContext.Controller == null)
          return;
        if (!filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
          return;
        try
        {
          ForumUser forumUser = DependencyResolver.Current.GetService<IRepository<ForumUser>>().ReadOne((ISpecification<ForumUser>) new ForumUserSpecifications.SpecificUsername(filterContext.RequestContext.HttpContext.User.Identity.Name));
          if (forumUser == null)
            return;
          forumUser.LastVisit = DateTime.UtcNow;
          forumUser.LastIP = filterContext.RequestContext.HttpContext.Request.UserHostAddress;
          service.SaveChanges();
        }
        catch (Exception ex)
        {
          DependencyResolver.Current.GetService<ILogger>().Log(EventType.Error, "OnResultExecuted failed.", ex);
        }
      }
    }
  }
}
