// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.TopicExtensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Logging;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Interfaces;
using System;
using System.Web.Mvc;

namespace mvcForum.Web.Extensions
{
  public static class TopicExtensions
  {
    public static void Track(this Topic topic)
    {
      IWebUserProvider service1 = DependencyResolver.Current.GetService<IWebUserProvider>();
      if (!service1.Authenticated)
        return;
      try
      {
        IContext service2 = DependencyResolver.Current.GetService<IContext>();
        service2.GetRepository<ForumUser>();
        IRepository<TopicTrack> repository = service2.GetRepository<TopicTrack>();
        ForumUser activeUser = service1.ActiveUser;
        if (activeUser == null)
          return;
        TopicTrack newEntity = repository.ReadOne((ISpecification<TopicTrack>) new TopicTrackSpecifications.SpecificTopicAndUser(topic, activeUser));
        if (newEntity == null)
        {
          newEntity = new TopicTrack(topic, activeUser);
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

    public static void TrackAndView(this Topic topic)
    {
      topic.Track();
      if (!DependencyResolver.Current.GetService<IWebUserProvider>().Authenticated)
        return;
      topic.Viewed();
      DependencyResolver.Current.GetService<IContext>().SaveChanges();
    }

    public static DateTimeOffset LastRead(this Topic topic)
    {
      IWebUserProvider service = DependencyResolver.Current.GetService<IWebUserProvider>();
      if (service.Authenticated)
      {
        try
        {
          DependencyResolver.Current.GetService<IRepository<ForumUser>>();
          ForumUser activeUser = service.ActiveUser;
          if (activeUser != null)
          {
            TopicTrack topicTrack = DependencyResolver.Current.GetService<IRepository<TopicTrack>>().ReadOne((ISpecification<TopicTrack>) new TopicTrackSpecifications.SpecificTopicAndUser(topic, activeUser));
            return topicTrack == null ? DateTimeOffset.MinValue.ToUniversalTime() : (DateTimeOffset) topicTrack.LastViewed;
          }
        }
        catch (Exception ex)
        {
          DependencyResolver.Current.GetService<ILogger>().Log(EventType.Error, "LatRead extension on Topic failed.", ex);
        }
      }
      return (DateTimeOffset) DateTime.UtcNow;
    }

    public static bool Following(this Topic topic)
    {
      IWebUserProvider service = DependencyResolver.Current.GetService<IWebUserProvider>();
      if (service.Authenticated)
      {
        DependencyResolver.Current.GetService<IRepository<ForumUser>>();
        ForumUser activeUser = service.ActiveUser;
        if (activeUser != null)
          return DependencyResolver.Current.GetService<IRepository<FollowTopic>>().ReadOne((ISpecification<FollowTopic>) new FollowTopicSpecifications.SpecificTopicAndUser(topic, activeUser)) != null;
      }
      return false;
    }
  }
}
