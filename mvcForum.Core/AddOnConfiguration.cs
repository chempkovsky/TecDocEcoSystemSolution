// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.AddOnConfiguration
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class AddOnConfiguration
  {
    public AddOnConfiguration()
    {
    }

    public AddOnConfiguration(string key, string value)
    {
      this.Key = key;
      this.Value = value;
    }

    public int Id { get; set; }

    [StringLength(50)]
    [Required]
    public string Key { get; set; }

    [StringLength(2147483647)]
    [Required]
    public string Value { get; set; }
  }
}
