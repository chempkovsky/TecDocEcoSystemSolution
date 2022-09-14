// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Entities.AuthoredEditionEntity
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Entities
{
  public abstract class AuthoredEditionEntity : AuthoredEntity
  {
    protected AuthoredEditionEntity()
    {
    }

    public AuthoredEditionEntity(string edition, User author)
      : base(author)
    {
      this.Edition = edition;
    }

    [Required]
    [StringLength(10)]
    public string Edition { get; set; }
  }
}
