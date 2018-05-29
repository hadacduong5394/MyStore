using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FX.Context.IdentityDomain;
using FX.Data.DBFactory;
using FX.Data.UnitOfWork;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using MyStore.Context.IdentityConfiguration;
using Owin;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(MyStore.Startup))]

namespace MyStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigAutofac(app);
            ConfigureAuth(app);
        }

        private void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FX.Data.UnitOfWork.UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<Context.dbFactory.DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<MyStore.Context.Context>().AsSelf().InstancePerRequest();

            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            FX.Core.ConfigAutofac.RegisterControllers(builder);

            FX.Core.ConfigAutofac.RegisterAssemblyTypes<FX.Identity.Implement.GroupService>(builder);
            FX.Core.ConfigAutofac.RegisterAssemblyTypes<FX.Identity.Implement.RoleGroupService>(builder);
            FX.Core.ConfigAutofac.RegisterAssemblyTypes<FX.Identity.Implement.RoleService>(builder);
            FX.Core.ConfigAutofac.RegisterAssemblyTypes<FX.Identity.Implement.UserGroupService>(builder);
            FX.Core.ConfigAutofac.RegisterAssemblyTypes<FX.Identity.Implement.UserService>(builder);

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}