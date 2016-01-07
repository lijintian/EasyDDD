using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyDDD.Infrastructure.Crosscutting.InversionOfControl
{
    public static class IoC
    {
        private static IDependencyResolver _dependencyResolver;

        public static void Initialize(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public static T Resolve<T>()
        {
            return _dependencyResolver.Resolve<T>();
        }

        public static T Resolve<T>(Type type)
        {
            return _dependencyResolver.Resolve<T>(type);
        }

        public static object Resolve(Type type)
        {
            return Resolve<object>(type);
        }

        public static T Resolve<T>(string name)
        {
            return _dependencyResolver.Resolve<T>(name);
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return _dependencyResolver.ResolveAll<T>();
        }
    }
}
