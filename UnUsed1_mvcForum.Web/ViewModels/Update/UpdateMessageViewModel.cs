// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.Update.UpdateMessageViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using mvcForum.Core;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace mvcForum.Web.ViewModels.Update
{
  public class UpdateMessageViewModel : ForumViewModelBase
  {
    [Required]
    public int Id { get; set; }

    [AllowHtml]
    [LocalizedDisplay("mvcForum.Web.UpdateMessage", "Message")]
    [Required]
    public string Body { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.Web.UpdateMessage", "Subject")]
    public string Subject { get; set; }

    [LocalizedDisplay("mvcForum.Web.UpdateMessage", "AttachFile")]
    public bool AttachFile { get; set; }

    [LocalizedDisplay("mvcForum.Web.UpdateMessage", "Flag")]
    public PostFlag Flag { get; set; }

    [LocalizedDisplay("mvcForum.Web.UpdateMessage", "Reason")]
    public string Reason { get; set; }

    public string TopicTitle { get; set; }

    public int TopicId { get; set; }

    public bool IsModerator { get; set; }
  }
}
