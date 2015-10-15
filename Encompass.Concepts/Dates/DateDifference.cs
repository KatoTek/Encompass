using System;
using Encompass.Concepts.DateRanges;

namespace Encompass.Concepts.Dates
{
    /// <summary>
    /// Object that represents the difference between two DateTimes
    /// </summary>
    public class DateDifference
    {
        private readonly DateTime _earlier;
        private readonly DateTime _later;

        /// <param name="date1">The first date</param>
        /// <param name="date2">The second date</param>
        public DateDifference(DateTime date1, DateTime date2)
        {
            if (date1 <= date2)
            {
                _earlier = date1;
                _later = date2;
            }
            else
            {
                _earlier = date2;
                _later = date1;
            }
        }

        /// <param name="dateRange">A date range to determine the difference of</param>
        public DateDifference(IDateRange dateRange)
        {
            if (dateRange.Start <= dateRange.End)
            {
                _earlier = dateRange.Start;
                _later = dateRange.End;
            }
            else
            {
                _earlier = dateRange.End;
                _later = dateRange.Start;
            }
        }

        /// <param name="dateRange">An open ended date range to determine the difference of</param>
        /// <exception cref="ArgumentException">End date of the date range must be greater than its start</exception>
        public DateDifference(IOpenEndedDateRange dateRange)
        {
            if (dateRange.End.HasValue)
            {
                if (dateRange.Start <= dateRange.End.Value)
                {
                    _earlier = dateRange.Start;
                    _later = dateRange.End.Value;
                }
                else
                {
                    _earlier = dateRange.End.Value;
                    _later = dateRange.Start;
                }
            }
            else
                throw new ArgumentException($"End date must be greater than {dateRange.Start:F}", nameof(dateRange));
        }

        /// <summary>
        /// The number of months between the first date and the second date
        /// </summary>
        public int Months => (_later.Month - _earlier.Month) + 12*(_later.Year - _earlier.Year);
    }
}
