// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.SplitViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels
{
  public class SplitViewModel
  {
    public ForumViewModel Forum { get; set; }

    public TopicViewModel Topic { get; set; }

    [Required]
    public int ForumId { get; set; }

    [Required]
    public int TopicId { get; set; }

    [Required]
    public string OriginalTopicTitle { get; set; }

    [Required]
    public string NewTopicTitle { get; set; }

    [Required]
    public int[] PostId { get; set; }
  }
}
