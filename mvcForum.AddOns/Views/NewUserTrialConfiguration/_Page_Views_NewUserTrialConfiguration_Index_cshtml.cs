// mvcForum.AddOns.Views.NewUserTrialConfiguration._Page_Views_NewUserTrialConfiguration_Index_cshtml
using MVCBootstrap.Web.Mvc.Extensions;
using mvcForum.AddOns.ViewModels;
using mvcForum.Web.Helpers;
using System.CodeDom.Compiler;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace mvcForum.AddOns.Views.NewUserTrialConfiguration
{

    [PageVirtualPath("~/Views/NewUserTrialConfiguration/Index.cshtml")]
    [GeneratedCode("MvcRazorClassGenerator", "1.0")]
    public class _Page_Views_NewUserTrialConfiguration_Index_cshtml : WebViewPage<NewUserTrialViewModel>
    {
        protected HttpApplication ApplicationInstance => Context.ApplicationInstance;

        public override void Execute()
        {
            string @namespace = "mvcForum.AddOns.NewUserTrial";
            base.ViewBag.Title = base.Html.LocalizedString("PageTitle", @namespace);
            Layout = "~/Areas/ForumAdmin/Views/Shared/adminlayout.cshtml";
            WriteLiteral("<section id=\"newusertrial\">\r\n\t<div class=\"row-fluid\">\r\n\t\t<div class=\"span12\">\r\n");
            if (base.TempData.ContainsKey("Saved") && (bool)base.TempData["Saved"])
            {
                WriteLiteral("\t\t\t<div class=\"alert alert-success\">\r\n\t\t\t\t<h4 class=\"alert-heading\">");
                Write(base.Html.LocalizedString("SuccessHeading", @namespace));
                WriteLiteral("</h4>\r\n\t\t\t\t");
                Write(base.Html.LocalizedString("SuccessBody", @namespace));
                WriteLiteral("\r\n\t\t\t</div>\r\n");
            }
            using (base.Html.BeginForm("index", "newusertrialconfiguration", new
            {
                area = "forumadmin"
            }, FormMethod.Post, new
            {
                @class = "form-horizontal"
            }))
            {
                WriteLiteral("\t\t\t\t<fieldset>\r\n\t\t\t\t\t<legend>");
                Write(base.Html.LocalizedString("Heading", @namespace));
                WriteLiteral("</legend>\r\n\t\t\t\t\t");
                Write(base.Html.ValidationSummary());
                WriteLiteral("\r\n\t\t\t\t\t<div class=\"control-group\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (NewUserTrialViewModel x) => x.Enabled, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.CheckBoxFor((NewUserTrialViewModel x) => x.Enabled, new
                {
                    data_bind = "checked: Enabled"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group hide\" data-bind=\"css: { 'hide': !Enabled() }\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (NewUserTrialViewModel x) => x.RunAsynchronously, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.CheckBoxFor((NewUserTrialViewModel x) => x.RunAsynchronously, new
                {
                    data_bind = "checked: RunAsynchronously"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group hide\" data-bind=\"css: { 'hide': !Enabled() || !RunAsynchronously() }\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (NewUserTrialViewModel x) => x.Delay, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.TextBoxFor((NewUserTrialViewModel x) => x.Delay, new
                {
                    @class = "input-small"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group hide\" data-bind=\"css: { 'hide': !Enabled() }\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (NewUserTrialViewModel x) => x.AutoLimit, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.TextBoxFor((NewUserTrialViewModel x) => x.AutoLimit, new
                {
                    @class = "input-small"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"control-group hide\" data-bind=\"css: { 'hide': !Enabled() }\">\r\n\t\t\t\t\t\t");
                Write(MVCBootstrap.Web.Mvc.Extensions.LabelExtensions.LabelFor(base.Html, (NewUserTrialViewModel x) => x.ExcludeGroups, new
                {
                    @class = "control-label"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t<div class=\"controls\">\r\n\t\t\t\t\t\t\t");
                Write(base.Html.ListBoxFor((NewUserTrialViewModel x) => x.ExcludeGroups, ForumHelper.GetGroups(), new
                {
                    @class = "input-medium",
                    multiple = "multiple"
                }));
                WriteLiteral("\r\n\t\t\t\t\t\t\t");
                WriteLiteral("\r\n\t\t\t\t\t\t</div>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"form-actions\">\r\n\t\t\t\t\t\t<button type=\"submit\" class=\"btn btn-primary\">");
                Write(base.Html.LocalizedString("Button", @namespace));
                WriteLiteral("</button>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t</fieldset>\r\n");
            }
            WriteLiteral("\t\t</div>\r\n\t</div>\r\n</section>\r\n<script type=\"text/javascript\">\r\n\tvar runAsync = ");
            Write(Json.Encode(base.Model.RunAsynchronously));
            WriteLiteral(";\r\n\tvar enabled = ");
            Write(Json.Encode(base.Model.Enabled));
            WriteLiteral(";\r\n</script>\r\n\r\n");
            DefineSection("scripts", delegate
            {
                WriteLiteral("\r\n\t<script type=\"text/javascript\" src=\"");
                Write(base.Url.Content("~/scripts/knockout.validation.js"));
                WriteLiteral("\"></script>\r\n\t<script type=\"text/javascript\" src=\"");
                Write(base.Url.Content("~/scripts/boostrap-collapse.js"));
                WriteLiteral("\"></script>\r\n\t<script type=\"text/javascript\" src=\"");
                Write(base.Url.Content("~/scripts/common.js"));
                WriteLiteral("\"></script>\r\n\t<script type=\"text/javascript\" src=\"");
                Write(base.Url.Content("~/scripts/viewmodels/newusertrial.js"));
                WriteLiteral("\"></script>\r\n");
            });
        }
    }
}
