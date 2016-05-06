

using EasyDDD.Infrastructure.Crosscutting.Exceptions;

namespace EasyDDD.Core.Event
{
    public class DomainEventResult : IDomainEventResult
    {
        public ExceptionBase Error { set; get; }
        public string ErrorCode { set; get; }
    }
}
