using ConsoleDemo.Domain;
using EasyDDD.Core;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDemo
{
    /// <summary>
    /// Step:注册业务Aggregate和数据库表明的关系
    /// </summary>
    public class Mapping : IMapping
    {
        public void Map()
        {
            BsonClassMap.RegisterClassMap<BusinessDemoAgg>();
        }


        public System.Collections.Generic.Dictionary<System.Type, string> GetEntityVsTableNameMappingInfos()
        {
            var dic = new Dictionary<Type, string>();
            dic.Add(typeof(BusinessDemoAgg), "BusinessDemoAgg");
            return dic;
        }
    }
}
