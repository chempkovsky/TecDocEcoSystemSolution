// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.AttachmentLimitReachedException
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core
{
  public class AttachmentLimitReachedException : AttachmentException
  {
    public readonly int Usage;
    public readonly int MaxUsage;

    public AttachmentLimitReachedException(int usage, int maxUsage)
      : base(string.Format("You're allowed to store {0} bytes of data, you have {1} bytes.", (object) usage, (object) maxUsage))
    {
      this.Usage = usage;
      this.MaxUsage = maxUsage;
    }
  }
}
