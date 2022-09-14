// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.AccessMask
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class AccessMask
  {
    public AccessMask()
    {
    }

    public AccessMask(Board board, string name, AccessFlag flag)
    {
      this.Board = board;
      this.Name = name;
      this.AccessFlag = flag;
    }

    public int Id { get; set; }

    [Required]
    public int BoardId { get; set; }

    public virtual Board Board { get; set; }

    [StringLength(200)]
    [Required]
    public string Name { get; set; }

    public AccessFlag AccessFlag
    {
      get
      {
        return (AccessFlag) this.AccessFlagValue;
      }
      set
      {
        this.AccessFlagValue = (int) value;
      }
    }

    [Required]
    public int AccessFlagValue { get; set; }
  }
}
