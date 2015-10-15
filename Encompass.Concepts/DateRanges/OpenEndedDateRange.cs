using System;

#pragma warning disable 1591

namespace Encompass.Concepts.DateRanges
{
    /// <summary>
    /// Object that represents an open ended date range
    /// </summary>
    public class OpenEndedDateRange : IOpenEndedDateRange
    {
        public OpenEndedDateRange()
        {
            Start = DateTime.Now;
            End = null;
        }

        /// <param name="start">The start date of the date range</param>
        public OpenEndedDateRange(DateTime start)
        {
            Start = start;
            End = null;
        }

        /// <param name="start">The start date of the date range</param>
        /// <param name="end">The end date of the date range</param>
        public OpenEndedDateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        /// <param name="start">The start date of the date range</param>
        /// <param name="end">The end date of the date range</param>
        public OpenEndedDateRange(DateTime start, DateTime? end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// The end date of the date range
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// The start date of the date range
        /// </summary>
        public DateTime Start { get; set; }
    }
}

#pragma warning restore 1591
