// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.CategoryController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using MVCBootstrap.Web.Mvc.Extensions;
using mvcForum.Web.Areas.ForumAPI.ViewModels;
using mvcForum.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class CategoryController : BaseAPIController
  {
    [HttpPost]
    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Create(mvcForum.Web.Areas.ForumAPI.ViewModels.Category model, string mode)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
      this.GetRepository<mvcForum.Core.Category>().Create(new mvcForum.Core.Category(this.GetRepository<mvcForum.Core.Board>().Read(model.BoardId), model.Name, model.SortOrder));
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    public ActionResult Read(int id, string mode)
    {
      mvcForum.Core.Category category = this.GetRepository<mvcForum.Core.Category>().Read(id);
      return (ActionResult) this.CustomJson((object) new mvcForum.Web.Areas.ForumAPI.ViewModels.Category()
      {
        Id = category.Id,
        BoardId = category.Board.Id,
        BoardUrl = this.Url.Action("read", "board", (object) new
        {
          area = "forumapi",
          id = category.Board.Id,
          mode = "json"
        }),
        SortOrder = category.SortOrder,
        Name = category.Name
      });
    }

    [HttpPost]
    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Update(mvcForum.Web.Areas.ForumAPI.ViewModels.Category model, string mode)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
      mvcForum.Core.Category category = this.GetRepository<mvcForum.Core.Category>().Read(model.Id);
      category.Name = model.Name;
      category.SortOrder = model.SortOrder;
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    [HttpPost]
    public ActionResult Delete(int id, string mode)
    {
      IRepository<mvcForum.Core.Category> repository = this.GetRepository<mvcForum.Core.Category>();
      mvcForum.Core.Category entity = repository.Read(id);
      repository.Delete(entity);
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    public ActionResult List(int id, string mode)
    {
      return (ActionResult) this.CustomJson((object) new
      {
        Forums = this.GetRepository<mvcForum.Core.Category>().Read(id).Forums.Where<mvcForum.Core.Forum>((Func<mvcForum.Core.Forum, bool>) (f => f.ParentForum == null)).Select<mvcForum.Core.Forum, ForumLight>((Func<mvcForum.Core.Forum, ForumLight>) (f => new ForumLight()
        {
          Id = f.Id,
          Name = f.Name,
          SortOrder = f.SortOrder,
          ApiUrl = this.Url.Action("read", "forum", (object) new
          {
            area = "forumapi",
            mode = "json",
            id = f.Id
          })
        })).OrderBy<ForumLight, int>((Func<ForumLight, int>) (f => f.SortOrder))
      });
    }
  }
}
