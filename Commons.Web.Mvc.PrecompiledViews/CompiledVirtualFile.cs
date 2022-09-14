// BoC.Web.Mvc.PrecompiledViews.CompiledVirtualFile
using System;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace BoC.Web.Mvc.PrecompiledViews
{

    public class CompiledVirtualFile : VirtualFile
    {
        public Type CompiledType
        {
            get;
            set;
        }

        public CompiledVirtualFile(string virtualPath, Type compiledType)
            : base(virtualPath)
        {
            CompiledType = compiledType;
        }

        public override Stream Open()
        {
            return new MemoryStream(Encoding.ASCII.GetBytes("@inherits " + CompiledType.AssemblyQualifiedName + "\n@{base.Execute();}"));
        }
    }

}
