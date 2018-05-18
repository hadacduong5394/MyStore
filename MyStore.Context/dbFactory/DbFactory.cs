using hdcontext;
using hddata.DBFactory;

namespace MyStore.Context.dbFactory
{
    public class DbFactory : Disposable, IDbFactory
    {
        private ContextConnection dbContext;

        public ContextConnection Init()
        {
            return dbContext ?? (dbContext = new Context());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}