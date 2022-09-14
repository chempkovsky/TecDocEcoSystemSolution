// Decompiled with JetBrains decompiler
// Type: CreativeMinds.StopForumSpam.Responses.FailResponse
// Assembly: CreativeMinds.StopForumSpam, Version=0.5.12.218, Culture=neutral, PublicKeyToken=null
// MVID: 7D79F5E3-250B-48C4-96EB-59D2EE004CFB
// Assembly location: C:\Development\WebCarShop\CreativeMinds.StopForumSpam.dll

namespace CreativeMinds.StopForumSpam.Responses
{
  public class FailResponse : Response
  {
    public FailResponse(string reply, string format)
      : base(reply, format)
    {
    }

    public string Error { get; internal set; }
  }
}
