// mvcForum.SearchProvider.Lucene.FileSystemDirectoryResolver
using Lucene.Net.Store;
using mvcForum.SearchProvider.Lucene.Indexes;

namespace mvcForum.SearchProvider.Lucene
{

    public class FileSystemDirectoryResolver : IDirectoryResolver
    {
        private readonly string root;

        public FileSystemDirectoryResolver(string root)
        {
            this.root = root;
        }

        public Directory GetDirectory()
        {
            return FSDirectory.Open(new System.IO.DirectoryInfo(System.IO.Path.Combine(root, "Indexes")));
        }
    }

}
