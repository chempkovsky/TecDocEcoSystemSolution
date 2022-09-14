// mvcForum.DataProvider.EntityFramework.SpecificRepositoryConfiguration
using ApplicationBoilerplate.DependencyInjection;
using mvcForum.Core.Interfaces.Data;
using mvcForum.DataProvider.EntityFramework.Repositories;
using System.Collections.Generic;

namespace mvcForum.DataProvider.EntityFramework
{

    public class SpecificRepositoryConfiguration : IDependencyBuilder
    {
        public virtual void Configure(IDependencyContainer container)
        {
            container.Register<ITopicRepository, TopicRepository>();
            container.Register<IPostRepository, PostRepository>();
            container.Register<IForumRepository, ForumRepository>();
            container.Register<IBoardRepository, BoardRepository>();
        }

        public void ValidateRequirements(IList<ApplicationRequirement> feedback)
        {
        }
    }

}
