// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Helpers.PathHelper
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Collections.Generic;

namespace mvcForum.Core.Helpers
{
  public static class PathHelper
  {
    private const string alpha = "abcdefghijklmnopqrstuvwxyz";

    public static string[] GetPath(int levels, long seed)
    {
      List<string> stringList = new List<string>();
      long num = 1;
      for (int index = 0; index < levels - 1; ++index)
        num *= (long) "abcdefghijklmnopqrstuvwxyz".Length;
      for (int index1 = 0; index1 <= levels - 1; ++index1)
      {
        int index2 = (int) (seed / num);
        seed %= num;
        num /= (long) "abcdefghijklmnopqrstuvwxyz".Length;
        stringList.Add("abcdefghijklmnopqrstuvwxyz"[index2].ToString());
      }
      return stringList.ToArray();
    }
  }
}
