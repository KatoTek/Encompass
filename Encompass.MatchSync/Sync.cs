using System;
using static Encompass.MatchSync.SyncResult;

namespace Encompass.MatchSync
{
    /// <summary>
    /// An object that indicates needs to take place for a particular IMatch object to keep the source and destination collections in sync
    /// </summary>
    /// <typeparam name="TS">The type of the source object</typeparam>
    /// <typeparam name="TD">The type of the destination object</typeparam>
    public class Sync<TS, TD> where TS : IMatch, IComparable<TD> where TD : IMatch
    {
        internal Sync(TS source)
        {
            Source = source;
        }

        internal Sync(TD destination)
        {
            Destination = destination;
        }

        internal Sync(TS source, TD destination)
        {
            Source = source;
            Destination = destination;
        }

        /// <summary>
        /// The object that will be updated
        /// </summary>
        public TD Destination { get; internal set; }

        /// <summary>
        /// A unique ID used to match instances of objects
        /// </summary>
        public string MatchId => Source != null ? Source.MatchId : Destination.MatchId;

        /// <summary>
        /// The result after syncing
        /// </summary>
        public SyncResult Result
        {
            get
            {
                if (Source != null && Destination == null)
                    return Add;

                if (Destination != null && Source == null)
                    return Delete;

                if (Destination != null && Source != null && Source.MatchId.Equals(Destination.MatchId))
                    return Source.CompareTo(Destination) == 0 ? Ignore : Update;

                return Ignore;
            }
        }

        /// <summary>
        /// The object that has the changes and will be used to make the updates
        /// </summary>
        public TS Source { get; internal set; }
    }
}
