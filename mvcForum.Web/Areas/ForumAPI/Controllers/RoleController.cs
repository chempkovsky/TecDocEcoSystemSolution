// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.RoleController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core.Interfaces.Services;
using mvcForum.Web.Controllers;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class RoleController : BaseAPIController
  {
    private readonly IMembershipService membershipService;

    public RoleController(IMembershipService membershipService)
    {
      this.membershipService = membershipService;
    }

    public ActionResult List(string mode)
    {
      return (ActionResult) this.CustomJson((object) new
      {
        Roles = this.membershipService.GetAllRoles()
      });
    }
  }
}
