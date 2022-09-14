// mvcForum.Web.Areas.Forum.Controllers.FileController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Web.Areas.Forum.Controllers;
using mvcForum.Web.Attributes;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    public class FileController : ThemedForumBaseController
    {
        private readonly IAttachmentService attachmentService;

        private readonly IConfiguration config;

        public FileController(IWebUserProvider userProvider, IContext context, IAttachmentService attachmentService, IConfiguration config)
            : base(userProvider, context)
        {
            this.attachmentService = attachmentService;
            this.config = config;
        }

        [Authorize]
        public ActionResult Attach(int id)
        {
            Post post = GetRepository<Post>().Read(id);
            if (post.Topic.Forum.HasAccess(AccessFlag.Upload) && base.ActiveUser.Id == post.Author.Id)
            {
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(post.Topic, path, base.Url);
                return View(new MessageViewModel(post)
                {
                    Path = path,
                    Topic = new TopicViewModel(post.Topic, new MessageViewModel[0], 0, 1, showDeleted: false)
                });
            }
            base.TempData.Add("Reason", ForumHelper.GetString("NoAccess.NoUpload"));
            return RedirectToRoute("NoAccess");
        }

        [HttpPost]
        [Authorize]
        [NotBanned]
        public ActionResult Attach(int id, HttpPostedFileBase[] files)
        {
            Post post = GetRepository<Post>().Read(id);
            if (post.Topic.Forum.HasAccess(AccessFlag.Upload) && base.ActiveUser.Id == post.Author.Id)
            {
                List<string> list = new List<string>();
                foreach (HttpPostedFileBase httpPostedFileBase in files)
                {
                    if (httpPostedFileBase != null)
                    {
                        AttachStatusCode attachStatusCode = attachmentService.AttachFile(base.ActiveUser, post, httpPostedFileBase.FileName, httpPostedFileBase.ContentType, httpPostedFileBase.ContentLength, httpPostedFileBase.InputStream);
                        if (attachStatusCode != 0)
                        {
                            list.Add(ForumHelper.GetString(attachStatusCode.ToString(), new
                            {
                                File = httpPostedFileBase.FileName,
                                Size = httpPostedFileBase.ContentLength,
                                MaxFileSize = config.MaxFileSize,
                                MaxAttachmentsSize = config.MaxAttachmentsSize,
                                Extensions = config.AllowedExtensions
                            }, "mvcForum.Web.AttachmentErrors"));
                        }
                    }
                }
                if (list.Any())
                {
                    base.TempData.Add("Feedback", from f in list
                                                  select new MvcHtmlString(f));
                }
                return new RedirectToRouteResult("ShowTopic", new RouteValueDictionary
            {
                {
                    "id",
                    post.Topic.Id
                },
                {
                    "title",
                    post.Topic.Title.ToSlug()
                }
            });
            }
            base.TempData.Add("Reason", ForumHelper.GetString("NoAccess.NoUpload"));
            return RedirectToRoute("NoAccess");
        }
    }

}