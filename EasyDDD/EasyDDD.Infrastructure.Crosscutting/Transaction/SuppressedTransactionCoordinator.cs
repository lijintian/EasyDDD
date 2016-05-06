
namespace EasyDDD.Infrastructure.Crosscutting.Transaction
{
    /// <summary>
    /// 弱事物协调器
    /// </summary>
    internal class SuppressedTransactionCoordinator : TransactionCoordinator
    {
        public SuppressedTransactionCoordinator(params IUnitOfWork[] unitOfWorks)
            : base(unitOfWorks)
        {
        }
    }
}
