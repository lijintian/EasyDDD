using System;

namespace EasyDDD.Infrastructure.Crosscutting.Transaction
{
    public interface ITransactionCoordinator : IUnitOfWork, IDisposable
    {
    }
}
