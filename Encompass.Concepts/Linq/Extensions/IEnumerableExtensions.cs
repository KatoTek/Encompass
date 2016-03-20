using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Encompass.Concepts.Linq.Extensions
{
    /// <summary>
    ///     IEnumerable extension methods
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class IEnumerableExtensions
    {
        #region methods

        /// <summary>
        ///     Determines if an IEnumerable is empty and if so then returns another IEnumerable instead
        /// </summary>
        /// <typeparam name="T">The type of IEnumerable</typeparam>
        /// <param name="enumerable">The IEnumerable to check if it is empty</param>
        /// <param name="then">The fallback IEnumerable to use if <paramref name="enumerable" /> is empty</param>
        /// <exception cref="ArgumentNullException"><paramref name="then" /> must not have a value</exception>
        /// <returns><paramref name="enumerable" /> if it is not empty, otherwise <paramref name="then" /></returns>
        public static IEnumerable<T> IfEmpty<T>(this IEnumerable<T> enumerable, IEnumerable<T> then)
        {
            if (then == null)
                throw new ArgumentNullException(nameof(then));

            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Any()
                       ? list
                       : then;
        }

        /// <summary>
        ///     Determines if an IEnumerable is null or empty and if so then returns another IEnumerable instead
        /// </summary>
        /// <typeparam name="T">The type of IEnumerable</typeparam>
        /// <param name="enumerable">The IEnumerable to check if it is null or empty</param>
        /// <param name="then">The fallback IEnumerable to use if <paramref name="enumerable" /> is null or empty</param>
        /// <exception cref="ArgumentNullException"><paramref name="then" /> must not have a value</exception>
        /// <returns><paramref name="enumerable" /> if it is not null or empty, otherwise <paramref name="then" /></returns>
        public static IEnumerable<T> IfNullOrEmpty<T>(this IEnumerable<T> enumerable, IEnumerable<T> then)
        {
            if (then == null)
                throw new ArgumentNullException(nameof(then));

            if (enumerable == null)
                enumerable = new List<T>();

            return enumerable.IfEmpty(then);
        }

        #endregion
    }
}
