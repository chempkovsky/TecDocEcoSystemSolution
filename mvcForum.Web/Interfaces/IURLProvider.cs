// mvcForum.Web.Interfaces.IURLProvider

namespace mvcForum.Web.Interfaces
{
    public interface IURLProvider
    {
        string RouteUrl(string routeName, object routeValues);
    }

}