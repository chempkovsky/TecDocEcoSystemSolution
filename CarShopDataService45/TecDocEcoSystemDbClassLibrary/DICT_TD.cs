// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.DICT_TD
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System;

namespace TecDocEcoSystemDbClassLibrary
{
  public class DICT_TD : IEquatable<DICT_TD>
  {
    public int DictId { get; set; }

    public string DictTitle { get; set; }

    public bool Equals(DICT_TD other)
    {
      if (object.ReferenceEquals((object) other, (object) null))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.DictId.Equals(other.DictId);
    }

    public override int GetHashCode()
    {
      return (this.DictTitle == null ? 0 : this.DictTitle.GetHashCode()) ^ this.DictId.GetHashCode();
    }
  }
}
