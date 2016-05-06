using System;
using System.Collections.Generic;

namespace EasyDDD.Infrastructure.Crosscutting.Event
{
    /// <summary>
    /// 事件聚合器
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandler">事件处理器</param>
        void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
           where TEvent : class, IEvent;


        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandler">多个事件处理器</param>
        void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandler)
            where TEvent : class, IEvent;

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandler">事件处理器</param>
        void Subscribe<TEvent>(params IEventHandler<TEvent>[] eventHandler)
            where TEvent : class, IEvent;

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandler">事件处理Action</param>
        void Subscribe<TEvent>(Action<TEvent> eventHandler)
            where TEvent : class, IEvent;

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandler">多个事件处理Action</param>
        void Subscribe<TEvent>(IEnumerable<Func<TEvent, bool>> eventHandler)
            where TEvent : class, IEvent;

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandlerFuncs">多个事件处理Action</param>
        void Subscribe<TEvent>(params Func<TEvent, bool>[] eventHandlerFuncs)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandler">事件处理器</param>
        void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandlers">多个事件处理器</param>
        void Unsubscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandlers">多个事件处理器</param>
        void Unsubscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandlerFunc">事件处理Action</param>
        void Unsubscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandlerFuncs">多个事件处理Action</param>
        void Unsubscribe<TEvent>(IEnumerable<Func<TEvent, bool>> eventHandlerFuncs)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="eventHandlerFuncs">多个事件处理Action</param>
        void Unsubscribe<TEvent>(params Func<TEvent, bool>[] eventHandlerFuncs)
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消指定事件类型订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        void UnsubscribeAll<TEvent>()
            where TEvent : class, IEvent;

        /// <summary>
        /// 取消全部事件订阅
        /// </summary>
        void UnsubscribeAll();

        /// <summary>
        /// 获取指定事件的订阅者者
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        IEnumerable<IEventHandler<TEvent>> GetSubscriptions<TEvent>()
            where TEvent : class, IEvent;

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="theEvent"></param>
        void Publish<TEvent>(TEvent theEvent)
            where TEvent : class, IEvent;

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <param name="theEvent">事件</param>
        /// <param name="callback">回调方法</param>
        /// <param name="timeout">超时时间</param>
        void Publish<TEvent>(TEvent theEvent, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent;
    }
}
