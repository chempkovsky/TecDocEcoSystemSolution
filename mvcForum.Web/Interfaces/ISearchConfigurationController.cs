// mvcForum.Web.Interfaces.ISearchConfigurationController
using System.Web.Mvc;

namespace mvcForum.Web.Interfaces
{

    public interface ISearchConfigurationController
    {
        string Name
        {
            get;
        }

        string Description
        {
            get;
        }

        ViewResult Index();
    }

}