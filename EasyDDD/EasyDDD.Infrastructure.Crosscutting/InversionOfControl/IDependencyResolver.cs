using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyDDD.Infrastructure.Crosscutting.InversionOfControl
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
        T Resolve<T>(Type type);
        T Resolve<T>(string name);
        IEnumerable<T> ResolveAll<T>();
    }
}
