// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Helpers.ForumHelper
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Specifications;
using mvcForum.Web.Extensions;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using SimpleLocalisation;
using SimpleLocalisation.Processing.ParameterValues;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Helpers
{
  public static class ForumHelper
  {
    private static bool debug = false;
    private static bool debugRead = false;
    private static Version version = (Version) null;
    private static object versionLock = new object();

    public static string GetLinkText(string key)
    {
      return ForumHelper.GetLinkText(key, (object) null);
    }

    public static string GetLinkText(string key, object values)
    {
      return ForumHelper.GetLinkText(key, values, "mvcForum.Web");
    }

    public static string GetLinkText(string key, object values, string ns)
    {
      string str = ForumHelper.GetString(key, values, ns);
      if (!string.IsNullOrEmpty(str))
        return str;
      return string.Format("##Missing text, key: {0}, namespace: {1}", (object) key, (object) ns);
    }

    public static string GetLinkText<T>(string key)
    {
      return ForumHelper.GetLinkText(key, (object) null);
    }

    public static string GetLinkText<T>(string key, object values)
    {
      string str = ForumHelper.GetString<T>(key, values);
      if (!string.IsNullOrEmpty(str))
        return str;
      return string.Format("##Missing text, key: {0}", (object) key);
    }

    public static MvcHtmlString GetHtmlString(string key)
    {
      return ForumHelper.GetHtmlString(key, (object) null);
    }

    public static MvcHtmlString GetHtmlString(string key, object values)
    {
      return ForumHelper.GetHtmlString<ForumConfigurator>(key, values);
    }

    public static MvcHtmlString GetHtmlString(
      string key,
      string @namespace,
      object values)
    {
      return new MvcHtmlString(ForumHelper.GetString(key, values, @namespace));
    }

    public static string GetString(string key)
    {
      return ForumHelper.GetString(key, (object[]) null);
    }

    public static string GetString(string key, object[] values)
    {
      return ForumHelper.GetString<ForumConfigurator>(key, (object) values);
    }

    public static string GetString(string key, object values)
    {
      return ForumHelper.GetString<ForumConfigurator>(key, values);
    }

    public static string GetString(string key, object values, string namespc)
    {
      TextManager service = DependencyResolver.Current.GetService<TextManager>();
      object obj = values;
      string str = namespc;
      string key1 = key;
      object values1 = obj;
      string ns = str;
      return service.Get(key1, values1, ns);
    }

    public static string GetString<T>(string key)
    {
      return ForumHelper.GetString<T>(key, (object) null);
    }

    public static string GetString<T>(string key, object values)
    {
      TextManager service = DependencyResolver.Current.GetService<TextManager>();
      object obj = values;
      string key1 = key;
      object values1 = obj;
      return service.Get<T>(key1, values1);
    }

    public static MvcHtmlString GetHtmlString<T>(string text)
    {
      return new MvcHtmlString(ForumHelper.GetString<T>(text));
    }

    public static MvcHtmlString GetHtmlString<T>(string text, object values)
    {
      return new MvcHtmlString(ForumHelper.GetString<T>(text, values));
    }

    public static FormatWrapper<T> Wrap<T>(this T value, string formatExpression)
    {
      return new FormatWrapper<T>(value, formatExpression);
    }

    public static MvcHtmlString AttachmentLink(
      this HtmlHelper html,
      AttachmentViewModel attachment)
    {
      TagBuilder tagBuilder = new TagBuilder("a");
      string str = string.Format("{0}{1}{2}", (object) attachment.Path, attachment.Path.EndsWith("/") ? (object) "" : (object) "/", (object) attachment.Filename);
      tagBuilder.Attributes.Add("href", str);
      tagBuilder.SetInnerText(attachment.Filename);
      return MvcHtmlString.Create(tagBuilder.ToString() + string.Format(" ({0} kB)", (object) Math.Ceiling((Decimal) attachment.Size / new Decimal(1024))));
    }

    private static MvcHtmlString ForumLink(
      int forumId,
      string forumName,
      RequestContext request,
      RouteCollection routeCollection,
      RouteValueDictionary htmlAttributes)
    {
      TagBuilder tagBuilder = new TagBuilder("a");
      string url = UrlHelper.GenerateUrl("ShowForum", (string) null, (string) null, (string) null, (string) null, (string) null, new RouteValueDictionary()
      {
        {
          "id",
          (object) forumId
        },
        {
          "title",
          (object) forumName.ToSlug()
        }
      }, routeCollection, request, false);
      tagBuilder.MergeAttributes<string, object>((IDictionary<string, object>) htmlAttributes);
      tagBuilder.MergeAttribute("href", url);
      tagBuilder.SetInnerText(forumName);
      return MvcHtmlString.Create(tagBuilder.ToString());
    }

    public static MvcHtmlString ForumLink(
      this HtmlHelper html,
      Forum forum,
      object htmlAttributes)
    {
      return ForumHelper.ForumLink(forum.Id, forum.Name, html.ViewContext.RequestContext, html.RouteCollection, new RouteValueDictionary(htmlAttributes));
    }

    public static MvcHtmlString ForumLink(
      this HtmlHelper html,
      int id,
      string name,
      object htmlAttributes)
    {
      return ForumHelper.ForumLink(id, name, html.ViewContext.RequestContext, html.RouteCollection, new RouteValueDictionary(htmlAttributes));
    }

    public static bool Authenticated()
    {
      return HttpContext.Current.User.Identity.IsAuthenticated;
    }

    public static MvcHtmlString GetForumMultiSelect(
      string name,
      IList<int> selected,
      IDictionary<string, object> htmlAttributes)
    {
      IRepository<Forum> service = DependencyResolver.Current.GetService<IRepository<Forum>>();
      TagBuilder tagBuilder1 = new TagBuilder("select");
      tagBuilder1.Attributes.Add(nameof (name), name);
      tagBuilder1.Attributes.Add("multiple", "multiple");
      if (htmlAttributes != null)
      {
        foreach (KeyValuePair<string, object> htmlAttribute in (IEnumerable<KeyValuePair<string, object>>) htmlAttributes)
          tagBuilder1.Attributes.Add(htmlAttribute.Key, htmlAttribute.Value.ToString());
      }
      foreach (Forum forum in service.ReadAll().ToList<Forum>())
      {
        if (forum.HasAccess(AccessFlag.Read))
        {
          TagBuilder tagBuilder2 = new TagBuilder("option");
          tagBuilder2.Attributes.Add("value", forum.Id.ToString());
          if (selected.Contains(forum.Id))
            tagBuilder2.Attributes.Add(nameof (selected), nameof (selected));
          tagBuilder2.SetInnerText(forum.Name);
          tagBuilder1.InnerHtml += tagBuilder2.ToString();
        }
      }
      return MvcHtmlString.Create(tagBuilder1.ToString());
    }

    public static IEnumerable<SelectListItem> GetTopicTypes(TopicType type)
    {
      return (IEnumerable<SelectListItem>) new List<SelectListItem>()
      {
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("TopicType." + TopicType.Regular.ToString()),
          Value = TopicType.Regular.ToString(),
          Selected = type == TopicType.Regular
        },
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("TopicType." + TopicType.Announcement.ToString()),
          Value = TopicType.Announcement.ToString(),
          Selected = type == TopicType.Announcement
        },
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("TopicType." + TopicType.Sticky.ToString()),
          Value = TopicType.Sticky.ToString(),
          Selected = type == TopicType.Sticky
        }
      };
    }

    public static IEnumerable<SelectListItem> GetTopicFlags(TopicFlag flag)
    {
      return (IEnumerable<SelectListItem>) new List<SelectListItem>()
      {
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("TopicFlag." + TopicFlag.Deleted.ToString()),
          Value = TopicFlag.Deleted.ToString(),
          Selected = flag == TopicFlag.Deleted
        },
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("TopicFlag." + TopicFlag.Locked.ToString()),
          Value = TopicFlag.Locked.ToString(),
          Selected = flag == TopicFlag.Locked
        },
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("TopicFlag." + TopicFlag.None.ToString()),
          Value = TopicFlag.None.ToString(),
          Selected = flag == TopicFlag.None
        },
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("TopicFlag." + TopicFlag.Quarantined.ToString()),
          Value = TopicFlag.Quarantined.ToString(),
          Selected = flag == TopicFlag.Quarantined
        }
      };
    }

    public static IEnumerable<SelectListItem> GetPostFlags(PostFlag flag)
    {
      return (IEnumerable<SelectListItem>) new List<SelectListItem>()
      {
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("PostFlag." + PostFlag.Deleted.ToString()),
          Value = PostFlag.Deleted.ToString(),
          Selected = flag == PostFlag.Deleted
        },
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("PostFlag." + PostFlag.None.ToString()),
          Value = PostFlag.None.ToString(),
          Selected = flag == PostFlag.None
        },
        new SelectListItem()
        {
          Text = ForumHelper.GetString<ForumConfigurator>("PostFlag." + PostFlag.Quarantined.ToString()),
          Value = PostFlag.Quarantined.ToString(),
          Selected = flag == PostFlag.Quarantined
        }
      };
    }

    public static IEnumerable<SelectListItem> GetContentEditors(
      string selected)
    {
      return DependencyResolver.Current.GetServices<IContentParser>().Select<IContentParser, SelectListItem>((Func<IContentParser, SelectListItem>) (p => new SelectListItem()
      {
        Text = p.Name,
        Value = p.Name,
        Selected = p.Name == selected
      }));
    }

    public static IEnumerable<SelectListItem> GetGroups()
    {
      return ForumHelper.GetGroups(new string[0]);
    }

    public static IEnumerable<SelectListItem> GetGroups(string[] selected)
    {
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      foreach (Group group in (IEnumerable<Group>) DependencyResolver.Current.GetService<IRepository<Group>>().ReadAll().OrderBy<Group, string>((Func<Group, string>) (x => x.Name)))
        selectListItemList.Add(new SelectListItem()
        {
          Text = group.Name,
          Value = group.Id.ToString(),
          Selected = ((IEnumerable<string>) selected).Contains<string>(group.Id.ToString())
        });
      return (IEnumerable<SelectListItem>) selectListItemList;
    }

    public static string GetEditorString()
    {
      string editor = DependencyResolver.Current.GetService<IConfiguration>().Editor;
      if (!string.IsNullOrWhiteSpace(editor))
        return editor;
      return "Regular";
    }

    public static string Quote(string author, string content)
    {
      IConfiguration config = DependencyResolver.Current.GetService<IConfiguration>();
      string str = string.Empty;
      try
      {
        str = DependencyResolver.Current.GetServices<IContentParser>().Where<IContentParser>((Func<IContentParser, bool>) (p => p.Name == config.Editor)).First<IContentParser>().Quote(author, content);
      }
      catch
      {
      }
      return str;
    }

    public static MvcHtmlString ParseContent(string content)
    {
      IConfiguration config = DependencyResolver.Current.GetService<IConfiguration>();
      MvcHtmlString empty = MvcHtmlString.Empty;
      try
      {
        empty = DependencyResolver.Current.GetServices<IContentParser>().Where<IContentParser>((Func<IContentParser, bool>) (p => p.Name == config.Editor)).First<IContentParser>().Parse(content);
      }
      catch
      {
      }
      return empty;
    }

    public static IEnumerable<Forum> GetAccessibleForums()
    {
      List<Forum> forumList = new List<Forum>();
      foreach (Forum forum in DependencyResolver.Current.GetService<IRepository<Forum>>().ReadAll().ToList<Forum>())
      {
        if (forum.HasAccess(AccessFlag.Read))
          forumList.Add(forum);
      }
      return (IEnumerable<Forum>) forumList;
    }

    private static List<Forum> GetForumsWithAccess(AccessFlag access)
    {
      List<Forum> accessibleForums = new List<Forum>();
      foreach (Category category in (IEnumerable<Category>) DependencyResolver.Current.GetService<IBoardRepository>().ReadManyOptimized((ISpecification<Board>) new BoardSpecifications.Enabled()).First<Board>().Categories)
      {
        foreach (Forum forum in category.Forums.Where<Forum>((Func<Forum, bool>) (t => t.ParentForum == null)))
          ForumHelper.GetForumsWithAccess(access, forum, accessibleForums);
      }
      return accessibleForums;
    }

    private static void GetForumsWithAccess(
      AccessFlag access,
      Forum forum,
      List<Forum> accessibleForums)
    {
      if (forum.HasAccess(access))
        accessibleForums.Add(forum);
      foreach (Forum subForum in (IEnumerable<Forum>) forum.SubForums)
        ForumHelper.GetForumsWithAccess(access, subForum, accessibleForums);
    }

    public static IEnumerable<SelectListItem> GetModerateForums(int forumId)
    {
      return (IEnumerable<SelectListItem>) ForumHelper.GetForumsWithAccess(AccessFlag.Moderator).Where<Forum>((Func<Forum, bool>) (f => f.Id != forumId)).Select<Forum, SelectListItem>((Func<Forum, SelectListItem>) (f => new SelectListItem()
      {
        Text = f.Name,
        Value = f.Id.ToString()
      })).OrderBy<SelectListItem, string>((Func<SelectListItem, string>) (i => i.Text));
    }

    public static IEnumerable<SelectListItem> GetAccessMasks(int forumId)
    {
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      IEnumerable<AccessMask> source = DependencyResolver.Current.GetService<IRepository<AccessMask>>().ReadMany((ISpecification<AccessMask>) new AccessMaskSpecifications.SpecificBoard(DependencyResolver.Current.GetService<IRepository<Forum>>().Read(forumId).Category.Board));
      selectListItemList.Add(new SelectListItem()
      {
        Text = "(not set)",
        Value = ""
      });
      foreach (AccessMask accessMask in (IEnumerable<AccessMask>) source.OrderBy<AccessMask, string>((Func<AccessMask, string>) (x => x.Name)))
        selectListItemList.Add(new SelectListItem()
        {
          Text = accessMask.Name,
          Value = accessMask.Id.ToString()
        });
      return (IEnumerable<SelectListItem>) selectListItemList;
    }

    public static IEnumerable<SelectListItem> SetSelected(
      IEnumerable<SelectListItem> items,
      string selected)
    {
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      foreach (SelectListItem selectListItem in items)
        selectListItemList.Add(new SelectListItem()
        {
          Text = selectListItem.Text,
          Value = selectListItem.Value,
          Selected = selectListItem.Value == selected
        });
      return (IEnumerable<SelectListItem>) selectListItemList;
    }

    public static string GetAccessMask(GroupViewModel groupVM, int forumId)
    {
      IRepository<Group> service1 = DependencyResolver.Current.GetService<IRepository<Group>>();
      IRepository<Forum> service2 = DependencyResolver.Current.GetService<IRepository<Forum>>();
      Group group = service1.Read(groupVM.Id);
      ForumAccess forumAccess = DependencyResolver.Current.GetService<IRepository<ForumAccess>>().ReadOne((ISpecification<ForumAccess>) new ForumAccessSpecifications.SpecificForumAndGroup(service2.Read(forumId), group));
      if (forumAccess != null)
        return forumAccess.AccessMask.Id.ToString();
      return "";
    }

    private static MvcHtmlString TopicLink(
      int id,
      string title,
      RequestContext request,
      RouteCollection routeCollection)
    {
      TagBuilder tagBuilder = new TagBuilder("a");
      string url = UrlHelper.GenerateUrl("ShowTopic", (string) null, (string) null, (string) null, (string) null, (string) null, new RouteValueDictionary()
      {
        {
          nameof (id),
          (object) id
        },
        {
          nameof (title),
          (object) title.ToSlug()
        }
      }, routeCollection, request, false);
      tagBuilder.Attributes.Add("href", url);
      tagBuilder.SetInnerText(title);
      return MvcHtmlString.Create(tagBuilder.ToString());
    }

    private static MvcHtmlString TopicLink(
      int id,
      string title,
      RequestContext request,
      RouteCollection routeCollection,
      RouteValueDictionary htmlAttributes)
    {
      TagBuilder tagBuilder = new TagBuilder("a");
      string url = UrlHelper.GenerateUrl("ShowTopic", (string) null, (string) null, (string) null, (string) null, (string) null, new RouteValueDictionary()
      {
        {
          nameof (id),
          (object) id
        },
        {
          nameof (title),
          (object) title.ToSlug()
        }
      }, routeCollection, request, false);
      tagBuilder.MergeAttributes<string, object>((IDictionary<string, object>) htmlAttributes);
      tagBuilder.MergeAttribute("href", url);
      tagBuilder.SetInnerText(title);
      return MvcHtmlString.Create(tagBuilder.ToString());
    }

    public static MvcHtmlString TopicLink(
      this HtmlHelper html,
      int id,
      string title,
      object htmlAttributes)
    {
      return ForumHelper.TopicLink(id, title, html.ViewContext.RequestContext, html.RouteCollection, new RouteValueDictionary(htmlAttributes));
    }

    public static bool Debug
    {
      get
      {
        if (!ForumHelper.debugRead)
        {
          ForumHelper.debug = !string.IsNullOrEmpty(ConfigurationManager.AppSettings[nameof (Debug)]) && ConfigurationManager.AppSettings[nameof (Debug)] == "true";
          ForumHelper.debugRead = true;
        }
        return ForumHelper.debug;
      }
    }

    private static Version Version
    {
      get
      {
        if (ForumHelper.version == (Version) null)
        {
          lock (ForumHelper.versionLock)
          {
            if (ForumHelper.version == (Version) null)
              ForumHelper.version = typeof (Forum).Assembly.GetName().Version;
          }
        }
        return ForumHelper.version;
      }
    }

    public static string GetVersion()
    {
      return ForumHelper.Version.ToString();
    }

    public static string GetShortVersion()
    {
      Version version = ForumHelper.Version;
      return string.Format("{0}.{1}{2}", (object) version.Major, (object) version.Minor, version.Build == 0 ? (object) "" : (object) string.Format(".{0}", (object) version.Build));
    }
  }
}
