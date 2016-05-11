using System.Diagnostics;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace EasyDDD.Infrastructure.Crosscutting.Adapter
{
    public static class TypeAdapter
    {
        private static ITypeAdapter InternalTypeAdapter
        {
            [DebuggerStepThrough]
            get { return IoC.Resolve<ITypeAdapter>(); }
        }

        public static TTarget Adapt<TSource, TTarget>(TSource source)
            where TTarget : class, new()
            where TSource : class
        {
            Check.Argument.IsNotNull(source, "source");
            return InternalTypeAdapter.Adapt<TSource, TTarget>(source);
        }

        public static TTarget Adapt<TTarget>(object source)
            where TTarget : class, new()
        {
            Check.Argument.IsNotNull(source, "source");
            return InternalTypeAdapter.Adapt<TTarget>(source);
        }
    }
}
