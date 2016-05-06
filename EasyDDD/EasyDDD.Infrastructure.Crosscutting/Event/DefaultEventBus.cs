using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace EasyDDD.Infrastructure.Crosscutting.Event
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultEventBus : DisposableObject, IEventBus
    {
        private readonly Guid id = Guid.NewGuid();
        private readonly ThreadLocal<Queue<object>> _messageQueue = new ThreadLocal<Queue<object>>(() => new Queue<object>());
        private readonly IEventAggregator _aggregator;
        private readonly ThreadLocal<bool> _committed = new ThreadLocal<bool>(() => true);
        private readonly MethodInfo _publishMethod;

        public DefaultEventBus(IEventAggregator aggregator)
        {
            this._aggregator = aggregator;
            _publishMethod = (from m in aggregator.GetType().GetMethods()
                              let parameters = m.GetParameters()
                              let methodName = m.Name
                              where methodName == "Publish" &&
                              parameters != null &&
                              parameters.Length == 1
                              select m).First();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _messageQueue.Dispose();
                _committed.Dispose();
            }
        }

        #region IBus Members

        public void Publish<TMessage>(TMessage message)
            where TMessage : class, IEvent
        {
            _messageQueue.Value.Enqueue(message);
            _committed.Value = false;
        }

        public void Publish<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : class, IEvent
        {
            foreach (var message in messages)
                Publish(message);
        }

        public void Clear()
        {
            _messageQueue.Value.Clear();
            _committed.Value = true;
        }

        #endregion

        #region IUnitOfWork Members

        public bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public bool Committed
        {
            get { return _committed.Value; }
        }

        public void Commit()
        {
            while (_messageQueue.Value.Count > 0)
            {
                var evnt = _messageQueue.Value.Dequeue();
                var evntType = evnt.GetType();
                var method = _publishMethod.MakeGenericMethod(evntType);
                method.Invoke(_aggregator, new object[] { evnt });
            }
            _committed.Value = true;
        }

        public void Rollback()
        {
            Clear();
        }

        public Guid ID
        {
            get { return id; }
        }

        #endregion
    }
}
