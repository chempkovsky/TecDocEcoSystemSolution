// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.ForumAccessController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core.Specifications;
using mvcForum.Web.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class ForumAccessController : BaseAPIController
  {
    [HttpPost]
    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Create(mvcForum.Web.Areas.ForumAPI.ViewModels.ForumAccess model, string mode)
    {
      mvcForum.Core.Forum forum = this.GetRepository<mvcForum.Core.Forum>().Read(model.ForumId);
      mvcForum.Core.Group group = this.GetRepository<mvcForum.Core.Group>().Read(model.GroupId);
      IRepository<mvcForum.Core.ForumAccess> repository = this.GetRepository<mvcForum.Core.ForumAccess>();
      mvcForum.Core.ForumAccess newEntity = repository.ReadOne((ISpecification<mvcForum.Core.ForumAccess>) new ForumAccessSpecifications.SpecificForumAndGroup(forum, group));
      if (newEntity == null)
      {
        newEntity = new mvcForum.Core.ForumAccess();
        newEntity.Forum = forum;
        newEntity.Group = group;
        newEntity.AccessMask = this.GetRepository<mvcForum.Core.AccessMask>().Read(model.AccessMaskId);
        repository.Create(newEntity);
      }
      else
        newEntity.AccessMask = this.GetRepository<mvcForum.Core.AccessMask>().Read(model.AccessMaskId);
      this.Context.SaveChanges();
      return (ActionResult) this.CustomJson((object) new
      {
        Id = newEntity.Id
      });
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    [HttpPost]
    public ActionResult Update(mvcForum.Web.Areas.ForumAPI.ViewModels.ForumAccess model, string mode)
    {
      mvcForum.Core.ForumAccess forumAccess = this.GetRepository<mvcForum.Core.ForumAccess>().Read(model.Id);
      if (forumAccess.Forum.Id == model.ForumId && forumAccess.Group.Id == model.GroupId)
      {
        forumAccess.AccessMask = this.GetRepository<mvcForum.Core.AccessMask>().Read(model.AccessMaskId);
        this.Context.SaveChanges();
      }
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    [HttpPost]
    public ActionResult Delete(int id, string mode)
    {
      IRepository<mvcForum.Core.ForumAccess> repository = this.GetRepository<mvcForum.Core.ForumAccess>();
      mvcForum.Core.ForumAccess entity = repository.Read(id);
      if (entity != null)
      {
        repository.Delete(entity);
        this.Context.SaveChanges();
      }
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult List(int id, string mode)
    {
      return (ActionResult) this.CustomJson((object) new
      {
        Masks = this.GetRepository<mvcForum.Core.ForumAccess>().ReadMany((ISpecification<mvcForum.Core.ForumAccess>) new ForumAccessSpecifications.SpecificForum(this.GetRepository<mvcForum.Core.Forum>().Read(id))).Select(m => new
        {
          Id = m.Id,
          Group = m.Group.Id,
          AccessMask = m.AccessMask.Id
        })
      });
    }
  }
}
