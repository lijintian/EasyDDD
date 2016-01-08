using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDDD.Core
{
    public interface IMapping
    {
        void Map();

        /// <summary>
        /// 获取实体和表名的映射关系, Mongodb 专有
        /// </summary>
        /// <returns></returns>
        Dictionary<Type, string> GetEntityVsTableNameMappingInfos();


    }
}
