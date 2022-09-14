// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.Update.UpdateTopicViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using mvcForum.Core;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace mvcForum.Web.ViewModels.Update
{
  public class UpdateTopicViewModel : ForumViewModelBase
  {
    [Required]
    public int Id { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.Web.EditTopic", "Message")]
    [AllowHtml]
    public string Body { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.Web.EditTopic", "TopicTitle")]
    public string Title { get; set; }

    [LocalizedDisplay("mvcForum.Web.EditTopic", "Reason")]
    public string Reason { get; set; }

    [LocalizedDisplay("mvcForum.Web.EditTopic", "Flag")]
    public TopicFlag Flag { get; set; }

    [LocalizedDisplay("mvcForum.Web.EditTopic", "Type")]
    public TopicType Type { get; set; }

    public bool IsModerator { get; set; }
  }
}
