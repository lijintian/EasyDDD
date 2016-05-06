using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EasyDDD.Infrastructure.Crosscutting.Event
{ 
    public class EventAggregator : IEventAggregator
    {
        private readonly object _lockObj = new object();
        private readonly Dictionary<Type, List<object>> _eventHandlers = new Dictionary<Type, List<object>>();
        private readonly MethodInfo _registerEventHandlerMethod;
        private readonly Func<object, object, bool> _eventHandlerEquals = (o1, o2) =>
        {
            bool isEquals = false;
            var o1Type = o1.GetType();
            var o2Type = o2.GetType();
            if (o1Type.IsGenericType &&
                o1Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>)
                &&
                o2Type.IsGenericType &&
                o2Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>))
            {
                isEquals = o1.Equals(o2);
            }
            else
            {
                isEquals = o1Type == o2Type;
            }

            return isEquals;
        };


        public EventAggregator()
        {
            _registerEventHandlerMethod = (from p in this.GetType().GetMethods()
                                           let methodName = p.Name
                                           let parameters = p.GetParameters()
                                           where methodName == "Subscribe" &&
                                           parameters != null &&
                                           parameters.Length == 1 &&
                                           parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                                           select p).First();
        }


        public EventAggregator(object[] handlers)
            : this()
        {
            foreach (var obj in handlers)
            {
                var type = obj.GetType();
                var implementedInterfaces = type.GetInterfaces();
                foreach (var implementedInterface in implementedInterfaces)
                {
                    if (implementedInterface.IsGenericType &&
                        implementedInterface.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    {
                        var eventType = implementedInterface.GetGenericArguments().First();
                        var method = _registerEventHandlerMethod.MakeGenericMethod(eventType);
                        method.Invoke(this, new object[] { obj });
                    }
                }
            }
        }
        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            lock (_lockObj)
            {
                var eventType = typeof(TEvent);
                if (_eventHandlers.ContainsKey(eventType))
                {
                    var handlers = _eventHandlers[eventType];
                    if (handlers != null)
                    {
                        if (!handlers.Exists(deh => _eventHandlerEquals(deh, eventHandler)))
                            handlers.Add(eventHandler);
                    }
                    else
                    {
                        handlers = new List<object>();
                        handlers.Add(eventHandler);
                    }
                }
                else
                    _eventHandlers.Add(eventType, new List<object> { eventHandler });
            }
        }

        public void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Subscribe<TEvent>(eventHandler);
        }

        public void Subscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Subscribe<TEvent>(eventHandler);
        }

        public void Subscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent
        {
            Subscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerFunc));
        }

        public void Subscribe<TEvent>(IEnumerable<Func<TEvent, bool>> eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerFunc in eventHandlerFuncs)
                Subscribe<TEvent>(eventHandlerFunc);
        }

        public void Subscribe<TEvent>(params Func<TEvent, bool>[] eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerFunc in eventHandlerFuncs)
                Subscribe<TEvent>(eventHandlerFunc);
        }

        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            lock (_lockObj)
            {
                var eventType = typeof(TEvent);
                if (_eventHandlers.ContainsKey(eventType))
                {
                    var handlers = _eventHandlers[eventType];
                    if (handlers != null &&
                        handlers.Exists(deh => _eventHandlerEquals(deh, eventHandler)))
                    {
                        var handlerToRemove = handlers.First(deh => _eventHandlerEquals(deh, eventHandler));
                        handlers.Remove(handlerToRemove);
                    }
                }
            }
        }

        public void Unsubscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Unsubscribe<TEvent>(eventHandler);
        }

        public void Unsubscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Unsubscribe<TEvent>(eventHandler);
        }

        public void Unsubscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent
        {
            Unsubscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerFunc));
        }

        public void Unsubscribe<TEvent>(IEnumerable<Func<TEvent, bool>> eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerFunc in eventHandlerFuncs)
                Unsubscribe<TEvent>(eventHandlerFunc);
        }

        public void Unsubscribe<TEvent>(params Func<TEvent, bool>[] eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerFunc in eventHandlerFuncs)
                Unsubscribe<TEvent>(eventHandlerFunc);
        }

        public void UnsubscribeAll<TEvent>()
            where TEvent : class, IEvent
        {
            lock (_lockObj)
            {
                var eventType = typeof(TEvent);
                if (_eventHandlers.ContainsKey(eventType))
                {
                    var handlers = _eventHandlers[eventType];
                    if (handlers != null)
                        handlers.Clear();
                }
            }
        }

        public void UnsubscribeAll()
        {
            lock (_lockObj)
            {
                _eventHandlers.Clear();
            }
        }

        public IEnumerable<IEventHandler<TEvent>> GetSubscriptions<TEvent>()
            where TEvent : class, IEvent
        {
            var eventType = typeof(TEvent);
            lock (_lockObj)
            {
                if (_eventHandlers.ContainsKey(eventType))
                {
                    var handlers = _eventHandlers[eventType];
                    if (handlers != null)
                        return handlers.Select(p => p as IEventHandler<TEvent>).ToList();
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public void Publish<TEvent>(TEvent evnt)
            where TEvent : class, IEvent
        {
            if (evnt == null)
                throw new ArgumentNullException("evnt");
            var eventType = evnt.GetType();
            lock (_lockObj)
            {
                if (_eventHandlers.ContainsKey(eventType) &&
                    _eventHandlers[eventType] != null &&
                    _eventHandlers[eventType].Count > 0)
                {
                    var handlers = _eventHandlers[eventType];
                    foreach (var handler in handlers)
                    {
                        var eventHandler = handler as IEventHandler<TEvent>;
                        if (eventHandler.GetType().IsDefined(typeof(AsyncEventHandleAttribute), false))
                        {
                            Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), evnt);
                        }
                        else
                        {
                            eventHandler.Handle(evnt);
                        }
                    }
                }
            }
        }

        public void Publish<TEvent>(TEvent evnt, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent
        {
            if (evnt == null)
                throw new ArgumentNullException("evnt");
            var eventType = evnt.GetType();
            lock (_lockObj)
            {
                if (_eventHandlers.ContainsKey(eventType) &&
                    _eventHandlers[eventType] != null &&
                    _eventHandlers[eventType].Count > 0)
                {
                    var handlers = _eventHandlers[eventType];
                    List<Task> tasks = new List<Task>();
                    try
                    {
                        foreach (var handler in handlers)
                        {
                            var eventHandler = handler as IEventHandler<TEvent>;
                            if (eventHandler.GetType().IsDefined(typeof(AsyncEventHandleAttribute), false))
                            {
                                tasks.Add(Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), evnt));
                            }
                            else
                            {
                                eventHandler.Handle(evnt);
                            }
                        }
                        if (tasks.Count > 0)
                        {
                            if (timeout == null)
                                Task.WaitAll(tasks.ToArray());
                            else
                                Task.WaitAll(tasks.ToArray(), timeout.Value);
                        }
                        callback(evnt, true, null);
                    }
                    catch (Exception ex)
                    {
                        callback(evnt, false, ex);
                    }
                }
                else
                    callback(evnt, false, null);
            }
        }
    }
}
