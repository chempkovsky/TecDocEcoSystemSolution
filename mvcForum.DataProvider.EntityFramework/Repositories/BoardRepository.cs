// mvcForum.DataProvider.EntityFramework.Repositories.BoardRepository
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces.Data;
using mvcForum.DataProvider.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace mvcForum.DataProvider.EntityFramework.Repositories
{

    public class BoardRepository : Repository<Board>, IBoardRepository, IRepository<Board>
    {
        public BoardRepository(DbContext context)
            : base(context)
        {
        }

        public Board ReadOneOptimized(ISpecification<Board> spec)
        {
            return ReadManyOptimized(spec).FirstOrDefault();
        }

        public IEnumerable<Board> ReadManyOptimized(ISpecification<Board> spec)
        {
            return set.Include((Board b) => b.Categories).Where(spec.IsSatisfied);
        }
    }
}
