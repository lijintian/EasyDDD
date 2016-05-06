
namespace EasyDDD.Infrastructure.Crosscutting.Event
{
    public interface IEventHandler<in TEvent>
        where TEvent : IEvent
    {
        /// <summary>
        /// 处理给定的事件。
        /// </summary>
        /// <param name="evnt">需要处理的事件。</param>
        void Handle(TEvent evnt);
    }
}
