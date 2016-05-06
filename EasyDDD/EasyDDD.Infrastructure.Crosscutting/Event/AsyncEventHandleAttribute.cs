using System;

namespace EasyDDD.Infrastructure.Crosscutting.Event
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AsyncEventHandleAttribute : Attribute
    {
    }
}
