// Decompiled with JetBrains decompiler
// Type: mvcForum.SearchProvider.Lucene.LuceneContentChangeEventListener
// Assembly: mvcForum.SearchProvider.Lucene, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 13A5DCA9-908F-4689-9C6D-F584F417B850
// Assembly location: C:\Development\WebCarShop\mvcForum.SearchProvider.Lucene.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;
using mvcForum.Core;
using mvcForum.Core.Abstractions;
using mvcForum.Core.Events;
using mvcForum.Core.Interfaces.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.SearchProvider.Lucene
{
  public class LuceneContentChangeEventListener : IAsyncEventListener<NewPostEvent>, IEventListener<NewPostEvent>, IAsyncEventListener<NewTopicEvent>, IEventListener<NewTopicEvent>, IAsyncEventListener<PostUpdatedEvent>, IEventListener<PostUpdatedEvent>, IAsyncEventListener<TopicUpdatedEvent>, IEventListener<TopicUpdatedEvent>, IAsyncEventListener<PostFlagUpdatedEvent>, IEventListener<PostFlagUpdatedEvent>, IAsyncEventListener<TopicFlagUpdatedEvent>, IEventListener<TopicFlagUpdatedEvent>, IAsyncEventListener<TopicSplitEvent>, IEventListener<TopicSplitEvent>, IAsyncEventListener<TopicMergedEvent>, IEventListener<TopicMergedEvent>, IAsyncEventListener<TopicMovedEvent>, IEventListener<TopicMovedEvent>, IEventListener
  {
    private readonly IEnumerable<IIndexer> indexers;
    private readonly ILogger logger;
    private readonly IRepository<Post> postRepo;
    private readonly IRepository<Topic> topicRepo;
    private readonly IAsyncTask task;
    private readonly AsyncAddOnConfiguration<Indexer> config;

    public LuceneContentChangeEventListener(
      IEnumerable<IIndexer> indexers,
      ILogger logger,
      IRepository<Post> postRepo,
      IRepository<Topic> topicRepo,
      IAsyncTask task,
      AsyncAddOnConfiguration<Indexer> config)
    {
      this.indexers = indexers.Where<IIndexer>((Func<IIndexer, bool>) (i => i.GetType() == typeof (Indexer)));
      this.logger = logger;
      this.postRepo = postRepo;
      this.topicRepo = topicRepo;
      this.task = task;
      this.config = config;
    }

    public void Queue(PostUpdatedEvent payload)
    {
      this.logger.Log(EventType.Debug, string.Format("The updated post {0} is queued.", (object) payload.PostId));
      this.task.Execute((IEventListener) this, (object) payload, this.config.Delay);
    }

    public void Queue(TopicUpdatedEvent payload)
    {
      this.logger.Log(EventType.Debug, string.Format("The updated topic {0} is queued.", (object) payload.TopicId));
      this.task.Execute((IEventListener) this, (object) payload, this.config.Delay);
    }

    public void Queue(NewTopicEvent payload)
    {
      this.logger.Log(EventType.Debug, string.Format("The new topic {0} is queued.", (object) payload.TopicId));
      this.task.Execute((IEventListener) this, (object) payload, this.config.Delay);
    }

    public void Queue(TopicSplitEvent payload)
    {
      this.logger.Log(EventType.Debug, string.Format("The split topic {0} is queued.", (object) payload.OriginalTopicId));
      this.task.Execute((IEventListener) this, (object) payload, this.config.Delay);
    }

    public void Queue(NewPostEvent payload)
    {
      this.logger.Log(EventType.Debug, string.Format("The new post {0} is queued.", (object) payload.PostId));
      this.task.Execute((IEventListener) this, (object) payload, this.config.Delay);
    }

    public void Queue(PostFlagUpdatedEvent payload)
    {
      this.logger.Log(EventType.Debug, string.Format("The updated post {0} is queued.", (object) payload.PostId));
      this.task.Execute((IEventListener) this, (object) payload, this.config.Delay);
    }

    public void Queue(TopicFlagUpdatedEvent payload)
    {
      this.logger.Log(EventType.Debug, string.Format("The updated topic {0} is queued.", (object) payload.TopicId));
      this.task.Execute((IEventListener) this, (object) payload, this.config.Delay);
    }

    public void Queue(TopicMergedEvent payload)
    {
      this.logger.Log(EventType.Debug, string.Format("The merged topic {0} is queued.", (object) payload.TopicId));
      this.task.Execute((IEventListener) this, (object) payload, this.config.Delay);
    }

    public void Queue(TopicMovedEvent payload)
    {
      this.logger.Log(EventType.Debug, string.Format("The moved topic {0} is queued.", (object) payload.TopicId));
      this.task.Execute((IEventListener) this, (object) payload, this.config.Delay);
    }

    public void Handle(object payload)
    {
      if (payload is TopicUpdatedEvent)
        this.Handle((TopicUpdatedEvent) payload);
      else if (payload is PostUpdatedEvent)
        this.Handle((PostUpdatedEvent) payload);
      else if (payload is NewTopicEvent)
        this.Handle((NewTopicEvent) payload);
      else if (payload is NewPostEvent)
        this.Handle((NewPostEvent) payload);
      else if (payload is PostFlagUpdatedEvent)
        this.Handle((PostFlagUpdatedEvent) payload);
      else if (payload is TopicFlagUpdatedEvent)
        this.Handle((TopicFlagUpdatedEvent) payload);
      else if (payload is TopicMergedEvent)
      {
        this.Handle((TopicMergedEvent) payload);
      }
      else
      {
        if (!(payload is TopicMovedEvent))
          throw new ApplicationException("Unknown payload!");
        this.Handle((TopicMovedEvent) payload);
      }
    }

    public void Handle(TopicMergedEvent payload)
    {
      Topic topic = this.topicRepo.Read(payload.TopicId);
      this.logger.Log(EventType.Debug, string.Format("A topic '{1}' was merged, time to index it ({0}).", (object) topic.Id, (object) topic.Title));
      this.Remove(topic);
      this.Index(topic);
    }

    public void Handle(TopicMovedEvent payload)
    {
      Topic topic = this.topicRepo.Read(payload.TopicId);
      this.logger.Log(EventType.Debug, string.Format("A topic '{1}' was moved, time to reindex it ({0}).", (object) topic.Id, (object) topic.Title));
      this.Remove(topic);
      this.Index(topic);
    }

    public void Handle(PostUpdatedEvent payload)
    {
      Post post = this.postRepo.Read(payload.PostId);
      this.logger.Log(EventType.Debug, string.Format("A post '{1}' was updated, time to index it ({0}).", (object) post.Id, (object) post.Subject));
      if (post.Flag == PostFlag.Deleted || post.Flag == PostFlag.Quarantined)
        this.Remove(post);
      else
        this.Index(post);
    }

    public void Handle(TopicUpdatedEvent payload)
    {
      Topic topic = this.topicRepo.Read(payload.TopicId);
      this.logger.Log(EventType.Debug, string.Format("A topic '{1}' was updated, time to index it ({0}).", (object) topic.Id, (object) topic.Title));
      if (topic.Flag == TopicFlag.Deleted || topic.Flag == TopicFlag.Quarantined)
        this.Remove(topic);
      else
        this.Index(topic);
    }

    public void Handle(TopicSplitEvent payload)
    {
      Topic topic1 = this.topicRepo.Read(payload.OriginalTopicId);
      Topic topic2 = this.topicRepo.Read(payload.NewTopicId);
      this.logger.Log(EventType.Debug, string.Format("A new topic '{1}', was created by splitting '{0}' in two.", (object) topic1.Title, (object) topic2.Title));
      this.Remove(topic1);
      if (topic1.Flag != TopicFlag.Deleted && topic1.Flag != TopicFlag.Quarantined && topic1.Flag != TopicFlag.Moved)
        this.Index(topic1);
      if (topic2.Flag == TopicFlag.Deleted || topic2.Flag == TopicFlag.Quarantined || topic2.Flag == TopicFlag.Moved)
        return;
      this.Index(topic2);
    }

    public void Handle(NewTopicEvent payload)
    {
      Topic topic = this.topicRepo.Read(payload.TopicId);
      this.logger.Log(EventType.Debug, string.Format("A new topic '{1}', was created, time to index it ({0}).", (object) topic.Id, (object) topic.Title));
      if (topic.Flag == TopicFlag.Quarantined || topic.Flag == TopicFlag.Deleted)
        return;
      this.Index(topic);
    }

    public void Handle(NewPostEvent payload)
    {
      Post post = this.postRepo.Read(payload.PostId);
      this.logger.Log(EventType.Debug, string.Format("A new post '{1}', was created, time to index it ({0}).", (object) post.Id, (object) post.Subject));
      if (post.Flag == PostFlag.Quarantined || post.Flag == PostFlag.Deleted)
        return;
      this.Index(post);
    }

    public void Handle(PostFlagUpdatedEvent payload)
    {
      Post post = this.postRepo.Read(payload.PostId);
      if (post.Flag == payload.OriginalFlag)
        return;
      if (post.Flag == PostFlag.None)
        this.Index(post);
      else
        this.Remove(post);
    }

    public void Handle(TopicFlagUpdatedEvent payload)
    {
      Topic topic = this.topicRepo.Read(payload.TopicId);
      if (topic.Flag == payload.OriginalFlag)
        return;
      if (topic.Flag == TopicFlag.None || topic.Flag == TopicFlag.Locked)
        this.Index(topic);
      else
        this.Remove(topic);
    }

    private void Index(Post post)
    {
      foreach (IIndexer indexer in this.indexers)
        indexer.Index(post);
    }

    private void Index(Topic topic)
    {
      foreach (IIndexer indexer in this.indexers)
        indexer.Index(topic);
    }

    private void Remove(Post post)
    {
      foreach (IIndexer indexer in this.indexers)
        indexer.Remove(post);
    }

    private void Remove(Topic topic)
    {
      foreach (IIndexer indexer in this.indexers)
        indexer.Remove(topic);
    }

    public bool RunAsynchronously
    {
      get
      {
        return this.config.RunAsynchronously;
      }
    }

    public byte Priority
    {
      get
      {
        return byte.MaxValue;
      }
    }

    public bool UniqueEvent
    {
      get
      {
        return false;
      }
    }
  }
}
