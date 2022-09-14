// mvcForum.Web.Interfaces.IWebUserProvider
using mvcForum.Core;

namespace mvcForum.Web.Interfaces
{

    public interface IWebUserProvider
    {
        ForumUser ActiveUser
        {
            get;
        }

        bool Authenticated
        {
            get;
        }
    }

}