using EasyDDD.Core.Aggregate;
using EasyDDD.Infrastructure.Crosscutting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyDDD.Core.Repository
{
    public abstract class RepositoryContext : DisposableObject, IRepositoryContext
    {
        #region Private Fields
        private readonly Guid _id = Guid.NewGuid();
        protected readonly ThreadLocal<Dictionary<string, object>> _localNewCollection = new ThreadLocal<Dictionary<string, object>>(() => new Dictionary<string, object>());
        protected readonly ThreadLocal<Dictionary<string, object>> _localModifiedCollection = new ThreadLocal<Dictionary<string, object>>(() => new Dictionary<string, object>());
        protected readonly ThreadLocal<Dictionary<string, object>> _localDeletedCollection = new ThreadLocal<Dictionary<string, object>>(() => new Dictionary<string, object>());
        protected readonly ThreadLocal<bool> _localCommitted = new ThreadLocal<bool>(() => true);
        //private readonly ThreadLocal<Dictionary<IAggregateRoot, List<IEvent>>> pendingEvents = new ThreadLocal<Dictionary<IAggregateRoot, List<IEvent>>>(() => new Dictionary<IAggregateRoot, List<IEvent>>());
        private readonly object _sync = new object();
        #endregion

        #region Ctor
        public RepositoryContext()
        {
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Clears all the registration in the repository context.
        /// </summary>
        /// <remarks>Note that this can only be called after the repository context has successfully committed.</remarks>
        protected void ClearRegistrations()
        {
            this._localNewCollection.Value.Clear();
            this._localModifiedCollection.Value.Clear();
            this._localDeletedCollection.Value.Clear();
        }
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._localCommitted.Dispose();
                this._localDeletedCollection.Dispose();
                this._localModifiedCollection.Dispose();
                this._localNewCollection.Dispose();
            }
        }

        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be added to the repository.
        /// </summary>
        protected IEnumerable<KeyValuePair<string, object>> NewCollection
        {
            get { return _localNewCollection.Value; }
        }
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be modified in the repository.
        /// </summary>
        protected IEnumerable<KeyValuePair<string, object>> ModifiedCollection
        {
            get { return _localModifiedCollection.Value; }
        }
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be deleted from the repository.
        /// </summary>
        protected IEnumerable<KeyValuePair<string, object>> DeletedCollection
        {
            get { return _localDeletedCollection.Value; }
        }
        #endregion

        #region IRepositoryContext Members
        /// <summary>
        /// Gets the Id of the repository context.
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }
        /// <summary>
        /// Registers a new object to the repository context.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterNew<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot
        {
            if (obj.Id.Equals(string.Empty))
                throw new ArgumentException("The Id of the object is empty.", "obj");
            if (_localModifiedCollection.Value.ContainsKey(obj.Id))
                throw new InvalidOperationException("The object cannot be registered as a new object since it was marked as modified.");
            if (_localNewCollection.Value.ContainsKey(obj.Id))
                throw new InvalidOperationException("The object has already been registered as a new object.");
            _localNewCollection.Value.Add(obj.Id, obj);
            _localCommitted.Value = false;
        }
        /// <summary>
        /// Registers a modified object to the repository context.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterModified<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot
        {
            if (obj.Id.Equals(string.Empty))
                throw new ArgumentException("The Id of the object is empty.", "obj");
            if (_localDeletedCollection.Value.ContainsKey(obj.Id))
                throw new InvalidOperationException("The object cannot be registered as a modified object since it was marked as deleted.");
            if (!_localModifiedCollection.Value.ContainsKey(obj.Id) && !_localNewCollection.Value.ContainsKey(obj.Id))
                _localModifiedCollection.Value.Add(obj.Id, obj);
            _localCommitted.Value = false;
        }
        /// <summary>
        /// Registers a deleted object to the repository context.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot
        {
            if (obj.Id.Equals(string.Empty))
                throw new ArgumentException("The Id of the object is empty.", "obj");
            if (_localNewCollection.Value.ContainsKey(obj.Id))
            {
                if (_localNewCollection.Value.Remove(obj.Id))
                    return;
            }
            bool removedFromModified = _localModifiedCollection.Value.Remove(obj.Id);
            bool addedToDeleted = false;
            if (!_localDeletedCollection.Value.ContainsKey(obj.Id))
            {
                _localDeletedCollection.Value.Add(obj.Id, obj);
                addedToDeleted = true;
            }
            _localCommitted.Value = !(removedFromModified || addedToDeleted);
        }

        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// 获得一个<see cref="System.Boolean"/>值,该值表示当前的Unit Of Work是否支持Microsoft分布式事务处理机制。
        /// </summary>
        public abstract bool DistributedTransactionSupported { get; }
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates whether the UnitOfWork
        /// was committed.
        /// </summary>
        public bool Committed
        {
            get { return _localCommitted.Value; }
            protected set { _localCommitted.Value = value; }
        }

        /// <summary>
        /// Commits the UnitOfWork.
        /// </summary>
        public abstract void Commit();
        /// <summary>
        /// Rolls-back the UnitOfWork.
        /// </summary>
        public abstract void Rollback();

        #endregion
    }
}
