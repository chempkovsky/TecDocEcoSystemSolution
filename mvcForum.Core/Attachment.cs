// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Attachment
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class Attachment
  {
    private bool imageChecked;
    private bool image;
    private DateTime created;

    public Attachment()
    {
    }

    public Attachment(ForumUser author, Post post, string path, string filename, int size)
    {
      this.Size = size;
      this.Post = post;
      this.Path = path;
      this.Filename = filename;
      this.DownloadCount = 0;
      this.Created = DateTime.UtcNow;
      this.Author = post.Author;
      this.Post = post;
    }

    public virtual bool IsImage
    {
      get
      {
        if (!this.imageChecked)
        {
          string lower = this.Filename.ToLower();
          this.image = lower.EndsWith(".png") || lower.EndsWith(".gif") || lower.EndsWith(".jpg");
          this.imageChecked = true;
        }
        return this.image;
      }
    }

    public int Id { get; set; }

    [StringLength(200)]
    [Required]
    public string Filename { get; set; }

    [Required]
    public int PostId { get; set; }

    public virtual Post Post { get; set; }

    [Required]
    public int Size { get; set; }

    [Required]
    public int DownloadCount { get; set; }

    [StringLength(500)]
    [Required]
    public string Path { get; set; }

    [Required]
    public int AuthorId { get; set; }

    public virtual ForumUser Author { get; set; }

    [Required]
    public DateTime Created
    {
      get
      {
        return this.created;
      }
      set
      {
        this.created = value.Handle();
      }
    }
  }
}
