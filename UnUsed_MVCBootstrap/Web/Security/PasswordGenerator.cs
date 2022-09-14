// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Security.PasswordGenerator
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Text;

namespace MVCBootstrap.Web.Security
{
  public static class PasswordGenerator
  {
    public static string Generate(int length)
    {
      return PasswordGenerator.Generate(length, PasswordStrength.AlphaNumeric);
    }

    public static string Generate(int length, PasswordStrength strength)
    {
      string empty = string.Empty;
      Random random = new Random();
      int num1 = (int) (2 + strength);
      for (int index = 0; index < length; ++index)
      {
        switch (random.Next(1, num1 + 1))
        {
          case 1:
            byte num2 = (byte) random.Next(65, 91);
            empty += Encoding.ASCII.GetString(new byte[1]
            {
              num2
            });
            break;
          case 2:
            byte num3 = (byte) random.Next(97, 123);
            empty += Encoding.ASCII.GetString(new byte[1]
            {
              num3
            });
            break;
          case 3:
            empty += random.Next(0, 10).ToString();
            break;
          case 4:
            byte num4 = (byte) random.Next(33, 47);
            empty += Encoding.ASCII.GetString(new byte[1]
            {
              num4
            });
            break;
          default:
            throw new ApplicationException("Ups");
        }
      }
      return empty;
    }
  }
}
