// BoC.Web.Mvc.PrecompiledViews.CompiledVirtualPathProvider
using BoC.Web.Mvc.PrecompiledViews;
using System;
using System.Collections;
using System.Linq;
using System.Web.Caching;
using System.Web.Hosting;

namespace BoC.Web.Mvc.PrecompiledViews
{

    public class CompiledVirtualPathProvider : VirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            if (!(GetCompiledType(virtualPath) != null))
            {
                return base.Previous.FileExists(virtualPath);
            }
            return true;
        }

        private Type GetCompiledType(string virtualPath)
        {
            return ApplicationPartRegistry.Instance.GetCompiledType(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (base.Previous.FileExists(virtualPath))
            {
                return base.Previous.GetFile(virtualPath);
            }
            Type compiledType = GetCompiledType(virtualPath);
            if (compiledType != null)
            {
                return new CompiledVirtualFile(virtualPath, compiledType);
            }
            return null;
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (virtualPathDependencies == null)
            {
                return base.Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }
            return base.Previous.GetCacheDependency(virtualPath, from string vp in virtualPathDependencies
                                                                 where GetCompiledType(vp) == null
                                                                 select vp, utcStart);
        }
    }

}
