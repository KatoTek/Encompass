using System;

namespace Encompass.Concepts.DateRanges
{
    /// <summary>
    ///     Defines the contract for an open ended date range
    /// </summary>
    public interface IOpenEndedDateRange
    {
        #region properties

        /// <summary>
        ///     The end date of the date range
        /// </summary>
        DateTime? End { get; set; }

        /// <summary>
        ///     The start date of the date range
        /// </summary>
        DateTime Start { get; set; }

        #endregion
    }
}
