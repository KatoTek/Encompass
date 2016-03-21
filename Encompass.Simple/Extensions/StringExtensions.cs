using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Encompass.Simple.Format;
using Microsoft.VisualBasic.FileIO;
using static System.Enum;
using static System.String;
using static System.StringComparison;
using static System.Text.Encoding;
using static System.Text.RegularExpressions.Regex;
using static System.Text.RegularExpressions.RegexOptions;
using static Encompass.Simple.Format.Html;
using static Encompass.Simple.Is;

#pragma warning disable 1573

namespace Encompass.Simple.Extensions
{
    /// <summary>
    ///     String extension methods
    /// </summary>
    public static class StringExtensions
    {
        #region methods

        /// <summary>
        ///     Converts a camel cased string to words separated by spaces
        /// </summary>
        /// <param name="source">The string to convert</param>
        /// <returns>A string with words separated by spaces</returns>
        public static string CamelCaseToWords(this string source) => Replace(source, "([A-Z])", " $1", Compiled)
            .Trim();

        /// <summary>
        ///     Determines if a string contains a substring
        /// </summary>
        /// <param name="source">The string to check</param>
        /// <param name="value">The substring to look for within the <paramref name="source" /></param>
        /// <param name="stringComparison">Rules to use when comparing the strings</param>
        /// <returns>True if the <paramref name="value" /> substring is found within the <paramref name="source" /> string</returns>
        public static bool Contains(this string source, string value, StringComparison stringComparison) => source.IndexOf(value, stringComparison) >= 0;

        /// <summary>
        ///     Checks if a string contains any of the given string values.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <param name="values">The string values to check whether contained within.</param>
        /// <returns>True when any of the values are contained within.</returns>
        public static bool ContainsAny(this string value, params string[] values) => values.Any(value.Contains);

        /// <summary>
        ///     Checks if a string contains any of the given characters.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <param name="values">The characters to check whether contained within.</param>
        /// <returns>True when any of the values are contained within.</returns>
        public static bool ContainsAny(this string value, params char[] values) => values.Any(value.Contains);

        /// <inheritdoc cref="String.Format(IFormatProvider, string, object[])" />
        /// <param name="source">
        ///     <inheritdoc cref="String.Format(IFormatProvider, string, object[])" select="/param[@name='format']/node()" />
        /// </param>
        public static string FormatWith(this string source, params object[] args) => string.Format(CultureInfo.CurrentCulture, source, args);

        /// <summary>
        ///     Converts a string to a specified Enum type
        /// </summary>
        /// <typeparam name="T">Type of enum to convert to</typeparam>
        /// <param name="source">The char to convert</param>
        /// <param name="ignoreWhiteSpace">Specifies whether whitespace should be ignored</param>
        /// <param name="ignoreCase">Specifies whether case should be ignored</param>
        /// <returns>The enum that matches the char</returns>
        public static T GetEnum<T>(this string source, bool ignoreWhiteSpace = false, bool ignoreCase = false) where T : struct, IConvertible
        {
            if (ignoreWhiteSpace)
                source = source.RemoveWhiteSpace();

            return (T)Parse(typeof(T), source, ignoreCase);
        }

        /// <summary>
        ///     Concatonates the members of a constructed collection of type String using a seperator and grouping characters.
        /// </summary>
        /// <param name="values">The collection to concatonate.</param>
        /// <param name="seperator">The string to use for seperation.</param>
        /// <param name="groupStart">The character used to signifiy a group start.</param>
        /// <param name="groupEnd">The character used to signify a group end.</param>
        /// <param name="forceGrouping">
        ///     When true, group characters are always used unconditionally. When false, group characters
        ///     are only used when string value contains seperator values or group characters.
        /// </param>
        /// <returns>A concatonated string.</returns>
        public static string GroupJoin(this IEnumerable<string> values, string seperator, char? groupStart, char? groupEnd, bool forceGrouping)
        {
            var s = IsNullOrEmpty(seperator)
                        ? Empty
                        : seperator;

            if (groupStart == null || groupEnd == null)
                return Join(s, values.Where(item => IsNullOrEmpty(s) || !item.Contains(s)));

            var gs = groupStart.Value.ToString();
            var ge = groupEnd.Value.ToString();

            if (forceGrouping)
                return Join(s, values.Select(value => $"{gs}{EscapeQualifiers(value, gs, ge)}{ge}"));

            var sArray = s.Select(c => c.ToString())
                          .Union(new[] { gs, ge })
                          .ToArray();

            return Join(s,
                        values.Select(value => value.ContainsAny(sArray)
                                                   ? $"{gs}{EscapeQualifiers(value, gs, ge)}{ge}"
                                                   : value));
        }

        /// <summary>
        ///     Concatonates the members of a constructed collection of type String using a seperator and grouping characters.
        /// </summary>
        /// <param name="values">The collection to concatonate.</param>
        /// <param name="seperator">The string to use for seperation.</param>
        /// <param name="groupStart">The character used to signifiy a group start.</param>
        /// <param name="groupEnd">The character used to signify a group end.</param>
        /// <returns>A concatonated string.</returns>
        public static string GroupJoin(this IEnumerable<string> values, string seperator, char? groupStart, char? groupEnd)
            => values.GroupJoin(seperator, groupStart, groupEnd, true);

        /// <summary>
        ///     Removes the middle of a string and replaces it with the string specified in the <paramref name="guts" /> param to
        ///     meet the specified max length requirements
        /// </summary>
        /// <param name="source">The string to work with</param>
        /// <param name="length">The number of characters to show at the beginning and end of the string</param>
        /// <param name="maxLength">The maximum number of characters to show</param>
        /// <param name="guts">The string to replace the middle of the <paramref name="source" /> string with</param>
        /// <returns>
        ///     A string with the middle replaced with the <paramref name="guts" /> param if it exceeds the specified max
        ///     length
        /// </returns>
        public static string Gut(this string source, int length, int maxLength, string guts = " ... ")
        {
            if (length > maxLength)
                throw new ArgumentException($"MaxLength must be greater than {length}", nameof(maxLength));

            guts.IfIsNullThen(() => guts = Empty);

            length = length - guts.Length;
            if (length < 0)
                throw new ArgumentException($"Length must be at least {guts.Length} characters.", nameof(length));

            if (!source.IsNotNullAndNotEmpty() || source.Length <= maxLength)
                return source;

            if (length%2 == 1)
                length++;

            var half = length/2;
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{source.Substring(0, half)}{guts}{source.Substring(source.Length - half, half)}");
            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Removes the middle of a string and replaces it with the string specified in the <paramref name="guts" /> param
        /// </summary>
        /// <param name="source">The string to work with</param>
        /// <param name="length">The number of characters to show at the beginning and end of the string</param>
        /// <param name="guts">The string to replace the middle of the <paramref name="source" /> string with</param>
        /// <returns>A string with the middle replaced with the <paramref name="guts" /> param</returns>
        public static string Gut(this string source, int length, string guts = " ... ") => source.Gut(length, length, guts);

        /// <summary>
        ///     Provides a way to use a replacement value when the <paramref name="source" /> string is null or empty
        /// </summary>
        /// <param name="source">The string to work with</param>
        /// <param name="else">The replacement string if <paramref name="source" /> is null or empty</param>
        /// <returns>
        ///     The <paramref name="source" /> string if it is not null and not empty, otherwise returns the
        ///     <paramref name="else" /> string
        /// </returns>
        public static string IfNotNullOrEmptyElse(this string source, string @else) => source.IsNullOrEmpty()
                                                                                           ? @else
                                                                                           : source;

        /// <summary>
        ///     Provides a way to use a replacement value when the <paramref name="source" /> string is null or whitespace
        /// </summary>
        /// <param name="source">The string to work with</param>
        /// <param name="else">The replacement string if <paramref name="source" /> is null or whitespace</param>
        /// <returns>
        ///     The <paramref name="source" /> string if it is not null and not whitespace, otherwise returns the
        ///     <paramref name="else" /> string
        /// </returns>
        public static string IfNotNullOrWhiteSpaceElse(this string source, string @else) => source.IsNullOrWhiteSpace()
                                                                                                ? @else
                                                                                                : source;

        /// <inheritdoc cref="Breaks(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Html.Breaks(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static string InjectHtmlLineBreaks(this string source) => Breaks(source);

        /// <inheritdoc cref="Paragraphs(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Html.Paragraphs(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static string InjectHtmlParagraphs(this string source) => Paragraphs(source);

        /// <inheritdoc cref="Is.AllNotNullAndNotEmpty(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNotNullAndNotEmpty(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAllNotNullAndNotEmpty(this IEnumerable<string> source) => AllNotNullAndNotEmpty(source.ToArray());

        /// <inheritdoc cref="AllNotNullAndNotEmpty(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNotNullAndNotEmpty(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAllNotNullAndNotEmpty(this string[] source) => AllNotNullAndNotEmpty(source);

        /// <inheritdoc cref="AllNotNullAndNotWhiteSpace(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNotNullAndNotWhiteSpace(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAllNotNullAndNotWhiteSpace(this IEnumerable<string> source) => AllNotNullAndNotWhiteSpace(source.ToArray());

        /// <inheritdoc cref="AllNotNullAndNotWhiteSpace(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNotNullAndNotWhiteSpace(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAllNotNullAndNotWhiteSpace(this string[] source) => AllNotNullAndNotWhiteSpace(source);

        /// <inheritdoc cref="AllNullOrEmpty(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNullOrEmpty(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAllNullOrEmpty(this IEnumerable<string> source) => AllNullOrEmpty(source.ToArray());

        /// <inheritdoc cref="AllNullOrEmpty(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNullOrEmpty(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAllNullOrEmpty(this string[] source) => AllNullOrEmpty(source);

        /// <inheritdoc cref="AllNullOrWhiteSpace(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNullOrWhiteSpace(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAllNullOrWhiteSpace(this IEnumerable<string> source) => AllNullOrWhiteSpace(source.ToArray());

        /// <inheritdoc cref="AllNullOrWhiteSpace(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNullOrWhiteSpace(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAllNullOrWhiteSpace(this string[] source) => AllNullOrWhiteSpace(source);

        /// <inheritdoc cref="AnyNullOrEmpty(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AnyNullOrEmpty(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAnyNullOrEmpty(this IEnumerable<string> source) => AnyNullOrEmpty(source.ToArray());

        /// <inheritdoc cref="AnyNullOrEmpty(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AnyNullOrEmpty(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAnyNullOrEmpty(this string[] source) => AnyNullOrEmpty(source);

        /// <inheritdoc cref="AnyNullOrWhiteSpace(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AnyNullOrWhiteSpace(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAnyNullOrWhiteSpace(this IEnumerable<string> source) => AnyNullOrWhiteSpace(source.ToArray());

        /// <inheritdoc cref="AnyNullOrWhiteSpace(string[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AnyNullOrWhiteSpace(string[])" select="/param[@name='values']/node()" />
        /// </param>
        public static bool IsAnyNullOrWhiteSpace(this string[] source) => AnyNullOrWhiteSpace(source);

        /// <inheritdoc cref="Is.Boolean(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Boolean(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsBoolean(this string source) => Boolean(source);

        /// <inheritdoc cref="Is.Boolean(string, out bool)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Boolean(string, out bool)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.Boolean(string, out bool)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsBoolean(this string source, out bool value) => Boolean(source, out value);

        /// <inheritdoc cref="Is.Byte(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Byte(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsByte(this string source) => Byte(source);

        /// <inheritdoc cref="Is.Byte(string, out byte)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Byte(string, out byte)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.Byte(string, out byte)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsByte(this string source, out byte value) => Byte(source, out value);

        /// <inheritdoc cref="Is.Decimal(string, out decimal)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Decimal(string, out decimal)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.Decimal(string, out decimal)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsDecimal(this string source, out decimal value) => Decimal(source, out value);

        /// <inheritdoc cref="Is.Decimal(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Decimal(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsDecimal(this string source) => Decimal(source);

        /// <inheritdoc cref="Is.Double(string, out double)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Double(string, out double)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.Double(string, out double)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsDouble(this string source, out double value) => Double(source, out value);

        /// <inheritdoc cref="Is.Double(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Double(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsDouble(this string source) => Double(source);

        /// <inheritdoc cref="Is.EmailAddress(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.EmailAddress(string)" select="/param[@name='str']/node()" />
        /// </param>
        public static bool IsEmailAddress(this string source) => EmailAddress(source);

        /// <inheritdoc cref="Is.Float(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Float(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsFloat(this string source) => Float(source);

        /// <inheritdoc cref="Is.Float(string, out float)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Float(string, out float)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.Float(string, out float)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsFloat(this string source, out float value) => Float(source, out value);

        /// <inheritdoc cref="Is.Html(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Html(string)" select="/param[@name='str']/node()" />
        /// </param>
        public static bool IsHtml(this string source) => Html(source);

        /// <inheritdoc cref="Is.Integer(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Integer(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsInteger(this string source) => Integer(source);

        /// <inheritdoc cref="Is.Integer(string, out int)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Integer(string, out int)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.Integer(string, out int)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsInteger(this string source, out int value) => Integer(source, out value);

        /// <inheritdoc cref="Is.IPv4(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.IPv4(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsIPv4(this string source) => IPv4(source);

        /// <inheritdoc cref="Is.Long(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Long(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsLong(this string source) => Long(source);

        /// <inheritdoc cref="Is.Long(string, out long)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Long(string, out long)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.Long(string, out long)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsLong(this string source, out long value) => Long(source, out value);

        /// <inheritdoc cref="Is.NotNullAndNotEmpty(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.NotNullAndNotEmpty(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsNotNullAndNotEmpty(this string source) => NotNullAndNotEmpty(source);

        /// <inheritdoc cref="Is.NotNullAndNotWhiteSpace(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.NotNullAndNotWhiteSpace(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsNotNullAndNotWhiteSpace(this string source) => NotNullAndNotWhiteSpace(source);

        /// <inheritdoc cref="Is.NullOrEmpty(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.NullOrEmpty(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsNullOrEmpty(this string source) => NullOrEmpty(source);

        /// <inheritdoc cref="Is.NullOrWhiteSpace(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.NullOrWhiteSpace(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsNullOrWhiteSpace(this string source) => NullOrWhiteSpace(source);

        /// <inheritdoc cref="Is.Numeric(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Numeric(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsNumeric(this string source) => Numeric(source);

        /// <inheritdoc cref="Is.Short(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Short(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsShort(this string source) => Short(source);

        /// <inheritdoc cref="Is.Short(string, out short)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.Short(string, out short)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.Short(string, out short)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsShort(this string source, out short value) => Short(source, out value);

        /// <inheritdoc cref="Is.SignedByte(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.SignedByte(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsSignedByte(this string source) => SignedByte(source);

        /// <inheritdoc cref="Is.SignedByte(string, out sbyte)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.SignedByte(string, out sbyte)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.SignedByte(string, out sbyte)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsSignedByte(this string source, out sbyte value) => SignedByte(source, out value);

        /// <inheritdoc cref="Is.UnsignedInteger(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.UnsignedInteger(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsUnsignedInteger(this string source) => UnsignedInteger(source);

        /// <inheritdoc cref="Is.UnsignedInteger(string, out uint)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.UnsignedInteger(string, out uint)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.UnsignedInteger(string, out uint)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsUnsignedInteger(this string source, out uint value) => UnsignedInteger(source, out value);

        /// <inheritdoc cref="Is.UnsignedLong(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.UnsignedLong(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsUnsignedLong(this string source) => UnsignedLong(source);

        /// <inheritdoc cref="Is.UnsignedLong(string, out ulong)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.UnsignedLong(string, out ulong)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.UnsignedLong(string, out ulong)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsUnsignedLong(this string source, out ulong value) => UnsignedLong(source, out value);

        /// <inheritdoc cref="Is.UnsignedShort(string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.UnsignedShort(string)" select="/param[@name='value']/node()" />
        /// </param>
        public static bool IsUnsignedShort(this string source) => UnsignedShort(source);

        /// <inheritdoc cref="Is.UnsignedShort(string, out ushort)" />
        /// <param name="source">
        ///     <inheritdoc cref="Is.UnsignedShort(string, out ushort)" select="/param[@name='value']/node()" />
        /// </param>
        /// <param name="value">
        ///     <inheritdoc cref="Is.UnsignedShort(string, out ushort)" select="/param[@name='returnValue']/node()" />
        /// </param>
        public static bool IsUnsignedShort(this string source, out ushort value) => UnsignedShort(source, out value);

        /// <summary>
        ///     Trims a string from the left by a single character occurance.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimChar">The character match to trim away.</param>
        /// <returns>A trimmed string.</returns>
        public static string LeftSingleTrim(this string value, char trimChar) => IsNullOrEmpty(value)
                                                                                     ? value
                                                                                     : value.StartsWith(trimChar.ToString())
                                                                                           ? value.Substring(1, value.Length - 1)
                                                                                           : value;

        /// <summary>
        ///     Trims a string from the left by a single string occurance.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimString">The string match to trim away.</param>
        /// <returns>A trimmed string.</returns>
        public static string LeftSingleTrim(this string value, string trimString) => IsNullOrEmpty(value)
                                                                                         ? value
                                                                                         : value.StartsWith(trimString)
                                                                                               ? value.Substring(trimString.Length, value.Length - trimString.Length)
                                                                                               : value;

        /// <summary>
        ///     Takes a string and separates it into segments using a comma as the delimiter and returns it as an array
        /// </summary>
        /// <param name="source">The string to parse</param>
        /// <returns>An array of strings</returns>
        public static string[] ParseCsvRow(this string source) => source.ParseCsvRow(new[] { "," });

        /// <summary>
        ///     Takes a string and separates it into segments using the specified delimiters and returns it as an array
        /// </summary>
        /// <param name="source">The string to parse</param>
        /// <param name="delimiters">The delimeters to use to break up the string</param>
        /// <returns>An array of strings</returns>
        public static string[] ParseCsvRow(this string source, string[] delimiters)
        {
            using (var parser = new TextFieldParser(new MemoryStream(UTF8.GetBytes(source))))
            {
                parser.Delimiters = delimiters;
                while (!parser.EndOfData)
                    return parser.ReadFields();
            }

            return new string[] { };
        }

        /// <summary>
        ///     Removes white space characters from a string
        /// </summary>
        /// <param name="source">The string to work with</param>
        /// <returns>The <paramref name="source" /> string without white space characters</returns>
        public static string RemoveWhiteSpace(this string source) => new string((from char c in source
                                                                                 where !char.IsWhiteSpace(c)
                                                                                 select c).ToArray());

        /// <summary>
        ///     Trims a string from the right by a single character occurance.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimChar">The character match to trim away.</param>
        /// <returns>A trimmed string.</returns>
        public static string RightSingleTrim(this string value, char trimChar) => IsNullOrEmpty(value)
                                                                                      ? value
                                                                                      : value.EndsWith(trimChar.ToString())
                                                                                            ? value.Substring(0, value.Length - 1)
                                                                                            : value;

        /// <summary>
        ///     Trims a string from the right by a single string occurance.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimString">The string match to trim away.</param>
        /// <returns>A trimmed string.</returns>
        public static string RightSingleTrim(this string value, string trimString) => IsNullOrEmpty(value)
                                                                                          ? value
                                                                                          : value.EndsWith(trimString)
                                                                                                ? value.Substring(0, value.Length - trimString.Length)
                                                                                                : value;

        /// <inheritdoc cref="ToSentenceFormat(IEnumerable{string}, string, string)" />
        /// <remarks>Use the method <see cref="ToSentenceFormat(IEnumerable{string}, string, string)" /> instead.</remarks>
        [Obsolete("Use the method ToSentenceFormat instead.")]
        public static string SentenceList(this IEnumerable<string> source) => source.ToSentenceFormat();

        /// <summary>
        ///     Trims a string from both ends by a single character occurance.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimChar">The character match to trim away.</param>
        /// <returns>A trimmed string.</returns>
        public static string SingleTrim(this string value, char trimChar) => value.LeftSingleTrim(trimChar)
                                                                                  .RightSingleTrim(trimChar);

        /// <summary>
        ///     Trims a string from both ends by a single string occurance.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <param name="trimString">The string match to trim away.</param>
        /// <returns>A trimmed string.</returns>
        public static string SingleTrim(this string value, string trimString) => value.LeftSingleTrim(trimString)
                                                                                      .RightSingleTrim(trimString);

        /// <summary>
        ///     Takes a string and formats it so it is SQL safe.
        /// </summary>
        /// <remarks>Use anytime the string is going back to the SQL Server layer.</remarks>
        /// <param name="source">The string containing SQL to format</param>
        /// <returns>A string that is safe to use as SQL</returns>
        public static string SqlFormat(this string source) => source.Replace("[", "[[]")
                                                                    .Replace("_", "[_]")
                                                                    .Replace("%", "[%]")
                                                                    .ToLower();

        /// <inheritdoc cref="SqlFormat(string)" />
        /// <param name="wildcard">A character to use as a wildcard</param>
        public static string SqlFormat(this string source, char wildcard) => source.SqlFormat()
                                                                                   .Replace(wildcard, '%');

        /// <summary>
        ///     Converts a string to a camel cased string
        /// </summary>
        /// <param name="source">The string to camel case</param>
        /// <returns>A camel cased string</returns>
        public static string ToCamelCase(this string source)
        {
            var chr = source[0];
            return Concat(chr.ToString()
                             .ToLowerInvariant(),
                          source.Substring(1));
        }

        /// <summary>
        ///     Converts a collection of strings into a string that can be used in a sentence.
        /// </summary>
        /// <param name="source">Collection of strings to work with</param>
        /// <param name="delimiter">The delimiter to use between multiple items in the sentence</param>
        /// <param name="finalDelimiter">The final delimiter to use before the last item in the sentence</param>
        /// <returns>A string containing all items in the <paramref name="source" /> collection in the format of a sentence</returns>
        public static string ToSentenceFormat(this IEnumerable<string> source, string delimiter = ", ", string finalDelimiter = " and ")
        {
            var sourceArray = source as string[] ?? source.ToArray();
            if (sourceArray.Length <= 1)
            {
                return sourceArray.Length == 1
                           ? sourceArray.FirstOrDefault()
                           : Empty;
            }

            var list = sourceArray.Aggregate((x, y) => $"{x}{delimiter}{y}");
            var lastSeperatorPosition = list.LastIndexOf(delimiter, Ordinal);
            return list.Remove(lastSeperatorPosition, delimiter.Length)
                       .Insert(lastSeperatorPosition, finalDelimiter);
        }

        /// <summary>
        ///     Trims a string and reduces any whitespace within to single spaces.
        /// </summary>
        /// <param name="value">The string to trim within.</param>
        /// <returns>A string with all whitespace reduced.</returns>
        public static string TrimWithin(this string value) => IsNullOrWhiteSpace(value)
                                                                  ? Empty
                                                                  : Replace(value, @"\s+|\t|\n|\r", " ")
                                                                        .Trim();

        /// <inheritdoc cref="StringExtensions.GetEnum{T}(string, bool, bool)" />
        /// <param name="result">The enum item that matches the <paramref name="source" /> string</param>
        public static bool TryGetEnum<T>(this string source, out T result, bool ignoreCase = false, bool ignoreWhiteSpace = false) where T : struct, IConvertible
        {
            if (ignoreWhiteSpace)
                source = source.RemoveWhiteSpace();

            return TryParse(source, ignoreCase, out result);
        }

        /// <summary>
        ///     Returns an array of string words defined by a given set of delimiter characters.
        /// </summary>
        /// <param name="value">The string to examine.</param>
        /// <param name="delimiters">The collection of delimiter characters used to split into string words.</param>
        /// <returns>An array of string words.</returns>
        public static string[] Words(this string value, IEnumerable<char> delimiters)
        {
            if (IsNullOrEmpty(value))
                return new string[0];

            if (delimiters == null)
                return new[] { value };

            var delimitersArray = delimiters as char[] ?? delimiters.ToArray();
            if (!delimitersArray.Any())
                return new[] { value };

            return
                Split(value,
                      $"[{delimitersArray.Aggregate(new StringBuilder(), (sb, delimiter) => sb.Append($"{Escape(delimiter.ToString())}")) .ToString() .Trim('|') .Replace(' ', 's')}]+")
                    .Select(s => s?.Trim())
                    .Where(s => !IsNullOrEmpty(s))
                    .ToArray();
        }

        /// <summary>
        ///     Returns an array of string words defined by a given qualifying character and set of delimiter characters.
        /// </summary>
        /// <param name="value">The string to examine.</param>
        /// <param name="qualifier">The qualifier character used to define string words.</param>
        /// <param name="delimiters">The collection of delimiter characters used to split into string words.</param>
        /// <returns>An array of string words.</returns>
        public static string[] Words(this string value, char? qualifier, IEnumerable<char> delimiters)
        {
            if (IsNullOrEmpty(value))
                return new string[0];

            if (!qualifier.HasValue)
                return value.Words(delimiters);

            if (delimiters == null)
                return new[] { value };

            var delimitersArray = delimiters as char[] ?? delimiters.ToArray();
            if (!delimitersArray.Any())
                return new[] { value };

            var d =
                $"[{delimitersArray.Aggregate(new StringBuilder(), (sb, delimiter) => sb.Append($"{Escape(delimiter.ToString())}")) .ToString() .Trim('|') .Replace(' ', 's')}]+";
            var q = Escape(qualifier.ToString());
            return Split(value, $"{d}(?=(?:[^{q}]*{q}[^{q}]*{q})*(?![^{q}]*{q}))")
                .Select(s => s?.Trim()
                               .SingleTrim(qualifier.Value)
                               .Replace($"{qualifier.Value.ToString()}{qualifier.Value.ToString()}", qualifier.Value.ToString()))
                .Where(s => !IsNullOrEmpty(s))
                .ToArray();
        }

        private static string EscapeQualifiers(string value, string q1, string q2)
        {
            return q1 == q2
                       ? value.Replace(q1, $"{q1}{q1}")
                       : value.Replace(q1, $"{q1}{q1}")
                              .Replace(q2, $"{q2}{q2}");
        }

        #endregion
    }
}

#pragma warning restore 1573
