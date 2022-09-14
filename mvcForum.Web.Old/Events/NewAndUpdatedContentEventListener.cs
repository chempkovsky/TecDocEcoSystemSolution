// mvcForum.Web.Events.NewAndUpdatedContentEventListener
using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Events;
using mvcForum.Core.Specifications;
using mvcForum.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.Web.Events
{

    public class NewAndUpdatedContentEventListener : IAsyncEventListener<NewPostEvent>, IEventListener<NewPostEvent>, IAsyncEventListener<NewTopicEvent>, IEventListener<NewTopicEvent>, IAsyncEventListener<PostUpdatedEvent>, IEventListener<PostUpdatedEvent>, IAsyncEventListener<TopicUpdatedEvent>, IEventListener<TopicUpdatedEvent>, IAsyncEventListener<TopicMergedEvent>, IEventListener<TopicMergedEvent>, IAsyncEventListener<TopicSplitEvent>, IEventListener<TopicSplitEvent>, IAsyncEventListener<TopicMovedEvent>, IEventListener<TopicMovedEvent>, IEventListener
    {
        private readonly ILogger logger;

        private readonly IAsyncTask task;

        private readonly IConfiguration config;

        private readonly IContext context;

        public bool RunAsynchronously => false;

        public byte Priority => byte.MaxValue;

        public bool UniqueEvent => false;

        public NewAndUpdatedContentEventListener(ILogger logger, IContext context, IAsyncTask task, IConfiguration config)
        {
            this.logger = logger;
            this.task = task;
            this.config = config;
            this.context = context;
        }

        public void Queue(PostUpdatedEvent payload)
        {
            logger.Log(EventType.Debug, $"The updated post {payload.PostId} is queued.");
            task.Execute(this, payload, 3);
        }

        public void Queue(TopicUpdatedEvent payload)
        {
            logger.Log(EventType.Debug, $"The updated topic {payload.TopicId} is queued.");
            task.Execute(this, payload, 3);
        }

        public void Queue(TopicSplitEvent payload)
        {
            logger.Log(EventType.Debug, $"The split topic {payload.OriginalTopicId} is queued.");
            task.Execute(this, payload, 3);
        }

        public void Queue(NewTopicEvent payload)
        {
            logger.Log(EventType.Debug, $"The new topic {payload.TopicId} is queued.");
            task.Execute(this, payload, 3);
        }

        public void Queue(NewPostEvent payload)
        {
            logger.Log(EventType.Debug, $"The new post {payload.PostId} is queued.");
            task.Execute(this, payload, 3);
        }

        public void Queue(TopicMergedEvent payload)
        {
            logger.Log(EventType.Debug, $"The merged topic {payload.TopicId} is queued.");
            task.Execute(this, payload, 3);
        }

        public void Queue(TopicMovedEvent payload)
        {
            logger.Log(EventType.Debug, $"The moved topic {payload.TopicId} is queued.");
            task.Execute(this, payload, 3);
        }

        public void Handle(object payload)
        {
            if (payload is TopicUpdatedEvent)
            {
                Handle((TopicUpdatedEvent)payload);
                return;
            }
            if (payload is PostUpdatedEvent)
            {
                Handle((PostUpdatedEvent)payload);
                return;
            }
            if (payload is NewTopicEvent)
            {
                Handle((NewTopicEvent)payload);
                return;
            }
            if (payload is NewPostEvent)
            {
                Handle((NewPostEvent)payload);
                return;
            }
            if (payload is TopicMergedEvent)
            {
                Handle((TopicMergedEvent)payload);
                return;
            }
            if (payload is TopicMovedEvent)
            {
                Handle((TopicMovedEvent)payload);
                return;
            }
            if (payload is TopicSplitEvent)
            {
                Handle((TopicSplitEvent)payload);
                return;
            }
            throw new ApplicationException("Unknown payload!");
        }

        public void Handle(TopicMergedEvent payload)
        {
            Topic topic = context.GetRepository<Topic>().Read(payload.TopicId);
            logger.Log(EventType.Debug, string.Format("A merged topic '{1}' ({0}).", topic.Id, topic.Title));
            IEnumerable<Post> source = topic.Posts.Visible(config);
            topic.PostCount = source.Count() - 1;
            if (source.Any())
            {
                Post post = (from p in source
                             orderby p.Posted descending
                             select p).First();
                topic.LastPostId = post.Id;
                topic.LastPostAuthorId = post.Author.Id;
                topic.LastPosted = post.Posted;
                topic.LastPostUsername = post.AuthorName;
            }
            context.SaveChanges();
            UpdateForum(topic.Forum, context);
        }

        public void Handle(TopicMovedEvent payload)
        {
            Forum forum = context.GetRepository<Forum>().Read(payload.SourceForumId);
            Forum forum2 = context.GetRepository<Forum>().Read(payload.DestinationForumId);
            logger.Log(EventType.Debug, $"A topic was moved ({payload.TopicId}).");
            UpdateForum(forum, context);
            UpdateForum(forum2, context);
        }

        public void Handle(PostUpdatedEvent payload)
        {
        }

        public void Handle(TopicUpdatedEvent payload)
        {
        }

        public void Handle(NewTopicEvent payload)
        {
            Topic topic = context.GetRepository<Topic>().Read(payload.TopicId);
            logger.Log(EventType.Debug, string.Format("A new topic '{1}', was created, time to index it ({0}).", topic.Id, topic.Title));
            if (topic.Flag == TopicFlag.None || topic.Flag == TopicFlag.Locked)
            {
                context.SaveChanges();
                UpdateForum(topic.Forum, context);
            }
        }

        public void Handle(TopicSplitEvent payload)
        {
            Topic topic = context.GetRepository<Topic>().Read(payload.OriginalTopicId);
            Topic topic2 = context.GetRepository<Topic>().Read(payload.NewTopicId);
            logger.Log(EventType.Debug, string.Format("A new topic '{1}', was created by splitting '{0}' in two.", topic.Title, topic2.Title));
            IEnumerable<Post> source = topic2.Posts.Where(delegate (Post p)
            {
                if (p.FlagValue == 0)
                {
                    return p.Position != 0;
                }
                return false;
            });
            topic2.PostCount = source.Count();
            if (topic2.PostCount > 0)
            {
                Post post2 = topic2.LastPost = (from p in source
                                                orderby p.Posted descending
                                                select p).First();
                topic2.LastPosted = post2.Posted;
                topic2.LastPostAuthor = post2.Author;
                topic2.LastPostUsername = post2.AuthorName;
            }
            source = topic.Posts.Where(delegate (Post p)
            {
                if (p.FlagValue == 0)
                {
                    return p.Position != 0;
                }
                return false;
            });
            topic.PostCount = source.Count();
            if (topic2.PostCount > 0)
            {
                Post post4 = topic.LastPost = (from p in source
                                               orderby p.Posted descending
                                               select p).First();
                topic.LastPosted = post4.Posted;
                topic.LastPostAuthor = post4.Author;
                topic.LastPostUsername = post4.AuthorName;
            }
            context.SaveChanges();
            UpdateForum(topic2.Forum, context);
        }

        public void Handle(NewPostEvent payload)
        {
            Post post = context.GetRepository<Post>().Read(payload.PostId);
            logger.Log(EventType.Debug, string.Format("A new post '{1}', was created, time to index it ({0}).", post.Id, post.Subject));
            if (post.Flag == PostFlag.None)
            {
                post.Topic.PostCount++;
                post.Topic.LastPost = post;
                post.Topic.LastPosted = post.Posted;
                post.Topic.LastPostAuthor = post.Author;
                post.Topic.LastPostUsername = post.AuthorName;
                context.SaveChanges();
                UpdateForum(post.Topic.Forum, context);
            }
        }

        public static void UpdateForum(Forum forum, IContext context)
        {
            IEnumerable<Topic> source = from t in forum.Topics.AsQueryable().Where(new TopicSpecifications.Visible().IsSatisfied)
                                        orderby t.LastPosted descending
                                        select t;
            Topic topic = source.FirstOrDefault();
            IEnumerable<Topic> source2 = from f in forum.SubForums
                                         select f.LastTopic into t
                                         where t != null
                                         orderby t.LastPosted descending
                                         select t;
            Topic topic2 = source2.FirstOrDefault();
            if (topic != null || topic2 != null)
            {
                if (topic2 != null && topic != null)
                {
                    if (topic.LastPosted < topic2.LastPosted)
                    {
                        topic = topic2;
                    }
                }
                else if (topic2 != null)
                {
                    topic = topic2;
                }
            }
            if (topic == null)
            {
                ResetLatest(forum);
            }
            else
            {
                SetLatest(forum, topic);
            }
            forum.TopicCount = source.Count() + forum.SubForums.Sum((Forum f) => f.TopicCount);
            forum.PostCount = source.SelectMany((Topic t) => t.Posts).Where(delegate (Post p)
            {
                if (p.Position != 0)
                {
                    return p.FlagValue == 0;
                }
                return false;
            }).Count() + forum.SubForums.Sum((Forum f) => f.PostCount);
            context.SaveChanges();
            if (forum.ParentForum != null)
            {
                UpdateForum(forum.ParentForum, context);
            }
        }

        private static void SetLatest(Forum forum, Topic latestTopic)
        {
            forum.LastTopic = latestTopic;
            if (latestTopic.LastPost == null)
            {
                Post post2 = forum.LastPost = (from p in latestTopic.Posts
                                               orderby p.Posted
                                               select p).First();
                forum.LastPosted = post2.Posted;
                forum.LastPostUser = post2.Author;
                forum.LastPostUsername = post2.AuthorName;
            }
            else
            {
                forum.LastPost = latestTopic.LastPost;
                forum.LastPosted = latestTopic.LastPosted;
                forum.LastPostUser = latestTopic.LastPostAuthor;
                forum.LastPostUsername = latestTopic.LastPostUsername;
            }
        }

        private static void ResetLatest(Forum forum)
        {
            forum.LastTopic = null;
            forum.LastPost = null;
            forum.LastPosted = null;
            forum.LastPostUser = null;
            forum.LastPostUsername = string.Empty;
        }
    }

}
