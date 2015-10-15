namespace Encompass.MatchSync
{
    /// <summary>
    /// Interface defining that an object can be used in MatchSync
    /// </summary>
    public interface IMatch
    {
        /// <summary>
        /// A unique ID to match on
        /// </summary>
        string MatchId { get; }
    }
}
