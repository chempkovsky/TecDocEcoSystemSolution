// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Topic
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace mvcForum.Core
{
  public class Topic : ICustomPropertyHolder
  {
    private DateTime lastPosted;
    private DateTime posted;

    public Topic()
    {
      this.SpamReporters = 0;
      this.SpamScore = 0;
      this.ViewCount = 0;
      this.PostCount = 0;
    }

    public Topic(ForumUser author, Forum forum, string subject)
    {
      this.Forum = forum;
      this.Title = subject;
      this.LastPosted = this.Posted = DateTime.UtcNow;
      this.Author = author;
      this.AuthorName = author.Name;
      this.SpamReporters = 0;
      this.SpamScore = 0;
      this.Type = TopicType.Regular;
    }

    private void SetLastPost(Post post)
    {
      this.LastPosted = post.Posted;
      this.LastPost = post;
      this.LastPostAuthor = post.Author;
      this.LastPostUsername = post.AuthorName;
    }

    private void ClearLastPost()
    {
      this.LastPost = (Post) null;
      this.LastPostAuthor = (ForumUser) null;
      this.LastPostUsername = (string) null;
      this.LastPosted = this.Posted;
    }

    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    public int ViewCount { get; set; }

    [Required]
    public int PostCount { get; set; }

    public TopicFlag Flag
    {
      get
      {
        return (TopicFlag) this.FlagValue;
      }
      private set
      {
        this.FlagValue = (int) value;
      }
    }

    [Required]
    public int FlagValue { get; set; }

    public TopicType Type
    {
      get
      {
        return (TopicType) this.TypeValue;
      }
      set
      {
        this.TypeValue = (int) value;
      }
    }

    [Required]
    public int TypeValue { get; set; }

    public DateTime LastPosted
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

    public int? LastPostId { get; set; }

    public virtual Post LastPost { get; set; }

    public int? LastPostAuthorId { get; set; }

    public virtual ForumUser LastPostAuthor { get; set; }

    [StringLength(256)]
    public string LastPostUsername { get; set; }

    [Required]
    public int ForumId { get; set; }

    public virtual Forum Forum { get; set; }

    [Required]
    public int AuthorId { get; set; }

    public virtual ForumUser Author { get; set; }

    [Required]
    [StringLength(256)]
    public string AuthorName { get; set; }

    [Required]
    public DateTime Posted
    {
      get
      {
        return this.posted;
      }
      set
      {
        this.posted = value.Handle();
      }
    }

    [Required]
    public int SpamScore { get; set; }

    [Required]
    public int SpamReporters { get; set; }

    public int? OriginalTopicId { get; set; }

    public virtual Topic OriginalTopic { get; set; }

    public string CustomProperties { get; set; }

    public XDocument CustomData { get; set; }

    public void SetFlag(TopicFlag flag)
    {
      this.Flag = flag;
    }

    public void Viewed()
    {
      ++this.ViewCount;
    }

    public void ReportSpam(int spamScore)
    {
      if (spamScore < 0 || spamScore > 100)
        return;
      ++this.SpamReporters;
      this.SpamScore += this.SpamScore;
    }

    private void PostChanged()
    {
    }

    public bool Locked
    {
      get
      {
        return (this.Flag & TopicFlag.Locked) == TopicFlag.Locked;
      }
    }

    public bool Sticky
    {
      get
      {
        return this.Type == TopicType.Sticky;
      }
    }

    public bool Announcement
    {
      get
      {
        return this.Type == TopicType.Announcement;
      }
    }

    public virtual ICollection<FollowTopic> Followers { get; set; }

    public virtual ICollection<Post> Posts { get; set; }
  }
}
