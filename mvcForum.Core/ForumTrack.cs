// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.ForumTrack
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class ForumTrack
  {
    private DateTime lastViewed;

    public ForumTrack()
    {
    }

    public ForumTrack(Forum forum, ForumUser user)
    {
      this.Forum = forum;
      this.ForumUser = user;
      this.LastViewed = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }

    public virtual void Viewed()
    {
      this.LastViewed = DateTime.UtcNow;
    }

    public int Id { get; set; }

    [Required]
    public DateTime LastViewed
    {
      get
      {
        return this.lastViewed;
      }
      set
      {
        this.lastViewed = value.Handle();
      }
    }

    [Required]
    public int ForumId { get; set; }

    public virtual Forum Forum { get; set; }

    [Required]
    public int ForumUserId { get; set; }

    public virtual ForumUser ForumUser { get; set; }
  }
}
