// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.ForumViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace mvcForum.Web.ViewModels
{
  public class ForumViewModel : ForumViewModelBase
  {
    public ForumViewModel()
    {
    }

    public ForumViewModel(Forum forum, int topicsPerPage)
    {
      this.Id = forum.Id;
      this.Name = forum.Name;
      this.SortOrder = forum.SortOrder;
      this.Posts = forum.PostCount;
      this.Description = forum.Description;
      this.Paging = new PagingModel();
      this.Paging.Count = forum.TopicCount;
      IEnumerable<Topic> source = (IEnumerable<Topic>) forum.Topics.AsQueryable<Topic>().Where<Topic>(new TopicSpecifications.Visible().IsSatisfied);
      int num1 = source.Where<Topic>((Func<Topic, bool>) (t => t.TypeValue == 2)).Count<Topic>();
      this.Paging.ActualCount = source.Count<Topic>();
      int num2 = topicsPerPage - num1;
      if (num2 > 0)
      {
        this.Paging.Pages = (int) Math.Ceiling((Decimal) (this.Paging.ActualCount - num1) / (Decimal) num2);
        if (this.Paging.Pages < 1)
          this.Paging.Pages = 1;
      }
      if (forum.LastPost != null)
      {
        DateTime? lastPosted = forum.LastPosted;
        this.LastPosted = lastPosted.HasValue ? new DateTimeOffset?((DateTimeOffset) lastPosted.GetValueOrDefault()) : new DateTimeOffset?();
        this.AuthorId = new int?(forum.LastPostUser.Id);
        this.LastUsername = this.Author = !forum.LastPostUser.UseFullName || string.IsNullOrWhiteSpace(forum.LastPostUser.FullName) ? forum.LastPostUsername : forum.LastPostUser.FullName;
        this.LastTopicTitle = forum.LastTopic.Title;
        this.LastTopicId = new int?(forum.LastTopic.Id);
      }
      this.Following = forum.Following();
      this.Access = forum.GetAccess();
      this.Unread = this.Authenticated && (forum.LastPost != null && forum.LastRead() < (DateTimeOffset) forum.LastPosted.Value && forum.LastPostUser.Id != this.CurrentUser.Id);
    }

    public PagingModel Paging { get; set; }

    private AccessFlag Access { get; set; }

    public bool IsModerator
    {
      get
      {
        return this.HasAccess(AccessFlag.Moderator);
      }
    }

    public bool Accessible
    {
      get
      {
        return this.HasAccess(AccessFlag.Read);
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

    private bool HasAccess(AccessFlag flag)
    {
      return (this.Access & flag) == flag;
    }

    public int Id { get; private set; }

    [Required]
    public string Name { get; private set; }

    [Required]
    public int SortOrder { get; private set; }

    [Required]
    public string Description { get; private set; }

    public bool Following { get; set; }

    public bool Unread { get; private set; }

    public int Posts { get; private set; }

    public DateTimeOffset? LastPosted { get; private set; }

    public string LastUsername { get; private set; }

    public string LastTopicTitle { get; private set; }

    public int? LastTopicId { get; private set; }

    public int? AuthorId { get; private set; }

    public string Author { get; private set; }

    public CategoryViewModel Category { get; set; }

    public ForumViewModel Parent { get; set; }

    public IEnumerable<ForumViewModel> SubForums { get; set; }

    public IEnumerable<TopicViewModel> Topics { get; set; }

    public ReadOnlyCollection<ForumAccessViewModel> ForumAccesses { get; set; }

    public ReadOnlyCollection<GroupViewModel> Groups { get; set; }
  }
}
