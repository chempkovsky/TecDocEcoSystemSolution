// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Entities.EditionedAuthoredEntity`1
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System.Collections.Generic;

namespace MVCBootstrap.Entities
{
  public abstract class EditionedAuthoredEntity<TEdition> : AuthoredEntity where TEdition : AuthoredEditionEntity
  {
    protected EditionedAuthoredEntity()
    {
    }

    protected EditionedAuthoredEntity(User author)
      : base(author)
    {
    }

    public virtual ICollection<TEdition> Editions { get; set; }
  }
}
