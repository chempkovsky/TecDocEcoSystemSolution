// Decompiled with JetBrains decompiler
// Type: CreativeMinds.StopForumSpam.HTTP.HTTPClient
// Assembly: CreativeMinds.StopForumSpam, Version=0.5.12.218, Culture=neutral, PublicKeyToken=null
// MVID: 7D79F5E3-250B-48C4-96EB-59D2EE004CFB
// Assembly location: C:\Development\WebCarShop\CreativeMinds.StopForumSpam.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace CreativeMinds.StopForumSpam.HTTP
{
  public abstract class HTTPClient
  {
    protected HTTPClient()
    {
      this.Timeout = 5;
    }

    public int Timeout { get; set; }

    protected string Get(Uri url)
    {
      return this.Get(url, (Dictionary<string, string>) null);
    }

    protected string Get(Uri url, Dictionary<string, string> parameters)
    {
      return this.Request(HTTPMethod.GET, url, parameters);
    }

    protected string Post(Uri url)
    {
      return this.Post(url, (Dictionary<string, string>) null);
    }

    protected string Post(Uri url, Dictionary<string, string> parameters)
    {
      return this.Request(HTTPMethod.POST, url, parameters);
    }

    protected string Request(HTTPMethod method, Uri url, Dictionary<string, string> parameters)
    {
      UTF8Encoding utF8Encoding = new UTF8Encoding();
      string s = string.Empty;
      if (parameters != null)
        s = this.BuildParameterString(parameters);
      if (method == HTTPMethod.GET)
        url = new Uri(url.ToString() + "?" + s);
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      httpWebRequest.Method = method.ToString();
      httpWebRequest.Timeout = this.Timeout * 1000;
      httpWebRequest.UserAgent = string.Format("Stop Forum Spam .Net v{0}", (object) this.GetType().Assembly.GetName().Version);
      if (method == HTTPMethod.POST)
      {
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        byte[] bytes = utF8Encoding.GetBytes(s);
        httpWebRequest.ContentLength = (long) bytes.Length;
        Stream requestStream = httpWebRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
      }
      WebResponse response = httpWebRequest.GetResponse();
      StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
      string end = streamReader.ReadToEnd();
      response.Close();
      streamReader.Close();
      return end;
    }

    private string BuildParameterString(Dictionary<string, string> parameters)
    {
      string empty = string.Empty;
      foreach (KeyValuePair<string, string> parameter in parameters)
        empty += this.BuildSet(parameter.Key, parameter.Value);
      return string.IsNullOrEmpty(empty) ? "" : empty.Substring(0, empty.Length - 1);
    }

    private string BuildSet(string key, string value)
    {
      return string.Format("{0}={1}&", (object) key, (object) HttpUtility.UrlEncode(value));
    }
  }
}
