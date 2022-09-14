// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.ForumAccess
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class ForumAccess
  {
    public ForumAccess()
    {
    }

    public ForumAccess(Forum forum, Group group, AccessMask mask)
    {
      this.Forum = forum;
      this.Group = group;
      this.AccessMask = mask;
    }

    public int Id { get; internal set; }

    [Required]
    public int ForumId { get; set; }

    public virtual Forum Forum { get; set; }

    [Required]
    public int GroupId { get; set; }

    public virtual Group Group { get; set; }

    [Required]
    public int AccessMaskId { get; set; }

    public virtual AccessMask AccessMask { get; set; }
  }
}
