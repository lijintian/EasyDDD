using EasyDDD.Core.IdGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDDD.Core.Aggregate
{
    public class AggregateRoot:IAggregateRoot
    {

        protected string _id;

        protected AggregateRoot()
        {
            this.GenerateNewIdentity();
        }

        protected AggregateRoot(string id)
        {
            _id = id;
        }

        public byte[] RowVersion { get; set; }

        public string Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        #region Public Methods

        /// <summary>
        ///     Check if this entity is transient, ie, without identity at this moment
        /// </summary>
        /// <returns>True if entity is transient, else false</returns>
        public bool IsTransient()
        {
            return string.IsNullOrEmpty(Id);
        }

        /// <summary>
        ///     Generate identity for this entity
        /// </summary>
        public void GenerateNewIdentity()
        {
            if (IsTransient())
                Id = IdentityGeneratorFactory.Instance.NewId();
        }

        /// <summary>
        ///     Change current identity for a new non transient identity
        /// </summary>
        /// <param name="identity">the new identity</param>
        public void ChangeCurrentIdentity(string identity)
        {
            if (identity != string.Empty)
                Id = identity;
        }

        #endregion

        #region Override Methods

        /// <summary>
        ///     确定指定的Object是否等于当前的Object。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns>如果指定的Object与当前Object相等，则返回true，否则返回false。</returns>
        /// <remarks>
        ///     有关此函数的更多信息，请参见：http://msdn.microsoft.com/zh-cn/library/system.object.equals。
        /// </remarks>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            var ar = obj as IAggregateRoot;
            if (ar == null)
                return false;
            return _id == ar.Id;
        }

        /// <summary>
        ///     用作特定类型的哈希函数。
        /// </summary>
        /// <returns>当前Object的哈希代码。</returns>
        /// <remarks>
        ///     有关此函数的更多信息，请参见：http://msdn.microsoft.com/zh-cn/library/system.object.gethashcode。
        /// </remarks>
        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public static bool operator ==(AggregateRoot left, AggregateRoot right)
        {
            if (Equals(left, null))
                return (Equals(right, null)) ? true : false;
            return left.Equals(right);
        }

        public static bool operator !=(AggregateRoot left, AggregateRoot right)
        {
            return !(left == right);
        }

        #endregion
    }
}
