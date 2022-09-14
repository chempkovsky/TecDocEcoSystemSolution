// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.Create.CreateTopicViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using mvcForum.Core;
using mvcForum.Web.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace mvcForum.Web.ViewModels.Create
{
  public class CreateTopicViewModel : ForumViewModelBase
  {
    private AccessFlag flag;

    public CreateTopicViewModel()
    {
    }

    public CreateTopicViewModel(mvcForum.Core.Forum forum, int topicsPerPage)
    {
      this.Type = TopicType.Regular;
      this.Forum = new ForumViewModel(forum, topicsPerPage);
      this.ForumId = forum.Id;
      this.flag = forum.GetAccess();
    }

    public bool IsModerator
    {
      get
      {
        return (this.flag & AccessFlag.Moderator) == AccessFlag.Moderator;
      }
    }

    public bool CanUpload
    {
      get
      {
        return (this.flag & AccessFlag.Upload) == AccessFlag.Upload;
      }
    }

    [Required]
    public int ForumId { get; set; }

    [LocalizedDisplay("mvcForum.Web.CreateTopic", "Subject")]
    [Required]
    public string Subject { get; set; }

    [LocalizedDisplay("mvcForum.Web.CreateTopic", "Type")]
    public TopicType Type { get; set; }

    [AllowHtml]
    [Required]
    [LocalizedDisplay("mvcForum.Web.CreateTopic", "Message")]
    public string Body { get; set; }

    [LocalizedDisplay("mvcForum.Web.CreateTopic", "AttachFile")]
    public bool AttachFile { get; set; }

    public ForumViewModel Forum { get; set; }
  }
}
