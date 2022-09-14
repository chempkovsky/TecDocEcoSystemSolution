// mvcForum.Web.Interfaces.IUserService
using mvcForum.Core;
using System.Collections.Generic;

namespace mvcForum.Web.Interfaces
{

    public interface IUserService
    {
        IEnumerable<ForumUser> GetOnlineUsers();
    }

}