// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.GroupMemberController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class GroupMemberController : BaseAPIController
  {
    [Authorize(Roles = "Solution Administrator")]
    [HttpPost]
    public ActionResult Create(int userId, int groupId, string mode)
    {
      ForumUser user = this.GetRepository<ForumUser>().Read(userId);
      this.GetRepository<GroupMember>().Create(new GroupMember(this.GetRepository<Group>().Read(groupId), user));
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [HttpPost]
    [Authorize(Roles = "Solution Administrator")]
    public ActionResult Delete(int id, string mode)
    {
      IRepository<GroupMember> repository = this.GetRepository<GroupMember>();
      GroupMember entity = repository.Read(id);
      repository.Delete(entity);
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Solution Administrator")]
    public ActionResult ListGroups(int id, string mode)
    {
      return (ActionResult) this.CustomJson((object) new
      {
        Groups = this.GetRepository<GroupMember>().ReadMany((ISpecification<GroupMember>) new GroupMemberSpecifications.SpecificUser(this.GetRepository<ForumUser>().Read(id))).Select(m => new
        {
          Id = m.Id,
          GroupId = m.Group.Id,
          Name = m.Group.Name
        })
      });
    }

    [Authorize(Roles = "Solution Administrator")]
    public ActionResult ListMembers(int id, string mode)
    {
      return (ActionResult) this.CustomJson((object) new
      {
        Groups = this.GetRepository<GroupMember>().ReadMany((ISpecification<GroupMember>) new GroupMemberSpecifications.SpecificGroup(this.GetRepository<Group>().Read(id))).Select(m => new
        {
          Id = m.Id,
          UserId = m.ForumUser.Id,
          Name = m.ForumUser.Name
        })
      });
    }
  }
}
