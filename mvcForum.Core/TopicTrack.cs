// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.TopicTrack
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class TopicTrack
  {
    public TopicTrack()
    {
    }

    public TopicTrack(Topic topic, ForumUser user)
    {
      this.Topic = topic;
      this.ForumUser = user;
      this.LastViewed = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }

    public virtual void Viewed()
    {
      this.LastViewed = DateTime.UtcNow;
    }

    public int Id { get; set; }

    [Required]
    public DateTime LastViewed { get; set; }

    [Required]
    public int TopicId { get; set; }

    public virtual Topic Topic { get; set; }

    [Required]
    public int ForumUserId { get; set; }

    public virtual ForumUser ForumUser { get; set; }
  }
}
