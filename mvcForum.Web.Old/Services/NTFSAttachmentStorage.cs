// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Services.NTFSAttachmentStorage
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using mvcForum.Core.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mvcForum.Web.Services
{
  public class NTFSAttachmentStorage : IAttachmentStorage
  {
    private const int bufferSize = 4096;
    private readonly string physicalRoot;
    private readonly string webRoot;

    public NTFSAttachmentStorage(string physicalRoot, string webRoot)
    {
      this.physicalRoot = physicalRoot;
      this.webRoot = webRoot;
    }

    public string CreateAttachment(Attachment attachment, Stream inputStream, string contentType)
    {
      IEnumerable<string> values = ((IEnumerable<string>) this.GeneratePath(attachment.Post)).Concat<string>((IEnumerable<string>) new string[1]
      {
        attachment.Id.ToString()
      });
      string str = Path.Combine(this.physicalRoot, this.Root.StartsWith("/") ? this.Root.Substring(1) : this.Root);
      foreach (string path2 in values)
      {
        str = Path.Combine(str, path2);
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
      }
      string path = Path.Combine(str, attachment.Filename);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        inputStream.CopyTo((Stream) memoryStream);
        File.WriteAllBytes(path, memoryStream.ToArray());
      }
      return string.Format("{0}/{1}", (object) this.Root, (object) string.Join("/", values));
    }

    public void DeleteAttachment(Attachment attachment)
    {
      string path = Path.Combine(this.physicalRoot, string.Join("\\", ((IEnumerable<string>) this.GeneratePath(attachment.Post)).Union<string>((IEnumerable<string>) new string[1]
      {
        attachment.Id.ToString()
      })));
      try
      {
        Directory.Delete(path);
      }
      catch
      {
      }
    }

    public void DeleteAttachments(Post post)
    {
      string path = Path.Combine(this.physicalRoot, string.Join("\\", (IEnumerable<string>) this.GeneratePath(post)));
      try
      {
        Directory.Delete(path, true);
      }
      catch
      {
      }
    }

    private string[] GeneratePath(Post post)
    {
      return mvcForum.Core.Helpers.PathHelper.GetPath(12, (long) post.Id);
    }

    public string Root
    {
      get
      {
        return this.webRoot;
      }
    }
  }
}
