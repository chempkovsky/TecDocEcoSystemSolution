// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.Delete.DeleteMessageViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels.Delete
{
  public class DeleteMessageViewModel : ForumViewModelBase
  {
    [Required]
    public int Id { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.Web.DeleteMessage", "Delete")]
    public bool Delete { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.Web.DeleteMessage", "Reason")]
    public string Reason { get; set; }

    public int TopicId { get; set; }

    public string TopicTitle { get; set; }

    public string Subject { get; set; }
  }
}
