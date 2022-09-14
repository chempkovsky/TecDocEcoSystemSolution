// BoC.Web.Mvc.PrecompiledViews.DictionaryBasedApplicationPartRegistry
using BoC.Web.Mvc.PrecompiledViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.WebPages;

namespace BoC.Web.Mvc.PrecompiledViews
{

    public class DictionaryBasedApplicationPartRegistry : IApplicationPartRegistry
    {
        private static readonly Type webPageType = typeof(WebPageRenderingBase);

        private readonly Dictionary<string, Type> registeredPaths = new Dictionary<string, Type>();

        public virtual Type GetCompiledType(string virtualPath)
        {
            if (virtualPath == null)
            {
                throw new ArgumentNullException("virtualPath");
            }
            if (virtualPath.StartsWith("/"))
            {
                virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
            }
            if (!virtualPath.StartsWith("~"))
            {
                virtualPath = ((!virtualPath.StartsWith("/")) ? ("~/" + virtualPath) : ("~" + virtualPath));
            }
            virtualPath = virtualPath.ToLower();
            if (!registeredPaths.ContainsKey(virtualPath))
            {
                return null;
            }
            return registeredPaths[virtualPath];
        }

        public void Register(Assembly applicationPart)
        {
            ((IApplicationPartRegistry)this).Register(applicationPart, (string)null);
        }

        public virtual void Register(Assembly applicationPart, string rootVirtualPath)
        {
            foreach (Type item in from type in applicationPart.GetTypes()
                                  where type.IsSubclassOf(webPageType)
                                  select type)
            {
                ((IApplicationPartRegistry)this).RegisterWebPage(item, rootVirtualPath);
            }
        }

        public void RegisterWebPage(Type type)
        {
            ((IApplicationPartRegistry)this).RegisterWebPage(type, string.Empty);
        }

        public virtual void RegisterWebPage(Type type, string rootVirtualPath)
        {
            PageVirtualPathAttribute pageVirtualPathAttribute = type.GetCustomAttributes(typeof(PageVirtualPathAttribute), inherit: false).Cast<PageVirtualPathAttribute>().SingleOrDefault();
            if (pageVirtualPathAttribute != null)
            {
                string rootRelativeVirtualPath = GetRootRelativeVirtualPath(rootVirtualPath ?? "", pageVirtualPathAttribute.VirtualPath);
                registeredPaths[rootRelativeVirtualPath.ToLower()] = type;
            }
        }

        private static string GetRootRelativeVirtualPath(string rootVirtualPath, string pageVirtualPath)
        {
            string text = pageVirtualPath;
            if (text.StartsWith("~/", StringComparison.Ordinal))
            {
                text = text.Substring(2);
            }
            if (!rootVirtualPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                rootVirtualPath += "/";
            }
            text = VirtualPathUtility.Combine(rootVirtualPath, text);
            if (!text.StartsWith("~"))
            {
                if (text.StartsWith("/"))
                {
                    return "~" + text;
                }
                return "~/" + text;
            }
            return text;
        }
    }

}
