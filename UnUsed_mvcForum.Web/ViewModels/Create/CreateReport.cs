// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.Create.CreateReport
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels.Create
{
  public class CreateReport : ForumViewModelBase
  {
    public CreateReport(Post post)
    {
      this.MessageId = post.Id;
    }

    [Required]
    public int MessageId { get; set; }

    [Required]
    [DisplayName("Reason:")]
    public string Reason { get; set; }

    [Required]
    [DisplayName("Notify me:")]
    public bool Notify { get; set; }

    [DisplayName("Further information:")]
    [Required]
    public string FurtherInfo { get; set; }
  }
}
