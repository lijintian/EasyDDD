using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDDD.Core.Aggregate
{
    public interface IEntity
    {
        #region Properties
        /// <summary>
        /// 获取当前领域实体类的全局唯一标识。
        /// </summary>
        string Id { get; }
        #endregion
    }
}
