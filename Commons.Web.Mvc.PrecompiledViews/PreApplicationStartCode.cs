// BoC.Web.Mvc.PrecompiledViews.PreApplicationStartCode
using BoC.Web.Mvc.PrecompiledViews;
using System.Web.Hosting;

namespace BoC.Web.Mvc.PrecompiledViews
{

    public static class PreApplicationStartCode
    {
        private static bool _startWasCalled;

        public static void Start()
        {
            if (!_startWasCalled)
            {
                _startWasCalled = true;
                HostingEnvironment.RegisterVirtualPathProvider(new CompiledVirtualPathProvider());
            }
        }
    }

}
