using System;
using System.Collections.Generic;
using System.Linq;
using static Encompass.MatchSync.SyncResult;

namespace Encompass.MatchSync
{
    /// <summary>
    ///     A class that provides ways of syncing collections of IMatch objects
    /// </summary>
    public static class MatchSync
    {
        #region methods

        /// <summary>
        ///     Determines if the one IMatch collection matches another IMatch collection
        /// </summary>
        /// <typeparam name="TSource">The type of the source collection</typeparam>
        /// <typeparam name="TDestination">The type of the destination collection</typeparam>
        /// <param name="source">The source collection to use for comparison</param>
        /// <param name="destination">The destination collection to use for comparison</param>
        /// <returns>True if the collections match, otherwise false</returns>
        public static bool IsMatch<TSource, TDestination>(IEnumerable<TSource> source, IEnumerable<TDestination> destination) where TSource : IMatch, IComparable<TDestination>
            where TDestination : IMatch
        {
            if (source == null && destination == null)
                return true;

            if (source == null || destination == null)
                return false;

            var sourceArray = source as TSource[] ?? source.ToArray();
            var destinationsArray = destination as TDestination[] ?? destination.ToArray();
            if ((from x in sourceArray
                 select (IMatch)x).Except(from x in destinationsArray
                                          select (IMatch)x,
                                          new MatchComparer())
                                  .Any())
                return false;

            if ((from s in sourceArray
                 join d in destinationsArray on s.MatchId equals d.MatchId
                 select new { s, d }).Any(u => u.s.CompareTo(u.d) != 0))
                return false;

            return !(from x in destinationsArray
                     select (IMatch)x).Except(from x in sourceArray
                                              select (IMatch)x,
                                              new MatchComparer())
                                      .Any();
        }

        /// <summary>
        ///     Syncs the destination IMatch collection using the source IMatch collection
        /// </summary>
        /// <typeparam name="TSource">The type of the source collection</typeparam>
        /// <typeparam name="TDestination">The type of the destination collection</typeparam>
        /// <param name="source">The source collection which will be used to persist changes to the destination</param>
        /// <param name="destination">The destination collection which will be updated based on the source collection</param>
        /// <returns>A collection of Sync object, which determine what took place during the sync process</returns>
        public static List<Sync<TSource, TDestination>> Sync<TSource, TDestination>(IEnumerable<TSource> source, IEnumerable<TDestination> destination)
            where TSource : IMatch, IComparable<TDestination> where TDestination : IMatch
        {
            if (source == null)
                source = new List<TSource>();

            if (destination == null)
                destination = new List<TDestination>();

            var sourceArray = source as TSource[] ?? source.ToArray();
            var destinationArray = destination as TDestination[] ?? destination.ToArray();
            var result = (from TSource s in (from x in sourceArray
                                             select (IMatch)x).Except(from x in destinationArray
                                                                      select (IMatch)x,
                                                                      new MatchComparer())
                          select new Sync<TSource, TDestination>(s)).ToList();

            result.AddRange((from s in sourceArray
                             join d in destinationArray on s.MatchId equals d.MatchId
                             select new { s, d }).Select(u => new Sync<TSource, TDestination>(u.s, u.d)));

            result.AddRange(from TDestination d in (from x in destinationArray
                                                    select (IMatch)x).Except(from x in sourceArray
                                                                             select (IMatch)x,
                                                                             new MatchComparer())
                            select new Sync<TSource, TDestination>(d));

            return result;
        }

        /// <summary>
        ///     Syncs the destination IMatch collection using the source IMatch collection
        /// </summary>
        /// <typeparam name="TSource">The type of the source collection</typeparam>
        /// <typeparam name="TDestination">The type of the destination collection</typeparam>
        /// <param name="source">The source collection which will be used to persist changes to the destination</param>
        /// <param name="destination">The destination collection which will be updated based on the source collection</param>
        /// <param name="insert">The function to run when an object should be inserted into the destination collection</param>
        /// <param name="update">The action to run when an object should be updated in the destination collection</param>
        /// <returns>A collection of Sync object, which determine what took place during the sync process</returns>
        public static List<Sync<TSource, TDestination>> Sync<TSource, TDestination>(IEnumerable<TSource> source,
                                                                                    IList<TDestination> destination,
                                                                                    Func<TSource, TDestination> insert,
                                                                                    Action<TSource, TDestination> update) where TSource : IMatch, IComparable<TDestination>
            where TDestination : IMatch => Sync(source, destination, insert, update, null);

        /// <summary>
        ///     Syncs the destination IMatch collection using the source IMatch collection
        /// </summary>
        /// <typeparam name="TSource">The type of the source collection</typeparam>
        /// <typeparam name="TDestination">The type of the destination collection</typeparam>
        /// <param name="source">The source collection which will be used to persist changes to the destination</param>
        /// <param name="destination">The destination collection which will be updated based on the source collection</param>
        /// <param name="insert">The function to run when an object is inserted into the destination collection</param>
        /// <param name="update">The action to run when an object is updated in the destination collection</param>
        /// <param name="onDeleted">The action to run when an object is deleted from the destination collection</param>
        /// <returns>A collection of Sync object, which determine what took place during the sync process</returns>
        public static List<Sync<TSource, TDestination>> Sync<TSource, TDestination>(IEnumerable<TSource> source,
                                                                                    IList<TDestination> destination,
                                                                                    Func<TSource, TDestination> insert,
                                                                                    Action<TSource, TDestination> update,
                                                                                    Action<TDestination> onDeleted) where TSource : IMatch, IComparable<TDestination>
            where TDestination : IMatch => Sync(source, destination, insert, update, null, onDeleted);

        /// <summary>
        ///     Syncs the destination IMatch collection using the source IMatch collection
        /// </summary>
        /// <typeparam name="TSource">The type of the source collection</typeparam>
        /// <typeparam name="TDestination">The type of the destination collection</typeparam>
        /// <param name="source">The source collection which will be used to persist changes to the destination</param>
        /// <param name="destination">The destination collection which will be updated based on the source collection</param>
        /// <param name="insert">The function to run when an object is inserted into the destination collection</param>
        /// <param name="update">The action to run when an object is updated in the destination collection</param>
        /// <param name="onIgnore">The action to run when an object is the same in the source and destination collections</param>
        /// <param name="onDeleted">The action to run when an object is deleted from the destination collection</param>
        /// <returns>A collection of Sync object, which determine what took place during the sync process</returns>
        public static List<Sync<TSource, TDestination>> Sync<TSource, TDestination>(IEnumerable<TSource> source,
                                                                                    IList<TDestination> destination,
                                                                                    Func<TSource, TDestination> insert,
                                                                                    Action<TSource, TDestination> update,
                                                                                    Action<TSource, TDestination> onIgnore,
                                                                                    Action<TDestination> onDeleted) where TSource : IMatch, IComparable<TDestination>
            where TDestination : IMatch
        {
            if (source == null)
                source = new List<TSource>();

            if (destination == null)
                destination = new List<TDestination>();

            if (insert == null)
                throw new ArgumentNullException(nameof(insert));

            if (update == null)
                throw new ArgumentNullException(nameof(update));

            var result = Sync(source, destination);

            lock (destination)
            {
                foreach (var sync in result.Where(r => r.Result.Equals(Add)))
                    destination.Add(insert(sync.Source));

                foreach (var sync in result.Where(r => r.Result.Equals(Update)))
                    update(sync.Source, sync.Destination);

                foreach (var sync in result.Where(r => r.Result.Equals(Delete)))
                {
                    destination.Remove(sync.Destination);
                    onDeleted?.Invoke(sync.Destination);
                }

                if (onIgnore == null)
                    return result;

                foreach (var sync in result.Where(r => r.Result.Equals(Ignore)))
                    onIgnore(sync.Source, sync.Destination);
            }

            return result;
        }

        #endregion
    }
}
