// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Services.ForumAccessService
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using System;

namespace mvcForum.Web.Services
{
  public class ForumAccessService : IForumAccessService
  {
    private readonly IRepository<ForumAccess> accessRepo;
    private readonly IRepository<Group> groupRepo;
    private readonly IGroupService groupService;

    public ForumAccessService(
      IRepository<ForumAccess> accessRepo,
      IRepository<Group> groupRepo,
      IGroupService groupService)
    {
      this.accessRepo = accessRepo;
      this.groupService = groupService;
      this.groupRepo = groupRepo;
    }

    public AccessFlag GetAccessFlag(Forum forum, ForumUser user)
    {
      if (forum == null)
        throw new ArgumentNullException(nameof (forum));
      AccessFlag accessFlag = AccessFlag.None;
      if (user != null)
      {
        foreach (Group group in this.groupService.GetGroups(user))
        {
          ForumAccess forumAccess = this.accessRepo.ReadOne((ISpecification<ForumAccess>) new ForumAccessSpecifications.SpecificForumAndGroup(forum, group));
          if (forumAccess != null)
            accessFlag |= forumAccess.AccessMask.AccessFlag;
        }
      }
      else
      {
        Group group = this.groupRepo.ReadOne((ISpecification<Group>) new GroupSpecifications.SpecificName("Guest"));
        ForumAccess forumAccess = this.GetForumAccess(forum, group);
        if (forumAccess != null)
          accessFlag = forumAccess.AccessMask.AccessFlag;
      }
      return accessFlag;
    }

    public bool HasAccess(Forum forum, ForumUser user, AccessFlag flag)
    {
      return (this.GetAccessFlag(forum, user) & flag) == flag;
    }

    public ForumAccess GetForumAccess(Forum forum, Group group)
    {
      return this.accessRepo.ReadOne((ISpecification<ForumAccess>) new ForumAccessSpecifications.SpecificForumAndGroup(forum, group));
    }
  }
}
