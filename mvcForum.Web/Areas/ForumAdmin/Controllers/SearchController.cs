// mvcForum.Web.Areas.ForumAdmin.Controllers.SearchController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Search;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using mvcForum.Web.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class SearchController : ForumAdminBaseController
    {
        private readonly IEnumerable<ISearchConfigurationController> controllers;

        private readonly IEnumerable<IIndexer> indexers;

        private readonly IConfiguration config;

        public SearchController(IWebUserProvider userProvider, IContext context, IEnumerable<ISearchConfigurationController> controllers, IEnumerable<IIndexer> indexers, IConfiguration config)
            : base(userProvider, context)
        {
            this.controllers = controllers;
            this.indexers = indexers;
            this.config = config;
        }

        public ActionResult Index()
        {
            return View(controllers);
        }

        [HttpPost]
        public ActionResult Index(bool index)
        {
            if (index)
            {
                foreach (IIndexer indexer in indexers)
                {
                    indexer.Clear();
                }
                IRepository<mvcForum.Core.Forum> repository = GetRepository<mvcForum.Core.Forum>();
                foreach (mvcForum.Core.Forum item in repository.ReadAll().ToList())
                {
                    foreach (Topic item2 in item.Topics.Where(delegate (Topic t)
                    {
                        if (t.Flag != TopicFlag.Quarantined)
                        {
                            return t.Flag != TopicFlag.Deleted;
                        }
                        return false;
                    }).ToList())
                    {
                        foreach (IIndexer indexer2 in indexers)
                        {
                            indexer2.Index(item2);
                            indexer2.BulkIndex(from p in item2.Posts.Visible(config)
                                               where p.Position > 0
                                               select p);
                        }
                    }
                }
            }
            return View(controllers);
        }
    }

}