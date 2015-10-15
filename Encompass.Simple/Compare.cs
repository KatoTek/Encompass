using System;
using static System.String;
using static System.StringComparison;

namespace Encompass.Simple
{
    /// <summary>
    /// Exposes methods that can be used for comparing objects
    /// </summary>
    public static class Compare
    {
        /// <summary>
        /// Compares the source nullable object to another nullable object
        /// </summary>
        /// <typeparam name="T">The type of nullable that will be compared</typeparam>
        /// <param name="a">The object to compare against</param>
        /// <param name="b">The object to compare to</param>
        /// <returns>A 32-bit signed integer that indicates whether <paramref name="a"/> precedes,
        /// follows, or occurs in the same position in the sort order as <paramref name="b"/>.
        /// Zero if <paramref name="a"/> equals <paramref name="b"/> or both are null,
        /// Less than zero if <paramref name="a"/> precedes <paramref name="b"/> or <paramref name="a"/> is null,
        /// Greater than zero if <paramref name="a"/> follows <paramref name="b"/> or <paramref name="b"/> is null</returns>
        public static int Nullables<T>(T? a, T? b) where T : struct, IComparable
        {
            if (a.HasValue && b.HasValue)
                return a.Value.CompareTo(b.Value);

            if (!a.HasValue && !b.HasValue)
                return 0;

            return a.HasValue ? 1 : -1;
        }

        /// <summary>
        /// Compares two strings factoring in null values
        /// </summary>
        /// <param name="a">The string to compare against</param>
        /// <param name="b">The string to compare to</param>
        /// <returns>A 32-bit signed integer that indicates whether this instance precedes,
        /// follows, or appears in the same position in the sort order as the <paramref name="a"/>
        /// parameter.Value Condition
        /// Less than zero <paramref name="a"/> precedes <paramref name="b"/>.-or- <paramref name="b"/> is null.
        /// Zero <paramref name="a"/> has the same position in the sort order as <paramref name="b"/>.
        /// Greater than zero <paramref name="a"/> follows <paramref name="b"/>.</returns>
        public static int Strings(string a, string b) => a == null ? b == null ? 0 : -1 : Compare(a, b, Ordinal);
    }
}
