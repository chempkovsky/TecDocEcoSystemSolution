// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.BoardController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using MVCBootstrap.Web.Mvc.Extensions;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Areas.ForumAdmin.ViewModels.List;
using mvcForum.Web.Areas.ForumAPI.ViewModels;
using mvcForum.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class BoardController : BaseAPIController
  {
    [HttpPost]
    [Authorize(Roles = "Solution Administrator")]
    public ActionResult Create(mvcForum.Web.Areas.ForumAPI.ViewModels.Board model, string mode)
    {
      this.GetRepository<mvcForum.Core.Board>().Create(new mvcForum.Core.Board()
      {
        Name = model.Name,
        Description = string.IsNullOrWhiteSpace(model.Description) ? "" : model.Description,
        Disabled = false
      });
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    public ActionResult Read(int id, string mode)
    {
      mvcForum.Core.Board board = this.GetRepository<mvcForum.Core.Board>().Read(id);
      return (ActionResult) this.CustomJson((object) new mvcForum.Web.Areas.ForumAPI.ViewModels.Board()
      {
        Id = board.Id,
        Name = board.Name,
        Disabled = board.Disabled,
        Description = board.Description
      });
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    [HttpPost]
    public ActionResult Update(mvcForum.Web.Areas.ForumAPI.ViewModels.Board model, string mode)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
      mvcForum.Core.Board board = this.GetRepository<mvcForum.Core.Board>().Read(model.Id);
      board.Name = model.Name;
      board.Description = model.Description;
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Solution Administrator")]
    public ActionResult Delete(int id, string mode)
    {
      IRepository<mvcForum.Core.Board> repository = this.GetRepository<mvcForum.Core.Board>();
      mvcForum.Core.Board entity = repository.Read(id);
      repository.Delete(entity);
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    public ActionResult List(int? id, string mode)
    {
      if (id.HasValue)
        return (ActionResult) this.CustomJson((object) new
        {
          Categories = this.GetRepository<mvcForum.Core.Board>().Read(id.Value).Categories.Select<mvcForum.Core.Category, CategoryLight>((Func<mvcForum.Core.Category, CategoryLight>) (c => new CategoryLight()
          {
            Id = c.Id,
            Name = c.Name,
            SortOrder = c.SortOrder,
            ApiUrl = this.Url.Action("read", "category", (object) new
            {
              area = "forumapi",
              mode = "json",
              id = c.Id
            })
          })).OrderBy<CategoryLight, int>((Func<CategoryLight, int>) (c => c.SortOrder))
        });
      return (ActionResult) this.CustomJson((object) new
      {
        Boards = this.GetRepository<mvcForum.Core.Board>().ReadAll().Select<mvcForum.Core.Board, BoardLight>((Func<mvcForum.Core.Board, BoardLight>) (b => new BoardLight()
        {
          Id = b.Id,
          Name = b.Name,
          ApiUrl = this.Url.Action("read", "board", (object) new
          {
            area = "forumapi",
            mode = "json",
            id = b.Id
          })
        })).OrderBy<BoardLight, string>((Func<BoardLight, string>) (b => b.Name))
      });
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult AccessMaskList(int id, string mode)
    {
      return (ActionResult) this.CustomJson((object) new
      {
        AccessMasks = this.GetRepository<mvcForum.Core.AccessMask>().ReadMany((ISpecification<mvcForum.Core.AccessMask>) new AccessMaskSpecifications.SpecificBoard(this.GetRepository<mvcForum.Core.Board>().Read(id))).Select<mvcForum.Core.AccessMask, mvcForum.Web.Areas.ForumAPI.ViewModels.AccessMask>((Func<mvcForum.Core.AccessMask, mvcForum.Web.Areas.ForumAPI.ViewModels.AccessMask>) (m => new mvcForum.Web.Areas.ForumAPI.ViewModels.AccessMask()
        {
          Id = m.Id,
          Name = m.Name,
          AccessFlag = m.AccessFlag,
          ApiUrl = this.Url.Action("read", "accessmask", (object) new
          {
            area = "forumapi",
            mode = "json",
            id = m.Id
          })
        })).OrderBy<mvcForum.Web.Areas.ForumAPI.ViewModels.AccessMask, string>((Func<mvcForum.Web.Areas.ForumAPI.ViewModels.AccessMask, string>) (m => m.Name))
      });
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Statistics(int id, string mode)
    {
      DateTime dateTime = DateTime.Parse(this.GetRepository<ForumSettings>().ReadOne((ISpecification<ForumSettings>) new ForumSettingsSpecifications.SpecificKey("InstallDate")).Value);
      return (ActionResult) this.CustomJson((object) new Statistics()
      {
        TopicCount = this.GetRepository<mvcForum.Core.Topic>().ReadMany((ISpecification<mvcForum.Core.Topic>) new TopicSpecifications.Visible()).Count<mvcForum.Core.Topic>(),
        PostCount = this.GetRepository<mvcForum.Core.Post>().ReadMany((ISpecification<mvcForum.Core.Post>) new PostSpecifications.Visible()).Count<mvcForum.Core.Post>(),
        AttachmentCount = this.GetRepository<Attachment>().ReadAll().Count<Attachment>(),
        UserCount = this.GetRepository<mvcForum.Core.ForumUser>().ReadMany((ISpecification<mvcForum.Core.ForumUser>) new ForumUserSpecifications.NotDeleted()).Count<mvcForum.Core.ForumUser>(),
        AttachmentSize = this.GetRepository<Attachment>().ReadAll().Sum<Attachment>((Func<Attachment, int>) (a => a.Size)),
        InstallDate = dateTime,
        Days = DateTime.UtcNow.Subtract(dateTime).Days,
        Version = this.GetType().Assembly.GetName().Version.ToString()
      });
    }
  }
}
