// mvcForum.DataProvider.EntityFramework.Repositories.PostRepository
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.DataProvider.EntityFramework.Repositories
{

    public class PostRepository : IPostRepository
    {
        private readonly IRepository<Post> baseRepo;

        private readonly IForumAccessService accessService;

        private readonly IRepository<TopicTrack> ttRepo;

        public PostRepository(IRepository<Post> baseRepo, IForumAccessService accessService, IRepository<TopicTrack> ttRepo)
        {
            this.baseRepo = baseRepo;
            this.accessService = accessService;
            this.ttRepo = ttRepo;
        }

        public IEnumerable<Post> ReadAll(ForumUser user, Topic topic, bool showDeleted)
        {
            if (topic == null)
            {
                throw new ArgumentNullException("topic");
            }
            if (accessService.HasAccess(topic.Forum, user, AccessFlag.Read))
            {
                return baseRepo.ReadMany(new PostSpecifications.RegularListing(topic, showDeleted));
            }
            return new Post[0];
        }

        public IEnumerable<Post> Read(ForumUser user, Topic topic, int pageIndex, int postsPerPage, bool showDeleted)
        {
            if (topic == null)
            {
                throw new ArgumentNullException("topic");
            }
            if (pageIndex < 1)
            {
                throw new ArgumentException("page");
            }
            if (accessService.HasAccess(topic.Forum, user, AccessFlag.Read))
            {
                return baseRepo.ReadMany(new PostSpecifications.RegularListing(topic, showDeleted)).Skip((pageIndex - 1) * postsPerPage).Take(postsPerPage)
                    .ToList();
            }
            return new Post[0];
        }

        public IEnumerable<Post> ReadSinceLast(ForumUser user, Topic topic, int postsPerPage, bool showDeleted, out DateTime? lastRead, out int showingPage)
        {
            lastRead = DateTime.MinValue;
            showingPage = 1;
            if (topic == null)
            {
                throw new ArgumentNullException("topic");
            }
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (accessService.HasAccess(topic.Forum, user, AccessFlag.Read))
            {
                TopicTrack topicTrack = ttRepo.ReadOne(new TopicTrackSpecifications.SpecificTopicAndUser(topic, user));
                int num = 0;
                if (topicTrack != null)
                {
                    DateTime topicLastViewed = topicTrack.LastViewed.ToUniversalTime();
                    lastRead = topicLastViewed;
                    int num2 = (from p in baseRepo.ReadMany(new PostSpecifications.RegularListing(topic, showDeleted))
                                where p.Posted <= topicLastViewed
                                select p).Count() - 1;
                    if (num2 > 0)
                    {
                        num = (int)Math.Floor((decimal)(++num2) / (decimal)postsPerPage);
                        showingPage = num + 1;
                    }
                }
                return Read(user, topic, num + 1, postsPerPage, showDeleted);
            }
            return new Post[0];
        }
    }
}
