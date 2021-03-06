﻿using System.Web.Mvc;

namespace MyStore.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { Controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "MyStore.Areas.Admin.Controllers" }
            );
        }
    }
}