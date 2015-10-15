using System;

#pragma warning disable 1591

namespace Encompass.Concepts.DateRanges
{
    /// <summary>
    /// Object that represents a date range
    /// </summary>
    public class DateRange : IDateRange
    {
        public DateRange()
        {
            var now = DateTime.Now;
            Start = now;
            End = now;
        }

        /// <param name="start">The start date of the date range</param>
        /// <param name="end">The end date of the date range</param>
        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// The end date of the date range
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// The start date of the date range
        /// </summary>
        public DateTime Start { get; set; }
    }
}

#pragma warning restore 1591
