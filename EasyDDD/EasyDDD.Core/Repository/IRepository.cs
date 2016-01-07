using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Paged;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EasyDDD.Core.Repository
{
    /// <summary>
    /// 泛型仓储
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        #region Methods
        /// <summary>
        /// 将指定的聚合根添加到仓储中。
        /// </summary>
        /// <param name="aggregateRoot">需要添加到仓储的聚合根实例。</param>
        void Add(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 根据聚合根的ID值，从仓储中读取聚合根。
        /// </summary>
        /// <param name="key">聚合根的ID值。</param>
        /// <returns>聚合根实例。</returns>
        TAggregateRoot GetByKey(string key);

        /// <summary>
        /// 通过条件获取聚合根
        /// </summary>
        /// <param name="specification">条件参数</param>
        /// <returns>聚合根实例。</returns>
        TAggregateRoot Get(ISpecification<TAggregateRoot> specification);

        /// <summary>
        /// 获取所有聚合根列表
        /// </summary>
        /// <returns>聚合根实例列表</returns>
        IList<TAggregateRoot> GetList();

        /// <summary>
        /// 通过条件获取聚合根列表
        /// </summary>
        /// <param name="specification">条件参数</param>
        /// <returns>聚合根实例列表</returns>
        IList<TAggregateRoot> GetList(ISpecification<TAggregateRoot> specification);

        /// <summary>
        /// 返回一个<see cref="Boolean"/>值，该值表示符合指定规约条件的聚合根是否存在。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <returns>如果符合指定规约条件的聚合根存在，则返回true，否则返回false。</returns>
        bool Exists(ISpecification<TAggregateRoot> specification);

        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoot">需要从仓储中移除的聚合根。</param>
        /// 
        void Remove(TAggregateRoot aggregateRoot);
        /// <summary>
        /// 更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoot">需要更新的聚合根。</param>
        void Update(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 根据指定的规约，以指定的排序字段和排序方式，以及分页参数，从仓储中读取所有聚合根。
        /// </summary>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页内行数</param>
        /// <param name="spceification">规约</param>
        /// <param name="orderBys">组合排序</param>
        /// <returns>分页结果</returns>
        PagedResult<TAggregateRoot> FindInPage(int pageNumber, int pageSize, ISpecification<TAggregateRoot> spceification,
                                                   Dictionary<Expression<Func<TAggregateRoot, dynamic>>, SortOrder> orderBys);
        #endregion
    }
}
