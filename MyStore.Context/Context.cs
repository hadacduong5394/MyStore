using FX.Context;
using FX.Context.IdentityDomain;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace MyStore.Context
{
    public class Context : ContextConnection
    {
        public Context() : base()
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static Context Create()
        {
            return new Context();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}