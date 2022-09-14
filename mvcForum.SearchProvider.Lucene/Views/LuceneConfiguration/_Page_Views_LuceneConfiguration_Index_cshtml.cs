// mvcForum.SearchProvider.Lucene.Views.LuceneConfiguration._Page_Views_LuceneConfiguration_Index_cshtml
using MVCBootstrap.Web.Mvc.Extensions;
using mvcForum.SearchProvider.Lucene.ViewModels;
using System.CodeDom.Compiler;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace mvcForum.SearchProvider.Lucene.Views.LuceneConfiguration
{

    [PageVirtualPath("~/Views/LuceneConfiguration/Index.cshtml")]
    [GeneratedCode("MvcRazorClassGenerator", "1.0")]
    public class _Page_Views_LuceneConfiguration_Index_cshtml : WebViewPage<LuceneViewModel>
    {
        protected HttpApplication ApplicationInstance => Context.ApplicationInstance;

        public override void Execute()
        {
            string @namespace = "mvcForum.SearchProvider.Lucene";
            base.ViewBag.Title = base.Html.LocalizedHtmlString("PageTitle", @namespace);
            Layout = "~/Areas/ForumAdmin/Views/Shared/adminlayout.cshtml";
            WriteLiteral("<section id=\"lucene\">\r\n\t<div class=\"row-fluid\">\r\n\t\t<div class=\"span12\">\r\n");
            if (base.TempData.ContainsKey("Saved") && (bool)base.TempData["Saved"])
            {
                WriteLiteral("\t\t\t<div class=\"alert alert-success\">\r\n\t\t\t\t<h4 class=\"alert-heading\">");
                Write(base.Html.LocalizedString("SuccessHeading", @namespace));
                WriteLiteral("</h4>\r\n\t\t\t\t");
                Write(base.Html.LocalizedString("SuccessBody", @namespace));
                WriteLiteral("\r\n\t\t\t</div>\r\n");
            }
            using (base.Html.BeginForm("index", "luceneconfiguration", new
            {
                area = "forumadmin"
            }, FormMethod.Post, new
            {
                @class = "form-horizontal"
            }))
            {
                WriteLiteral("\t\t\t\t<fieldset>\r\n\t\t\t\t\t<legend>");
                Write(base.Html.LocalizedString("UpdateForm", @namespace));
                WriteLiteral("</legend>\r\n\t\t\t\t\t");
                Write(base.Html.ValidationSummary());
                WriteLiteral("\r\n\t\t\t\t\t<div class=\"control-group\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (LuceneViewModel x) => x.Enabled, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.CheckBoxFor((LuceneViewModel x) => x.Enabled));
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (LuceneViewModel x) => x.RunAsynchronously, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.CheckBoxFor((LuceneViewModel x) => x.RunAsynchronously));
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (LuceneViewModel x) => x.Delay, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.TextBoxFor((LuceneViewModel x) => x.Delay, new
                {
                    @class = "input-small"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (LuceneViewModel x) => x.TitleWeight, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.TextBoxFor((LuceneViewModel x) => x.TitleWeight, new
                {
                    @class = "input-small"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (LuceneViewModel x) => x.TopicWeight, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.TextBoxFor((LuceneViewModel x) => x.TopicWeight, new
                {
                    @class = "input-small"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (LuceneViewModel x) => x.StickyWeight, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.TextBoxFor((LuceneViewModel x) => x.StickyWeight, new
                {
                    @class = "input-small"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (LuceneViewModel x) => x.AnnouncementWeight, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.TextBoxFor((LuceneViewModel x) => x.AnnouncementWeight, new
                {
                    @class = "input-small"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"form-actions\">\r\n\t\t\t\t\t\t<button type=\"submit\" class=\"btn btn-primary\">");
                Write(base.Html.LocalizedString("Button", @namespace));
                WriteLiteral("</button>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t</fieldset>\r\n");
            }
            WriteLiteral("\t\t</div>\r\n\t</div>\r\n</section>\r\n");
        }
    }

}
