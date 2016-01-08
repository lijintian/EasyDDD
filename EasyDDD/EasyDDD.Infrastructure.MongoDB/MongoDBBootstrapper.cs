

using EasyDDD.Core;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace EasyDDD.Infrastructure.Data.MongoDB
{
    public class MongoDBBootstrapper
    {
        private static readonly object _lockObj = new object();
        private static bool _isMapping;
        public static void Init()
        {
            lock (_lockObj)
            {
                if (!_isMapping)
                {
                    MongoDBRepositoryContext.RegisterConventions();

                    var mapping = IoC.Resolve<IMapping>();
                    if (mapping != null)
                    {
                        mapping.Map();
                    }
                    _isMapping = true;
                }
            }

        }
    }
}
