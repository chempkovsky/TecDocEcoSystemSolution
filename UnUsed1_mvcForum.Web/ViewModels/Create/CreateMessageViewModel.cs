// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.Create.CreateMessageViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace mvcForum.Web.ViewModels.Create
{
  public class CreateMessageViewModel : ForumViewModelBase
  {
    [LocalizedDisplay("mvcForum.Web.CreateMessage", "Subject")]
    [Required]
    public string Subject { get; set; }

    [LocalizedDisplay("mvcForum.Web.CreateMessage", "Message")]
    [AllowHtml]
    [Required]
    public string Body { get; set; }

    [LocalizedDisplay("mvcForum.Web.CreateMessage", "AttachFile")]
    public bool AttachFile { get; set; }

    public bool CanUpload { get; set; }

    public int? ReplyTo { get; set; }

    public TopicViewModel Topic { get; set; }

    public int TopicId { get; set; }

    public IList<MessageViewModel> Posts { get; set; }
  }
}
