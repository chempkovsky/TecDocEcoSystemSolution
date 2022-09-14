// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.ForumExtensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Logging;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Extensions
{
  public static class ForumExtensions
  {
    public static void Track(this Forum forum)
    {
      IWebUserProvider service1 = DependencyResolver.Current.GetService<IWebUserProvider>();
      if (!service1.Authenticated)
        return;
      try
      {
        IContext service2 = DependencyResolver.Current.GetService<IContext>();
        service2.GetRepository<ForumUser>();
        IRepository<ForumTrack> repository = service2.GetRepository<ForumTrack>();
        ForumUser activeUser = service1.ActiveUser;
        if (activeUser == null)
          return;
        ForumTrack newEntity = repository.ReadOne((ISpecification<ForumTrack>) new ForumTrackSpecifications.SpecificForumAndUser(forum, activeUser));
        if (newEntity == null)
        {
          newEntity = new ForumTrack(forum, activeUser);
          repository.Create(newEntity);
        }
        newEntity.Viewed();
        service2.SaveChanges();
      }
      catch (Exception ex)
      {
        DependencyResolver.Current.GetService<ILogger>().Log(EventType.Error, "Track extension on Forum failed.", ex);
      }
    }

    public static bool HasAccess(this Forum forum, AccessFlag flag)
    {
      return (forum.GetAccess() & flag) == flag;
    }

    public static ForumAccess GetAccess(this Forum forum, Group group)
    {
      return DependencyResolver.Current.GetService<IRepository<ForumAccess>>().ReadOne((ISpecification<ForumAccess>) new ForumAccessSpecifications.SpecificForumAndGroup(forum, group));
    }

    public static AccessFlag GetAccess(this Forum forum)
    {
      AccessFlag accessFlag = AccessFlag.None;
      IWebUserProvider service1 = DependencyResolver.Current.GetService<IWebUserProvider>();
      if (service1.Authenticated)
      {
        DependencyResolver.Current.GetService<IRepository<ForumUser>>();
        ForumUser activeUser = service1.ActiveUser;
        if (activeUser != null)
        {
          IRepository<ForumAccess> service2 = DependencyResolver.Current.GetService<IRepository<ForumAccess>>();
          foreach (Group group in activeUser.Groups())
          {
            ForumAccess forumAccess = service2.ReadOne((ISpecification<ForumAccess>) new ForumAccessSpecifications.SpecificForumAndGroup(forum, group));
            if (forumAccess != null)
              accessFlag |= forumAccess.AccessMask.AccessFlag;
          }
        }
      }
      else
      {
        Group group = DependencyResolver.Current.GetService<IRepository<Group>>().ReadOne((ISpecification<Group>) new GroupSpecifications.SpecificName("Guest"));
        ForumAccess access = forum.GetAccess(group);
        if (access != null)
          accessFlag = access.AccessMask.AccessFlag;
      }
      return accessFlag;
    }

    public static DateTimeOffset LastRead(this Forum forum)
    {
      IWebUserProvider service = DependencyResolver.Current.GetService<IWebUserProvider>();
      if (service.Authenticated)
      {
        DependencyResolver.Current.GetService<IRepository<ForumUser>>();
        ForumUser activeUser = service.ActiveUser;
        if (activeUser != null)
        {
          ForumTrack forumTrack = DependencyResolver.Current.GetService<IRepository<ForumTrack>>().ReadOne((ISpecification<ForumTrack>) new ForumTrackSpecifications.SpecificForumAndUser(forum, activeUser));
          if (forumTrack != null)
            return (DateTimeOffset) forumTrack.LastViewed;
          return DateTimeOffset.MinValue.ToUniversalTime();
        }
      }
      return (DateTimeOffset) DateTime.UtcNow;
    }

    public static IEnumerable<SelectListItem> ToSelectList(
      this IEnumerable<ForumViewModel> forums)
    {
      return forums.ToSelectList((ForumViewModel) null);
    }

    public static IEnumerable<SelectListItem> ToSelectList(
      this IEnumerable<ForumViewModel> forums,
      ForumViewModel selected)
    {
      return forums.Select<ForumViewModel, SelectListItem>((Func<ForumViewModel, SelectListItem>) (x => new SelectListItem()
      {
        Value = x.Id.ToString(),
        Text = x.Name,
        Selected = selected != null && selected.Id == x.Id
      }));
    }

    public static bool Following(this Forum forum)
    {
      IWebUserProvider service = DependencyResolver.Current.GetService<IWebUserProvider>();
      if (service.Authenticated)
      {
        DependencyResolver.Current.GetService<IRepository<ForumUser>>();
        ForumUser activeUser = service.ActiveUser;
        if (activeUser != null)
          return DependencyResolver.Current.GetService<IRepository<FollowForum>>().ReadOne((ISpecification<FollowForum>) new FollowForumSpecifications.SpecificForumAndUser(forum, activeUser)) != null;
      }
      return false;
    }
  }
}
