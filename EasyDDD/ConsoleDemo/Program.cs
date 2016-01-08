using ConsoleDemo.Domain;
using ConsoleDemo.Repository;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Infrastructure.Crosscutting.Ioc.Unity;
using EasyDDD.Infrastructure.Data.MongoDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ConsoleDemo
{
    class Program
    {
        /// <summary>
        /// Step:引用EasyDDD.Core
        /// Step:引用EasyDDD.Infrastructure.Crosscutting
        /// Step:引用EasyDDD.Infrastructure.Crosscutting.Ioc.Unity
        /// Step:引用EasyDDD.Infrastructure.Data.MongoDB
        /// 以上可做成NuGet包
        /// Step:NuGet安装Mongo C# driver
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            UnityDependencyResolver unityDependencyResolver = new UnityDependencyResolver(); 
            IoC.Initialize(unityDependencyResolver);
            var context = IoC.Resolve<IMongoDBRepositoryContext>();

            var repository =  new  RepositoryDemo(context);
            var businessAgg = new BusinessDemoAgg() { StringProperty = "Test", IntProperty = 1, DateTimeProperty = DateTime.Now };
            repository.Add(businessAgg);

            repository.Context.Commit();

            Console.ReadKey();
        }
    }
}
