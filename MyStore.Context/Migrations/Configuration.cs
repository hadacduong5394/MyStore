namespace MyStore.Context.Migrations
{
    using FX.Context.IdentityDomain;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyStore.Context.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyStore.Context.Context context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MyStore.Context.Context()));

            var user = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "Hà Đắc Dương",
                CreateDate = DateTime.Now,
                CreateBy = "admin",
                Status = true,
            };
            manager.Create(user, "123456");
        }
    }
}
