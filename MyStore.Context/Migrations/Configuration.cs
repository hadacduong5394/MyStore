namespace MyStore.Context.Migrations
{
    using FX.Context.IdentityDomain;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
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

            var lstRole = new List<Role>
            {
                new Role { Name = "Role1", CreateBy = "duonghd", CreateDate = DateTime.Now, Descreption = "Role1", Status = true },
                new Role { Name = "Role2", CreateBy = "duonghd", CreateDate = DateTime.Now, Descreption = "Role2", Status = true }
            };

            var groups = new List<Group>
            {
                new Group { ComId = 0, Name = "Group1", Descreption = "Group1", CreateBy = "duonghd", CreateDate = DateTime.Now, Status = true },
            };
            context.ApplicationRoles.AddRange(lstRole);
            context.Groups.AddRange(groups);
            context.SaveChanges();
        }
    }
}
