using EasyDDD.Core.Aggregate;
using EasyDDD.Infrastructure.Crosscutting.Event;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace EasyDDD.Core.Event
{
    /// <summary>
    /// 表示继承于该类的类型为领域事件。
    /// </summary>
    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        #region Private Fields
        private readonly IEntity _source;
        private Guid _id = Guid.NewGuid();
        private DateTime _timeStamp = DateTime.UtcNow;
        #endregion

        #region Ctor

        protected DomainEvent() { }

        protected DomainEvent(IEntity source)
        {
            this._source = source;
        }
        #endregion

        #region IDomainEvent Members
        /// <summary>
        /// 获取领域事件的全局唯一标识。
        /// </summary>
        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 获取产生领域事件的时间戳。
        /// </summary>
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        public IEntity Source
        {
            get { return _source; }
        }
        #endregion

        #region Public Static Methods

        public static void Publish<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent
        {
            IEnumerable<IDomainEventHandler<TDomainEvent>> handlers = IoC.ResolveAll<IDomainEventHandler<TDomainEvent>>(); ;
            foreach (var handler in handlers)
            {
                if (handler.GetType().IsDefined(typeof(AsyncEventHandleAttribute), false))
                {
                    var handler1 = handler;
                    Task.Factory.StartNew(() => handler1.Handle(domainEvent));
                }
                else
                {
                    handler.Handle(domainEvent);
                }
            }
        }

        public static void Publish<TDomainEvent, TDomainEventResult>(TDomainEvent domainEvent, Action<TDomainEventResult> callback, TimeSpan? timeout = null)
            where TDomainEvent : class, IDomainEvent
            where TDomainEventResult : class , IDomainEventResult
        {
            var handlers = IoC.ResolveAll<IDomainEventHandler<TDomainEvent>>();
            if (handlers != null && handlers.Count() > 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (var handler in handlers)
                {
                    if (handler.GetType().IsDefined(typeof(AsyncEventHandleAttribute), false))
                    {

                        tasks.Add(Task.Factory.StartNew(() => handler.Handle(domainEvent, callback)));
                    }
                    else
                    {
                        handler.Handle(domainEvent, callback);
                    }
                }
                if (tasks.Count > 0)
                {
                    if (timeout == null)
                    {
                        Task.WaitAll(tasks.ToArray());
                    }
                    else
                    {
                        Task.WaitAll(tasks.ToArray(), timeout.Value);
                    }
                }
            }
        }
        #endregion
    }

}
