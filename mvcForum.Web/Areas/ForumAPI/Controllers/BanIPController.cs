// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.BanIPController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Controllers;
using System;
using System.Net;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class BanIPController : BaseAPIController
  {
    [HttpPost]
    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Add(string ip, string mode)
    {
      IPAddress address;
      if (string.IsNullOrEmpty(ip) || !IPAddress.TryParse(ip, out address))
        return (ActionResult) new HttpStatusCodeResult(500, "Invalid IP address");
      this.GetRepository<BannedIP>().Create(new BannedIP()
      {
        IP = address.ToString(),
        Timestamp = DateTime.UtcNow
      });
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    [HttpPost]
    public ActionResult Remove(string ip, string mode)
    {
      IPAddress address;
      if (string.IsNullOrEmpty(ip) || !IPAddress.TryParse(ip, out address))
        return (ActionResult) new HttpStatusCodeResult(500, "Invalid IP address");
      IRepository<BannedIP> repository = this.GetRepository<BannedIP>();
      BannedIP entity = repository.ReadOne((ISpecification<BannedIP>) new BannedIPSpecifications.SpecificIP(address.ToString()));
      if (entity != null)
      {
        repository.Delete(entity);
        this.Context.SaveChanges();
      }
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult CheckStatus(string ip, string mode)
    {
      IPAddress address;
      if (string.IsNullOrEmpty(ip) || !IPAddress.TryParse(ip, out address))
        return (ActionResult) new HttpStatusCodeResult(500, "Invalid IP address");
      return (ActionResult) this.CustomJson((object) new
      {
        Banned = (this.GetRepository<BannedIP>().ReadOne((ISpecification<BannedIP>) new BannedIPSpecifications.SpecificIP(address.ToString())) != null)
      });
    }
  }
}
