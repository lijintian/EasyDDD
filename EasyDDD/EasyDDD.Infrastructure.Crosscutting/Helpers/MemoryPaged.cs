using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EasyDDD.Infrastructure.Crosscutting.Paged;

namespace EasyDDD.Infrastructure.Crosscutting.Helpers
{
    public static class MemoryPagedHelper
    {
        public static PagedResult<T> FindInPage<T>(
            IEnumerable<T> dataSource,
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> whereExpression,
            Dictionary<Expression<Func<T, dynamic>>, SortOrder> orderBys)
            where T : class
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");


            IQueryable<T> list = null;
            if (whereExpression == null)
            {
                list = dataSource.AsQueryable<T>();
            }
            else
            {
                list = dataSource.AsQueryable<T>().Where(whereExpression);
            }
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
            int totalCount = list.Count();
            int totalPages = (totalCount + pageSize - 1) / pageSize;


            IOrderedQueryable<T> orderlist = null;

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

            List<T> pageList = null;
            if (orderlist != null) {
                pageList = orderlist.Skip(skip).Take(take).ToList();
            }
            else
            {
                pageList = list.Skip(skip).Take(take).ToList();
            }
            if (pageList == null)
            {
                pageList = new List<T>();
            }
            return new PagedResult<T>(totalCount, totalPages, pageSize, pageNumber, pageList);

        }
    }
}
