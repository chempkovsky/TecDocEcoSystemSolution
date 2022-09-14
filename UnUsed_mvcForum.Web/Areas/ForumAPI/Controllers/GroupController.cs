// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.GroupController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using MVCBootstrap.Web.Mvc.Extensions;
using mvcForum.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class GroupController : BaseAPIController
  {
    [Authorize(Roles = "Solution Administrator")]
    [HttpPost]
    public ActionResult Create(mvcForum.Web.Areas.ForumAPI.ViewModels.Group model, string mode)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
      this.GetRepository<mvcForum.Core.Group>().Create(new mvcForum.Core.Group(model.Name));
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Read(int id, string mode)
    {
      mvcForum.Core.Group group = this.GetRepository<mvcForum.Core.Group>().Read(id);
      return (ActionResult) this.CustomJson((object) new
      {
        Id = group.Id,
        Name = group.Name
      });
    }

    [Authorize(Roles = "Solution Administrator")]
    [HttpPost]
    public ActionResult Update(mvcForum.Web.Areas.ForumAPI.ViewModels.Group model, string mode)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
      this.GetRepository<mvcForum.Core.Group>().Read(model.Id).Name = model.Name;
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Solution Administrator")]
    [HttpPost]
    public ActionResult Delete(int id, string mode)
    {
      IRepository<mvcForum.Core.Group> repository = this.GetRepository<mvcForum.Core.Group>();
      mvcForum.Core.Group entity = repository.Read(id);
      repository.Delete(entity);
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult List(int? page, string mode)
    {
      return (ActionResult) this.CustomJson((object) new
      {
        Groups = this.GetRepository<mvcForum.Core.Group>().ReadAll().Select<mvcForum.Core.Group, mvcForum.Web.Areas.ForumAPI.ViewModels.Group>((Func<mvcForum.Core.Group, mvcForum.Web.Areas.ForumAPI.ViewModels.Group>) (g => new mvcForum.Web.Areas.ForumAPI.ViewModels.Group()
        {
          Id = g.Id,
          Name = g.Name,
          ApiUrl = this.Url.Action("read", "group", (object) new
          {
            mode = "json",
            area = "forumapi",
            id = g.Id
          })
        })).OrderBy<mvcForum.Web.Areas.ForumAPI.ViewModels.Group, string>((Func<mvcForum.Web.Areas.ForumAPI.ViewModels.Group, string>) (g => g.Name))
      });
    }
  }
}
