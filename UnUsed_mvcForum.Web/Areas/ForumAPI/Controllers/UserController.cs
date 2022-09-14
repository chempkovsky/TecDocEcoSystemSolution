// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.UserController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using MVCBootstrap.Web.Mvc.Extensions;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using mvcForum.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class UserController : BaseAPIController
  {
    private readonly IMembershipService membership;

    public UserController(IMembershipService membership)
    {
      this.membership = membership;
    }

    [HttpPost]
    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Create(mvcForum.Web.Areas.ForumAPI.ViewModels.ForumUser model, string mode)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
      string errorMessage = string.Empty;
      if (!this.membership.CreateAccount(model.Name, model.Password, model.EmailAddress, out errorMessage))
        return (ActionResult) new HttpStatusCodeResult(500, errorMessage);
      this.membership.UnlockAccount(model.Name);
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Read(int id, string mode)
    {
      mvcForum.Core.ForumUser forumUser = this.GetRepository<mvcForum.Core.ForumUser>().Read(id);
      IAccount accountByEmailAddress = this.membership.GetAccountByEmailAddress(forumUser.EmailAddress);
      return (ActionResult) this.CustomJson((object) new mvcForum.Web.Areas.ForumAPI.ViewModels.ForumUser()
      {
        Id = forumUser.Id,
        Name = forumUser.Name,
        EmailAddress = forumUser.EmailAddress,
        Deleted = forumUser.Deleted,
        FirstVisit = forumUser.FirstVisit,
        LastVisit = forumUser.LastVisit,
        LastIP = forumUser.LastIP,
        Active = forumUser.Active,
        Locked = accountByEmailAddress.IsLockedOut
      });
    }

    [HttpPost]
    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Update(mvcForum.Web.Areas.ForumAPI.ViewModels.ForumUser model, string mode)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
      mvcForum.Core.ForumUser forumUser = this.GetRepository<mvcForum.Core.ForumUser>().Read(model.Id);
      forumUser.Deleted = model.Deleted;
      IAccount accountByName = this.membership.GetAccountByName(forumUser.Name);
      accountByName.EmailAddress = model.EmailAddress;
      this.membership.UpdateAccount(accountByName);
      forumUser.Active = model.Active;
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [HttpPost]
    [Authorize(Roles = "Solution Administrator")]
    public ActionResult UpdateRoles(int id, string roles, string mode)
    {
      IAccount account = this.membership.GetAccount((object) this.GetRepository<mvcForum.Core.ForumUser>().Read(id).ProviderId);
      string[] existingRoles = this.membership.GetRolesForAccount(account.AccountName);
      if (string.IsNullOrWhiteSpace(roles) && ((IEnumerable<string>) existingRoles).Any<string>())
        this.membership.RemoveAccountFromRoles(account.AccountName, existingRoles);
      else if (!string.IsNullOrWhiteSpace(roles))
      {
        string[] roleArray = roles.Split(new string[1]
        {
          ","
        }, StringSplitOptions.RemoveEmptyEntries);
        string[] array1 = ((IEnumerable<string>) existingRoles).Where<string>((Func<string, bool>) (r => !((IEnumerable<string>) roleArray).Contains<string>(r))).ToArray<string>();
        string[] array2 = ((IEnumerable<string>) roleArray).Where<string>((Func<string, bool>) (r => !((IEnumerable<string>) existingRoles).Contains<string>(r))).ToArray<string>();
        if (((IEnumerable<string>) array1).Any<string>())
          this.membership.RemoveAccountFromRoles(account.AccountName, array1);
        if (((IEnumerable<string>) array2).Any<string>())
          this.membership.AddAccountToRoles(account.AccountName, array2);
      }
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    [HttpPost]
    public ActionResult Unlock(int id, string mode)
    {
      this.membership.UnlockAccount(this.GetRepository<mvcForum.Core.ForumUser>().Read(id).Name);
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [HttpPost]
    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Delete(int id, string mode)
    {
      mvcForum.Core.ForumUser forumUser = this.GetRepository<mvcForum.Core.ForumUser>().Read(id);
      forumUser.Deleted = true;
      this.membership.DeleteAccount(forumUser.Name);
      this.Context.SaveChanges();
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Search(string query, string mode)
    {
      IEnumerable<mvcForum.Core.ForumUser> forumUsers = (IEnumerable<mvcForum.Core.ForumUser>) new List<mvcForum.Core.ForumUser>();
      if (!string.IsNullOrWhiteSpace(query))
        forumUsers = this.ForumUserRepository.ReadMany((ISpecification<mvcForum.Core.ForumUser>) new ForumUserSpecifications.Search(query));
      return (ActionResult) this.GetUserList(forumUsers, forumUsers.Count<mvcForum.Core.ForumUser>(), 0);
    }

    private ContentResult GetUserList(
      IEnumerable<mvcForum.Core.ForumUser> users,
      int pageSize,
      int skipped)
    {
      int num = users.Count<mvcForum.Core.ForumUser>();
      return this.CustomJson((object) new
      {
        Count = num,
        PageSize = pageSize,
        Skipped = skipped,
        Users = users.Skip<mvcForum.Core.ForumUser>(skipped).Take<mvcForum.Core.ForumUser>(pageSize).Select<mvcForum.Core.ForumUser, mvcForum.Web.Areas.ForumAPI.ViewModels.ForumUser>((Func<mvcForum.Core.ForumUser, mvcForum.Web.Areas.ForumAPI.ViewModels.ForumUser>) (u => new mvcForum.Web.Areas.ForumAPI.ViewModels.ForumUser()
        {
          Id = u.Id,
          Name = u.Name,
          EmailAddress = u.EmailAddress,
          FirstVisit = u.FirstVisit,
          Deleted = u.Deleted,
          Active = u.Active,
          ApiUrl = this.Url.Action("read", "user", (object) new
          {
            mode = "json",
            area = "forumapi",
            id = u.Id
          })
        })).OrderBy<mvcForum.Web.Areas.ForumAPI.ViewModels.ForumUser, string>((Func<mvcForum.Web.Areas.ForumAPI.ViewModels.ForumUser, string>) (u => u.Name)),
        Next = (skipped + pageSize < num ? this.Url.Action("list", "user", (object) new
        {
          area = "forumapi",
          mode = "json",
          skip = (skipped + pageSize)
        }) : string.Empty),
        Previous = (skipped > 0 ? this.Url.Action("list", "user", (object) new
        {
          area = "forumapi",
          mode = "json",
          skip = (skipped - pageSize > 0 ? skipped - pageSize : 0)
        }) : string.Empty)
      });
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult List(int? skip, string mode)
    {
      int skipped = 0;
      int pageSize = 15;
      if (skip.HasValue && skip.Value > 0)
        skipped = skip.Value;
      return (ActionResult) this.GetUserList((IEnumerable<mvcForum.Core.ForumUser>) this.ForumUserRepository.ReadAll().OrderByDescending<mvcForum.Core.ForumUser, DateTime>((Func<mvcForum.Core.ForumUser, DateTime>) (u => u.FirstVisit)), pageSize, skipped);
    }

    public ActionResult RoleList(int id, string mode)
    {
      return (ActionResult) this.CustomJson((object) new
      {
        Roles = this.membership.GetRolesForAccount(this.GetRepository<mvcForum.Core.ForumUser>().Read(id).Name)
      });
    }
  }
}
