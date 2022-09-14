// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Category
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class Category
  {
    public Category()
    {
    }

    public Category(Board board, string name, int sortOrder)
    {
      this.Board = board;
      this.Name = name;
      this.SortOrder = sortOrder;
    }

    public int Id { get; set; }

    [Required]
    public int BoardId { get; set; }

    public virtual Board Board { get; set; }

    [StringLength(200)]
    [Required]
    public string Name { get; set; }

    [Required]
    public int SortOrder { get; set; }

    public virtual ICollection<Forum> Forums { get; set; }
  }
}
