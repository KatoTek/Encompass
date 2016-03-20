using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Encompass.Concepts.DateRanges;
using Encompass.Concepts.DateRanges.Extensions;
using Encompass.Concepts.Dates.Extensions;
using static System.DayOfWeek;
using static System.Math;

#pragma warning disable 1573

namespace Encompass.Concepts.Dates
{
    /// <summary>
    ///     A class that provides methods that can be used to simplify working with Date objects
    /// </summary>
    public static class Date
    {
        #region fields

        private const double TOLERANCE = 1E-10;

        #endregion

        #region methods

        /// <inheritdoc cref="AddBusinessDays(DateTime, double, DayOfWeek[], DateTime[])" />
        /// <remarks>Saturday and Sunday are weekend days</remarks>
        public static DateTime AddBusinessDays(DateTime dateTime, double days, params DateTime[] holidays) => AddBusinessDays(dateTime, days, new[] { Saturday, Sunday }, holidays);

        /// <summary>
        ///     Adds a specified number of business <paramref name="days" /> to a <paramref name="dateTime" /> taking into account
        ///     weekends and holidays
        /// </summary>
        /// <param name="dateTime">The DateTime to add the business days to</param>
        /// <param name="days">The number of business days to add</param>
        /// <param name="weekendDays">
        ///     The days of the week to consider as the weekend. These days of the week will be excluded when
        ///     determining business days.
        /// </param>
        /// <param name="holidays">Holidays dates to exclude when determining business days</param>
        /// <returns>The new DateTime after adding the specified number of business <paramref name="days" /></returns>
        public static DateTime AddBusinessDays(DateTime dateTime, double days, DayOfWeek[] weekendDays, params DateTime[] holidays)
        {
            if (Abs(days) < TOLERANCE)
                return dateTime;

            var wholeDays = Truncate(days);
            var partDay = Abs(wholeDays) > TOLERANCE
                              ? days%wholeDays
                              : 0;
            var range = Abs(wholeDays);
            var direction = Sign(wholeDays);

            for (var wholeDay = 0; wholeDay < range; wholeDay++)
            {
                do
                    dateTime = dateTime.AddDays(direction); while (IsOutsideBusinessDays(dateTime, weekendDays, holidays));
            }

            if (IsOutsideBusinessDays(dateTime, weekendDays, holidays))
                dateTime = dateTime.AddDays(direction);

            dateTime = dateTime.AddDays(partDay);
            while (IsOutsideBusinessDays(dateTime, weekendDays, holidays))
                dateTime = dateTime.AddDays(direction);

            return dateTime;
        }

        /// <inheritdoc
        ///     cref="AddBusinessHours(DateTime, double, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>8am is the start of the business day. 5pm is the end of the business day. A break occurs from 12pm to 1pm.</remarks>
        public static DateTime AddBusinessHours(DateTime dateTime, double hours, string timeZoneId, DayOfWeek[] weekendDays, params DateTime[] holidays)
            => AddBusinessHours(dateTime, hours, timeZoneId, 8, 0, 17, 0, 12, 0, 13, 0, weekendDays, holidays);

        /// <inheritdoc cref="AddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>
        ///     8am is the start of the business day. 5pm is the end of the business day. DateTime is treated as a local time
        ///     so Time Zone is not factored in.
        /// </remarks>
        public static DateTime AddBusinessHours(DateTime dateTime, double hours, DayOfWeek[] weekendDays, params DateTime[] holidays)
            => AddBusinessHours(dateTime, hours, 8, 0, 17, 0, 12, 0, 13, 0, weekendDays, holidays);

        /// <inheritdoc
        ///     cref="AddBusinessHours(DateTime, double, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>
        ///     8am is the start of the business day. 5pm is the end of the business day. A break occurs from 12pm to 1pm.
        ///     Saturday and Sunday are weekend days.
        /// </remarks>
        public static DateTime AddBusinessHours(DateTime dateTime, double hours, string timeZoneId, params DateTime[] holidays)
            => AddBusinessHours(dateTime, hours, timeZoneId, 8, 0, 17, 0, 12, 0, 13, 0, new[] { Saturday, Sunday }, holidays);

        /// <inheritdoc cref="AddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>
        ///     8am is the start of the business day. 5pm is the end of the business day. A break occurs from 12pm to 1pm.
        ///     DateTime is treated as a local time so Time Zone is not factored in. Saturday and Sunday are weekend days.
        /// </remarks>
        public static DateTime AddBusinessHours(DateTime dateTime, double hours, params DateTime[] holidays)
            => AddBusinessHours(dateTime, hours, 8, 0, 17, 0, 12, 0, 13, 0, new[] { Saturday, Sunday }, holidays);

        /// <inheritdoc
        ///     cref="AddBusinessHours(DateTime, double, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>Saturday and Sunday are weekend days. No break during the business day.</remarks>
        public static DateTime AddBusinessHours(DateTime dateTime,
                                                double hours,
                                                string timeZoneId,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                params DateTime[] holidays)
            => AddBusinessHours(dateTime, hours, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, 0, 0, 0, 0, new[] { Saturday, Sunday }, holidays);

        /// <inheritdoc cref="AddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>
        ///     DateTime is treated as a local time so Time Zone is not factored in. Saturday and Sunday are weekend days. No
        ///     break during the business day.
        /// </remarks>
        public static DateTime AddBusinessHours(DateTime dateTime,
                                                double hours,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                params DateTime[] holidays)
            => AddBusinessHours(dateTime, hours, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, 0, 0, 0, 0, new[] { Saturday, Sunday }, holidays);

        /// <inheritdoc
        ///     cref="AddBusinessHours(DateTime, double, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>No break during the business day.</remarks>
        public static DateTime AddBusinessHours(DateTime dateTime,
                                                double hours,
                                                string timeZoneId,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                DayOfWeek[] weekendDays,
                                                params DateTime[] holidays)
            => AddBusinessHours(dateTime, hours, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, 0, 0, 0, 0, weekendDays, holidays);

        /// <inheritdoc cref="AddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>DateTime is treated as a local time so Time Zone is not factored in. No break during the business day.</remarks>
        public static DateTime AddBusinessHours(DateTime dateTime,
                                                double hours,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                DayOfWeek[] weekendDays,
                                                params DateTime[] holidays)
            => AddBusinessHours(dateTime, hours, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, 0, 0, 0, 0, weekendDays, holidays);

        /// <inheritdoc
        ///     cref="AddBusinessHours(DateTime, double, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>Saturday and Sunday are weekend days</remarks>
        public static DateTime AddBusinessHours(DateTime dateTime,
                                                double hours,
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
            =>
                AddBusinessHours(dateTime,
                                 hours,
                                 timeZoneId,
                                 startOfDayHour,
                                 startOfDayMinute,
                                 endOfDayHour,
                                 endOfDayMinute,
                                 breakStartHour,
                                 breakStartMin,
                                 breakEndHour,
                                 breakEndMin,
                                 new[] { Saturday, Sunday },
                                 holidays);

        /// <inheritdoc cref="AddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>DateTime is treated as a local time so Time Zone is not factored in. Saturday and Sunday are weekend days</remarks>
        public static DateTime AddBusinessHours(DateTime dateTime,
                                                double hours,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                int breakStartHour,
                                                int breakStartMin,
                                                int breakEndHour,
                                                int breakEndMin,
                                                params DateTime[] holidays)
            =>
                AddBusinessHours(dateTime,
                                 hours,
                                 startOfDayHour,
                                 startOfDayMinute,
                                 endOfDayHour,
                                 endOfDayMinute,
                                 breakStartHour,
                                 breakStartMin,
                                 breakEndHour,
                                 breakEndMin,
                                 new[] { Saturday, Sunday },
                                 holidays);

        /// <inheritdoc
        ///     cref="LocalDateTimeAddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <param name="dateTime">
        ///     <inheritdoc
        ///         cref="LocalDateTimeAddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])"
        ///         select="/param[@name='localDateTime']/node()" />
        /// </param>
        /// <remarks>DateTime is treated as a local time so Time Zone is not factored in</remarks>
        public static DateTime AddBusinessHours(DateTime dateTime,
                                                double hours,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                int breakStartHour,
                                                int breakStartMin,
                                                int breakEndHour,
                                                int breakEndMin,
                                                DayOfWeek[] weekendDays,
                                                params DateTime[] holidays)
            =>
                LocalDateTimeAddBusinessHours(dateTime,
                                              hours,
                                              startOfDayHour,
                                              startOfDayMinute,
                                              endOfDayHour,
                                              endOfDayMinute,
                                              breakStartHour,
                                              breakStartMin,
                                              breakEndHour,
                                              breakEndMin,
                                              weekendDays,
                                              holidays);

        /// <inheritdoc
        ///     cref="LocalDateTimeAddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <param name="dateTime">
        ///     <inheritdoc
        ///         cref="LocalDateTimeAddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])"
        ///         select="/param[@name='localDateTime']/node()" />
        /// </param>
        /// <param name="timeZoneId">
        ///     The name of the Time Zone that should be used to calculate business hours. The Time Zones IDs
        ///     are the same as the ones that work with the <see cref="System.TimeZoneInfo" /> class
        /// </param>
        public static DateTime AddBusinessHours(DateTime dateTime,
                                                double hours,
                                                string timeZoneId,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                int breakStartHour,
                                                int breakStartMin,
                                                int breakEndHour,
                                                int breakEndMin,
                                                DayOfWeek[] weekendDays,
                                                params DateTime[] holidays) => Abs(hours) < TOLERANCE
                                                                                   ? dateTime
                                                                                   : LocalDateTimeAddBusinessHours(dateTime.ToLocalFromUtc(timeZoneId),
                                                                                                                   hours,
                                                                                                                   startOfDayHour,
                                                                                                                   startOfDayMinute,
                                                                                                                   endOfDayHour,
                                                                                                                   endOfDayMinute,
                                                                                                                   breakStartHour,
                                                                                                                   breakStartMin,
                                                                                                                   breakEndHour,
                                                                                                                   breakEndMin,
                                                                                                                   weekendDays,
                                                                                                                   holidays);

        /// <inheritdoc cref="BusinessDays(DateTime, DateTime, DayOfWeek[], DateTime[])" />
        /// <remarks>Saturday and Sunday are weekend days</remarks>
        public static int BusinessDays(DateTime start, DateTime end, params DateTime[] holidays) => BusinessDays(start, end, new[] { Saturday, Sunday }, holidays);

        /// <summary>
        ///     Determines the number of business days between two dates
        /// </summary>
        /// <param name="start">The DateTime to use as the start when determining the difference</param>
        /// <param name="end">The DateTime to use as the end when determining the difference</param>
        /// <param name="weekendDays">
        ///     The days of the week to consider as the weekend. These days of the week will be excluded when
        ///     determining business days.
        /// </param>
        /// <param name="holidays">Holidays dates to exclude when determining business days</param>
        /// <returns>The number of business days between the <paramref name="start" /> and <paramref name="end" /> dates</returns>
        public static int BusinessDays(DateTime start, DateTime end, DayOfWeek[] weekendDays, params DateTime[] holidays)
        {
            start = start.Date;
            end = end.Date;

            if (start > end)
                throw new ArgumentException($"End date must be greater than {start:F}", nameof(end));

            if (weekendDays == null || !weekendDays.Any())
                throw new ArgumentException("Weekend Days is null or empty");

            var lastWeekendDay = weekendDays.Last();
            var span = end - start;
            var businessDays = span.Days + 1;
            var fullWeekCount = businessDays/7;

            if (businessDays > fullWeekCount*7)
            {
                var startDayOfWeek = start.DayOfWeek == lastWeekendDay
                                         ? 7
                                         : (int)start.DayOfWeek;
                var endDayOfWeek = end.DayOfWeek == lastWeekendDay
                                       ? 7
                                       : (int)end.DayOfWeek;

                if (endDayOfWeek < startDayOfWeek)
                    endDayOfWeek += 7;

                if (startDayOfWeek <= 6)
                {
                    if (endDayOfWeek >= 7)
                        businessDays -= 2;
                    else if (endDayOfWeek >= 6)
                        businessDays -= 1;
                }
                else if (startDayOfWeek <= 7 && endDayOfWeek >= 7)
                    businessDays -= 1;
            }

            businessDays -= (from holiday in holidays
                             where holiday.Date >= start && holiday.Date <= end && weekendDays.Any(weekendDay => weekendDay != holiday.DayOfWeek)
                             select holiday).Count() + 2*fullWeekCount;

            return businessDays;
        }

        /// <inheritdoc
        ///     cref="BusinessHours(DateTime, DateTime, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>8am is the start of the business day. 5pm is the end of the business day.</remarks>
        public static int BusinessHours(DateTime start, DateTime end, string timeZoneId, DayOfWeek[] weekendDays, params DateTime[] holidays)
            => BusinessHours(start, end, timeZoneId, 8, 0, 17, 0, 12, 0, 13, 0, weekendDays, holidays);

        /// <inheritdoc cref="BusinessHours(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>
        ///     8am is the start of the business day. 5pm is the end of the business day. A break occurs from 12pm to 1pm.
        ///     DateTime is treated as a local time so Time Zone is not factored in.
        /// </remarks>
        public static int BusinessHours(DateTime start, DateTime end, DayOfWeek[] weekendDays, params DateTime[] holidays)
            => BusinessHours(start, end, 8, 0, 17, 0, 12, 0, 13, 0, weekendDays, holidays);

        /// <inheritdoc
        ///     cref="BusinessHours(DateTime, DateTime, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>
        ///     8am is the start of the business day. 5pm is the end of the business day. A break occurs from 12pm to 1pm.
        ///     Saturday and Sunday are weekend days.
        /// </remarks>
        public static int BusinessHours(DateTime start, DateTime end, string timeZoneId, params DateTime[] holidays)
            => BusinessHours(start, end, timeZoneId, 8, 0, 17, 0, 12, 0, 13, 0, holidays);

        /// <inheritdoc cref="BusinessHours(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>
        ///     8am is the start of the business day. 5pm is the end of the business day. A break occurs from 12pm to 1pm.
        ///     DateTime is treated as a local time so Time Zone is not factored in. Saturday and Sunday are weekend days.
        /// </remarks>
        public static int BusinessHours(DateTime start, DateTime end, params DateTime[] holidays) => BusinessHours(start, end, 8, 0, 17, 0, 12, 0, 13, 0, holidays);

        /// <inheritdoc
        ///     cref="BusinessHours(DateTime, DateTime, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>No break during the business day.</remarks>
        public static int BusinessHours(DateTime start,
                                        DateTime end,
                                        string timeZoneId,
                                        int startOfDayHour,
                                        int startOfDayMinute,
                                        int endOfDayHour,
                                        int endOfDayMinute,
                                        DayOfWeek[] weekendDays,
                                        params DateTime[] holidays)
            => BusinessHours(start, end, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, 0, 0, 0, 0, weekendDays, holidays);

        /// <inheritdoc cref="BusinessHours(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>DateTime is treated as a local time so Time Zone is not factored in. No break during the business day.</remarks>
        public static int BusinessHours(DateTime start,
                                        DateTime end,
                                        int startOfDayHour,
                                        int startOfDayMinute,
                                        int endOfDayHour,
                                        int endOfDayMinute,
                                        DayOfWeek[] weekendDays,
                                        params DateTime[] holidays)
            => BusinessHours(start, end, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, 0, 0, 0, 0, weekendDays, holidays);

        /// <inheritdoc
        ///     cref="BusinessHours(DateTime, DateTime, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>Saturday and Sunday are weekend days. No break during the business day.</remarks>
        public static int BusinessHours(DateTime start,
                                        DateTime end,
                                        string timeZoneId,
                                        int startOfDayHour,
                                        int startOfDayMinute,
                                        int endOfDayHour,
                                        int endOfDayMinute,
                                        params DateTime[] holidays)
            => BusinessHours(start, end, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, 0, 0, 0, 0, holidays);

        /// <inheritdoc cref="BusinessHours(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>
        ///     DateTime is treated as a local time so Time Zone is not factored in. Saturday and Sunday are weekend days. No
        ///     break during the business day.
        /// </remarks>
        public static int BusinessHours(DateTime start, DateTime end, int startOfDayHour, int startOfDayMinute, int endOfDayHour, int endOfDayMinute, params DateTime[] holidays)
            => BusinessHours(start, end, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, 0, 0, 0, 0, holidays);

        /// <inheritdoc
        ///     cref="BusinessHours(DateTime, DateTime, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>Saturday and Sunday are weekend days.</remarks>
        public static int BusinessHours(DateTime start,
                                        DateTime end,
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
            =>
                BusinessHours(start,
                              end,
                              timeZoneId,
                              startOfDayHour,
                              startOfDayMinute,
                              endOfDayHour,
                              endOfDayMinute,
                              breakStartHour,
                              breakStartMin,
                              breakEndHour,
                              breakEndMin,
                              new[] { Saturday, Sunday },
                              holidays);

        /// <inheritdoc cref="BusinessHours(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <remarks>DateTime is treated as a local time so Time Zone is not factored in. Saturday and Sunday are weekend days.</remarks>
        public static int BusinessHours(DateTime start,
                                        DateTime end,
                                        int startOfDayHour,
                                        int startOfDayMinute,
                                        int endOfDayHour,
                                        int endOfDayMinute,
                                        int breakStartHour,
                                        int breakStartMin,
                                        int breakEndHour,
                                        int breakEndMin,
                                        params DateTime[] holidays)
            =>
                BusinessHours(start,
                              end,
                              startOfDayHour,
                              startOfDayMinute,
                              endOfDayHour,
                              endOfDayMinute,
                              breakStartHour,
                              breakStartMin,
                              breakEndHour,
                              breakEndMin,
                              new[] { Saturday, Sunday },
                              holidays);

        /// <inheritdoc
        ///     cref="BusinessHoursBetween(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <param name="start">
        ///     <inheritdoc
        ///         cref="BusinessHoursBetween(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])"
        ///         select="/param[@name='localStart']/node()" />
        /// </param>
        /// <param name="end">
        ///     <inheritdoc
        ///         cref="BusinessHoursBetween(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])"
        ///         select="/param[@name='localEnd']/node()" />
        /// </param>
        /// <param name="timeZoneId">
        ///     The name of the Time Zone that should be used to calculate business hours. The Time Zones IDs
        ///     are the same as the ones that work with the <see cref="System.TimeZoneInfo" /> class
        /// </param>
        public static int BusinessHours(DateTime start,
                                        DateTime end,
                                        string timeZoneId,
                                        int startOfDayHour,
                                        int startOfDayMinute,
                                        int endOfDayHour,
                                        int endOfDayMinute,
                                        int breakStartHour,
                                        int breakStartMin,
                                        int breakEndHour,
                                        int breakEndMin,
                                        DayOfWeek[] weekendDays,
                                        params DateTime[] holidays)
            =>
                BusinessHoursBetween(start.ToLocalFromUtc(timeZoneId),
                                     end.ToLocalFromUtc(timeZoneId),
                                     startOfDayHour,
                                     startOfDayMinute,
                                     endOfDayHour,
                                     endOfDayMinute,
                                     breakStartHour,
                                     breakStartMin,
                                     breakEndHour,
                                     breakEndMin,
                                     weekendDays,
                                     holidays);

        /// <inheritdoc
        ///     cref="BusinessHoursBetween(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        /// <param name="start">
        ///     <inheritdoc
        ///         cref="BusinessHoursBetween(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])"
        ///         select="/param[@name='localStart']/node()" />
        /// </param>
        /// <param name="end">
        ///     <inheritdoc
        ///         cref="BusinessHoursBetween(DateTime, DateTime, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])"
        ///         select="/param[@name='localEnd']/node()" />
        /// </param>
        /// <remarks>DateTime is treated as a local time so Time Zone is not factored in.</remarks>
        public static int BusinessHours(DateTime start,
                                        DateTime end,
                                        int startOfDayHour,
                                        int startOfDayMinute,
                                        int endOfDayHour,
                                        int endOfDayMinute,
                                        int breakStartHour,
                                        int breakStartMin,
                                        int breakEndHour,
                                        int breakEndMin,
                                        DayOfWeek[] weekendDays,
                                        params DateTime[] holidays)
            =>
                BusinessHoursBetween(start,
                                     end,
                                     startOfDayHour,
                                     startOfDayMinute,
                                     endOfDayHour,
                                     endOfDayMinute,
                                     breakStartHour,
                                     breakStartMin,
                                     breakEndHour,
                                     breakEndMin,
                                     weekendDays,
                                     holidays);

        /// <summary>
        ///     Creates a <see cref="DateDifference" /> object using the <paramref name="start" /> and <paramref name="end" />
        ///     dates passed in
        /// </summary>
        /// <param name="start">The DateTime to use as the start when determining the difference</param>
        /// <param name="end">The DateTime to use as the end when determining the difference</param>
        /// <returns>
        ///     A <see cref="DateDifference" /> object using the <paramref name="start" /> and <paramref name="end" /> date
        ///     parameters
        /// </returns>
        public static DateDifference DateDifference(DateTime start, DateTime end) => new DateDifference(start, end);

        /// <summary>
        ///     Calculates the number of days between two dates
        /// </summary>
        /// <param name="start">The DateTime to use as the start when determining the difference</param>
        /// <param name="end">The DateTime to use as the end when determining the difference</param>
        /// <returns>The number of days between the <paramref name="start" /> date and the <paramref name="end" /> date</returns>
        public static int Days(DateTime start, DateTime end)
        {
            start = start.Date;
            end = end.Date;

            if (start > end)
                throw new ArgumentException($"End date must be greater than {start:F}", nameof(end));

            return (end - start).Days + 1;
        }

        /// <inheritdoc cref="IsBusinessDay(DateTime, DayOfWeek[], DateTime[])" />
        /// <remarks>Saturday and Sunday are weekend days.</remarks>
        public static bool IsBusinessDay(DateTime dateTime, params DateTime[] holidays) => IsBusinessDay(dateTime, new[] { Saturday, Sunday }, holidays);

        /// <summary>
        ///     Determines if the <paramref name="dateTime" /> date parameter is a business day based on the supplied
        ///     <paramref name="weekendDays" /> and <paramref name="holidays" />
        /// </summary>
        /// <param name="dateTime">The DateTime to check</param>
        /// <param name="weekendDays">
        ///     The days of the week to consider as the weekend. These days of the week will be excluded when
        ///     determining business days.
        /// </param>
        /// <param name="holidays">Holidays dates to exclude when determining business days</param>
        /// <returns>True if the <paramref name="dateTime" /> is a business day</returns>
        public static bool IsBusinessDay(DateTime dateTime, DayOfWeek[] weekendDays, params DateTime[] holidays)
            => !(weekendDays.Any(weekendDay => weekendDay == dateTime.DayOfWeek) || holidays.Any(holiday => holiday.Date == dateTime.Date));

        /// <summary>
        ///     Determines if two date ranges overlap each other
        /// </summary>
        /// <param name="rangeOneStart">The start of the first date range</param>
        /// <param name="rangeOneEnd">The end of the first date range</param>
        /// <param name="rangeTwoStart">The start of the second date range</param>
        /// <param name="rangeTwoEnd">The end of the second date range</param>
        /// <param name="allowSameEndPoints">
        ///     Specifies if the end of the first date range can equal the start of the second date
        ///     range
        /// </param>
        /// <exception cref="ArgumentException">The end of the first date range must be greater than its start</exception>
        /// <exception cref="ArgumentException">The end of the second date range must be greater than its start</exception>
        /// <returns>True if the two date ranges overlap each other</returns>
        public static bool Overlaps(DateTime rangeOneStart, DateTime rangeOneEnd, DateTime rangeTwoStart, DateTime rangeTwoEnd, bool allowSameEndPoints = true)
        {
            if (rangeOneStart > rangeOneEnd)
                throw new ArgumentException($"End date must be greater than {rangeOneStart:F}", nameof(rangeOneEnd));

            if (rangeTwoStart > rangeTwoEnd)
                throw new ArgumentException($"End date must be greater than {rangeTwoEnd:F}", nameof(rangeTwoEnd));

            return allowSameEndPoints
                       ? rangeOneStart < rangeTwoEnd && rangeTwoStart < rangeOneEnd
                       : rangeOneStart <= rangeTwoEnd && rangeTwoStart <= rangeOneEnd;
        }

        /// <summary>
        ///     Encodes a DateTime into a format that is safe for use in a URL
        /// </summary>
        /// <param name="dateTime">The DateTime to convert</param>
        /// <returns>The <paramref name="dateTime" /> parameter as an encoded string that is safe to use in a URL</returns>
        public static string SafeUrlFormat(DateTime dateTime) => dateTime.ToString(CultureInfo.InvariantCulture)
                                                                         .Replace("/", "-")
                                                                         .Replace(" ", "_")
                                                                         .Replace(":", "|");

        /// <inheritdoc cref="SafeUrlFormat(DateTime)" />
        public static string SafeUrlFormat(DateTime? dateTime) => dateTime.HasValue
                                                                      ? SafeUrlFormat(dateTime.Value)
                                                                      : "null";

        /// <summary>
        ///     Calculates the amount of time between two dates
        /// </summary>
        /// <param name="start">The DateTime to use as the start when determining the difference</param>
        /// <param name="end">The DateTime to use as the end when determining the difference</param>
        /// <returns>The amount of time between the <paramref name="start" /> and <paramref name="end" /> dates</returns>
        public static TimeSpan TimeSpan(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException($"End date must be greater than {start:F}", nameof(end));

            return end - start;
        }

        /// <summary>
        ///     Attempts to decode a string as a DateTime that was encoded to be safely used in a URL
        /// </summary>
        /// <param name="safeUrlFormattedDateTime">The string that represents a DateTime that was encoded for use in a URL</param>
        /// <param name="dateTime">The out value if the string can be parsed back to a DateTime</param>
        /// <returns>True if the string can be decoded and parsed back to a DateTime</returns>
        public static bool TrySafeUrlFormatDecode(string safeUrlFormattedDateTime, out DateTime dateTime) => DateTime.TryParse(safeUrlFormattedDateTime.Replace("|", ":")
                                                                                                                                                       .Replace("_", " ")
                                                                                                                                                       .Replace("-", "/"),
                                                                                                                               out dateTime);

        /// <inheritdoc cref="TrySafeUrlFormatDecode(string, out DateTime)" />
        public static bool TrySafeUrlFormatDecode(string safeUrlFormattedDateTime, out DateTime? dateTime)
        {
            dateTime = null;
            if (safeUrlFormattedDateTime == "null")
                return true;

            DateTime dt;
            if (!TrySafeUrlFormatDecode(safeUrlFormattedDateTime, out dt))
                return false;

            dateTime = dt;
            return true;
        }

        /// <summary>
        ///     Determines the number of business hours between two dates
        /// </summary>
        /// <param name="localStart">The DateTime to use as the start when determining the difference</param>
        /// <param name="localEnd">The DateTime to use as the end when determining the difference</param>
        /// <param name="startOfDayHour">The hour of the day that defines the start of the business day</param>
        /// <param name="startOfDayMinute">The start of the business day minute value</param>
        /// <param name="endOfDayHour">The hour of the day that defines the end of the business day</param>
        /// <param name="endOfDayMinute">The end of the business day minute value</param>
        /// <param name="breakStartHour">The hour of the day that defines the start of a break period</param>
        /// <param name="breakStartMin">The start of the break period minute value</param>
        /// <param name="breakEndHour">The hour of the day that defines the end of a break period</param>
        /// <param name="breakEndMin">The end of the break period minute value</param>
        /// <param name="weekendDays">
        ///     The days of the week to consider as the weekend. These days of the week will be excluded when
        ///     determining business days.
        /// </param>
        /// <param name="holidays">Holidays dates to exclude when determining business days</param>
        /// <returns>The number of business hours between the start and end dates</returns>
        private static int BusinessHoursBetween(DateTime localStart,
                                                DateTime localEnd,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                int breakStartHour,
                                                int breakStartMin,
                                                int breakEndHour,
                                                int breakEndMin,
                                                DayOfWeek[] weekendDays,
                                                params DateTime[] holidays)
        {
            if (localStart > localEnd)
                throw new ArgumentException($"End date must be greater than {localStart:F}", nameof(localEnd));

            var start = new DateTime(localStart.Ticks);
            var end = new DateTime(localEnd.Ticks);

            var hoursPerDay = GetHourCountOfBusinessDay(new DateTime(localStart.Year, localStart.Month, localStart.Day, startOfDayHour, startOfDayMinute, 0),
                                                        new DateTime(localStart.Year, localStart.Month, localStart.Day, endOfDayHour, endOfDayMinute, 0),
                                                        breakStartHour,
                                                        breakStartMin,
                                                        breakEndHour,
                                                        breakEndMin);

            start = GetHourWithinBusinessDay(start, startOfDayHour, endOfDayHour, startOfDayMinute, endOfDayMinute);
            end = GetHourWithinBusinessDay(end, startOfDayHour, endOfDayHour, startOfDayMinute, endOfDayMinute);

            if (start.Date == end.Date)
                return (int)GetHourCountOfBusinessDay(start, end, breakStartHour, breakStartMin, breakEndHour, breakEndMin);
            var startDayTotalHours = start.IsBusinessDay(weekendDays, holidays)
                                         ? GetHourCountOfBusinessDay(start,
                                                                     new DateTime(start.Year, start.Month, start.Day, endOfDayHour, endOfDayMinute, 0),
                                                                     breakStartHour,
                                                                     breakStartMin,
                                                                     breakEndHour,
                                                                     breakEndMin)
                                         : 0;

            var middleDaysTotalHours = 0.0;
            if ((end.Date - start.Date).TotalDays >= 2)
            {
                var daysBetweenStartAndEnd = BusinessDays(start.Date.AddDays(1), end.Date.AddDays(-1), weekendDays, holidays);
                middleDaysTotalHours = daysBetweenStartAndEnd > 0
                                           ? daysBetweenStartAndEnd*hoursPerDay
                                           : 0;
            }

            var endDayTotalHours = end.IsBusinessDay(holidays)
                                       ? GetHourCountOfBusinessDay(new DateTime(end.Year, end.Month, end.Day, startOfDayHour, startOfDayMinute, 0),
                                                                   end,
                                                                   breakStartHour,
                                                                   breakStartMin,
                                                                   breakEndHour,
                                                                   breakEndMin)
                                       : 0;

            return (int)(startDayTotalHours + middleDaysTotalHours + endDayTotalHours);
        }

        private static IEnumerable<DateRange> GetBusinessHourRanges(DateTime start, DateTime end, int breakStartHour, int breakStartMin, int breakEndHour, int breakEndMin)
        {
            var ranges = new List<DateRange>();

            var startOfBreak = new DateTime(start.Year, start.Month, start.Day, breakStartHour, breakStartMin, 0);
            var endOfBreak = new DateTime(start.Year, start.Month, start.Day, breakEndHour, breakEndMin, 0);

            if (Overlaps(start, end, startOfBreak, endOfBreak))
            {
                if (start <= startOfBreak)
                    ranges.Add(new DateRange(start, startOfBreak));

                if (end >= endOfBreak)
                    ranges.Add(new DateRange(endOfBreak, end));
            }
            else
                ranges.Add(new DateRange(start, end));

            return ranges.ToArray();
        }

        private static double GetHourCountOfBusinessDay(DateTime start, DateTime end, int breakStartHour, int breakStartMin, int breakEndHour, int breakEndMin)
            => GetBusinessHourRanges(start, end, breakStartHour, breakStartMin, breakEndHour, breakEndMin)
                .Sum(range => range.TimeSpan()
                                   .TotalHours);

        private static DateTime GetHourWithinBusinessDay(DateTime dateTime, int startOfDayHour, int endOfDayHour, int startOfDayMinute, int endOfDayMinute)
        {
            var startOfDay = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, startOfDayHour, startOfDayMinute, 0);
            var endOfDay = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, endOfDayHour, endOfDayMinute, 0);

            if (dateTime < startOfDay)
                return startOfDay;

            return dateTime > endOfDay
                       ? startOfDay.AddBusinessDays(1)
                       : dateTime;
        }

        private static bool IsOutsideBusinessDays(DateTime dateTime, IEnumerable<DayOfWeek> weekendDays, params DateTime[] holidays)
            => weekendDays.Any(weekendDay => weekendDay == dateTime.DayOfWeek) || holidays.Any(holiday => holiday.Date == dateTime.Date);

        private static bool IsOutsideBusinessHours(DateTime dateTime,
                                                   int startOfDayHour,
                                                   int startOfDayMinute,
                                                   int endOfDayHour,
                                                   int endOfDayMinute,
                                                   int breakStartHour,
                                                   int breakStartMin,
                                                   int breakEndHour,
                                                   int breakEndMin,
                                                   IEnumerable<DayOfWeek> weekendDays,
                                                   params DateTime[] holidays)
            =>
                weekendDays.Any(weekendDay => weekendDay == dateTime.DayOfWeek) || holidays.Any(holiday => holiday.Date == dateTime.Date) ||
                GetBusinessHourRanges(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, startOfDayHour, startOfDayMinute, 0),
                                      new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, endOfDayHour, endOfDayMinute, 0),
                                      breakStartHour,
                                      breakStartMin,
                                      breakEndHour,
                                      breakEndMin)
                    .All(dateRange => !dateTime.Within(dateRange));

        /// <summary>
        ///     Adds a specified number of business <paramref name="hours" /> to a <cref name="DateTime" /> taking into account
        ///     weekends and holidays
        /// </summary>
        /// <param name="localDateTime">The DateTime to add the business hours to</param>
        /// <param name="hours">The number of business hours to add</param>
        /// <param name="startOfDayHour">The hour of the day that defines the start of the business day</param>
        /// <param name="startOfDayMinute">The start of the business day minute value</param>
        /// <param name="endOfDayHour">The hour of the day that defines the end of the business day</param>
        /// <param name="endOfDayMinute">The end of the business day minute value</param>
        /// <param name="breakStartHour">The hour of the day that defines the start of a break period</param>
        /// <param name="breakStartMin">The start of the break period minute value</param>
        /// <param name="breakEndHour">The hour of the day that defines the end of a break period</param>
        /// <param name="breakEndMin">The end of the break period minute value</param>
        /// <param name="weekendDays">
        ///     The days of the week to consider as the weekend. These days of the week will be excluded when
        ///     determining business days.
        /// </param>
        /// <param name="holidays">Holidays dates to exclude when determining business days</param>
        /// <returns>The new DateTime after adding the specified number of business <paramref name="hours" /></returns>
        private static DateTime LocalDateTimeAddBusinessHours(DateTime localDateTime,
                                                              double hours,
                                                              int startOfDayHour,
                                                              int startOfDayMinute,
                                                              int endOfDayHour,
                                                              int endOfDayMinute,
                                                              int breakStartHour,
                                                              int breakStartMin,
                                                              int breakEndHour,
                                                              int breakEndMin,
                                                              DayOfWeek[] weekendDays,
                                                              params DateTime[] holidays)
        {
            if (Abs(hours) < TOLERANCE)
                return localDateTime;

            var dateTime = new DateTime(localDateTime.Ticks);
            var wholeHours = Truncate(hours);
            var partHour = Abs(wholeHours) > TOLERANCE
                               ? hours%wholeHours
                               : 0;
            var range = Abs(wholeHours);
            var direction = Sign(wholeHours);

            for (var wholeHour = 0; wholeHour < range; wholeHour++)
            {
                do
                    dateTime = dateTime.AddHours(direction); while (IsOutsideBusinessHours(dateTime,
                                                                                           startOfDayHour,
                                                                                           startOfDayMinute,
                                                                                           endOfDayHour,
                                                                                           endOfDayMinute,
                                                                                           breakStartHour,
                                                                                           breakStartMin,
                                                                                           breakEndHour,
                                                                                           breakEndMin,
                                                                                           weekendDays,
                                                                                           holidays));
            }

            if (IsOutsideBusinessHours(dateTime,
                                       startOfDayHour,
                                       startOfDayMinute,
                                       endOfDayHour,
                                       endOfDayMinute,
                                       breakStartHour,
                                       breakStartMin,
                                       breakEndHour,
                                       breakEndMin,
                                       weekendDays,
                                       holidays))
                dateTime = dateTime.AddHours(direction);

            dateTime = dateTime.AddHours(partHour);
            while (IsOutsideBusinessHours(dateTime,
                                          startOfDayHour,
                                          startOfDayMinute,
                                          endOfDayHour,
                                          endOfDayMinute,
                                          breakStartHour,
                                          breakStartMin,
                                          breakEndHour,
                                          breakEndMin,
                                          weekendDays,
                                          holidays))
                dateTime = dateTime.AddHours(direction);

            return dateTime;
        }

        #endregion
    }
}

#pragma warning restore 1573
