// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.ViewModels.ContactUs
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Web.Mvc.ViewModels
{
  public class ContactUs
  {
    [Required]
    [LocalizedDisplay(typeof (ContactUs), "EmailAddress")]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; }

    [LocalizedDisplay(typeof (ContactUs), "Subject")]
    [Required]
    public string Subject { get; set; }

    [Required]
    [LocalizedDisplay(typeof (ContactUs), "Message")]
    public string Message { get; set; }

    [LocalizedDisplay(typeof (ContactUs), "Result")]
    [Required]
    public AlgebraCaptcha Result { get; set; }
  }
}
