// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.BannedIP
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class BannedIP
  {
    private DateTime timestamp;

    public BannedIP()
    {
    }

    public BannedIP(string ip)
    {
      this.IP = ip;
      this.Timestamp = DateTime.UtcNow;
    }

    public int Id { get; set; }

    [StringLength(50)]
    [Required]
    public string IP { get; set; }

    [Required]
    public DateTime Timestamp
    {
      get
      {
        return this.timestamp;
      }
      set
      {
        this.timestamp = value.Handle();
      }
    }
  }
}
