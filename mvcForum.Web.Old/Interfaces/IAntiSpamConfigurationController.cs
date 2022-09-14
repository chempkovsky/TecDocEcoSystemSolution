// mvcForum.Web.Interfaces.IAntiSpamConfigurationController
using System.Web.Mvc;

namespace mvcForum.Web.Interfaces
{

    public interface IAntiSpamConfigurationController
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