using System.Collections.Generic;

namespace Encompass.MatchSync
{
    /// <summary>
    ///     Defines MatchSynce comparison methods
    /// </summary>
    internal class MatchComparer : IEqualityComparer<IMatch>
    {
        #region methods

        /// <summary>
        ///     Determines whether an IMatch object is equal to another IMatch object
        /// </summary>
        /// <param name="source">The source IMatch object to compare</param>
        /// <param name="destination">The destination IMatch object to compare</param>
        /// <returns>True if source is equal to destination, otherwise false</returns>
        public bool Equals(IMatch source, IMatch destination) => source.MatchId.Equals(destination.MatchId);

        /// <summary>
        ///     Returns the hash code for a IMatch.MatchId
        /// </summary>
        /// <param name="m">The IMatch object for which to get the hash code</param>
        /// <returns>The IMatch.MatchId hash code</returns>
        public int GetHashCode(IMatch m) => m.MatchId.GetHashCode();

        #endregion
    }
}
