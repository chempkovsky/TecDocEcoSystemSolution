// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Configuration.MVCForumSection
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using mvcForum.Core.Configuration;
using System;
using System.Configuration;

namespace mvcForum.Core.Configuration
{
  public class MVCForumSection : ConfigurationSection
  {
    internal const string databaseBuilder = "databaseBuilder";
    internal const string dependencyContainerBuilder = "dependencyContainerBuilder";
    internal const string searchBuilders = "searchBuilders";
    internal const string contentParserComponents = "contentParserComponents";
    internal const string eventListenerComponents = "eventListenerComponents";
    internal const string repositoryComponents = "repositoryComponents";
    internal const string themeProviderComponent = "themeProviderComponent";
    internal const string eventPublisherComponent = "eventPublisherComponent";
    internal const string themeUrlProviderComponent = "themeUrlProviderComponent";
    internal const string asyncTaskComponent = "asyncTaskComponent";
    internal const string urlProviderComponent = "urlProviderComponent";
    internal const string mailServiceComponent = "mailServiceComponent";
    internal const string membershipServiceComponent = "membershipServiceComponent";
    internal const string formsAuthenticationComponent = "formsAuthenticationComponent";
    internal const string userProviderComponent = "userProviderComponent";
    internal const string storageBuilder = "storageBuilder";
    internal const string additionalBuilders = "additionalBuilders";
    internal const string loggingProviderComponent = "loggingProviderComponent";

    [ConfigurationProperty("databaseBuilder")]
    public UniqueComponentElement DatabaseBuilder
    {
      get
      {
        return (UniqueComponentElement) this["databaseBuilder"];
      }
    }

    [ConfigurationProperty("dependencyContainerBuilder")]
    public UniqueComponentElement DependencyContainerBuilder
    {
      get
      {
        return (UniqueComponentElement) this["dependencyContainerBuilder"];
      }
    }

    [ConfigurationProperty("themeProviderComponent")]
    public ThemeProviderComponentElement ThemeProviderComponent
    {
      get
      {
        return (ThemeProviderComponentElement) this["themeProviderComponent"];
      }
    }

    [ConfigurationProperty("themeUrlProviderComponent")]
    public ThemeUrlProviderComponentElement ThemeUrlProviderComponent
    {
      get
      {
        return (ThemeUrlProviderComponentElement) this["themeUrlProviderComponent"];
      }
    }

    [ConfigurationProperty("eventPublisherComponent")]
    public EventPublisherComponentElement EventPublisherComponent
    {
      get
      {
        return (EventPublisherComponentElement) this["eventPublisherComponent"];
      }
    }

    [ConfigurationProperty("asyncTaskComponent")]
    public AsyncTaskComponentElement AsyncTaskComponent
    {
      get
      {
        return (AsyncTaskComponentElement) this["asyncTaskComponent"];
      }
    }

    [ConfigurationProperty("loggingProviderComponent")]
    public LoggingProviderComponentElement LoggingProviderComponent
    {
      get
      {
        return (LoggingProviderComponentElement) this["loggingProviderComponent"];
      }
    }

    [ConfigurationProperty("urlProviderComponent")]
    public UrlProviderComponentElement UrlProviderComponent
    {
      get
      {
        return (UrlProviderComponentElement) this["urlProviderComponent"];
      }
    }

    [ConfigurationProperty("mailServiceComponent")]
    public MailServiceComponentElement MailServiceComponent
    {
      get
      {
        return (MailServiceComponentElement) this["mailServiceComponent"];
      }
    }

    [ConfigurationProperty("membershipServiceComponent")]
    public MembershipServiceComponentElement MembershipServiceComponent
    {
      get
      {
        return (MembershipServiceComponentElement) this["membershipServiceComponent"];
      }
    }

    [ConfigurationProperty("formsAuthenticationComponent")]
    public FormsAuthenticationComponentElement FormsAuthenticationComponent
    {
      get
      {
        return (FormsAuthenticationComponentElement) this["formsAuthenticationComponent"];
      }
    }

    [ConfigurationProperty("userProviderComponent")]
    public UserProviderComponentElement UserProviderComponent
    {
      get
      {
        return (UserProviderComponentElement) this["userProviderComponent"];
      }
    }

    [ConfigurationProperty("storageBuilder")]
    public UniqueComponentElement StorageBuilder
    {
      get
      {
        return (UniqueComponentElement) this["storageBuilder"];
      }
    }

    [ConfigurationCollection(typeof (NamedComponentsElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
    [ConfigurationProperty("searchBuilders", IsDefaultCollection = false)]
    public NamedComponentsElementCollection SearchBuilders
    {
      get
      {
        return (NamedComponentsElementCollection) this["searchBuilders"];
      }
    }

    [ConfigurationCollection(typeof (NamedComponentsElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
    [ConfigurationProperty("additionalBuilders", IsDefaultCollection = false)]
    public NamedComponentsElementCollection AdditionalBuilders
    {
      get
      {
        return (NamedComponentsElementCollection) this["additionalBuilders"];
      }
    }

    [ConfigurationCollection(typeof (NamedComponentsElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
    [ConfigurationProperty("contentParserComponents", IsDefaultCollection = false)]
    public NamedComponentsElementCollection ContentParserComponents
    {
      get
      {
        return (NamedComponentsElementCollection) this["contentParserComponents"];
      }
    }

    [ConfigurationCollection(typeof (NamedComponentsElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
    [ConfigurationProperty("eventListenerComponents", IsDefaultCollection = false)]
    public NamedComponentsElementCollection EventListenerComponents
    {
      get
      {
        return (NamedComponentsElementCollection) this["eventListenerComponents"];
      }
    }

    public static MVCForumSection Get(System.Configuration.Configuration config)
    {
      if (config == null)
        throw new ArgumentNullException(nameof (config));
      return config.GetSection("mvcForum") as MVCForumSection;
    }
  }
}
