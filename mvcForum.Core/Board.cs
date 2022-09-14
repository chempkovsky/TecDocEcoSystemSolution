// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Board
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class Board
  {
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [StringLength(2147483647)]
    public string Description { get; set; }

    [Required]
    public bool Disabled { get; set; }

    public virtual ICollection<Category> Categories { get; set; }
  }
}
