﻿using FX.Context.IdentityDomain;
using Microsoft.AspNet.Identity;
using System.Web;

namespace MyStore.Context.IdentityConfiguration
{
    public class CurrentUser
    {
        private static CurrentUser instance;

        private CurrentUser()
        {
        }

        public static CurrentUser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CurrentUser();
                }
                return instance;
            }
        }

        private ApplicationUser _user;

        public ApplicationUser User
        {
            get
            {
                var userIdentity = HttpContext.Current.GetOwinContext().Authentication.User;
                var currentUser = UserManager.Instance.UserManagerment.FindByName(userIdentity.Identity.Name);

                return currentUser;
            }
            private set
            {
                _user = value;
            }
        }
    }
}