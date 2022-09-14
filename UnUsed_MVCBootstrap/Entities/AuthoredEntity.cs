// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Entities.AuthoredEntity
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Entities
{
  public abstract class AuthoredEntity
  {
    private DateTime created;
    private DateTime lastEdited;

    protected AuthoredEntity()
    {
    }

    protected AuthoredEntity(User author)
    {
      if (author == null)
        throw new ArgumentNullException(nameof (author));
      this.Author = this.LastEditor = author;
      this.Created = this.LastEdited = DateTime.UtcNow;
    }

    [Required]
    public virtual User Author { get; set; }

    [Required]
    public virtual User LastEditor { get; set; }

    [Required]
    public DateTime Created
    {
      get
      {
        return this.created;
      }
      set
      {
        this.created = value.ToUtc();
      }
    }

    [Required]
    public DateTime LastEdited
    {
      get
      {
        return this.lastEdited;
      }
      set
      {
        this.lastEdited = value.ToUtc();
      }
    }
  }
}
