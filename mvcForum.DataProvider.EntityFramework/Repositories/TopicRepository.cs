// mvcForum.DataProvider.EntityFramework.Repositories.TopicRepository
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Specifications;
using mvcForum.DataProvider.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace mvcForum.DataProvider.EntityFramework.Repositories
{

    public class TopicRepository : Repository<Topic>, ITopicRepository, IRepository<Topic>
    {
        private readonly IConfiguration config;

        public TopicRepository(DbContext context, IConfiguration config)
            : base(context)
        {
            this.config = config;
        }

        public IList<Topic> ReadTopics(Forum forum, int page, ForumUser user, bool isModerator)
        {
            IEnumerable<Topic> enumerable = ReadAnnouncements(forum, user, isModerator);
            if (enumerable.Count() < config.TopicsPerPage)
            {
                enumerable = enumerable.Concat(ReadStickiesAndRegulars(forum, page, enumerable.Count(), user, isModerator));
            }
            return enumerable.ToList();
        }

        public IList<Topic> ReadStickiesAndRegulars(Forum forum, int page, int announcementCount, ForumUser user, bool isModerator)
        {
            IEnumerable<Topic> source = from t in ReadManyOptimized(new TopicSpecifications.StickiesAndRegularsNotSpam(forum, config.SpamAverage, user, isModerator))
                                        orderby t.Type, t.LastPosted descending
                                        select t;
            return source.Skip((page - 1) * (config.TopicsPerPage - announcementCount)).Take(config.TopicsPerPage - announcementCount).ToList();
        }

        public IList<Topic> ReadQuarantined(Forum forum)
        {
            return (from t in (from t in ReadAllOptimized()
                               where t.ForumId == forum.Id
                               select t).AsQueryable().Where(new TopicSpecifications.Visible().IsSatisfied)
                    orderby t.Posted descending
                    select t).ToList();
        }

        public IList<Topic> ReadAnnouncements(Forum forum, ForumUser user, bool isModerator)
        {
            return (from t in ReadManyOptimized(new TopicSpecifications.AnnouncementsNotSpam(forum, config.SpamAverage, user, isModerator))
                    orderby t.LastPosted descending
                    select t).ThenByDescending((Topic x) => x.Posted).ToList();
        }

        public IEnumerable<Topic> ReadManyOptimized(ISpecification<Topic> spec)
        {
            return ReadOptimized(spec);
        }

        private IQueryable<Topic> ReadOptimized(ISpecification<Topic> spec)
        {
            return set.Include((Topic t) => t.Forum).Include((Topic t) => t.LastPost).Include((Topic t) => t.LastPostAuthor)
                .Include((Topic t) => t.Author)
                .Where(spec.IsSatisfied);
        }

        public Topic ReadOneOptimized(ISpecification<Topic> spec)
        {
            return ReadOptimized(spec).FirstOrDefault();
        }

        public Topic ReadOneOptimizedWithPosts(int id)
        {
            return ReadOptimized(new TopicSpecifications.ById(id)).Include((Topic t) => t.Posts).FirstOrDefault();
        }

        public IEnumerable<Topic> ReadAllOptimized()
        {
            return set.Include((Topic t) => t.Forum).Include((Topic t) => t.LastPost).Include((Topic t) => t.LastPostAuthor)
                .Include((Topic t) => t.Author);
        }

        public Topic ReadOneOptimizedWithPosts(ISpecification<Topic> spec)
        {
            return ReadOptimized(spec).Include((Topic t) => t.Posts).FirstOrDefault();
        }
    }
}
