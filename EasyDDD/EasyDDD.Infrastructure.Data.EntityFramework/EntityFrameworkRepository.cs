
using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Repository;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Infrastructure.Crosscutting.Paged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyDDD.Infrastructure.Data.EntityFramework
{
    public class EntityFrameworkRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class ,IAggregateRoot
    {
        private readonly IEntityFrameworkRepositoryContext _context;

        public EntityFrameworkRepository(IRepositoryContext context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                _context = context as IEntityFrameworkRepositoryContext;
        }

        public void Add(TAggregateRoot aggregateRoot)
        {
            _context.RegisterNew(aggregateRoot);
        }

        public TAggregateRoot GetByKey(string key)
        {
            return _context.Context.Set<TAggregateRoot>().FirstOrDefault(c => c.Id == key);
        }

        public TAggregateRoot Get(ISpecification<TAggregateRoot> specification)
        {
            Check.Argument.IsNotNull(specification, "specification");
            return _context.Context.Set<TAggregateRoot>().Where(specification.GetExpression()).FirstOrDefault();
        }

        public IList<TAggregateRoot> GetList()
        {
            return _context.Context.Set<TAggregateRoot>().ToList();
        }

        public IList<TAggregateRoot> GetList(ISpecification<TAggregateRoot> specification)
        {
            if (specification == null)
                return GetList();
            return _context.Context.Set<TAggregateRoot>().Where(specification.GetExpression()).ToList();
        }

        public bool Exists(ISpecification<TAggregateRoot> specification)
        {
            Check.Argument.IsNotNull(specification, "specification");
            return _context.Context.Set<TAggregateRoot>().Any(specification.GetExpression());
        }

        public void Remove(TAggregateRoot aggregateRoot)
        {
            _context.RegisterDeleted(aggregateRoot);
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            _context.RegisterModified(aggregateRoot);
        }

        public Crosscutting.Paged.PagedResult<TAggregateRoot> FindInPage(int pageNumber, int pageSize, ISpecification<TAggregateRoot> specification, Dictionary<System.Linq.Expressions.Expression<Func<TAggregateRoot, dynamic>>, Crosscutting.Paged.SortOrder> orderBys)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");

            IQueryable<TAggregateRoot> list;
            if (specification == null) list = _context.Context.Set<TAggregateRoot>().AsQueryable();
            else
            {
                list = _context.Context.Set<TAggregateRoot>()
                .Where(specification.GetExpression());
            }

            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
            int totalCount = list.Count();
            int totalPages = (totalCount + pageSize - 1) / pageSize;


            IOrderedQueryable<TAggregateRoot> orderlist = null;

            if (orderBys != null && orderBys.Count > 0)
            {
                foreach (var item in orderBys)
                {
                    if (item.Value == SortOrder.Descending)
                    {
                        orderlist = orderlist == null
                            ? list.OrderByDescending(item.Key)
                            : orderlist.ThenByDescending(item.Key);
                    }
                    else
                    {
                        orderlist = orderlist == null ? list.OrderBy(item.Key) : orderlist.ThenBy(item.Key);
                    }
                }
            }
            else
            {
                orderlist = list.OrderBy(c => c.Id);
            }

            List<TAggregateRoot> pageList = null;
            if (orderlist != null) pageList = orderlist.Skip(skip).Take(take).ToList();
            if (pageList == null) pageList = new List<TAggregateRoot>();
            return new PagedResult<TAggregateRoot>(totalCount, totalPages, pageSize, pageNumber, pageList);
        }
    }
}
