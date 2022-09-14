// Decompiled with JetBrains decompiler
// Type: CreativeMinds.StopForumSpam.Responses.Response
// Assembly: CreativeMinds.StopForumSpam, Version=0.5.12.218, Culture=neutral, PublicKeyToken=null
// MVID: 7D79F5E3-250B-48C4-96EB-59D2EE004CFB
// Assembly location: C:\Development\WebCarShop\CreativeMinds.StopForumSpam.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CreativeMinds.StopForumSpam.Responses
{
  public abstract class Response
  {
    private readonly string reply = string.Empty;
    private List<ResponsePart> parts = new List<ResponsePart>();

    protected Response(string reply, string format)
    {
      this.reply = reply;
    }

    protected string GetValue(string key)
    {
      return string.Empty;
    }

    public bool Success { get; internal set; }

    public ResponsePart[] ResponseParts
    {
      get
      {
        return this.parts.ToArray();
      }
    }

    internal void Add(ResponsePart part)
    {
      this.parts.Add(part);
    }

    public static Response Parse(string reply, string format)
    {
      JToken jtoken = (JToken) JObject.Parse(reply);
      if ((int) jtoken.SelectToken("success") == 0)
      {
        FailResponse failResponse = new FailResponse(reply, format);
        failResponse.Error = (string) jtoken.SelectToken("error");
        failResponse.Success = false;
        return (Response) failResponse;
      }
      SuccessResponse successResponse = new SuccessResponse(reply, format);
      successResponse.Success = true;
      Response response = (Response) successResponse;
      JToken token1 = jtoken.SelectToken("email");
      if (token1 != null)
        response.Add(Response.ParseJSONPart(token1, RequestType.EmailAddress));
      JToken token2 = jtoken.SelectToken("username");
      if (token2 != null)
        response.Add(Response.ParseJSONPart(token2, RequestType.Username));
      JToken token3 = jtoken.SelectToken("ip");
      if (token3 != null)
        response.Add(Response.ParseJSONPart(token3, RequestType.IPAddress));
      return response;
    }

    private static ResponsePart ParseJSONPart(JToken token, RequestType type)
    {
      ResponsePart responsePart = new ResponsePart()
      {
        Type = type
      };
      responsePart.Frequency = (int) token.SelectToken("frequency");
      responsePart.Appears = (int) token.SelectToken("appears");
      JToken jtoken = token.SelectToken("lastseen");
      DateTime result;
      if (jtoken != null && DateTime.TryParse((string) jtoken, out result))
        responsePart.LastSeen = result;
      return responsePart;
    }
  }
}
