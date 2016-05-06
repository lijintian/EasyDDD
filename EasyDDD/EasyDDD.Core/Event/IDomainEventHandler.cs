
using EasyDDD.Infrastructure.Crosscutting.Event;
using System;

namespace EasyDDD.Core.Event
{
    public interface IDomainEventHandler<in TDomainEvent> : IEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        void Handle<TDomainEventResult>(TDomainEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult;

        new void Handle(TDomainEvent domainEvent);
    }



}
