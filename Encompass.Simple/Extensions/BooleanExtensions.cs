using System.Collections.Generic;
using System.Linq;
using static Encompass.Simple.Is;

namespace Encompass.Simple.Extensions
{
    /// <summary>
    /// Boolean extension methods
    /// </summary>
    public static class BooleanExtensions
    {
        /// <inheritdoc cref="AllFalse(bool[])"/>
        /// <param name="source"><inheritdoc cref="AllFalse(bool[])" select="/param[@name='bools']/node()"/></param>
        public static bool IsAllFalse(this IEnumerable<bool> source) => AllFalse(source.ToArray());

        /// <inheritdoc cref="AllTrue(bool[])"/>
        /// <param name="source"><inheritdoc cref="AllTrue(bool[])" select="/param[@name='bools']/node()"/></param>
        public static bool IsAllTrue(this IEnumerable<bool> source) => AllTrue(source.ToArray());

        /// <inheritdoc cref="AnyFalse(bool[])"/>
        /// <param name="source"><inheritdoc cref="AnyFalse(bool[])" select="/param[@name='bools']/node()"/></param>
        public static bool IsAnyFalse(this IEnumerable<bool> source) => AnyFalse(source.ToArray());

        /// <inheritdoc cref="AnyTrue(bool[])"/>
        /// <param name="source"><inheritdoc cref="AnyTrue(bool[])" select="/param[@name='bools']/node()"/></param>
        public static bool IsAnyTrue(this IEnumerable<bool> source) => AnyTrue(source.ToArray());
    }
}
