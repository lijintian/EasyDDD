using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDDD.Infrastructure.Crosscutting.Ioc.Unity
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Configuration section name for Unity.
        /// </summary>
        public const string DefaultUnityConfigurationSectionName = "unity";

        private readonly IUnityContainer _unityContainer;
        public UnityDependencyResolver()
            : this(DefaultUnityConfigurationSectionName)
        {

        }

        /// <summary>
        /// 通过配置文件的容器名生成unityContainer
        /// </summary>
        /// <param name="containerName">unityContainer 配置名</param>
        public UnityDependencyResolver(string containerName) 
        {
            _unityContainer = new UnityContainer();
            try
            {
                UnityConfigurationSection section = ConfigurationManager.GetSection(containerName) as UnityConfigurationSection;
                if (section != null)
                {
                    _unityContainer.LoadConfiguration(section);
                }
            }
            catch (Exception)
            {
                _unityContainer.Dispose();
                throw;
            }
        }

        public IUnityContainer UnityContainer
        {
            get { return _unityContainer; }
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public T Resolve<T>(string name)
        {
            return _unityContainer.Resolve<T>(name);
        }

        public T Resolve<T>(Type type)
        {
            var obj = _unityContainer.Resolve(type);
            return obj == null ? default(T) : (T)obj;
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }


        public IEnumerable<T> ResolveAll<T>()
        {
            return _unityContainer.ResolveAll<T>();
        }
    }
}
