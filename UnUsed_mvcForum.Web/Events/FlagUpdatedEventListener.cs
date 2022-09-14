// mvcForum.Web.Events.FlagUpdatedEventListener
using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Events;
using mvcForum.Web.Events;
using mvcForum.Web.Extensions;
using System;
using System.Linq;

namespace mvcForum.Web.Events
{

    public class FlagUpdatedEventListener : IAsyncEventListener<PostFlagUpdatedEvent>, IEventListener<PostFlagUpdatedEvent>, IAsyncEventListener<TopicFlagUpdatedEvent>, IEventListener<TopicFlagUpdatedEvent>, IEventListener
    {
        private readonly ILogger logger;

        private readonly IAsyncTask task;

        private readonly IConfiguration config;

        private readonly IContext context;

        public byte Priority => byte.MaxValue;

        public bool UniqueEvent => false;

        public bool RunAsynchronously => false;

        public FlagUpdatedEventListener(ILogger logger, IContext context, IAsyncTask task, IConfiguration config)
        {
            this.logger = logger;
            this.task = task;
            this.config = config;
            this.context = context;
        }

        public void Queue(PostFlagUpdatedEvent payload)
        {
            logger.Log(EventType.Debug, $"Post {payload.PostId} is queued.");
            task.Execute(this, payload, 3);
        }

        public void Queue(TopicFlagUpdatedEvent payload)
        {
            logger.Log(EventType.Debug, $"Topic {payload.TopicId} is queued.");
            task.Execute(this, payload, 3);
        }

        public void Handle(object payload)
        {
            if (payload is PostFlagUpdatedEvent)
            {
                Handle((PostFlagUpdatedEvent)payload);
                return;
            }
            if (payload is TopicFlagUpdatedEvent)
            {
                Handle((TopicFlagUpdatedEvent)payload);
                return;
            }
            throw new ApplicationException("Unknown payload!");
        }

        public void Handle(PostFlagUpdatedEvent payload)
        {
            Post post = context.GetRepository<Post>().Read(payload.PostId);
            logger.Log(EventType.Debug, string.Format("A post '{1}' was updated, time to index it ({0}).", post.Id, post.Subject));
            post.Topic.PostCount = post.Topic.Posts.Visible(config).Count() - 1;
            context.SaveChanges();
            NewAndUpdatedContentEventListener.UpdateForum(post.Topic.Forum, context);
        }

        public void Handle(TopicFlagUpdatedEvent payload)
        {
            Topic topic = context.GetRepository<Topic>().Read(payload.TopicId);
            logger.Log(EventType.Debug, string.Format("A topic '{1}' was updated, time to index it ({0}).", topic.Id, topic.Title));
            if (topic.Flag != payload.OriginalFlag && (payload.OriginalFlag != TopicFlag.Locked || topic.Flag != 0) && (payload.OriginalFlag != 0 || topic.Flag != TopicFlag.Locked) && (payload.OriginalFlag != TopicFlag.Quarantined || topic.Flag != TopicFlag.Deleted) && (payload.OriginalFlag != TopicFlag.Deleted || topic.Flag != TopicFlag.Quarantined))
            {
                topic.Posts.Visible(config).Count();
                if (payload.OriginalFlag == TopicFlag.Deleted || payload.OriginalFlag == TopicFlag.Quarantined)
                {
                    NewAndUpdatedContentEventListener.UpdateForum(topic.Forum, context);
                }
                else
                {
                    NewAndUpdatedContentEventListener.UpdateForum(topic.Forum, context);
                }
            }
        }
    }

}
