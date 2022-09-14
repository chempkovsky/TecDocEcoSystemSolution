﻿// BoC.Web.Mvc.PrecompiledViews.ApplicationPartRegistry
using BoC.Web.Mvc.PrecompiledViews;
using System;
using System.Reflection;

namespace BoC.Web.Mvc.PrecompiledViews
{

    public static class ApplicationPartRegistry
    {
        public static IApplicationPartRegistry Instance
        {
            get;
            set;
        }

        static ApplicationPartRegistry()
        {
            Instance = new DictionaryBasedApplicationPartRegistry();
        }

        public static Type GetCompiledType(string virtualPath)
        {
            return Instance.GetCompiledType(virtualPath);
        }

        public static void Register(Assembly applicationPart)
        {
            Register(applicationPart, null);
        }

        public static void Register(Assembly applicationPart, string rootVirtualPath)
        {
            Instance.Register(applicationPart, rootVirtualPath);
        }

        public static void RegisterWebPage(Type type)
        {
            RegisterWebPage(type, null);
        }

        public static void RegisterWebPage(Type type, string rootVirtualPath)
        {
            Instance.RegisterWebPage(type, rootVirtualPath);
        }
    }

}
