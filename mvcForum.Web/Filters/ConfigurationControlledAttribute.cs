// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Filters.ConfigurationControlledAttribute
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core.Abstractions.Interfaces;
using System;
using System.Web.Mvc;

namespace mvcForum.Web.Filters
{
  public class ConfigurationControlledAttribute : ActionFilterAttribute
  {
    private ConfigurationArea area;
    private string name;

    public ConfigurationControlledAttribute(ConfigurationArea area, string name)
    {
      this.area = area;
      this.name = name;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      base.OnActionExecuting(filterContext);
      if (filterContext == null)
        throw new ArgumentNullException(nameof (filterContext));
      if (filterContext.Controller == null)
        return;
      try
      {
        IConfiguration service = DependencyResolver.Current.GetService<IConfiguration>();
        if (this.area != ConfigurationArea.AccountController)
          throw new ArgumentException("area");
        bool flag = service.UseForumAccountController;
        if (flag && !string.IsNullOrWhiteSpace(this.name))
        {
          switch (this.name)
          {
            case "SignUp":
              flag = service.AllowLocalUsers && service.AllowSignUp;
              break;
            case "Local":
              flag = service.AllowLocalUsers;
              break;
            case "OpenAuth":
              flag = service.AllowOpenAuthUsers;
              break;
            case "LocalOrOpenAuth":
              flag = service.AllowOpenAuthUsers || service.AllowLocalUsers;
              break;
            default:
              throw new ApplicationException("name");
          }
        }
        if (flag)
          return;
        filterContext.Result = (ActionResult) new HttpNotFoundResult();
      }
      catch
      {
      }
    }
  }
}
