// mvcForum.Web.Areas.Forum.Controllers.SearchController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Interfaces.Search;
using mvcForum.Core.Search;
using mvcForum.Core.Specifications;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    public class SearchController : ThemedForumBaseController
    {
        private readonly IConfiguration config;

        private readonly IEnumerable<ISearcher> searchers;

        private readonly ITopicRepository topicRepo;

        public SearchController(IWebUserProvider userProvider, IContext context, IConfiguration config, IEnumerable<ISearcher> searchers, ITopicRepository topicRepo)
            : base(userProvider, context)
        {
            this.config = config;
            this.searchers = searchers;
            this.topicRepo = topicRepo;
        }

        [ActionName("Active Topics")]
        public ActionResult ActiveTopics()
        {
            return View();
        }

        [ActionName("Unanswered Posts")]
        public ActionResult UnansweredPosts()
        {
            return RedirectToAction("Unanswered Topics");
        }

        [ActionName("Unanswered Topics")]
        public ActionResult UnansweredTopics([DefaultValue(1)] int page)
        {
            List<int> accessibleForums = (from x in ForumHelper.GetAccessibleForums()
                                          select x.Id).ToList();
            IEnumerable<Topic> source = (from t in GetRepository<Topic>().ReadMany(new TopicSpecifications.EmptyTopic())
                                         where accessibleForums.Contains(t.ForumId)
                                         select t into x
                                         orderby x.Posted descending
                                         select x).Skip((page - 1) * config.TopicsPerPage).Take(config.TopicsPerPage).ToList();
            UnansweredTopicsViewModel unansweredTopicsViewModel = new UnansweredTopicsViewModel();
            unansweredTopicsViewModel.Path = new Dictionary<string, string>();
            unansweredTopicsViewModel.Path.Add("/forum/search/unanswered topics", ForumHelper.GetString("SearchUnanswered.Breadcrumb", null, "mvcForum.Web"));
            unansweredTopicsViewModel.Topics = from t in source
                                               select new TopicViewModel(t, new MessageViewModel[0], 0, 1, showDeleted: false);
            return View(unansweredTopicsViewModel);
        }

        public ActionResult Index()
        {
            return View(new SearchViewModel
            {
                Forums = new int[0]
            });
        }

        [HttpPost]
        public ActionResult Index(SearchViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                IList<int> forums = (model.Forums != null) ? model.Forums.ToList() : (from x in ForumHelper.GetAccessibleForums()
                                                                                      select x.Id).ToList();
                IEnumerable<SearchResult> enumerable = new List<SearchResult>();
                foreach (ISearcher searcher in searchers)
                {
                    enumerable = enumerable.Concat(searcher.Search(model.Query, forums));
                }
                model.Results = new List<TopicViewModel>();
                if (enumerable.Any())
                {
                    GetRepository<Post>();
                    try
                    {
                        List<TopicViewModel> list = new List<TopicViewModel>();
                        using (IEnumerator<SearchResult> enumerator2 = (from r in enumerable
                                                                        orderby r.Score descending
                                                                        select r).GetEnumerator())
                        {
                            SearchResult result;
                            while (enumerator2.MoveNext())
                            {
                                result = enumerator2.Current;
                                if (!(from vm in list
                                      where vm.Id == result.TopicId
                                      select vm).Any())
                                {
                                    Topic topic = topicRepo.ReadOneOptimizedWithPosts(result.TopicId);
                                    list.Add(new TopicViewModel(topic, new MessageViewModel[0], topic.Posts.Visible(config).Count() - 1, config.MessagesPerPage, showDeleted: false));
                                }
                            }
                        }
                        model.Results = list;
                    }
                    catch
                    {
                    }
                }
            }
            if (model.Forums == null)
            {
                model.Forums = new int[0];
            }
            return View(model);
        }
    }

}