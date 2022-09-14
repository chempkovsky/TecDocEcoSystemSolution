// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Group
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class Group
  {
    public Group()
    {
    }

    public Group(string name)
    {
      this.Name = name;
    }

    public int Id { get; set; }

    [StringLength(100)]
    [Required]
    public string Name { get; set; }
  }
}
