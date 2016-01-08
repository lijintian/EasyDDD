using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Repository;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using EasyDDD.Infrastructure.Crosscutting.Paged;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SortOrder = EasyDDD.Infrastructure.Crosscutting.Paged.SortOrder;

namespace EasyDDD.Infrastructure.Data.MongoDB
{
    public class MongoDBRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class ,IAggregateRoot
    {

        protected readonly IMongoDBRepositoryContext _mongoDBRepositoryContext;
        public MongoDBRepository(IRepositoryContext context)
        {
            var repositoryContext = context as MongoDBRepositoryContext;
            if (repositoryContext != null)
            {
                _mongoDBRepositoryContext = repositoryContext;
            }
        }

        public IRepositoryContext Context
        {
            get { return _mongoDBRepositoryContext; }
        }


        public void Add(TAggregateRoot aggregateRoot)
        {
            _mongoDBRepositoryContext.RegisterNew(aggregateRoot);
        }

        public TAggregateRoot GetByKey(string key)
        {
            MongoCollection collection = _mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));
            return collection.AsQueryable<TAggregateRoot>().FirstOrDefault(p => p.Id == key);
        }

        public TAggregateRoot Get(ISpecification<TAggregateRoot> specification)
        {
            MongoCollection collection = _mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));
            return collection.AsQueryable<TAggregateRoot>().Where(specification.GetExpression()).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有聚合根列表
        /// </summary>
        /// <returns>聚合根实例列表</returns>
        public IList<TAggregateRoot> GetList()
        {
            MongoCollection collection = _mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));
            return collection.AsQueryable<TAggregateRoot>().ToList();
        }

        public IList<TAggregateRoot> GetList(ISpecification<TAggregateRoot> specification)
        { 
            if (specification == null) return GetList();
             
            MongoCollection collection = _mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));
            
            return collection.AsQueryable<TAggregateRoot>().Where(specification.GetExpression()).ToList();

        }

        public bool Exists(ISpecification<TAggregateRoot> specification)
        {
            Check.Argument.IsNotNull(specification, "specification");

            MongoCollection collection = _mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));
            return collection.AsQueryable<TAggregateRoot>().Where(specification.GetExpression()).FirstOrDefault() !=
                   null;
        }

        public void Remove(TAggregateRoot aggregateRoot)
        {
            _mongoDBRepositoryContext.RegisterDeleted(aggregateRoot);
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            _mongoDBRepositoryContext.RegisterModified(aggregateRoot);
        }


        public PagedResult<TAggregateRoot> FindInPage(
            int pageNumber,
            int pageSize,
            ISpecification<TAggregateRoot> spceification,
            Dictionary<Expression<Func<TAggregateRoot, dynamic>>, SortOrder> orderBys)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");


            MongoCollection collection = _mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));

            IQueryable<TAggregateRoot> list = null;
            if (spceification == null)
            {
                list = collection.AsQueryable<TAggregateRoot>();
            }
            else
            {
                list = collection.AsQueryable<TAggregateRoot>().Where(spceification.GetExpression());
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
