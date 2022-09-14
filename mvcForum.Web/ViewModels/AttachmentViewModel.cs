// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.AttachmentViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;

namespace mvcForum.Web.ViewModels
{
  public class AttachmentViewModel
  {
    public AttachmentViewModel(Attachment attachment)
    {
      this.Filename = attachment.Filename;
      this.Path = attachment.Path;
      this.Size = (long) attachment.Size;
      this.IsImage = attachment.IsImage;
    }

    public string Filename { get; private set; }

    public string Path { get; private set; }

    public long Size { get; private set; }

    public bool IsImage { get; private set; }
  }
}
