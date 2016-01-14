using EasyDDD.Core.Repository;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;


namespace EasyDDD.Infrastructure.Data.EntityFramework
{
    public class EntityFrameworkRepositoryContext : RepositoryContext, IEntityFrameworkRepositoryContext
    {
        private readonly ThreadLocal<DbContext> _localCtx = new ThreadLocal<DbContext>();

        public EntityFrameworkRepositoryContext(DbContext dbContext)
        {
            _localCtx.Value = dbContext;
        }

        public override void RegisterNew<TAggregateRoot>(TAggregateRoot obj)
        {
            _localCtx.Value.Set<TAggregateRoot>().Add(obj);
            Committed = false;
        }

        public override void RegisterModified<TAggregateRoot>(TAggregateRoot obj)
        {
            _localCtx.Value.Entry<TAggregateRoot>(obj).State = EntityState.Modified;
            Committed = false;
        }

        public override void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj)
        {
            _localCtx.Value.Set<TAggregateRoot>().Remove(obj);
            Committed = false;
        }

        public override bool DistributedTransactionSupported
        {
            get { return true; }
        }

        public override void Commit()
        {
            if (!Committed)
            {
                var validationErrors = _localCtx.Value.GetValidationErrors();
                var dbEntityValidationResults = validationErrors as DbEntityValidationResult[] ?? validationErrors.ToArray();
                if (dbEntityValidationResults.Any())
                {
                    throw new DbEntityValidationException("Entity Validation Error", dbEntityValidationResults);
                }
                var count = _localCtx.Value.SaveChanges();
                Committed = true;
            }
        }

        public override void Rollback()
        {
            if (!Committed)
                Commit();
            _localCtx.Value.Dispose();
            _localCtx.Dispose();
            base.Dispose();
        }

        public System.Data.Entity.DbContext Context
        {
            get { return _localCtx.Value; }
        }
    }
}
