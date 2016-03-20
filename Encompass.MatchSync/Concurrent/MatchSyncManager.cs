using System;
using System.Collections.Generic;
using System.Linq;
using static System.GC;
using static Encompass.MatchSync.SyncResult;

#pragma warning disable 1591

namespace Encompass.MatchSync.Concurrent
{
    /// <summary>
    ///     Manager class that provides ways of syncing collections of IMatch objects concurrently so that multiple instances
    ///     can be working with the same data without wiping out changes across instances effectively avoiding the "last one
    ///     wins" result.
    /// </summary>
    /// <typeparam name="TSource">The type of the source collection</typeparam>
    /// <typeparam name="TWorking">The type of the working collection</typeparam>
    [Serializable]
    public class MatchSyncManager<TSource, TWorking> : IDisposable where TSource : IMatch where TWorking : IMatch, IComparable<TWorking>, IComparable<TSource>
    {
        #region fields

        private bool _disposed;
        private object _locker = new object();
        private Dictionary<ConcurrencyToken, RegisteredList<TWorking>> _registeredLists = new Dictionary<ConcurrencyToken, RegisteredList<TWorking>>();
        private Converter<TSource, TWorking> _sourceCollectionItemConverter;

        #endregion

        #region constructors

        /// <summary>
        ///     Manager class for Syncing that attempts to address concurrency concerns.
        /// </summary>
        /// <param name="sourceCollectionConverter">A converter needed to handle converting a source type to a working type.</param>
        public MatchSyncManager(Converter<TSource, TWorking> sourceCollectionConverter)
        {
            if (sourceCollectionConverter == null)
                throw new ArgumentNullException(nameof(sourceCollectionConverter));

            _sourceCollectionItemConverter = sourceCollectionConverter;
        }

        /// <summary>
        ///     Disposes the manager.
        /// </summary>
        ~MatchSyncManager()
        {
            Dispose(false);
        }

        #endregion

        #region methods

        /// <summary>
        ///     Performs a sync between the working and source collections.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <param name="onDeleted">
        ///     A function that allows additional work to be accomplished when a delete is occuring on the
        ///     source collection.
        /// </param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> ContinuousSync(RegisteredList<TWorking> workingCollection,
                                                       IList<TSource> sourceCollection,
                                                       Func<TWorking, TSource> insert,
                                                       Action<TWorking, TSource> update,
                                                       Action<TSource> onDeleted)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            if (onDeleted == null)
                throw new ArgumentNullException(nameof(onDeleted));

            return ConcurrentSync(workingCollection, sourceCollection, insert, update, onDeleted, null, null, null);
        }

        /// <summary>
        ///     Performs a sync between the working and source collections.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> ContinuousSync(RegisteredList<TWorking> workingCollection,
                                                       IList<TSource> sourceCollection,
                                                       Func<TWorking, TSource> insert,
                                                       Action<TWorking, TSource> update)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            return ConcurrentSync(workingCollection, sourceCollection, insert, update, null, null, null, null);
        }

        /// <summary>
        ///     Performs a sync between the working and source collections.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <param name="updatedItemConflictHandler">
        ///     A function that describes how to resolve a conflict involving changes made on
        ///     both the working item and source item.
        /// </param>
        /// <param name="sourceItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which changes
        ///     were made to an item that was deleted from the source.
        /// </param>
        /// <param name="workItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which an item was
        ///     deleted that contained changes made in the source.
        /// </param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> ContinuousSync(RegisteredList<TWorking> workingCollection,
                                                       IList<TSource> sourceCollection,
                                                       Func<TWorking, TSource> insert,
                                                       Action<TWorking, TSource> update,
                                                       UpdateItemConflictHandler<TWorking, TSource> updatedItemConflictHandler,
                                                       DeletedItemConflictHandler<TWorking> sourceItemDeletedConflictHandler,
                                                       DeletedItemConflictHandler<TSource> workItemDeletedConflictHandler)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            if (updatedItemConflictHandler == null)
                throw new ArgumentNullException(nameof(updatedItemConflictHandler));

            if (sourceItemDeletedConflictHandler == null)
                throw new ArgumentNullException(nameof(sourceItemDeletedConflictHandler));

            if (workItemDeletedConflictHandler == null)
                throw new ArgumentNullException(nameof(workItemDeletedConflictHandler));

            return ConcurrentSync(workingCollection,
                                  sourceCollection,
                                  insert,
                                  update,
                                  null,
                                  updatedItemConflictHandler,
                                  sourceItemDeletedConflictHandler,
                                  workItemDeletedConflictHandler);
        }

        /// <summary>
        ///     Performs a sync between the working and source collections.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <param name="onDeleted">
        ///     A function that allows additional work to be accomplished when a delete is occuring on the
        ///     source collection.
        /// </param>
        /// <param name="updatedItemConflictHandler">
        ///     A function that describes how to resolve a conflict involving changes made on
        ///     both the working item and source item.
        /// </param>
        /// <param name="sourceItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which changes
        ///     were made to an item that was deleted from the source.
        /// </param>
        /// <param name="workItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which an item was
        ///     deleted that contained changes made in the source.
        /// </param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> ContinuousSync(RegisteredList<TWorking> workingCollection,
                                                       IList<TSource> sourceCollection,
                                                       Func<TWorking, TSource> insert,
                                                       Action<TWorking, TSource> update,
                                                       Action<TSource> onDeleted,
                                                       UpdateItemConflictHandler<TWorking, TSource> updatedItemConflictHandler,
                                                       DeletedItemConflictHandler<TWorking> sourceItemDeletedConflictHandler,
                                                       DeletedItemConflictHandler<TSource> workItemDeletedConflictHandler)
            =>
                ContinuousSync(workingCollection,
                               sourceCollection,
                               insert,
                               update,
                               null,
                               onDeleted,
                               updatedItemConflictHandler,
                               sourceItemDeletedConflictHandler,
                               workItemDeletedConflictHandler);

        /// <summary>
        ///     Performs a sync between the working and source collections.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <param name="onIgnore">A function that describes what to do when objects have not changes.</param>
        /// <param name="onDeleted">
        ///     A function that allows additional work to be accomplished when a delete is occuring on the
        ///     source collection.
        /// </param>
        /// <param name="updatedItemConflictHandler">
        ///     A function that describes how to resolve a conflict involving changes made on
        ///     both the working item and source item.
        /// </param>
        /// <param name="sourceItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which changes
        ///     were made to an item that was deleted from the source.
        /// </param>
        /// <param name="workItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which an item was
        ///     deleted that contained changes made in the source.
        /// </param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> ContinuousSync(RegisteredList<TWorking> workingCollection,
                                                       IList<TSource> sourceCollection,
                                                       Func<TWorking, TSource> insert,
                                                       Action<TWorking, TSource> update,
                                                       Action<TWorking, TSource> onIgnore,
                                                       Action<TSource> onDeleted,
                                                       UpdateItemConflictHandler<TWorking, TSource> updatedItemConflictHandler,
                                                       DeletedItemConflictHandler<TWorking> sourceItemDeletedConflictHandler,
                                                       DeletedItemConflictHandler<TSource> workItemDeletedConflictHandler)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            if (onDeleted == null)
                throw new ArgumentNullException(nameof(onDeleted));

            if (updatedItemConflictHandler == null)
                throw new ArgumentNullException(nameof(updatedItemConflictHandler));

            if (sourceItemDeletedConflictHandler == null)
                throw new ArgumentNullException(nameof(sourceItemDeletedConflictHandler));

            if (workItemDeletedConflictHandler == null)
                throw new ArgumentNullException(nameof(workItemDeletedConflictHandler));

            return ConcurrentSync(workingCollection,
                                  sourceCollection,
                                  insert,
                                  update,
                                  onIgnore,
                                  onDeleted,
                                  updatedItemConflictHandler,
                                  sourceItemDeletedConflictHandler,
                                  workItemDeletedConflictHandler);
        }

        /// <summary>
        ///     Disposes the manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            SuppressFinalize(this);
        }

        /// <summary>
        ///     Initializes a sync operation by registering the current state of the source used for subsequent syncs.
        /// </summary>
        /// <param name="sourceCollection">The source collection to register.</param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> Register(IEnumerable<TSource> sourceCollection)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            if (sourceCollection == null)
                sourceCollection = new List<TSource>();

            var collection = sourceCollection as IList<TSource> ?? sourceCollection.ToList();
            var registered = new RegisteredList<TSource>(new ConcurrencyToken(Guid.NewGuid()
                                                                                  .ToString()),
                                                         collection).ConvertAll(_sourceCollectionItemConverter);

            lock (_locker)
            {
                _registeredLists.Add(registered.Token, registered);
            }

            return new RegisteredList<TSource>(registered.Token, collection).ConvertAll(_sourceCollectionItemConverter);
        }

        /// <summary>
        ///     Performs a sync between the working and source collections.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <param name="onDeleted">
        ///     A function that allows additional work to be accomplished when a delete is occuring on the
        ///     source collection.
        /// </param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> Sync(RegisteredList<TWorking> workingCollection,
                                             IList<TSource> sourceCollection,
                                             Func<TWorking, TSource> insert,
                                             Action<TWorking, TSource> update,
                                             Action<TSource> onDeleted)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            if (onDeleted == null)
                throw new ArgumentNullException(nameof(onDeleted));

            return UpdateRegisteredCollection(ConcurrentSync(workingCollection, sourceCollection, insert, update, onDeleted, null, null, null), sourceCollection);
        }

        /// <summary>
        ///     Performs a sync between the working and source collections.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> Sync(RegisteredList<TWorking> workingCollection,
                                             IList<TSource> sourceCollection,
                                             Func<TWorking, TSource> insert,
                                             Action<TWorking, TSource> update)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            return UpdateRegisteredCollection(ConcurrentSync(workingCollection, sourceCollection, insert, update, null, null, null, null), sourceCollection);
        }

        /// <summary>
        ///     Performs a sync between the working and source collections.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <param name="updatedItemConflictHandler">
        ///     A function that describes how to resolve a conflict involving changes made on
        ///     both the working item and source item.
        /// </param>
        /// <param name="sourceItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which changes
        ///     were made to an item that was deleted from the source.
        /// </param>
        /// <param name="workItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which an item was
        ///     deleted that contained changes made in the source.
        /// </param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> Sync(RegisteredList<TWorking> workingCollection,
                                             IList<TSource> sourceCollection,
                                             Func<TWorking, TSource> insert,
                                             Action<TWorking, TSource> update,
                                             UpdateItemConflictHandler<TWorking, TSource> updatedItemConflictHandler,
                                             DeletedItemConflictHandler<TWorking> sourceItemDeletedConflictHandler,
                                             DeletedItemConflictHandler<TSource> workItemDeletedConflictHandler)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            if (updatedItemConflictHandler == null)
                throw new ArgumentNullException(nameof(updatedItemConflictHandler));

            if (sourceItemDeletedConflictHandler == null)
                throw new ArgumentNullException(nameof(sourceItemDeletedConflictHandler));

            if (workItemDeletedConflictHandler == null)
                throw new ArgumentNullException(nameof(workItemDeletedConflictHandler));

            return
                UpdateRegisteredCollection(
                                           ConcurrentSync(workingCollection,
                                                          sourceCollection,
                                                          insert,
                                                          update,
                                                          null,
                                                          updatedItemConflictHandler,
                                                          sourceItemDeletedConflictHandler,
                                                          workItemDeletedConflictHandler),
                                           sourceCollection);
        }

        /// <summary>
        ///     Performs a sync between the working and source collections. The results of the sync on the source collection will
        ///     be registered. Therefore you must save the changes of the source collection to keep aligned with the registered
        ///     collection. Not doing so will result in unexpected behavior.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <param name="onIgnore">A function that describes what to do when objects have not changes.</param>
        /// <param name="onDeleted">
        ///     A function that allows additional work to be accomplished when a delete is occuring on the
        ///     source collection.
        /// </param>
        /// <param name="updatedItemConflictHandler">
        ///     A function that describes how to resolve a conflict involving changes made on
        ///     both the working item and source item.
        /// </param>
        /// <param name="sourceItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which changes
        ///     were made to an item that was deleted from the source.
        /// </param>
        /// <param name="workItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which an item was
        ///     deleted that contained changes made in the source.
        /// </param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> Sync(RegisteredList<TWorking> workingCollection,
                                             IList<TSource> sourceCollection,
                                             Func<TWorking, TSource> insert,
                                             Action<TWorking, TSource> update,
                                             Action<TWorking, TSource> onIgnore,
                                             Action<TSource> onDeleted,
                                             UpdateItemConflictHandler<TWorking, TSource> updatedItemConflictHandler,
                                             DeletedItemConflictHandler<TWorking> sourceItemDeletedConflictHandler,
                                             DeletedItemConflictHandler<TSource> workItemDeletedConflictHandler)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            if (onDeleted == null)
                throw new ArgumentNullException(nameof(onDeleted));

            if (updatedItemConflictHandler == null)
                throw new ArgumentNullException(nameof(updatedItemConflictHandler));

            if (sourceItemDeletedConflictHandler == null)
                throw new ArgumentNullException(nameof(sourceItemDeletedConflictHandler));

            if (workItemDeletedConflictHandler == null)
                throw new ArgumentNullException(nameof(workItemDeletedConflictHandler));

            return
                UpdateRegisteredCollection(
                                           ConcurrentSync(workingCollection,
                                                          sourceCollection,
                                                          insert,
                                                          update,
                                                          onIgnore,
                                                          onDeleted,
                                                          updatedItemConflictHandler,
                                                          sourceItemDeletedConflictHandler,
                                                          workItemDeletedConflictHandler),
                                           sourceCollection);
        }

        /// <summary>
        ///     Performs a sync between the working and source collections. The results of the sync on the source collection will
        ///     be registered. Therefore you must save the changes of the source collection to keep aligned with the registered
        ///     collection. Not doing so will result in unexpected behavior.
        /// </summary>
        /// <param name="workingCollection">A collection of objects edited in work operations.</param>
        /// <param name="sourceCollection">The originating source of objects and destination of the sync.</param>
        /// <param name="insert">
        ///     A function that describes how a working object is made into a source object that occurs when any
        ///     working object needs to be inserted into the source.
        /// </param>
        /// <param name="update">A function that describes how to update a source object from a given working object.</param>
        /// <param name="onDeleted">
        ///     A function that allows additional work to be accomplished when a delete is occuring on the
        ///     source collection.
        /// </param>
        /// <param name="updatedItemConflictHandler">
        ///     A function that describes how to resolve a conflict involving changes made on
        ///     both the working item and source item.
        /// </param>
        /// <param name="sourceItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which changes
        ///     were made to an item that was deleted from the source.
        /// </param>
        /// <param name="workItemDeletedConflictHandler">
        ///     A function that describes how to resolve a conflict in which an item was
        ///     deleted that contained changes made in the source.
        /// </param>
        /// <returns>A Registered list of type <typeparamref name="TWorking" />.</returns>
        public RegisteredList<TWorking> Sync(RegisteredList<TWorking> workingCollection,
                                             IList<TSource> sourceCollection,
                                             Func<TWorking, TSource> insert,
                                             Action<TWorking, TSource> update,
                                             Action<TSource> onDeleted,
                                             UpdateItemConflictHandler<TWorking, TSource> updatedItemConflictHandler,
                                             DeletedItemConflictHandler<TWorking> sourceItemDeletedConflictHandler,
                                             DeletedItemConflictHandler<TSource> workItemDeletedConflictHandler)
            =>
                Sync(workingCollection,
                     sourceCollection,
                     insert,
                     update,
                     null,
                     onDeleted,
                     updatedItemConflictHandler,
                     sourceItemDeletedConflictHandler,
                     workItemDeletedConflictHandler);

        /// <summary>
        ///     Used to finalize syncing operations on the work items passed into the collection. This should be done to prevent
        ///     memory leaks.
        /// </summary>
        /// <param name="workingCollection">The collection of working items no longer needing syncing operations performed.</param>
        public void Terminate(RegisteredList<TWorking> workingCollection)
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType() .FullName}");

            if (workingCollection == null)
                throw new ArgumentNullException(nameof(workingCollection));

            lock (_locker)
            {
                if (_registeredLists.ContainsKey(workingCollection.Token))
                    _registeredLists.Remove(workingCollection.Token);
                else
                    throw new IndexOutOfRangeException($"Token {workingCollection.Token} for registered list not found");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing) {}

            AdditionalCleanup();

            _disposed = true;
        }

        private void AdditionalCleanup()
        {
            lock (_locker)
            {
                _registeredLists.Clear();
                _registeredLists = null;
            }
            _locker = null;
            _sourceCollectionItemConverter = null;
        }

        private RegisteredList<TWorking> ConcurrentSync(RegisteredList<TWorking> workingCollection,
                                                        IList<TSource> sourceCollection,
                                                        Func<TWorking, TSource> insert,
                                                        Action<TWorking, TSource> update,
                                                        Action<TSource> onDeleted,
                                                        UpdateItemConflictHandler<TWorking, TSource> updatedItemConflictHandler,
                                                        DeletedItemConflictHandler<TWorking> sourceItemDeletedConflictHandler,
                                                        DeletedItemConflictHandler<TSource> workItemDeletedConflictHandler)
            =>
                ConcurrentSync(workingCollection,
                               sourceCollection,
                               insert,
                               update,
                               null,
                               onDeleted,
                               updatedItemConflictHandler,
                               sourceItemDeletedConflictHandler,
                               workItemDeletedConflictHandler);

        private RegisteredList<TWorking> ConcurrentSync(RegisteredList<TWorking> workingCollection,
                                                        IList<TSource> sourceCollection,
                                                        Func<TWorking, TSource> insert,
                                                        Action<TWorking, TSource> update,
                                                        Action<TWorking, TSource> onIgnore,
                                                        Action<TSource> onDeleted,
                                                        UpdateItemConflictHandler<TWorking, TSource> updatedItemConflictHandler,
                                                        DeletedItemConflictHandler<TWorking> sourceItemDeletedConflictHandler,
                                                        DeletedItemConflictHandler<TSource> workItemDeletedConflictHandler)
        {
            if (workingCollection == null)
                throw new ArgumentNullException(nameof(workingCollection));

            if (insert == null)
                throw new ArgumentNullException(nameof(insert));

            if (update == null)
                throw new ArgumentNullException(nameof(update));

            if (sourceCollection == null)
                sourceCollection = new List<TSource>();

            RegisteredList<TWorking> registered;
            lock (_locker)
            {
                if (!_registeredLists.TryGetValue(workingCollection.Token, out registered))
                    throw new IndexOutOfRangeException($"Token {workingCollection.Token} for registered list not found");
            }

            var sourceSync = MatchSync.Sync(registered, sourceCollection);
            var workingSync = MatchSync.Sync(workingCollection, registered);

            //adding
            foreach (var sync in (from w in workingSync
                                  where w.Result == Add
                                  select w).Where(sync => !sourceCollection.Any(s => s.MatchId.Equals(sync.MatchId))))
                sourceCollection.Add(insert(sync.Source));

            //conflict updating deleted
            foreach (var sync in from sync in (from s in sourceSync
                                               join w in workingSync on s.MatchId equals w.MatchId
                                               where w.Result == Update && s.Result == Add
                                               select new { s, w })
                                 let resolution = sourceItemDeletedConflictHandler?.Invoke(sync.w.Source) ?? ConflictResolution.KeepSourceChanges
                                 where resolution == ConflictResolution.KeepWorkingChanges
                                 where !sourceCollection.Any(s => s.MatchId.Equals(sync.w.MatchId))
                                 select sync)
                sourceCollection.Add(insert(sync.w.Source));

            //updating
            foreach (var sync in
                from s in sourceSync
                join w in workingSync on s.MatchId equals w.MatchId
                where w.Result == Update && s.Result == Ignore
                select new { s, w })
                update(sync.w.Source, sync.s.Destination);

            //conflict updating updated
            foreach (var sync in from sync in (from s in sourceSync
                                               join w in workingSync on s.MatchId equals w.MatchId
                                               where w.Result == Update && s.Result == Update
                                               select new { s, w })
                                 let resolution = updatedItemConflictHandler?.Invoke(sync.w.Source, sync.s.Destination) ?? ConflictResolution.KeepWorkingChanges
                                 where resolution == ConflictResolution.KeepWorkingChanges
                                 select sync)
                update(sync.w.Source, sync.s.Destination);

            //deleting
            var deleted = new List<string>();
            foreach (var sync in
                from s in sourceSync
                join w in workingSync on s.MatchId equals w.MatchId
                where w.Result == Delete && s.Result == Ignore
                select new { s, w })
            {
                deleted.Add(sync.w.MatchId);
                sourceCollection.Remove(sync.s.Destination);
                onDeleted?.Invoke(sync.s.Destination);
            }
            deleted.ForEach(i => sourceSync.RemoveAll(s => s.MatchId.Equals(i)));

            //conflict deleting updated
            foreach (var sync in from sync in (from s in sourceSync
                                               join w in workingSync on s.MatchId equals w.MatchId
                                               where w.Result == Delete && s.Result == Update
                                               select new { s, w })
                                 let resolution = workItemDeletedConflictHandler?.Invoke(sync.s.Destination) ?? ConflictResolution.KeepWorkingChanges
                                 where resolution == ConflictResolution.KeepWorkingChanges
                                 select sync)
            {
                sourceCollection.Remove(sync.s.Destination);
                onDeleted?.Invoke(sync.s.Destination);
            }

            //ignoring
            if (onIgnore == null)
                return new RegisteredList<TSource>(registered.Token, sourceCollection).ConvertAll(_sourceCollectionItemConverter);

            foreach (var sync in
                from s in sourceSync
                join w in workingSync on s.MatchId equals w.MatchId
                where w.Result == Ignore && s.Result == Ignore
                select new { s, w })
                onIgnore(sync.w.Source, sync.s.Destination);

            return new RegisteredList<TSource>(registered.Token, sourceCollection).ConvertAll(_sourceCollectionItemConverter);
        }

        private RegisteredList<TWorking> UpdateRegisteredCollection(RegisteredList<TWorking> workCollection, IEnumerable<TSource> sourceCollection)
        {
            lock (_locker)
            {
                RegisteredList<TWorking> registered;
                if (!_registeredLists.TryGetValue(workCollection.Token, out registered))
                    throw new IndexOutOfRangeException($"Token {workCollection.Token} for registered list not found");

                registered.Clear();
                registered.AddRange(sourceCollection.Select(item => _sourceCollectionItemConverter(item)));
            }

            return workCollection;
        }

        #endregion
    }
}

#pragma warning restore 1591
