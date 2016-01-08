using ConsoleDemo.Domain;
using EasyDDD.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDemo.Repository
{
    /// <summary>
    /// Step:声明IRepositoryDemo 仓储接口 继承IRepository<BusinessDemoAgg> 用于定义BusinessDemoAgg 持久化
    /// </summary>
    public interface IRepositoryDemo :IRepository<BusinessDemoAgg>
    {
        List<BusinessDemoAgg> NotBaseMethod();
    }
}
