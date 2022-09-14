// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.AccessMaskController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using MVCBootstrap.Web.Mvc.Extensions;
using mvcForum.Web.Controllers;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class AccessMaskController : BaseAPIController
  {
    [HttpPost]
    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Create(mvcForum.Web.Areas.ForumAPI.ViewModels.AccessMask model, string mode)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
      this.GetRepository<mvcForum.Core.AccessMask>().Create(new mvcForum.Core.AccessMask(this.GetRepository<mvcForum.Core.Board>().Read(model.BoardId), model.Name, model.AccessFlag));
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    public ActionResult Read(int id, string mode)
    {
      mvcForum.Core.AccessMask accessMask = this.GetRepository<mvcForum.Core.AccessMask>().Read(id);
      return (ActionResult) this.CustomJson((object) new mvcForum.Web.Areas.ForumAPI.ViewModels.AccessMask()
      {
        Id = accessMask.Id,
        Name = accessMask.Name,
        AccessFlag = accessMask.AccessFlag,
        BoardId = accessMask.Board.Id
      });
    }

    [HttpPost]
    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Update(mvcForum.Web.Areas.ForumAPI.ViewModels.AccessMask model, string mode)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
      mvcForum.Core.AccessMask accessMask = this.GetRepository<mvcForum.Core.AccessMask>().Read(model.Id);
      accessMask.Name = model.Name;
      accessMask.AccessFlag = model.AccessFlag;
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    [HttpPost]
    public ActionResult Delete(int id, string mode)
    {
      IRepository<mvcForum.Core.AccessMask> repository = this.GetRepository<mvcForum.Core.AccessMask>();
      mvcForum.Core.AccessMask entity = repository.Read(id);
      repository.Delete(entity);
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }
  }
}
