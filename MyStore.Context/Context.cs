using hdcontext;

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
    }
}