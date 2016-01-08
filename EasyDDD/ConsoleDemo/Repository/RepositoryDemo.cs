using ConsoleDemo.Domain;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Data.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDemo.Repository
{
    /// <summary>
    /// Step:声明仓储RepositoryDemo，继承具体要持久化的数据库仓储泛型类，实现持久化操作
    /// </summary>
    public class RepositoryDemo :  MongoDBRepository<BusinessDemoAgg>, IRepositoryDemo
    {
        public RepositoryDemo(IRepositoryContext context)
            : base(context)
        {
                  
        }

        /// <summary>
        /// 非基础方法（如复杂的报表统计），需写对应的数据库命令来实现仓储操作
        /// </summary>
        /// <returns></returns>
        public List<BusinessDemoAgg> NotBaseMethod()
        {
            //一些数据库命令
            return null;
        }
    }
}
