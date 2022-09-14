// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Events.AsyncWebTask
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace MVCBootstrap.Web.Events
{
  public class AsyncWebTask : AsyncTask
  {
    private readonly IAsyncConfiguration configuration;

    public AsyncWebTask(IAsyncConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public override void Execute(string key, object data)
    {
      string str1 = this.configuration.SiteUrl();
      string str2 = this.configuration.EndPoint();
      string listener = this.GetListener(key);
      if (!str1.EndsWith("/"))
        str1 += "/";
      if (str2.StartsWith("/"))
        str2 = str2.Substring(1);
      Uri uri = new Uri(string.Format("{0}{1}", (object) str1, (object) str2));
      StringBuilder sb = new StringBuilder();
      new JsonSerializer()
      {
        PreserveReferencesHandling = PreserveReferencesHandling.None,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      }.Serialize((JsonWriter) new JsonTextWriter((TextWriter) new StringWriter(sb)), data);
      AsyncWebTask.Post(new Dictionary<string, object>()
      {
        {
          "listener",
          (object) listener
        },
        {
          nameof (data),
          (object) sb.ToString()
        },
        {
          "type",
          (object) data.GetType().FullName
        },
        {
          "assembly",
          (object) data.GetType().Assembly.FullName
        }
      }, uri);
    }

    private static string BuildSet(string key, string value)
    {
      return string.Format("{0}={1}&", (object) key, (object) HttpUtility.UrlEncode(value));
    }

    private static string BuildParameterString(Dictionary<string, object> parameters)
    {
      string empty = string.Empty;
      foreach (KeyValuePair<string, object> parameter in parameters)
      {
        if (parameter.Value == null)
          empty += AsyncWebTask.BuildSet(parameter.Key, "");
        else if (parameter.Value is string)
          empty += AsyncWebTask.BuildSet(parameter.Key, (string) parameter.Value);
        else if (parameter.Value is int)
          empty += AsyncWebTask.BuildSet(parameter.Key, ((int) parameter.Value).ToString());
        else if (parameter.Value is int)
          empty += AsyncWebTask.BuildSet(parameter.Key, ((int) parameter.Value).ToString());
        else if (parameter.Value is System.Enum)
          empty += AsyncWebTask.BuildSet(parameter.Key, ((System.Enum) parameter.Value).ToString());
        else if (parameter.Value.GetType().IsGenericType && parameter.Value is List<string>)
        {
          foreach (string str in (List<string>) parameter.Value)
            empty += AsyncWebTask.BuildSet(parameter.Key, str);
        }
      }
      if (!string.IsNullOrEmpty(empty))
        return empty.Substring(0, empty.Length - 1);
      return "";
    }

    public static string Post(Dictionary<string, object> parameters, Uri uri)
    {
      UTF8Encoding utF8Encoding = new UTF8Encoding();
      string s = string.Empty;
      if (parameters != null && parameters.Count > 0)
        s = AsyncWebTask.BuildParameterString(parameters);
      byte[] bytes = utF8Encoding.GetBytes(s);
      WebRequest webRequest = WebRequest.Create(uri);
      webRequest.Method = "POST";
      webRequest.Timeout = 30000;
      webRequest.ContentType = "application/x-www-form-urlencoded";
      webRequest.ContentLength = (long) bytes.Length;
      Stream requestStream = webRequest.GetRequestStream();
      requestStream.Write(bytes, 0, bytes.Length);
      requestStream.Close();
      string str = string.Empty;
      try
      {
        using (WebResponse response = webRequest.GetResponse())
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            str = streamReader.ReadToEnd();
            streamReader.Close();
          }
        }
      }
      catch (Exception ex)
      {
      }
      return str;
    }
  }
}
