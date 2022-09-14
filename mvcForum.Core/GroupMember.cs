// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.GroupMember
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class GroupMember
  {
    public GroupMember()
    {
    }

    public GroupMember(Group group, ForumUser user)
    {
      this.Group = group;
      this.ForumUser = user;
    }

    public int Id { get; set; }

    [Required]
    public int GroupId { get; set; }

    public virtual Group Group { get; set; }

    [Required]
    public int ForumUserId { get; set; }

    public virtual ForumUser ForumUser { get; set; }
  }
}
