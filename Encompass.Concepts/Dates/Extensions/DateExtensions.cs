using System;
using System.Linq;
using Encompass.Concepts.DateRanges;
using Encompass.Simple.Extensions;
using static System.String;
using static System.StringSplitOptions;
using static System.TimeZoneInfo;

#pragma warning disable 1573

namespace Encompass.Concepts.Dates.Extensions
{
    /// <summary>
    ///     Date extension methods
    /// </summary>
    public static class DateExtensions
    {
        #region fields

        private const string INVALID_TIME_FORMAT_EXCEPTION_MESSAGE = "{0} is an invalid time format.";

        #endregion

        #region methods

        /// <inheritdoc cref="Date.AddBusinessDays(DateTime, double, DayOfWeek[], DateTime[])" />
        public static DateTime AddBusinessDays(this DateTime dateTime, double days, DayOfWeek[] weekendDays, params DateTime[] holidays)
            => Date.AddBusinessDays(dateTime, days, weekendDays, holidays);

        /// <inheritdoc cref="Date.AddBusinessDays(DateTime, double, DateTime[])" />
        public static DateTime AddBusinessDays(this DateTime dateTime, double days, params DateTime[] holidays) => Date.AddBusinessDays(dateTime, days, holidays);

        /// <inheritdoc cref="Date.AddBusinessDays(DateTime, double, DateTime[])" />
        public static DateTime? AddBusinessDays(this DateTime? dateTime, double days, params DateTime[] holidays) => dateTime.HasValue
                                                                                                                         ? new DateTime?(dateTime.Value.AddBusinessDays(days,
                                                                                                                                                                        holidays))
                                                                                                                         : null;

        /// <inheritdoc cref="Date.AddBusinessDays(DateTime, double, DayOfWeek[], DateTime[])" />
        public static DateTime? AddBusinessDays(this DateTime? dateTime, double days, DayOfWeek[] weekendDays, params DateTime[] holidays) => dateTime.HasValue
                                                                                                                                                  ? new DateTime?(
                                                                                                                                                        dateTime.Value
                                                                                                                                                                .AddBusinessDays(
                                                                                                                                                                                 days,
                                                                                                                                                                                 weekendDays,
                                                                                                                                                                                 holidays))
                                                                                                                                                  : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, DayOfWeek[], DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime, double hours, string timeZoneId, DayOfWeek[] weekendDays, params DateTime[] holidays)
            => Date.AddBusinessHours(dateTime, hours, timeZoneId, weekendDays, holidays);

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime, double hours, string timeZoneId, params DateTime[] holidays)
            => Date.AddBusinessHours(dateTime, hours, timeZoneId, holidays);

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, int, int, int, int, DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime,
                                                double hours,
                                                string timeZoneId,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                params DateTime[] holidays)
            => Date.AddBusinessHours(dateTime, hours, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, holidays);

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, int, int, int, int, DayOfWeek[], DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime,
                                                double hours,
                                                string timeZoneId,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                DayOfWeek[] weekendDays,
                                                params DateTime[] holidays)
            => Date.AddBusinessHours(dateTime, hours, timeZoneId, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, weekendDays, holidays);

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, int, int, int, int, int, int, int, int, DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime,
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
                Date.AddBusinessHours(dateTime,
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
                                      holidays);

        /// <inheritdoc
        ///     cref="Date.AddBusinessHours(DateTime, double, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime,
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
                                                params DateTime[] holidays)
            =>
                Date.AddBusinessHours(dateTime,
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
                                      weekendDays,
                                      holidays);

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, DayOfWeek[], DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime, double hours, string timeZoneId, DayOfWeek[] weekendDays, params DateTime[] holidays) => dateTime.HasValue
                                                                                                                                                                       ? new DateTime
                                                                                                                                                                             ?(
                                                                                                                                                                             dateTime
                                                                                                                                                                                 .Value
                                                                                                                                                                                 .AddBusinessHours
                                                                                                                                                                                 (hours,
                                                                                                                                                                                  timeZoneId,
                                                                                                                                                                                  weekendDays,
                                                                                                                                                                                  holidays))
                                                                                                                                                                       : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime, double hours, string timeZoneId, params DateTime[] holidays) => dateTime.HasValue
                                                                                                                                              ? new DateTime?(
                                                                                                                                                    dateTime.Value.AddBusinessHours(
                                                                                                                                                                                    hours,
                                                                                                                                                                                    timeZoneId,
                                                                                                                                                                                    holidays))
                                                                                                                                              : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, int, int, int, int, DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime,
                                                 double hours,
                                                 string timeZoneId,
                                                 int startOfDayHour,
                                                 int startOfDayMinute,
                                                 int endOfDayHour,
                                                 int endOfDayMinute,
                                                 params DateTime[] holidays) => dateTime.HasValue
                                                                                    ? new DateTime?(dateTime.Value.AddBusinessHours(hours,
                                                                                                                                    timeZoneId,
                                                                                                                                    startOfDayHour,
                                                                                                                                    startOfDayMinute,
                                                                                                                                    endOfDayHour,
                                                                                                                                    endOfDayMinute,
                                                                                                                                    holidays))
                                                                                    : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, int, int, int, int, DayOfWeek[], DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime,
                                                 double hours,
                                                 string timeZoneId,
                                                 int startOfDayHour,
                                                 int startOfDayMinute,
                                                 int endOfDayHour,
                                                 int endOfDayMinute,
                                                 DayOfWeek[] weekendDays,
                                                 params DateTime[] holidays) => dateTime.HasValue
                                                                                    ? new DateTime?(dateTime.Value.AddBusinessHours(hours,
                                                                                                                                    timeZoneId,
                                                                                                                                    startOfDayHour,
                                                                                                                                    startOfDayMinute,
                                                                                                                                    endOfDayHour,
                                                                                                                                    endOfDayMinute,
                                                                                                                                    weekendDays,
                                                                                                                                    holidays))
                                                                                    : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, string, int, int, int, int, int, int, int, int, DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime,
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
                                                 params DateTime[] holidays) => dateTime.HasValue
                                                                                    ? new DateTime?(dateTime.Value.AddBusinessHours(hours,
                                                                                                                                    timeZoneId,
                                                                                                                                    startOfDayHour,
                                                                                                                                    startOfDayMinute,
                                                                                                                                    endOfDayHour,
                                                                                                                                    endOfDayMinute,
                                                                                                                                    breakStartHour,
                                                                                                                                    breakStartMin,
                                                                                                                                    breakEndHour,
                                                                                                                                    breakEndMin,
                                                                                                                                    holidays))
                                                                                    : null;

        /// <inheritdoc
        ///     cref="Date.AddBusinessHours(DateTime, double, string, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime,
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
                                                 params DateTime[] holidays) => dateTime.HasValue
                                                                                    ? new DateTime?(dateTime.Value.AddBusinessHours(hours,
                                                                                                                                    timeZoneId,
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
                                                                                    : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, DayOfWeek[], DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime, double hours, DayOfWeek[] weekendDays, params DateTime[] holidays)
            => Date.AddBusinessHours(dateTime, hours, weekendDays, holidays);

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime, double hours, params DateTime[] holidays) => Date.AddBusinessHours(dateTime, hours, holidays);

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, int, int, int, int, DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime,
                                                double hours,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                params DateTime[] holidays)
            => Date.AddBusinessHours(dateTime, hours, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, holidays);

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, int, int, int, int, DayOfWeek[], DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime,
                                                double hours,
                                                int startOfDayHour,
                                                int startOfDayMinute,
                                                int endOfDayHour,
                                                int endOfDayMinute,
                                                DayOfWeek[] weekendDays,
                                                params DateTime[] holidays)
            => Date.AddBusinessHours(dateTime, hours, startOfDayHour, startOfDayMinute, endOfDayHour, endOfDayMinute, weekendDays, holidays);

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime,
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
                Date.AddBusinessHours(dateTime,
                                      hours,
                                      startOfDayHour,
                                      startOfDayMinute,
                                      endOfDayHour,
                                      endOfDayMinute,
                                      breakStartHour,
                                      breakStartMin,
                                      breakEndHour,
                                      breakEndMin,
                                      holidays);

        /// <inheritdoc
        ///     cref="Date.AddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        public static DateTime AddBusinessHours(this DateTime dateTime,
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
                Date.AddBusinessHours(dateTime,
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

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, DayOfWeek[], DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime, double hours, DayOfWeek[] weekendDays, params DateTime[] holidays) => dateTime.HasValue
                                                                                                                                                    ? new DateTime?(
                                                                                                                                                          dateTime.Value
                                                                                                                                                                  .AddBusinessHours(
                                                                                                                                                                                    hours,
                                                                                                                                                                                    weekendDays,
                                                                                                                                                                                    holidays))
                                                                                                                                                    : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime, double hours, params DateTime[] holidays) => dateTime.HasValue
                                                                                                                           ? new DateTime?(dateTime.Value.AddBusinessHours(hours,
                                                                                                                                                                           holidays))
                                                                                                                           : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, int, int, int, int, DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime,
                                                 double hours,
                                                 int startOfDayHour,
                                                 int startOfDayMinute,
                                                 int endOfDayHour,
                                                 int endOfDayMinute,
                                                 params DateTime[] holidays) => dateTime.HasValue
                                                                                    ? new DateTime?(dateTime.Value.AddBusinessHours(hours,
                                                                                                                                    startOfDayHour,
                                                                                                                                    startOfDayMinute,
                                                                                                                                    endOfDayHour,
                                                                                                                                    endOfDayMinute,
                                                                                                                                    holidays))
                                                                                    : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, int, int, int, int, DayOfWeek[], DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime,
                                                 double hours,
                                                 int startOfDayHour,
                                                 int startOfDayMinute,
                                                 int endOfDayHour,
                                                 int endOfDayMinute,
                                                 DayOfWeek[] weekendDays,
                                                 params DateTime[] holidays) => dateTime.HasValue
                                                                                    ? new DateTime?(dateTime.Value.AddBusinessHours(hours,
                                                                                                                                    startOfDayHour,
                                                                                                                                    startOfDayMinute,
                                                                                                                                    endOfDayHour,
                                                                                                                                    endOfDayMinute,
                                                                                                                                    weekendDays,
                                                                                                                                    holidays))
                                                                                    : null;

        /// <inheritdoc cref="Date.AddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime,
                                                 double hours,
                                                 int startOfDayHour,
                                                 int startOfDayMinute,
                                                 int endOfDayHour,
                                                 int endOfDayMinute,
                                                 int breakStartHour,
                                                 int breakStartMin,
                                                 int breakEndHour,
                                                 int breakEndMin,
                                                 params DateTime[] holidays) => dateTime.HasValue
                                                                                    ? new DateTime?(dateTime.Value.AddBusinessHours(hours,
                                                                                                                                    startOfDayHour,
                                                                                                                                    startOfDayMinute,
                                                                                                                                    endOfDayHour,
                                                                                                                                    endOfDayMinute,
                                                                                                                                    breakStartHour,
                                                                                                                                    breakStartMin,
                                                                                                                                    breakEndHour,
                                                                                                                                    breakEndMin,
                                                                                                                                    holidays))
                                                                                    : null;

        /// <inheritdoc
        ///     cref="Date.AddBusinessHours(DateTime, double, int, int, int, int, int, int, int, int, DayOfWeek[], DateTime[])" />
        public static DateTime? AddBusinessHours(this DateTime? dateTime,
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
                                                 params DateTime[] holidays) => dateTime.HasValue
                                                                                    ? new DateTime?(dateTime.Value.AddBusinessHours(hours,
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
                                                                                    : null;

        /// <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" />
        /// <param name="dateTime">
        ///     <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" select="/param[@name='start']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" select="/param[@name='end']/node()" />
        /// </param>
        public static DateDifference DateDifference(this DateTime dateTime, DateTime value) => Date.DateDifference(dateTime, value);

        /// <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" />
        /// <param name="dateTime">
        ///     <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" select="/param[@name='start']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" select="/param[@name='end']/node()" />
        /// </param>
        public static DateDifference DateDifference(this DateTime? dateTime, DateTime value) => dateTime.HasValue
                                                                                                    ? Date.DateDifference(dateTime.Value, value)
                                                                                                    : null;

        /// <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" />
        /// <param name="dateTime">
        ///     <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" select="/param[@name='start']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" select="/param[@name='end']/node()" />
        /// </param>
        public static DateDifference DateDifference(this DateTime dateTime, DateTime? value) => value.HasValue
                                                                                                    ? Date.DateDifference(dateTime, value.Value)
                                                                                                    : null;

        /// <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" />
        /// <param name="dateTime">
        ///     <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" select="/param[@name='start']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Date.DateDifference(DateTime, DateTime)" select="/param[@name='end']/node()" />
        /// </param>
        public static DateDifference DateDifference(this DateTime? dateTime, DateTime? value) => dateTime.HasValue && value.HasValue
                                                                                                     ? Date.DateDifference(dateTime.Value, value.Value)
                                                                                                     : null;

        /// <inheritdoc cref="Date.IsBusinessDay(DateTime, DateTime[])" />
        public static bool IsBusinessDay(this DateTime dateTime, params DateTime[] holidays) => Date.IsBusinessDay(dateTime, holidays);

        /// <inheritdoc cref="Date.IsBusinessDay(DateTime, DayOfWeek[], DateTime[])" />
        public static bool IsBusinessDay(this DateTime dateTime, DayOfWeek[] weekendDays, params DateTime[] holidays) => Date.IsBusinessDay(dateTime, weekendDays, holidays);

        /// <inheritdoc cref="Date.IsBusinessDay(DateTime, DateTime[])" />
        public static bool IsBusinessDay(this DateTime? dateTime, params DateTime[] holidays) => dateTime.HasValue && Date.IsBusinessDay(dateTime.Value, holidays);

        /// <inheritdoc cref="Date.IsBusinessDay(DateTime, DayOfWeek[], DateTime[])" />
        public static bool IsBusinessDay(this DateTime? dateTime, DayOfWeek[] weekendDays, params DateTime[] holidays)
            => dateTime.HasValue && Date.IsBusinessDay(dateTime.Value, weekendDays, holidays);

        /// <summary>
        ///     Determines if the supplied <paramref name="dateTime" /> is a business day and if so returns it, otherwise returns
        ///     the next business day
        /// </summary>
        /// <param name="dateTime">The DateTime to determine is a business day or not</param>
        /// <param name="weekendDays">
        ///     The days of the week to consider as the weekend. These days of the week will be excluded when
        ///     determining business days.
        /// </param>
        /// <param name="holidays">Holidays dates to exclude when determining business days</param>
        /// <returns>The supplied <paramref name="dateTime" /> if it is a valid business day, otherwise the next business day</returns>
        public static DateTime OrNextBusinessDay(this DateTime dateTime, DayOfWeek[] weekendDays, params DateTime[] holidays) => dateTime.IsBusinessDay(weekendDays, holidays)
                                                                                                                                     ? dateTime
                                                                                                                                     : dateTime.AddBusinessDays(1,
                                                                                                                                                                weekendDays,
                                                                                                                                                                holidays);

        /// <inheritdoc cref="OrNextBusinessDay(DateTime, DayOfWeek[], DateTime[])" />
        /// <remarks>Saturday and Sunday are weekend days.</remarks>
        public static DateTime OrNextBusinessDay(this DateTime dateTime, params DateTime[] holidays) => dateTime.IsBusinessDay(holidays)
                                                                                                            ? dateTime
                                                                                                            : dateTime.AddBusinessDays(1, holidays);

        /// <inheritdoc cref="OrNextBusinessDay(DateTime, DayOfWeek[], DateTime[])" />
        public static DateTime? OrNextBusinessDay(this DateTime? dateTime, DayOfWeek[] weekendDays, params DateTime[] holidays) => dateTime.IsBusinessDay(weekendDays, holidays)
                                                                                                                                       ? dateTime
                                                                                                                                       : dateTime.AddBusinessDays(1,
                                                                                                                                                                  weekendDays,
                                                                                                                                                                  holidays);

        /// <inheritdoc cref="OrNextBusinessDay(DateTime, DateTime[])" />
        public static DateTime? OrNextBusinessDay(this DateTime? dateTime, params DateTime[] holidays) => dateTime.IsBusinessDay(holidays)
                                                                                                              ? dateTime
                                                                                                              : dateTime.AddBusinessDays(1, holidays);

        /// <summary>
        ///     Determines if the supplied <paramref name="dateTime" /> is a business day and if so returns it, otherwise returns
        ///     the previous business day
        /// </summary>
        /// <param name="dateTime">The DateTime to determine is a business day or not</param>
        /// <param name="weekendDays">
        ///     The days of the week to consider as the weekend. These days of the week will be excluded when
        ///     determining business days.
        /// </param>
        /// <param name="holidays">Holidays dates to exclude when determining business days</param>
        /// <returns>The supplied <paramref name="dateTime" /> if it is a valid business day, otherwise the previous business day</returns>
        public static DateTime OrPriorBusinessDay(this DateTime dateTime, DayOfWeek[] weekendDays, params DateTime[] holidays) => dateTime.IsBusinessDay(weekendDays, holidays)
                                                                                                                                      ? dateTime
                                                                                                                                      : dateTime.AddBusinessDays(-1,
                                                                                                                                                                 weekendDays,
                                                                                                                                                                 holidays);

        /// <inheritdoc cref="OrPriorBusinessDay(DateTime, DayOfWeek[], DateTime[])" />
        /// <remarks>Saturday and Sunday are weekend days.</remarks>
        public static DateTime OrPriorBusinessDay(this DateTime dateTime, params DateTime[] holidays) => dateTime.IsBusinessDay(holidays)
                                                                                                             ? dateTime
                                                                                                             : dateTime.AddBusinessDays(-1, holidays);

        /// <inheritdoc cref="OrPriorBusinessDay(DateTime, DayOfWeek[], DateTime[])" />
        public static DateTime? OrPriorBusinessDay(this DateTime? dateTime, DayOfWeek[] weekendDays, params DateTime[] holidays) => dateTime.IsBusinessDay(weekendDays, holidays)
                                                                                                                                        ? dateTime
                                                                                                                                        : dateTime.AddBusinessDays(-1,
                                                                                                                                                                   weekendDays,
                                                                                                                                                                   holidays);

        /// <inheritdoc cref="OrPriorBusinessDay(DateTime, DateTime[])" />
        public static DateTime? OrPriorBusinessDay(this DateTime? dateTime, params DateTime[] holidays) => dateTime.IsBusinessDay(holidays)
                                                                                                               ? dateTime
                                                                                                               : dateTime.AddBusinessDays(-1, holidays);

        /// <summary>
        ///     Removes the time from a DateTime object
        /// </summary>
        /// <param name="dateTime">The DateTime to work with</param>
        /// <exception cref="NotImplementedException">RemoveTime has been replaced by WithoutTime. Please use that instead.</exception>
        /// <returns>A DateTime without the time</returns>
        [Obsolete("RemoveTime has been replaced by WithoutTime. Please use that instead.", true)]
        public static DateTime RemoveTime(this DateTime dateTime) => dateTime.WithoutTime();

        /// <inheritdoc cref="RemoveTime(DateTime)" />
        [Obsolete("RemoveTime has been replaced by WithoutTime. Please use that instead.", true)]
        public static DateTime? RemoveTime(this DateTime? dateTime) => dateTime.WithoutTime();

        /// <summary>
        ///     Converts <paramref name="dateTime" /> to a local time using the specified <paramref name="timeZoneId" />
        /// </summary>
        /// <param name="dateTime">The DateTime to convert</param>
        /// <param name="timeZoneId">
        ///     The name of the Time Zone that should be used when converting the DateTime. The Time Zones IDs
        ///     are the same as the ones that work with the <see cref="System.TimeZoneInfo" /> class
        /// </param>
        /// <exception cref="ArgumentException"><paramref name="dateTime" /> must have a Kind of DateTimeKind.Utc</exception>
        /// <returns>
        ///     The <paramref name="dateTime" /> converted to local time using the specified <paramref name="timeZoneId" />
        /// </returns>
        public static DateTime ToLocalFromUtc(this DateTime dateTime, string timeZoneId)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
                throw new ArgumentException("DateTime must have a Kind of DateTimeKind.Utc", nameof(dateTime));

            return ConvertTimeFromUtc(dateTime, FindSystemTimeZoneById(timeZoneId));
        }

        /// <inheritdoc cref="Date.SafeUrlFormat(DateTime)" />
        public static string ToSafeUrlFormat(this DateTime dateTime) => Date.SafeUrlFormat(dateTime);

        /// <inheritdoc cref="Date.SafeUrlFormat(DateTime)" />
        public static string ToSafeUrlFormat(this DateTime? dateTime) => Date.SafeUrlFormat(dateTime);

        /// <inheritdoc cref="Date.TrySafeUrlFormatDecode(string, out DateTime)" />
        public static bool TryDecodeSafeUrlFormat(this string safeUrlFormattedDateTime, out DateTime dateTime)
            => Date.TrySafeUrlFormatDecode(safeUrlFormattedDateTime, out dateTime);

        /// <inheritdoc cref="Date.TrySafeUrlFormatDecode(string, out DateTime)" />
        public static bool TryDecodeSafeUrlFormat(this string safeUrlFormattedDateTime, out DateTime? dateTime)
            => Date.TrySafeUrlFormatDecode(safeUrlFormattedDateTime, out dateTime);

        /// <summary>
        ///     Determines if the specified <paramref name="dateTime" /> is within the given <paramref name="dateRange" />
        /// </summary>
        /// <param name="dateTime">The DateTime to look for within <paramref name="dateRange" /></param>
        /// <param name="dateRange">The date range to look in</param>
        /// <param name="allowSameEndPoints">
        ///     Determines whether the beginning and end of <paramref name="dateRange" /> can equal
        ///     <paramref name="dateTime" />
        /// </param>
        /// <returns>True if <paramref name="dateTime" /> is within <paramref name="dateRange" /></returns>
        public static bool Within(this DateTime dateTime, IDateRange dateRange, bool allowSameEndPoints = true)
        {
            if (dateRange == null)
                return false;

            return allowSameEndPoints
                       ? dateTime > dateRange.Start && dateTime < dateRange.End
                       : dateTime >= dateRange.Start && dateTime <= dateRange.End;
        }

        /// <inheritdoc cref="Within(DateTime, IDateRange, bool)" />
        public static bool Within(this DateTime dateTime, IOpenEndedDateRange dateRange, bool allowSameEndPoints = true)
        {
            if (dateRange == null)
                return false;

            return allowSameEndPoints
                       ? dateTime > dateRange.Start
                       : dateTime >= dateRange.Start;
        }

        /// <inheritdoc cref="Within(DateTime, IDateRange, bool)" />
        public static bool Within(this DateTime? dateTime, IDateRange dateRange, bool allowSameEndPoints = true)
        {
            if (dateRange == null)
                return false;

            return dateTime?.Within(dateRange, allowSameEndPoints) ?? false;
        }

        /// <inheritdoc cref="Within(DateTime, IDateRange, bool)" />
        public static bool Within(this DateTime? dateTime, IOpenEndedDateRange dateRange, bool allowSameEndPoints = true)
        {
            if (dateRange == null)
                return false;

            return dateTime?.Within(dateRange, allowSameEndPoints) ?? false;
        }

        /// <inheritdoc cref="WithNewKind(DateTime, DateTimeKind)" />
        public static DateTime WithNewKind(this DateTime dateTime, string dateTimeKind)
        {
            DateTimeKind kind;
            dateTimeKind.TryGetEnum(out kind);
            return dateTime.WithNewKind(kind);
        }

        /// <inheritdoc cref="WithNewKind(DateTime, DateTimeKind)" />
        public static DateTime? WithNewKind(this DateTime? dateTime, string dateTimeKind)
        {
            DateTimeKind kind;
            dateTimeKind.TryGetEnum(out kind);
            return dateTime.WithNewKind(kind);
        }

        /// <summary>
        ///     Changes the DateTimeKind of the supplied <paramref name="dateTime" />
        /// </summary>
        /// <param name="dateTime">The DateTime for which to change its DateTimeKind</param>
        /// <param name="dateTimeKind">The value to change to</param>
        /// <returns>A DateTime with the new DateTimeKind</returns>
        public static DateTime WithNewKind(this DateTime dateTime, DateTimeKind dateTimeKind)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, dateTimeKind);

        /// <inheritdoc cref="WithNewKind(DateTime, DateTimeKind)" />
        public static DateTime? WithNewKind(this DateTime? dateTime, DateTimeKind dateTimeKind) => dateTime.HasValue
                                                                                                       ? new DateTime?(new DateTime(dateTime.Value.Year,
                                                                                                                                    dateTime.Value.Month,
                                                                                                                                    dateTime.Value.Day,
                                                                                                                                    dateTime.Value.Hour,
                                                                                                                                    dateTime.Value.Minute,
                                                                                                                                    dateTime.Value.Second,
                                                                                                                                    dateTime.Value.Millisecond,
                                                                                                                                    dateTimeKind))
                                                                                                       : null;

        /// <inheritdoc cref="WithNewTime(DateTime, string)" />
        public static DateTime? WithNewTime(this DateTime? dateTime, string time) => dateTime.HasValue
                                                                                         ? new DateTime?(dateTime.Value.WithNewTime(time))
                                                                                         : null;

        /// <summary>
        ///     Changes the time of the supplied <paramref name="dateTime" />
        /// </summary>
        /// <param name="dateTime">The DateTime for which to change its time</param>
        /// <param name="time">The time as a string to change to</param>
        /// <exception cref="FormatException">The provided <paramref name="time" /> format is invalid</exception>
        /// <returns>The provided <paramref name="dateTime" /> with the specified <paramref name="time" /></returns>
        /// <example>
        ///     <code language="c#">
        /// <![CDATA[
        /// var dateTime = DateTime.Now.WithNewTime("11:59:59 PM");
        /// ]]>
        /// </code>
        /// </example>
        public static DateTime WithNewTime(this DateTime dateTime, string time)
        {
            var exceptionMessage = Format(INVALID_TIME_FORMAT_EXCEPTION_MESSAGE, time);
            var hourModifier = 0;
            var hourMode24 = false;
            var hours = 0;
            var minutes = 0;
            var seconds = 0;
            var milliseconds = 0;

            if (IsNullOrWhiteSpace(time))
                throw new FormatException(exceptionMessage);

            time = time.Trim()
                       .ToLowerInvariant();
            if (time.EndsWith("am"))
            {
                time = time.Replace("am", Empty)
                           .Trim();
            }
            else if (time.EndsWith("pm"))
            {
                time = time.Replace("pm", Empty)
                           .Trim();
                hourModifier = 12;
            }
            else
                hourMode24 = true;

            var timeParts = time.Split(new[] { ':' }, RemoveEmptyEntries);

            if (!timeParts.Any())
                throw new FormatException(exceptionMessage);

            for (var index = 0; index < timeParts.Length; index++)
            {
                int timePart;
                if (!int.TryParse(timeParts[index], out timePart))
                    throw new FormatException(exceptionMessage);

                switch (index)
                {
                    case 0:
                        hours = hourMode24
                                    ? timePart
                                    : timePart + hourModifier;

                        if (hours < 0 || hours > 23)
                            throw new FormatException(exceptionMessage);

                        break;
                    case 1:
                        minutes = timePart;

                        if (minutes < 0 || minutes > 59)
                            throw new FormatException(exceptionMessage);

                        break;
                    case 2:
                        seconds = timePart;

                        if (seconds < 0 || seconds > 59)
                            throw new FormatException(exceptionMessage);

                        break;
                    case 3:
                        milliseconds = timePart;

                        if (milliseconds < 0 || milliseconds > 999)
                            throw new FormatException(exceptionMessage);

                        break;
                    default:
                        throw new FormatException(exceptionMessage);
                }
            }

            return dateTime.WithNewTime(hours, minutes, seconds, milliseconds);
        }

        /// <inheritdoc cref="WithNewTime(DateTime, string)" />
        /// <param name="hours">The hours (0 through 23)</param>
        /// <param name="minutes">The minutes (0 through 59)</param>
        /// <param name="seconds">The seconds (0 through 59)</param>
        /// <param name="milleseconds">The milliseconds (0 through 999)</param>
        /// <example>
        ///     <code language="c#">
        /// <![CDATA[
        /// var dateTime = DateTime.Now.WithNewTime(11, 59, 59, 0);
        /// ]]>
        /// </code>
        /// </example>
        public static DateTime WithNewTime(this DateTime dateTime, int hours, int minutes = 0, int seconds = 0, int milleseconds = 0)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hours, minutes, seconds, milleseconds, dateTime.Kind);

        /// <inheritdoc cref="WithNewTime(DateTime, int, int, int, int)" />
        public static DateTime? WithNewTime(this DateTime? dateTime, int hours, int minutes = 0, int seconds = 0, int milleseconds = 0) => dateTime.HasValue
                                                                                                                                               ? new DateTime?(
                                                                                                                                                     new DateTime(
                                                                                                                                                         dateTime.Value.Year,
                                                                                                                                                         dateTime.Value.Month,
                                                                                                                                                         dateTime.Value.Day,
                                                                                                                                                         hours,
                                                                                                                                                         minutes,
                                                                                                                                                         seconds,
                                                                                                                                                         milleseconds,
                                                                                                                                                         dateTime.Value.Kind))
                                                                                                                                               : null;

        /// <summary>
        ///     Removes the time portion of a DateTime
        /// </summary>
        /// <param name="dateTime">The DateTime to work with</param>
        /// <returns>A DateTime without the time value</returns>
        public static DateTime WithoutTime(this DateTime dateTime) => dateTime.Date;

        /// <inheritdoc cref="WithoutTime(DateTime)" />
        public static DateTime? WithoutTime(this DateTime? dateTime) => dateTime.HasValue
                                                                            ? new DateTime?(dateTime.Value.WithoutTime())
                                                                            : null;

        #endregion
    }
}

#pragma warning restore 1573
