// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Security.SimpleRoleProvider
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCBootstrap.Web.Security
{
  public class SimpleRoleProvider : RoleProvider
  {
    protected IContext Context
    {
      get
      {
        return DependencyResolver.Current.GetService<IContext>();
      }
    }

    protected IRepository<User> UserRepository
    {
      get
      {
        return DependencyResolver.Current.GetService<IRepository<User>>();
      }
    }

    protected IRepository<Role> RoleRepository
    {
      get
      {
        return DependencyResolver.Current.GetService<IRepository<Role>>();
      }
    }

    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
      IEnumerable<Role> source = this.RoleRepository.ReadMany((ISpecification<Role>) new RoleSpecifications.SpecificNames(roleNames));
      if (!source.Any<Role>())
        return;
      foreach (User user in this.UserRepository.ReadMany((ISpecification<User>) new UserSpecifications.SpecificUsernames(usernames)))
      {
        foreach (Role role in source)
          user.Roles.Add(role);
      }
      this.Context.SaveChanges();
    }

    public override string ApplicationName { get; set; }

    public override void CreateRole(string roleName)
    {
      if (this.RoleRepository.ReadOne((ISpecification<Role>) new RoleSpecifications.SpecificName(roleName)) != null)
        return;
      this.RoleRepository.Create(new Role()
      {
        Name = roleName
      });
      this.Context.SaveChanges();
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
      Role entity = this.RoleRepository.ReadOne((ISpecification<Role>) new RoleSpecifications.SpecificName(roleName));
      if (entity != null)
      {
        if (entity.Users.Any<User>() && throwOnPopulatedRole)
          throw new ApplicationException("Role in use");
        this.RoleRepository.Delete(entity);
        this.Context.SaveChanges();
      }
      return true;
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
      throw new NotImplementedException("");
    }

    public override string[] GetAllRoles()
    {
      return this.RoleRepository.ReadAll().Select<Role, string>((Func<Role, string>) (r => r.Name)).ToArray<string>();
    }

    public override string[] GetRolesForUser(string username)
    {
      User user = this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificUsername(username));
      if (user != null)
        return user.Roles.Select<Role, string>((Func<Role, string>) (r => r.Name)).ToArray<string>();
      return new string[0];
    }

    public override string[] GetUsersInRole(string roleName)
    {
      Role role = this.RoleRepository.ReadOne((ISpecification<Role>) new RoleSpecifications.SpecificName(roleName));
      if (role != null)
        return role.Users.Select<User, string>((Func<User, string>) (r => r.Username)).ToArray<string>();
      return new string[0];
    }

    public override bool IsUserInRole(string username, string roleName)
    {
      Role role = this.RoleRepository.ReadOne((ISpecification<Role>) new RoleSpecifications.SpecificName(roleName));
      if (role != null)
      {
        User user = this.UserRepository.ReadOne((ISpecification<User>) new UserSpecifications.SpecificUsername(username));
        if (user != null)
          return role.Users.Contains(user);
      }
      return false;
    }

    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
      IEnumerable<User> users = this.UserRepository.ReadMany((ISpecification<User>) new UserSpecifications.SpecificUsernames(usernames));
      IEnumerable<Role> roles = this.RoleRepository.ReadMany((ISpecification<Role>) new RoleSpecifications.SpecificNames(roleNames));
      foreach (User user in users)
      {
        foreach (Role role in (IEnumerable<Role>) user.Roles.Where<Role>((Func<Role, bool>) (r => roles.Contains<Role>(r))).ToList<Role>())
          user.Roles.Remove(role);
      }
      this.Context.SaveChanges();
    }

    public override bool RoleExists(string roleName)
    {
      return this.RoleRepository.ReadOne((ISpecification<Role>) new RoleSpecifications.SpecificName(roleName)) != null;
    }
  }
}
