// Decompiled with JetBrains decompiler
// Type: CreativeMinds.StopForumSpam.Responses.ResponsePart
// Assembly: CreativeMinds.StopForumSpam, Version=0.5.12.218, Culture=neutral, PublicKeyToken=null
// MVID: 7D79F5E3-250B-48C4-96EB-59D2EE004CFB
// Assembly location: C:\Development\WebCarShop\CreativeMinds.StopForumSpam.dll

using System;

namespace CreativeMinds.StopForumSpam.Responses
{
  public class ResponsePart
  {
    public RequestType Type { get; set; }

    public int Frequency { get; set; }

    public int Appears { get; set; }

    public DateTime LastSeen { get; set; }
  }
}
