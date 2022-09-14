// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Forum
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class Forum
  {
    private DateTime? lastPosted;

    public Forum()
    {
    }

    public Forum(Category category, string name, int sortOrder, string description)
      : this(category, name, sortOrder, description, (Forum) null)
    {
    }

    public Forum(
      Category category,
      string name,
      int sortOrder,
      string description,
      Forum parentForum)
    {
      this.Category = category;
      this.Name = name;
      this.SortOrder = sortOrder;
      this.Description = description;
      if (parentForum != null)
        this.ParentForum = parentForum;
      this.PostCount = 0;
      this.TopicCount = 0;
    }

    public int Id { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    public int SortOrder { get; set; }

    public int? ParentForumId { get; set; }

    public virtual Forum ParentForum { get; set; }

    [StringLength(2147483647)]
    public string Description { get; set; }

    public DateTime? LastPosted
    {
      get
      {
        return this.lastPosted;
      }
      set
      {
        this.lastPosted = value.Handle();
      }
    }

    public int? LastTopicId { get; set; }

    public virtual Topic LastTopic { get; set; }

    public int? LastPostId { get; set; }

    public virtual Post LastPost { get; set; }

    public int? LastPostUserId { get; set; }

    public virtual ForumUser LastPostUser { get; set; }

    [StringLength(256)]
    public string LastPostUsername { get; set; }

    [Required]
    public int TopicCount { get; set; }

    [Required]
    public int PostCount { get; set; }

    public virtual ICollection<Forum> SubForums { get; set; }

    public virtual ICollection<ForumAccess> ForumAccesses { get; set; }

    public virtual ICollection<Topic> Topics { get; set; }

    public virtual ICollection<FollowForum> Followers { get; set; }
  }
}
