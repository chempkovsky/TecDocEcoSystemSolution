// mvcForum.Web.Controllers.BaseAPIController
using mvcForum.Web.Controllers;
using System;
using System.Web.Mvc;

namespace mvcForum.Web.Controllers
{

    public abstract class BaseAPIController : ForumBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.ActionParameters.ContainsKey("mode") && (string)filterContext.ActionParameters["mode"] == "json")
            {
                return;
            }
            throw new NotSupportedException("Only JSON is supported");
        }
    }

}