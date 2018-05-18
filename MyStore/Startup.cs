using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using hdcontext.IdentityDomain;
using hddata.DBFactory;
using hddata.UnitOfWork;
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
            builder.RegisterType<hddata.UnitOfWork.UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<Context.dbFactory.DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<MyStore.Context.Context>().AsSelf().InstancePerRequest();

            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            hdcore.ConfigAutofac.RegisterControllers(builder);

            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.ErrorService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.GroupService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.RoleGroupService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.RoleService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.UserGroupService>(builder);
            hdcore.ConfigAutofac.RegisterAssemblyTypes<hdidentity.Implement.UserService>(builder);

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}