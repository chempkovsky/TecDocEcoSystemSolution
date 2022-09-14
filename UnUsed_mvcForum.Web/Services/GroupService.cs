// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Services.GroupService
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.Web.Services
{
  public class GroupService : IGroupService
  {
    private readonly IRepository<Group> groupRepo;
    private readonly IRepository<GroupMember> gmRepo;

    public GroupService(IRepository<Group> groupRepo, IRepository<GroupMember> gmRepo)
    {
      this.groupRepo = groupRepo;
      this.gmRepo = gmRepo;
    }

    public IEnumerable<Group> GetGroups(ForumUser user)
    {
      return this.gmRepo.ReadMany((ISpecification<GroupMember>) new GroupMemberSpecifications.SpecificUser(user)).Select<GroupMember, Group>((Func<GroupMember, Group>) (x => x.Group)).Distinct<Group>();
    }
  }
}
