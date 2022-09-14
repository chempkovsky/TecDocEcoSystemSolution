// mvcForum.DataProvider.EntityFramework.Repositories.ForumRepository
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces.Data;
using mvcForum.DataProvider.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace mvcForum.DataProvider.EntityFramework.Repositories
{

    public class ForumRepository : Repository<Forum>, IForumRepository, IRepository<Forum>
    {
        public ForumRepository(DbContext context)
            : base(context)
        {
        }

        public IEnumerable<Forum> ReadManyOptimized(ISpecification<Forum> spec)
        {
            return set.Include((Forum f) => f.SubForums).Include((Forum f) => f.LastTopic).Include((Forum f) => f.LastPost)
                .Include((Forum f) => f.LastPostUser)
                .Where(spec.IsSatisfied);
        }

        public Forum ReadOneOptimized(ISpecification<Forum> spec)
        {
            return ReadManyOptimized(spec).FirstOrDefault();
        }

        public override void Delete(int id)
        {
            Forum forum = new Forum();
            forum.Id = id;
            Forum entity = forum;
            set.Attach(entity);
            Delete(entity);
        }
    }
}
