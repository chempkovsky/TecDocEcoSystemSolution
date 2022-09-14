// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.ForumSettings
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace mvcForum.Core
{
  public class ForumSettings
  {
    public ForumSettings()
    {
    }

    public ForumSettings(string key, int value)
      : this(key, value.ToString())
    {
    }

    public ForumSettings(string key, bool value)
      : this(key, value.ToString())
    {
    }

    public ForumSettings(string key, string value)
    {
      this.Key = key;
      this.Value = value;
    }

    public virtual int? GetInt32()
    {
      int? nullable = new int?();
      int result;
      if (int.TryParse(this.Value, out result))
        return new int?(result);
      return nullable;
    }

    public virtual void SetInt32(int value)
    {
      this.Value = value.ToString();
    }

    public virtual bool GetBoolean()
    {
      bool result = false;
      bool.TryParse(this.Value, out result);
      return result;
    }

    public virtual void SetBoolean(bool value)
    {
      this.Value = value.ToString();
    }

    public virtual IList<int> GetInt32List()
    {
      return (IList<int>) ((IEnumerable<string>) this.Value.Split(new char[1]
      {
        ';'
      }, StringSplitOptions.RemoveEmptyEntries)).Select<string, int>((Func<string, int>) (v => int.Parse(v))).ToList<int>();
    }

    public void SetInt32List(IList<int> values)
    {
      this.Value = "";
      foreach (int num in (IEnumerable<int>) values)
        this.Value += string.Format("{0};", (object) num);
    }

    public int Id { get; set; }

    [StringLength(100)]
    [Required]
    public string Key { get; set; }

    [StringLength(2147483647)]
    public string Value { get; set; }
  }
}
