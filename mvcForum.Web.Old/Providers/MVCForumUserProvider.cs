// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Providers.MVCForumUserProvider
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Interfaces;
using System.Web;
using System.Web.Security;

namespace mvcForum.Web.Providers
{
  public class MVCForumUserProvider : IWebUserProvider
  {
    private readonly IRepository<ForumUser> userRepo;
    private ForumUser user;
    protected bool checkedAuthenticated;
    protected bool authenticated;

    public MVCForumUserProvider(IRepository<ForumUser> userRepo)
    {
      this.userRepo = userRepo;
    }

    public ForumUser ActiveUser
    {
      get
      {
        if (this.Authenticated)
          return this.user;
        return (ForumUser) null;
      }
    }

    public bool Authenticated
    {
      get
      {
        if (!this.checkedAuthenticated)
        {
          if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
          {
            MembershipUser user = Membership.GetUser(false);
            this.authenticated = user != null;
            if (this.authenticated)
            {
              try
              {
                this.user = this.userRepo.ReadOne((ISpecification<ForumUser>) new ForumUserSpecifications.SpecificProviderUserKey(user.ProviderUserKey.ToString()));
              }
              catch
              {
              }
              this.authenticated = this.user != null;
            }
          }
          this.checkedAuthenticated = true;
        }
        return this.authenticated;
      }
    }
  }
}
