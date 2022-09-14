// Decompiled with JetBrains decompiler
// Type: CreativeMinds.StopForumSpam.Client
// Assembly: CreativeMinds.StopForumSpam, Version=0.5.12.218, Culture=neutral, PublicKeyToken=null
// MVID: 7D79F5E3-250B-48C4-96EB-59D2EE004CFB
// Assembly location: C:\Development\WebCarShop\CreativeMinds.StopForumSpam.dll

using CreativeMinds.StopForumSpam.HTTP;
using CreativeMinds.StopForumSpam.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace CreativeMinds.StopForumSpam
{
  public class Client : HTTPClient
  {
    private readonly string apiKey;
    private readonly string format;

    public Client()
      : this(string.Empty)
    {
    }

    public Client(string apiKey)
    {
      this.apiKey = apiKey;
      this.format = "json";
    }

    public Response CheckEmailAddress(string emailAddress)
    {
      return this.Check((object) new{ email = emailAddress });
    }

    public Response CheckUsername(string username)
    {
      return this.Check((object) new{ username = username });
    }

    public Response CheckIPAddress(string ipAddress)
    {
      IPAddress address;
      if (!IPAddress.TryParse(ipAddress, out address))
        throw new ArgumentException("The ipAddress argument is not a valid IP address.");
      return this.Check((object) new
      {
        ip = address.ToString()
      });
    }

    public Response CheckIPAddress(IPAddress ipAddress)
    {
      return this.Check((object) new{ ip = ipAddress });
    }

    public Response Check(string username, string emailAddress, string ipAddress)
    {
      IPAddress address = (IPAddress) null;
      if (!string.IsNullOrWhiteSpace(ipAddress) && !IPAddress.TryParse(ipAddress, out address))
        throw new ArgumentException("The ipAddress argument is not a valid IP address.");
      return this.Check(username, emailAddress, address);
    }

    public Response Check(string username, string emailAddress, IPAddress ipAddress)
    {
      Dictionary<string, string> parameters = new Dictionary<string, string>();
      if (!string.IsNullOrWhiteSpace(username))
        parameters.Add(nameof (username), username);
      if (!string.IsNullOrWhiteSpace(emailAddress))
        parameters.Add("email", emailAddress);
      if (ipAddress != null)
        parameters.Add("ip", ipAddress.ToString());
      return this.Check(parameters);
    }

    private Response Check(object values)
    {
      Dictionary<string, string> parameters = new Dictionary<string, string>();
      if (values != null)
      {
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(values))
          parameters.Add(property.Name, property.GetValue(values).ToString());
      }
      return this.Check(parameters);
    }

    private Response Check(Dictionary<string, string> parameters)
    {
      parameters.Add("f", this.format);
      return Response.Parse(this.Get(new Uri("http://www.stopforumspam.com/api"), parameters), this.format);
    }

    public Response AddSpammer(string username, string emailAddress, string ipAddress)
    {
      IPAddress address;
      if (IPAddress.TryParse(ipAddress, out address))
        return this.AddSpammer(username, emailAddress, address);
      throw new ArgumentException("The ipAddress argument is not a valid IP address.");
    }

    public Response AddSpammer(string username, string emailAddress, IPAddress ipAddress)
    {
      if (string.IsNullOrWhiteSpace(this.apiKey))
        throw new ArgumentNullException("apiKey");
      if (string.IsNullOrWhiteSpace(username))
        throw new ArgumentNullException(nameof (username));
      if (string.IsNullOrWhiteSpace(emailAddress))
        throw new ArgumentNullException(nameof (emailAddress));
      return Response.Parse(this.Post(new Uri("http://www.stopforumspam.com/add.php"), new Dictionary<string, string>()
      {
        {
          nameof (username),
          username
        },
        {
          "ip_addr",
          username
        },
        {
          "email",
          emailAddress
        },
        {
          "api_key",
          this.apiKey
        }
      }), this.format);
    }
  }
}
