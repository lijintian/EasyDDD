using System;
using System.Collections.Generic;

namespace EasyDDD.Infrastructure.Crosscutting.Event
{
    public interface IEventBus : IUnitOfWork, IDisposable
    {
        Guid ID { get; }
        /// <summary>
        /// Publishes the specified message to the bus.
        /// </summary>
        /// <param name="message">The message to be published.</param>
        void Publish<TMessage>(TMessage message)
            where TMessage : class, IEvent;
        /// <summary>
        /// Publishes a collection of messages to the bus.
        /// </summary>
        /// <param name="messages">The messages to be published.</param>
        void Publish<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : class, IEvent;
        /// <summary>
        /// Clears the published messages waiting for commit.
        /// </summary>
        void Clear();
    }
}
