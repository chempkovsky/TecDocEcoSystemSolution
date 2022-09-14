// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.TopicViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using mvcForum.Web.Extensions;
using System;
using System.Collections.Generic;

namespace mvcForum.Web.ViewModels
{
  public class TopicViewModel : ForumViewModelBase
  {
    public TopicViewModel(
      Topic topic,
      IEnumerable<MessageViewModel> posts,
      int postsCount,
      int postsPerPage,
      bool showDeleted)
    {
      this.Id = topic.Id;
      this.ForumId = topic.Forum.Id;
      this.Title = topic.Title;
      this.Author = !topic.Author.UseFullName || string.IsNullOrWhiteSpace(topic.Author.FullName) ? topic.AuthorName : topic.Author.FullName;
      this.Views = topic.ViewCount;
      this.MessageCount = postsCount;
      this.PerPageCount = postsPerPage;
      this.Pages = (int) Math.Ceiling((Decimal) (this.MessageCount + 1) / (Decimal) this.PerPageCount);
      this.Posted = topic.Posted;
      this.AuthorId = topic.Author.Id;
      if (topic.LastPostAuthor != null)
      {
        this.LastPosted = new DateTime?(topic.LastPosted);
        this.LastPostUserId = new int?(topic.LastPostAuthor.Id);
        this.LastUsername = !topic.LastPostAuthor.UseFullName || string.IsNullOrWhiteSpace(topic.LastPostAuthor.FullName) ? topic.LastPostUsername : topic.LastPostAuthor.FullName;
      }
      this.Moved = topic.Flag == TopicFlag.Moved;
      this.Sticky = topic.Sticky;
      this.Announcement = topic.Announcement;
      this.Deleted = (topic.Flag & TopicFlag.Deleted) != TopicFlag.None;
      this.Quarantined = (topic.Flag & TopicFlag.Quarantined) != TopicFlag.None;
      this.Locked = (topic.Flag & TopicFlag.Locked) != TopicFlag.None;
      int num;
      if (!this.Authenticated)
        num = 0;
      else if (topic.LastPostAuthor != null || topic.AuthorId == this.CurrentUser.Id || !(topic.LastRead() < (DateTimeOffset) topic.Posted))
      {
        if (topic.LastPostAuthor != null)
        {
          int? lastPostAuthorId = topic.LastPostAuthorId;
          int id = this.CurrentUser.Id;
          if ((lastPostAuthorId.GetValueOrDefault() != id ? 1 : (!lastPostAuthorId.HasValue ? 1 : 0)) != 0)
          {
            num = topic.LastRead() < (DateTimeOffset) topic.LastPosted ? 1 : 0;
            goto label_10;
          }
        }
        num = 0;
      }
      else
        num = 1;
label_10:
      this.HasUnread = num != 0;
      this.Access = topic.Forum.GetAccess();
      this.Following = topic.Following();
      this.ShowDeleted = showDeleted;
      this.Posts = posts;
      if (this.Moved && topic.OriginalTopic != null)
        this.OriginalTopic = new TopicViewModel(topic.OriginalTopic, (IEnumerable<MessageViewModel>) new List<MessageViewModel>(), 0, postsPerPage, false);
      this.CustomProperties = (IDictionary<string, string>) topic.GetCustomProperties();
    }

    public TopicViewModel OriginalTopic { private set; get; }

    public PagingModel Paging { get; set; }

    private AccessFlag Access { get; set; }

    public bool IsModerator
    {
      get
      {
        return this.HasAccess(AccessFlag.Moderator);
      }
    }

    public bool CanUpload
    {
      get
      {
        return this.HasAccess(AccessFlag.Upload);
      }
    }

    public bool CanPost
    {
      get
      {
        return this.HasAccess(AccessFlag.Post);
      }
    }

    public bool Moved { get; private set; }

    public bool Deleted { get; private set; }

    public bool Locked { get; private set; }

    public bool Quarantined { get; private set; }

    private bool HasAccess(AccessFlag flag)
    {
      return (this.Access & flag) == flag;
    }

    public IEnumerable<MessageViewModel> Posts { get; private set; }

    public int Id { get; private set; }

    public int ForumId { get; private set; }

    public string Title { get; private set; }

    public string Author { get; private set; }

    public int AuthorId { get; private set; }

    public DateTime Posted { get; private set; }

    public int Views { get; private set; }

    public int MessageCount { get; private set; }

    public DateTime? LastPosted { get; private set; }

    public string LastUsername { get; private set; }

    public int? LastPostUserId { get; private set; }

    public bool HasUnread { get; private set; }

    public bool Sticky { get; private set; }

    public bool Announcement { get; private set; }

    public bool ShowDeleted { get; private set; }

    public int PerPageCount { get; private set; }

    public int Page { get; set; }

    public int Pages { get; private set; }

    public bool Following { get; private set; }

    public IDictionary<string, string> CustomProperties { get; private set; }
  }
}
