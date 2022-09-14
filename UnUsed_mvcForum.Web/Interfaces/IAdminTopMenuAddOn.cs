// mvcForum.Web.Interfaces.IAdminTopMenuAddOn<TController>
using mvcForum.Core.Interfaces.AddOns;
using System.Web.Mvc;

namespace mvcForum.Web.Interfaces
{

    public interface IAdminTopMenuAddOn<TController> : IAddOn where TController : Controller
    {
    }

}