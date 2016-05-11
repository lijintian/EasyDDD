using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace EasyDDD.Infrastructure.Crosscutting.Adapter.AutoMapper
{
    /// <summary>
    /// 基于AutoMapper实现的类型转换
    /// </summary>
    public class AutomapperTypeAdapter : ITypeAdapter
    {
        /// <summary>
        /// 该构造函数初始化时候，会遍历当前程序集中的所有已加载的程序集
        /// 动态获取所有继承了AutoMapper.Profile的类型来初始化AutoMap
        /// </summary>
        public AutomapperTypeAdapter()
        {
            //根据当前已加载的区获取已加载的所有程序集，并从中查询出所有类型（继承了AutoMapper.Profile的）
            var profiles = AppDomain.CurrentDomain.GetAssemblies()
                                   .SelectMany(a => a.GetLoadableTypes())
                                   .Where(t => t.BaseType == typeof(Profile)).ToList();

            InitMapper(profiles);
        }

        /// <summary>
        /// 通过指定的程序集，动态获取所有继承了AutoMapper.Profile的类型来初始化AutoMap
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        public AutomapperTypeAdapter(string assemblyName)
        {
            //加载程序集
            var assembly = Assembly.Load(assemblyName);

            var profiles = assembly.GetLoadableTypes().Where(t => t.BaseType == typeof(Profile)).ToList();

            InitMapper(profiles);
        }

        /// <summary>
        /// 初始化AutoMapper
        /// </summary>
        /// <param name="profiles">所有继承Profile的类型列表</param>
        private void InitMapper(List<Type> profiles)
        {
            //AutoMapper初始化
            Mapper.Initialize(cfg =>
            {
                //遍历所有继承了AutoMapper.Profile的对象
                foreach (var item in profiles)
                {
                    //排除AutoMapper.Profile本身
                    if (item.FullName != "AutoMapper.SelfProfiler`2")
                    {
                        //添加配置
                        cfg.AddProfile(Activator.CreateInstance(item) as Profile);
                    }
                }
            });
        }

        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            return Mapper.Map<TSource, TTarget>(source);
        }

        public TTarget Adapt<TTarget>(object source) where TTarget : class, new()
        {
            return Mapper.Map<TTarget>(source);
        }
    }


}
