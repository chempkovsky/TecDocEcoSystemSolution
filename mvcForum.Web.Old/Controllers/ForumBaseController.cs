// mvcForum.Web.Controllers.ForumBaseController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Web.Areas.Forum.Controllers;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using System.Web.Mvc;

namespace mvcForum.Web.Controllers
{

    public abstract class ForumBaseController : BaseController
    {
        private readonly IRepository<ForumUser> userRepository;

        protected readonly IContext context;

        private ForumUser user;

        protected new ForumUser ActiveUser => userProvider.ActiveUser;

        protected IContext Context => context;

        protected IRepository<ForumUser> ForumUserRepository => userRepository;

        protected ForumBaseController()
            : this(null, null)
        {
        }

        protected ForumBaseController(IWebUserProvider userProvider, IContext context)
            : base(userProvider)
        {
            this.context = ((context == null) ? DependencyResolver.Current.GetService<IContext>() : context);
            userRepository = this.context.GetRepository<ForumUser>();
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && userProvider.Authenticated)
            {
                ForumUser activeUser = userProvider.ActiveUser;
                if (activeUser != null && activeUser.ExternalAccount && filterContext.Controller.GetType() != typeof(ProfileController))
                {
                    activeUser.EmailAddress.EndsWith("repl@ce.this");
                }
            }
        }

        protected IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return Context.GetRepository<TEntity>();
        }
    }

}