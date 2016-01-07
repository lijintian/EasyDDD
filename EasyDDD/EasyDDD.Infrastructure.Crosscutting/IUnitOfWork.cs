using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyDDD.Infrastructure.Crosscutting
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 获得一个<see cref="System.Boolean"/>值,该值表示当前的Unit Of Work是否支持Microsoft分布式事务处理机制。
        /// </summary>
        bool DistributedTransactionSupported { get; }
        /// <summary>
        /// 获得一个<see cref="System.Boolean"/>值,该值表述了当前的Unit Of Work事务是否已被提交。
        /// </summary>
        bool Committed { get; }
        /// <summary>
        /// 提交当前的Unit Of Work事务。
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚当前的Unit Of Work事务。
        /// </summary>
        void Rollback();

    }
}
