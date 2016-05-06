using System;

namespace EasyDDD.Infrastructure.Crosscutting.Event
{
    public class ActionDelegatedEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        private readonly Action<TEvent> _eventHandlerDelegate;

        /// <summary>
        /// 初始化一个新的<c>ActionDelegatedDomainEventHandler{TEvent}</c>实例。
        /// </summary>
        /// <param name="eventHandlerDelegate">用于当前事件处理器所代理的事件处理委托。</param>
        public ActionDelegatedEventHandler(Action<TEvent> eventHandlerDelegate)
        {
            this._eventHandlerDelegate = eventHandlerDelegate;
        }

        public void Handle(TEvent evnt)
        {
            this._eventHandlerDelegate(evnt);
        }

        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前对象是否与给定的类型相同的另一对象相等。
        /// </summary>
        /// <param name="other">需要比较的与当前对象类型相同的另一对象。</param>
        /// <returns>如果两者相等，则返回true，否则返回false。</returns>
        public override bool Equals(object other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if ((object)other == (object)null)
                return false;
            ActionDelegatedEventHandler<TEvent> otherDelegate = other as ActionDelegatedEventHandler<TEvent>;
            if ((object)otherDelegate == (object)null)
                return false;
            // 使用Delegate.Equals方法判定两个委托是否是代理的同一方法。
            return Delegate.Equals(this._eventHandlerDelegate, otherDelegate._eventHandlerDelegate);
        }

        public override int GetHashCode()
        {
            return this._eventHandlerDelegate.GetHashCode();
        }
    }
}
