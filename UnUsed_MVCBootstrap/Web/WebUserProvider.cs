// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.WebUserProvider
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Web;
using System.Web.Security;

namespace MVCBootstrap.Web
{
  public class WebUserProvider : IUserProvider
  {
    private readonly IRepository<User> userRepo;
    private User user;
    public bool checkedAuthenticated;
    public bool authenticated;

    public WebUserProvider(IRepository<User> userRepo)
    {
      this.userRepo = userRepo;
    }

    public User ActiveUser
    {
      get
      {
        if (this.Authenticated)
          return this.user;
        return (User) null;
      }
    }

    public bool Authenticated
    {
      get
      {
        if (!this.checkedAuthenticated)
        {
          MembershipUser user = Membership.GetUser(false);
          this.authenticated = HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated && user != null;
          if (this.authenticated)
          {
            try
            {
              this.user = this.userRepo.ReadOne((ISpecification<User>) new UserSpecifications.SpecificProviderUserKey((Guid) user.ProviderUserKey));
            }
            catch
            {
            }
            this.authenticated = this.user != null;
          }
          this.checkedAuthenticated = true;
        }
        return this.authenticated;
      }
    }
  }
}
