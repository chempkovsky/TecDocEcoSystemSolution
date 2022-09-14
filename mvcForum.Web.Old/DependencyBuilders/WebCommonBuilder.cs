// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.DependencyBuilders.WebCommonBuilder
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DependencyInjection;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Abstractions.Interfaces.DependencyManagement;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Web.Areas.Forum.Controllers;
using mvcForum.Web.Cache;
using mvcForum.Web.Interfaces;
using mvcForum.Web.Providers;
using mvcForum.Web.Services;
using SimpleAuthentication.Mvc;
using SimpleLocalisation;
using SimpleLocalisation.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace mvcForum.Web.DependencyBuilders
{
  public class WebCommonBuilder : IDependencyBuilder
  {
    public void Configure(IDependencyContainer container)
    {
      try
      {
        TextManager instance = new TextManager((ICultureContext) new WebCultureContext(), (ITextSource) new XmlFileTextSource((Func<string>) (() => HttpContext.Current.Server.MapPath("~/app_data/texts"))), new Language[1]
        {
          new Language("en-GB")
        });
        container.RegisterSingleton<TextManager>(instance);
      }
      catch
      {
      }
      container.RegisterPerRequest<IPerRequestCache, PerRequestCache>();
      container.RegisterSingleton<IKnownTypeHolder, KnownTypeHolder>();
      container.Register<ITopicService, TopicService>();
      container.Register<IGroupService, GroupService>();
      container.Register<IForumAccessService, ForumAccessService>();
      container.Register<IAttachmentService, AttachmentService>();
      container.Register<IPostService, PostService>();
      container.Register<IUserService, UserService>();
      container.Register<IAuthenticationCallbackProvider, AuthController>();
    }

    public void ValidateRequirements(IList<ApplicationRequirement> feedback)
    {
      if (ConfigurationManager.GetSection("system.net/mailSettings/smtp") == null)
        feedback.Add(new ApplicationRequirement()
        {
          Feedback = "Unable to find a SMTP configuration in the web.config file. MVC Forum will not be able to send e-mails.",
          Level = RequirementLevel.Warning
        });
      else
        feedback.Add(new ApplicationRequirement()
        {
          Feedback = "SMTP configuration located.",
          Level = RequirementLevel.Info
        });
      object section1 = ConfigurationManager.GetSection("system.web/membership");
      if (section1 == null)
      {
        feedback.Add(new ApplicationRequirement()
        {
          Feedback = "No membership provider found, MVC Forum can not run without a membership provider configuration.",
          Level = RequirementLevel.Warning
        });
      }
      else
      {
        MembershipSection membershipSection = (MembershipSection) section1;
        if (membershipSection.Providers[membershipSection.DefaultProvider] != null)
        {
          if (membershipSection.Providers[membershipSection.DefaultProvider].Type == string.Format("{0}.{1}", (object) typeof (MembershipProviderWrapper).Namespace, (object) typeof (MembershipProviderWrapper).Name))
          {
            feedback.Add(new ApplicationRequirement()
            {
              Feedback = "Membership provider configuration located.",
              Level = RequirementLevel.Info
            });
            string index = membershipSection.Providers[membershipSection.DefaultProvider].Parameters.Get("WrappedProvider");
            if (!string.IsNullOrWhiteSpace(index))
            {
              if (membershipSection.Providers[index] != null)
                feedback.Add(new ApplicationRequirement()
                {
                  Feedback = "The wrapped membership provider was located.",
                  Level = RequirementLevel.Info
                });
              else
                feedback.Add(new ApplicationRequirement()
                {
                  Feedback = string.Format("The wrapper membership provider '{0}' was not found.", (object) index),
                  Level = RequirementLevel.Warning
                });
            }
            else
              feedback.Add(new ApplicationRequirement()
              {
                Feedback = "The membership provider wrapper requires 'WrappedProvider' attribute.",
                Level = RequirementLevel.Warning
              });
          }
          else
            feedback.Add(new ApplicationRequirement()
            {
              Feedback = string.Format("The default membership provider should be {0}, that is the only way MVC Forum will work.", (object) typeof (MembershipProviderWrapper).Name),
              Level = RequirementLevel.Warning
            });
        }
        else
          feedback.Add(new ApplicationRequirement()
          {
            Feedback = "No default membership provider found, MVC Forum can not run without a membership provider configuration.",
            Level = RequirementLevel.Warning
          });
      }
      object section2 = ConfigurationManager.GetSection("system.web/roleManager");
      if (section2 == null)
      {
        feedback.Add(new ApplicationRequirement()
        {
          Feedback = "No role manager provider found, MVC Forum can not run without a role manager provider configuration.",
          Level = RequirementLevel.Warning
        });
      }
      else
      {
        RoleManagerSection roleManagerSection = (RoleManagerSection) section2;
        if (roleManagerSection.Providers[roleManagerSection.DefaultProvider] != null)
          return;
        feedback.Add(new ApplicationRequirement()
        {
          Feedback = "No default role manager provider found, MVC Forum can not run without a role manager provider configuration.",
          Level = RequirementLevel.Warning
        });
      }
    }
  }
}
