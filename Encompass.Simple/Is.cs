using System;
using System.Linq;
using static System.String;
using static System.Text.RegularExpressions.Regex;
using static System.Web.HttpUtility;

namespace Encompass.Simple
{
    /// <summary>
    /// Exposes methods that are read as "Is" {MethodName}. Intended to simplify syntax and read like English.
    /// </summary>
    public static class Is
    {
        /// <summary>
        /// Checks if all booleans are false.
        /// </summary>
        /// <param name="bools">The booleans to check.</param>
        /// <returns>True when all booleans are false.</returns>
        public static bool AllFalse(params bool[] bools) => !AnyTrue(bools);

        /// <summary>
        /// Checks all arguments are not null.
        /// </summary>
        /// <param name="objs">Objects to check.</param>
        /// <returns>True when all objects are not null.</returns>
        public static bool AllNotNull(params object[] objs) => objs.All(obj => !Null(obj));

        /// <summary>
        /// Checks all strings are not null and are not empty
        /// </summary>
        /// <param name="values">Strings to check.</param>
        /// <returns>True when all strings are not null and are not empty.</returns>
        public static bool AllNotNullAndNotEmpty(params string[] values) => values.All(value => !NullOrEmpty(value));

        /// <summary>
        /// Checks all strings are not null and are not WhiteSpace
        /// </summary>
        /// <param name="values">Strings to check.</param>
        /// <returns>True when all strings are not null and are not WhiteSpace.</returns>
        public static bool AllNotNullAndNotWhiteSpace(params string[] values) => values.All(value => !NullOrWhiteSpace(value));

        /// <summary>
        /// Checks all arguments are null.
        /// </summary>
        /// <param name="objs">Objects to check.</param>
        /// <returns>True when all objects are null.</returns>
        public static bool AllNull(params object[] objs) => objs.All(obj => !NotNull(obj));

        /// <summary>
        /// Checks all strings are null or empty.
        /// </summary>
        /// <param name="values">Strings to check.</param>
        /// <returns>True when all strings are null or empty.</returns>
        public static bool AllNullOrEmpty(params string[] values) => values.All(value => !NotNullAndNotEmpty(value));

        /// <summary>
        /// Checkes all strings are null or WhiteSpace.
        /// </summary>
        /// <param name="values">Strings to check.</param>
        /// <returns>True when all strings are null or WhiteSpace.</returns>
        public static bool AllNullOrWhiteSpace(params string[] values) => values.All(value => !NotNullAndNotWhiteSpace(value));

        /// <summary>
        /// Checks if all booleans are true.
        /// </summary>
        /// <param name="bools">The booleans to check.</param>
        /// <returns>True when all booleans are true.</returns>
        public static bool AllTrue(params bool[] bools) => !AnyFalse(bools);

        /// <summary>
        /// Checks if any booleans are false.
        /// </summary>
        /// <param name="bools">The booleans to check.</param>
        /// <returns>True when any boolean is false.</returns>
        public static bool AnyFalse(params bool[] bools) => bools.Any(b => !b);

        /// <summary>
        /// Checks if any arguments are null.
        /// </summary>
        /// <param name="objs">Objects to check</param>
        /// <returns>True when any object is null.</returns>
        public static bool AnyNull(params object[] objs) => objs.Any(Null);

        /// <summary>
        /// Checks if any string is null or empty.
        /// </summary>
        /// <param name="values">Strings to check.</param>
        /// <returns>True when any string is null or empty.</returns>
        public static bool AnyNullOrEmpty(params string[] values) => values.Any(NullOrEmpty);

        /// <summary>
        /// Checks if any string is null or WhiteSpace.
        /// </summary>
        /// <param name="values">Strings to check.</param>
        /// <returns>True when any string is null or WhiteSpace.</returns>
        public static bool AnyNullOrWhiteSpace(params string[] values) => values.Any(NullOrWhiteSpace);

        /// <summary>
        /// Checks if any booleans are true.
        /// </summary>
        /// <param name="bools">The booleans to check.</param>
        /// <returns>True when any boolean is true.</returns>
        public static bool AnyTrue(params bool[] bools) => bools.Any(b => b);

        /// <summary>
        /// Checks if a string can be parsed to a Boolean.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a Boolean.</returns>
        public static bool Boolean(string value)
        {
            bool returnValue;
            return Boolean(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a Boolean.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The Boolean value of the parsed string.</param>
        /// <returns>True if can be parsed to a Boolean.</returns>
        public static bool Boolean(string value, out bool returnValue) => bool.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string can be parsed to a Byte.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a Byte.</returns>
        public static bool Byte(string value)
        {
            byte returnValue;
            return Byte(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a Byte.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The Byte value of the parsed string.</param>
        /// <returns>True if can be parsed to a Byte.</returns>
        public static bool Byte(string value, out byte returnValue) => byte.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string can be parsed to a Decimal.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The Decimal value of the parsed string.</param>
        /// <returns>True if can be parsed to a Decimal.</returns>
        public static bool Decimal(string value, out decimal returnValue) => decimal.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string can be parsed to a Decimal.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a Decimal.</returns>
        public static bool Decimal(string value)
        {
            decimal returnValue;
            return Decimal(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a Double.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The Double value of the parsed string.</param>
        /// <returns>True if can be parsed to a Double.</returns>
        public static bool Double(string value, out double returnValue) => double.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string can be parsed to a Double.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a Double.</returns>
        public static bool Double(string value)
        {
            double returnValue;
            return Double(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string is a valid email address.
        /// </summary>
        /// <param name="str">String to check.</param>
        /// <returns>True when the string is a valid email address.</returns>
        public static bool EmailAddress(string str)
            => !IsNullOrWhiteSpace(str) && IsMatch(str.ToLower(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

        /// <summary>
        /// Checks if a string can be parsed to a Single.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a Single.</returns>
        public static bool Float(string value)
        {
            float returnValue;
            return Float(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a Single.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The Single value of the parsed string.</param>
        /// <returns>True if can be parsed to a Single.</returns>
        public static bool Float(string value, out float returnValue) => float.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string is or contains HTML
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True when the string contains HTML.</returns>
        public static bool Html(string str) => str != HtmlEncode(str);

        /// <summary>
        /// Checks if a string can be parsed to a Int32.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a Int32.</returns>
        public static bool Integer(string value)
        {
            int returnValue;
            return Integer(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a Int32.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The Int32 value of the parsed string.</param>
        /// <returns>True if can be parsed to a Int32.</returns>
        public static bool Integer(string value, out int returnValue) => int.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string can be parsed to a Int64.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a Int64.</returns>
        public static bool Long(string value)
        {
            long returnValue;
            return Long(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a Int64.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The Int64 value of the parsed string.</param>
        /// <returns>True if can be parsed to a Int64.</returns>
        public static bool Long(string value, out long returnValue) => long.TryParse(value, out returnValue);

        /// <summary>
        /// Checks an argument is not null.
        /// </summary>
        /// <param name="obj">Object to check.</param>
        /// <returns>True when the argument is not null.</returns>
        private static bool NotNull(object obj) => !Null(obj);

        /// <summary>
        /// Checks a string is not null and is not empty.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True when the string is not null and is not empty.</returns>
        public static bool NotNullAndNotEmpty(string value) => !IsNullOrEmpty(value);

        /// <summary>
        /// Checks a string is not null and is not WhiteSpace.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True when the string is not null and is not WhiteSpace.</returns>
        public static bool NotNullAndNotWhiteSpace(string value) => !IsNullOrWhiteSpace(value);

        /// <summary>
        /// Checks that the argument is not null then does something with that argument.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="obj">Object to check.</param>
        /// <param name="action">The action to perform on the object.</param>
        /// <returns>The object whether null or not.</returns>
        public static T NotNullThen<T>(T obj, Action<T> action)
        {
            if (NotNull(obj))
                action(obj);

            return obj;
        }

        /// <summary>
        /// Checks that the argument is not null then returns something derived from that object.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="obj">Object to check.</param>
        /// <param name="func">The function to perform on the object that returns the intended result.</param>
        /// <returns>The intended result.</returns>
        public static TResult NotNullThenGet<T, TResult>(T obj, Func<T, TResult> func)
        {
            var result = default(TResult);
            if (NotNull(obj))
                result = func(obj);

            return result;
        }

        /// <summary>
        /// Checks that the argument is null.
        /// </summary>
        /// <param name="obj">Object to check.</param>
        /// <returns>True when the argument is null.</returns>
        private static bool Null(object obj) => obj == null;

        /// <summary>
        /// Checks that a string is null or empty.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True when the string is null or empty.</returns>
        public static bool NullOrEmpty(string value) => IsNullOrEmpty(value);

        /// <summary>
        /// Checks that a string is null or WhiteSpace.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True when the string is null or WhiteSpace.</returns>
        public static bool NullOrWhiteSpace(string value) => IsNullOrWhiteSpace(value);

        /// <summary>
        /// Checks that the argument is null then does something.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="obj">Object to check.</param>
        /// <param name="action">The action to perform when the object is null.</param>
        /// <returns>The object whether null or not.</returns>
        public static T NullThen<T>(T obj, Action action)
        {
            if (Null(obj))
                action();

            return obj;
        }

        /// <summary>
        /// Checks that the argument is null and if so then sets the value and returns it.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="obj">Object to check.</param>
        /// <param name="value">The value to set and retun when the object is null.</param>
        /// <returns>The value set object.</returns>
        public static T NullThenSet<T>(T obj, T value)
        {
            if (Null(obj))
                obj = value;

            return obj;
        }

        /// <summary>
        /// Checks that the argument is null and if so then sets the value and returns it.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="obj">Object to check.</param>
        /// <param name="func">The function to set and return when the object is null.</param>
        /// <returns>The value set object.</returns>
        public static T NullThenSet<T>(T obj, Func<T> func)
        {
            if (Null(obj))
                obj = func();

            return obj;
        }

        /// <summary>
        /// Checks a string that it can be parsed to a numeric value.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True when the string can be parsed to a numerice type.</returns>
        public static bool Numeric(string value)
            => Byte(value) || SignedByte(value) || Short(value) || UnsignedShort(value) || Integer(value) || UnsignedInteger(value) || Long(value) || UnsignedLong(value) || Decimal(value) || Float(value) || Double(value);

        /// <summary>
        /// Checks if a string can be parsed to a Int16.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a Int16.</returns>
        public static bool Short(string value)
        {
            short returnValue;
            return Short(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a Int16.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The Int16 value of the parsed string.</param>
        /// <returns>True if can be parsed to a Int16.</returns>
        public static bool Short(string value, out short returnValue) => short.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string can be parsed to a SByte.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a SByte.</returns>
        public static bool SignedByte(string value)
        {
            sbyte returnValue;
            return SignedByte(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a SByte.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The SByte value of the parsed string.</param>
        /// <returns>True if can be parsed to a SByte.</returns>
        public static bool SignedByte(string value, out sbyte returnValue) => sbyte.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string can be parsed to a UInt32.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a UInt32.</returns>
        public static bool UnsignedInteger(string value)
        {
            uint returnValue;
            return UnsignedInteger(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a UInt32.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The UInt32 value of the parsed string.</param>
        /// <returns>True if can be parsed to a UInt32.</returns>
        public static bool UnsignedInteger(string value, out uint returnValue) => uint.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string can be parsed to a UInt64.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a UInt64.</returns>
        public static bool UnsignedLong(string value)
        {
            ulong returnValue;
            return UnsignedLong(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a UInt64.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The UInt64 value of the parsed string.</param>
        /// <returns>True if can be parsed to a UInt64.</returns>
        public static bool UnsignedLong(string value, out ulong returnValue) => ulong.TryParse(value, out returnValue);

        /// <summary>
        /// Checks if a string can be parsed to a UInt16.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if can be parsed to a UInt16.</returns>
        public static bool UnsignedShort(string value)
        {
            ushort returnValue;
            return UnsignedShort(value, out returnValue);
        }

        /// <summary>
        /// Checks if a string can be parsed to a UInt16.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="returnValue">The UInt16 value of the parsed string.</param>
        /// <returns>True if can be parsed to a UInt16.</returns>
        public static bool UnsignedShort(string value, out ushort returnValue) => ushort.TryParse(value, out returnValue);
    }
}
