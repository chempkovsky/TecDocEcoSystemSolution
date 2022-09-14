// mvcForum.Web.Interfaces.IContentParser
using mvcForum.Core.Interfaces.AddOns;
using System.Web.Mvc;

namespace mvcForum.Web.Interfaces
{

    public interface IContentParser : IAddOn
    {
        string Name
        {
            get;
        }

        MvcHtmlString Parse(string content);

        string Quote(string author, string content);
    }

}