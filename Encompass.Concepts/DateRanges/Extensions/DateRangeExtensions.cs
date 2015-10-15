using System;
using System.Collections.Generic;
using System.Linq;
using Encompass.Concepts.Dates;
using Encompass.Concepts.Dates.Extensions;

#pragma warning disable 1573

namespace Encompass.Concepts.DateRanges.Extensions
{
    /// <summary>
    /// DateRange extension methods
    /// </summary>
    public static class DateRangeExtensions
    {
        /// <inheritdoc cref="Date.BusinessDays(DateTime, DateTime, DateTime[])"/>
        /// <param name="dateRange">The date range to check</param>
        public static int BusinessDays(this IDateRange dateRange, params DateTime[] holidays) => dateRange == null ? 0 : Date.BusinessDays(dateRange.Start, dateRange.End, holidays);

        /// <inheritdoc cref="Date.BusinessHours(DateTime, DateTime, string, DateTime[])"/>
        /// <param name="dateRange">The date range to check</param>
        public static int BusinessHours(this IDateRange dateRange, string timeZoneId, params DateTime[] holidays) => dateRange == null ? 0 : Date.BusinessHours(dateRange.Start, dateRange.End, timeZoneId, holidays);

        /// <inheritdoc cref="Date.BusinessHours(DateTime, DateTime, string, int, int, int, int, DateTime[])"/>
        /// <param name="dateRange">The date range to check</param>
        public static int BusinessHours(this IDateRange dateRange, string timeZoneId, int startOfDayHour, int startOfDayMinute, int endOfDayHour, int endOfDayMinute, params DateTime[] holidays)
        {
            if (dateRange == null)
                return 0;

            return Date.BusinessHours(dateRange.Start, dateRange.End, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, holidays);
        }

        /// <inheritdoc cref="Date.BusinessHours(DateTime, DateTime, string, int, int, int, int, DateTime[])"/>
        /// <param name="dateRange">The date range to check</param>
        public static int BusinessHours(this IDateRange dateRange,
                                        string timeZoneId,
                                        int startOfDayHour,
                                        int startOfDayMinute,
                                        int endOfDayHour,
                                        int endOfDayMinute,
                                        int breakStartHour,
                                        int breakStartMin,
                                        int breakEndHour,
                                        int breakEndMin,
                                        params DateTime[] holidays)
            => dateRange == null ? 0 : Date.BusinessHours(dateRange.Start, dateRange.End, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, breakStartHour, breakStartMin, breakEndHour, breakEndMin, holidays);

        /// <summary>
        /// Gets all dates within a date range
        /// </summary>
        /// <param name="dateRange">The date range to work with</param>
        /// <returns>A collection of dates within the specified <paramref name="dateRange"/></returns>
        public static IEnumerable<DateTime> Dates(this IDateRange dateRange) => dateRange == null ? null : Enumerable.Range(0, dateRange.Days()).Select(index => dateRange.Start.WithoutTime().AddDays(index));

        /// <summary>
        /// Determines the number of days within a date range
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <returns>The number of days within a date range</returns>
        public static int Days(this IDateRange dateRange) => dateRange == null ? 0 : Date.Days(dateRange.Start, dateRange.End);

        /// <summary>
        /// Extends a date range by a specified number of business <paramref name="days"/>
        /// </summary>
        /// <param name="dateRange">The date range to extend</param>
        /// <param name="days">The number of business days to extend the date range</param>
        /// <param name="holidays">Holidays dates to exclude when determining business days</param>
        /// <returns>A date range where the end date is determined by adding the specified number of business <paramref name="days"/> to the original end date</returns>
        public static IDateRange ExtendBusinessDays(this IDateRange dateRange, double days, params DateTime[] holidays)
        {
            if (dateRange != null)
                dateRange.End = dateRange.End.AddBusinessDays(days, holidays);

            return dateRange;
        }

        /// <inheritdoc cref="ExtendBusinessDays(IDateRange, double, DateTime[])"/>
        public static IOpenEndedDateRange ExtendBusinessDays(this IOpenEndedDateRange dateRange, double days, params DateTime[] holidays)
        {
            if (dateRange?.End != null)
                dateRange.End = dateRange.End.Value.AddBusinessDays(days, holidays);

            return dateRange;
        }

        /// <summary>
        /// Extends a date range by a specified number of <paramref name="days"/>
        /// </summary>
        /// <param name="dateRange">The date range to extend</param>
        /// <param name="days">The number of days to extend the date range</param>
        /// <returns>A date range where the end date is determined by adding the specified number of <paramref name="days"/> to the original end date</returns>
        public static IDateRange ExtendDays(this IDateRange dateRange, double days)
        {
            if (dateRange != null)
                dateRange.End = dateRange.End.AddDays(days);

            return dateRange;
        }

        /// <inheritdoc cref="ExtendDays(IDateRange, double)"/>
        public static IOpenEndedDateRange ExtendDays(this IOpenEndedDateRange dateRange, double days)
        {
            if (dateRange?.End != null)
                dateRange.End = dateRange.End.Value.AddDays(days);

            return dateRange;
        }

        /// <summary>
        /// Determines if an open ended date range has an end date
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <returns>True if the date range has an end date</returns>
        public static bool IsClosed(this IOpenEndedDateRange dateRange) => dateRange?.End.HasValue ?? false;

        /// <summary>
        /// Determines if an open ended date range does not have an end date
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <returns>True if the date range does not have an end date</returns>
        public static bool IsOpen(this IOpenEndedDateRange dateRange) => !dateRange?.End.HasValue ?? true;

        /// <summary>
        /// Determines if a date range is valid
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <returns>True if the date range is valid</returns>
        public static bool IsValid(this IDateRange dateRange)
        {
            try
            {
                ValidityCheck(dateRange);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc cref="IsValid(IDateRange)"/>
        public static bool IsValid(this IOpenEndedDateRange dateRange)
        {
            try
            {
                ValidityCheck(dateRange);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines the number of months within a date range
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <returns>The number of months within a date range</returns>
        public static int Months(this IDateRange dateRange)
        {
            ValidityCheck(dateRange);

            return dateRange == null ? 0 : new DateDifference(dateRange).Months;
        }

        /// <inheritdoc cref="Date.Overlaps(DateTime, DateTime, DateTime, DateTime, bool)"/>
        /// <param name="dateRange">The date range compare</param>
        /// <param name="compareRange">The date range to compare to</param>
        public static bool Overlaps(this IDateRange dateRange, IDateRange compareRange, bool allowSameEndPoints = true)
        {
            if (dateRange == null || compareRange == null)
                return false;

            return Date.Overlaps(dateRange.Start, dateRange.End, compareRange.Start, compareRange.End, allowSameEndPoints);
        }

        /// <inheritdoc cref="Overlaps(IDateRange, IDateRange, bool)"/>
        public static bool Overlaps(this IDateRange dateRange, IOpenEndedDateRange compareRange, bool allowSameEndPoints = true)
        {
            if (dateRange == null || compareRange == null)
                return false;

            if (compareRange.End.HasValue)
                return Date.Overlaps(dateRange.Start, dateRange.End, compareRange.Start, compareRange.End.Value, allowSameEndPoints);

            ValidityCheck(dateRange);
            ValidityCheck(compareRange);

            return allowSameEndPoints ? dateRange.Start > compareRange.Start || dateRange.End > compareRange.Start : dateRange.Start >= compareRange.Start || dateRange.End >= compareRange.Start;
        }

        /// <inheritdoc cref="Overlaps(IDateRange, IDateRange, bool)"/>
        public static bool Overlaps(this IOpenEndedDateRange dateRange, IDateRange compareRange, bool allowSameEndPoints = true)
        {
            if (dateRange == null || compareRange == null)
                return false;

            if (dateRange.End.HasValue)
                return Date.Overlaps(dateRange.Start, dateRange.End.Value, compareRange.Start, compareRange.End, allowSameEndPoints);

            ValidityCheck(dateRange);
            ValidityCheck(compareRange);

            return allowSameEndPoints ? compareRange.Start > dateRange.Start || compareRange.End > dateRange.Start : compareRange.Start >= dateRange.Start || compareRange.End >= dateRange.Start;
        }

        /// <inheritdoc cref="Overlaps(IDateRange, IDateRange, bool)"/>
        public static bool Overlaps(this IOpenEndedDateRange dateRange, IOpenEndedDateRange compareRange, bool allowSameEndPoints = true)
        {
            if (dateRange == null || compareRange == null)
                return false;

            ValidityCheck(dateRange);
            ValidityCheck(compareRange);

            if (!dateRange.End.HasValue && compareRange.End.HasValue)
                return allowSameEndPoints ? compareRange.Start > dateRange.Start || compareRange.End.Value > dateRange.Start : compareRange.Start >= dateRange.Start || compareRange.End.Value >= dateRange.Start;

            if (dateRange.End.HasValue && !compareRange.End.HasValue)
                return allowSameEndPoints ? dateRange.Start > compareRange.Start || dateRange.End.Value > compareRange.Start : dateRange.Start >= compareRange.Start || dateRange.End.Value >= compareRange.Start;

            if (!dateRange.End.HasValue && !compareRange.End.HasValue)
                return true;

            return compareRange.End != null && dateRange.End != null && Date.Overlaps(dateRange.Start, dateRange.End.Value, compareRange.Start, compareRange.End.Value, allowSameEndPoints);
        }

        /// <summary>
        /// Adds a specified number of business <paramref name="days"/> to the beginning of a date range
        /// </summary>
        /// <param name="dateRange">The date range to prepend to</param>
        /// <param name="days">The number of business days to prepend to the date range</param>
        /// <param name="holidays">Holidays dates to exclude when determining business days</param>
        /// <returns>A date range where the start date is determined by subtracting the specified number of business <paramref name="days"/> from the original start date</returns>
        public static IDateRange PrependBusinessDays(this IDateRange dateRange, double days, params DateTime[] holidays)
        {
            if (dateRange != null)
                dateRange.Start = dateRange.Start.AddBusinessDays(-days);

            return dateRange;
        }

        /// <inheritdoc cref="PrependBusinessDays(IDateRange, double, DateTime[])"/>
        public static IOpenEndedDateRange PrependBusinessDays(this IOpenEndedDateRange dateRange, double days, params DateTime[] holidays)
        {
            if (dateRange != null)
                dateRange.Start = dateRange.Start.AddBusinessDays(-days);

            return dateRange;
        }

        /// <summary>
        /// Adds a specified number of <paramref name="days"/> to the beginning of a date range
        /// </summary>
        /// <param name="dateRange">The date range to prepend to</param>
        /// <param name="days">The number of days to prepend to the date range</param>
        /// <returns>A date range where the start date is determined by subtracting the specified number of <paramref name="days"/> from the original start date</returns>
        public static IDateRange PrependDays(this IDateRange dateRange, double days)
        {
            if (dateRange != null)
                dateRange.Start = dateRange.Start.AddDays(-days);

            return dateRange;
        }

        /// <inheritdoc cref="PrependDays(IDateRange, double)"/>
        public static IOpenEndedDateRange PrependDays(this IOpenEndedDateRange dateRange, double days)
        {
            if (dateRange != null)
                dateRange.Start = dateRange.Start.AddDays(-days);

            return dateRange;
        }

        /// <summary>
        /// Calculates the amount of time within a date range
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <exception cref="ArgumentNullException"><paramref name="dateRange"/> parameter cannot be null</exception>
        /// <returns>The amount of time within the <paramref name="dateRange"/></returns>
        public static TimeSpan TimeSpan(this IDateRange dateRange)
        {
            if (dateRange == null)
                throw new ArgumentNullException(nameof(dateRange));

            return Date.TimeSpan(dateRange.Start, dateRange.End);
        }

        /// <inheritdoc cref="Date.BusinessDays(DateTime, DateTime, DateTime[])"/>
        /// <summary>Attempts to determine the number of business days in a date range</summary>
        /// <param name="dateRange">The date range to check</param>
        /// <param name="businessDays">The number of business days in the date range</param>
        /// <returns>True if the number of business days in the date range could be determined</returns>
        public static bool TryBusinessDays(this IOpenEndedDateRange dateRange, out int businessDays, params DateTime[] holidays)
        {
            businessDays = 0;
            if (dateRange == null)
                return true;

            if (!dateRange.End.HasValue)
                return false;

            try
            {
                businessDays = Date.BusinessDays(dateRange.Start, dateRange.End.Value, holidays);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc cref="Date.BusinessHours(DateTime, DateTime, string, DateTime[])" select="remarks"/>
        /// <inheritdoc cref="TryBusinessHours(IOpenEndedDateRange, string, int, int, int, int, int, int, int, int, out int, DateTime[])"/>
        public static bool TryBusinessHours(this IOpenEndedDateRange dateRange, string timeZoneId, out int businessHours, params DateTime[] holidays)
        {
            businessHours = 0;
            if (dateRange == null)
                return true;

            if (!dateRange.End.HasValue)
                return false;

            try
            {
                businessHours = Date.BusinessHours(dateRange.Start, dateRange.End.Value, timeZoneId, holidays);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc cref="Date.BusinessHours(DateTime, DateTime, string, int, int, int, int, DateTime[])" select="remarks"/>
        /// <inheritdoc cref="TryBusinessHours(IOpenEndedDateRange, string, int, int, int, int, int, int, int, int, out int, DateTime[])"/>
        public static bool TryBusinessHours(this IOpenEndedDateRange dateRange, string timeZoneId, int startOfDayHour, int startOfDayMinute, int endOfDayHour, int endOfDayMinute, out int businessHours, params DateTime[] holidays)
        {
            businessHours = 0;
            if (dateRange == null)
                return true;

            if (!dateRange.End.HasValue)
                return false;

            try
            {
                businessHours = Date.BusinessHours(dateRange.Start, dateRange.End.Value, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, holidays);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc cref="Date.BusinessHours(DateTime, DateTime, string, int, int, int, int, int, int, int, int, DateTime[])"/>
        /// <summary>
        /// Attempts to determine the number of business hours in a date range
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <param name="businessHours">The number of business hours in the date range</param>
        /// <returns>True if the number of business hours in the date range could be determined</returns>
        public static bool TryBusinessHours(this IOpenEndedDateRange dateRange,
                                            string timeZoneId,
                                            int startOfDayHour,
                                            int startOfDayMinute,
                                            int endOfDayHour,
                                            int endOfDayMinute,
                                            int breakStartHour,
                                            int breakStartMin,
                                            int breakEndHour,
                                            int breakEndMin,
                                            out int businessHours,
                                            params DateTime[] holidays)
        {
            businessHours = 0;
            if (dateRange == null)
                return true;

            if (!dateRange.End.HasValue)
                return false;

            try
            {
                businessHours = Date.BusinessHours(dateRange.Start, dateRange.End.Value, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, breakStartHour, breakStartMin, breakEndHour, breakEndMin, holidays);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to get all dates within a date range
        /// </summary>
        /// <param name="dateRange">The date range to work with</param>
        /// <param name="dates">A collection of dates within the date range</param>
        /// <returns>True if the dates within a date range could be retrieved</returns>
        public static bool TryDates(this IOpenEndedDateRange dateRange, out IEnumerable<DateTime> dates)
        {
            dates = new List<DateTime>();
            if (dateRange == null)
                return true;

            int days;
            if (!dateRange.TryDays(out days))
                return false;

            dates = Enumerable.Range(0, days).Select(index => dateRange.Start.WithoutTime().AddDays(index));
            return true;
        }

        /// <summary>
        /// Attempts to determine the number of days within a date range
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <param name="days">The number of days within the date range</param>
        /// <returns>True if the number of days within the date range could be determined</returns>
        public static bool TryDays(this IOpenEndedDateRange dateRange, out int days)
        {
            days = 0;
            if (dateRange == null)
                return true;

            if (!dateRange.End.HasValue)
                return false;

            try
            {
                days = Date.Days(dateRange.Start, dateRange.End.Value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to determine the number of months within a date range
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <param name="months">The number of months within the date range</param>
        /// <returns>True if the number of months within the date range could be determined</returns>
        public static bool TryMonths(this IOpenEndedDateRange dateRange, out int months)
        {
            months = 0;
            if (dateRange == null)
                return true;

            try
            {
                months = new DateDifference(dateRange).Months;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to calculate the amount of time within a date range
        /// </summary>
        /// <param name="dateRange">The date range to check</param>
        /// <param name="timeSpan">The amount of time within the date range</param>
        /// <returns>True if the amount of time within the <paramref name="dateRange"/> could be calculated</returns>
        public static bool TryTimeSpan(this IOpenEndedDateRange dateRange, out TimeSpan timeSpan)
        {
            timeSpan = default(TimeSpan);

            if (dateRange?.End == null)
                return false;

            timeSpan = Date.TimeSpan(dateRange.Start, dateRange.End.Value);
            return true;
        }

        private static void ValidityCheck(IDateRange dateRange)
        {
            if (dateRange != null)
                ValidityCheck(dateRange.Start, dateRange.End);
        }

        private static void ValidityCheck(IOpenEndedDateRange dateRange)
        {
            if (dateRange?.End != null)
                ValidityCheck(dateRange.Start, dateRange.End.Value);
        }

        /// <summary>
        /// Determines if two dates are valid when used as a date range
        /// </summary>
        /// <param name="start">The start date of the date range</param>
        /// <param name="end">The end date of the date range</param>
        /// <exception cref="ArgumentException">The <paramref name="end"/> date must be greater than the <paramref name="start"/> date</exception>
        private static void ValidityCheck(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException($"End date must be greater than {start:F}", nameof(end));
        }
    }
}

#pragma warning restore 1573
