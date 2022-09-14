// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.ForumAPIAreaRegistration
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI
{
  public class ForumAPIAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "forumapi";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute("ForumAPI_default", "forumapi/{mode}/{controller}/{action}/{id}", (object) new
      {
        controller = "Home",
        action = "Index",
        mode = "json",
        id = UrlParameter.Optional
      }, new string[1]
      {
        "mvcForum.Web.Areas.ForumAPI.Controllers"
      });
    }
  }
}
