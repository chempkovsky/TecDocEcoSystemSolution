// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Attributes.LocalizedCompareAttribute
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using SimpleLocalisation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class LocalizedCompareAttribute : ValidationAttribute
  {
    public LocalizedCompareAttribute(string otherProperty, Type type, string key)
    {
      if (string.IsNullOrWhiteSpace(otherProperty))
        throw new ArgumentNullException(nameof (otherProperty));
      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentNullException(nameof (key));
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      this.OtherProperty = otherProperty;
      this.Key = key;
      this.Type = type;
    }

    public string OtherProperty { get; private set; }

    public string Key { get; private set; }

    public Type Type { get; private set; }

    private static string GetText(string key, Type type, string otherProperty)
    {
      TextManager service = DependencyResolver.Current.GetService<TextManager>();
      if (service == null)
        return key;
      TextManager textManager = service;
      object obj = (object) new
      {
        OtherProperty = otherProperty
      };
      string fullName = type.FullName;
      string key1 = key;
      object values = obj;
      string ns = fullName;
      return textManager.Get(key1, values, ns);
    }

    protected override ValidationResult IsValid(
      object value,
      ValidationContext validationContext)
    {
      PropertyInfo property = validationContext.ObjectType.GetProperty(this.OtherProperty);
      if (property == (PropertyInfo) null)
        return new ValidationResult(LocalizedCompareAttribute.GetText("UnknownProperty", this.GetType(), this.OtherProperty));
      object objB = property.GetValue(validationContext.ObjectInstance, (object[]) null);
      if (!object.Equals(value, objB))
        return new ValidationResult(LocalizedCompareAttribute.GetText(this.Key, this.Type, this.OtherProperty));
      return (ValidationResult) null;
    }
  }
}
