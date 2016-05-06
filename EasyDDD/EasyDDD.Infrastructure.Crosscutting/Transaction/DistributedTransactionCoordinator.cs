
using System.Transactions;

namespace EasyDDD.Infrastructure.Crosscutting.Transaction
{
    internal sealed class DistributedTransactionCoordinator : TransactionCoordinator
    {
        private readonly TransactionScope _scope = new TransactionScope();

        public DistributedTransactionCoordinator(params IUnitOfWork[] unitOfWorks)
            : base(unitOfWorks)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _scope.Dispose(); }

        }


        public override void Commit()
        {
            base.Commit();
            _scope.Complete();
        }

    }
}
