using System;
using System.Collections.Generic;
using System.Linq;

namespace Encompass.Concepts.Linq.Extensions
{
    /// <summary>
    ///     IQueryable extension methods used to recursively select data
    /// </summary>
    public static class RecursiveSelectExtensions
    {
        #region methods

        /// <inheritdoc
        ///     cref="RecursiveSelect{TSource, TResult}(IQueryable{TSource}, Func{TSource, IQueryable{TSource}}, Func{TSource, int, int, TResult}, int)" />
        public static IQueryable<TSource> RecursiveSelect<TSource>(this IQueryable<TSource> source, Func<TSource, IQueryable<TSource>> childSelector)
            => RecursiveSelect(source, childSelector, element => element);

        /// <inheritdoc
        ///     cref="RecursiveSelect{TSource, TResult}(IQueryable{TSource}, Func{TSource, IQueryable{TSource}}, Func{TSource, int, int, TResult}, int)" />
        public static IQueryable<TResult> RecursiveSelect<TSource, TResult>(this IQueryable<TSource> source,
                                                                            Func<TSource, IQueryable<TSource>> childSelector,
                                                                            Func<TSource, TResult> selector)
            => RecursiveSelect(source, childSelector, (element, index, depth) => selector(element));

        /// <inheritdoc
        ///     cref="RecursiveSelect{TSource, TResult}(IQueryable{TSource}, Func{TSource, IQueryable{TSource}}, Func{TSource, int, int, TResult}, int)" />
        public static IQueryable<TResult> RecursiveSelect<TSource, TResult>(this IQueryable<TSource> source,
                                                                            Func<TSource, IQueryable<TSource>> childSelector,
                                                                            Func<TSource, int, TResult> selector)
            => RecursiveSelect(source, childSelector, (element, index, depth) => selector(element, index));

        /// <inheritdoc
        ///     cref="RecursiveSelect{TSource, TResult}(IQueryable{TSource}, Func{TSource, IQueryable{TSource}}, Func{TSource, int, int, TResult}, int)" />
        public static IQueryable<TResult> RecursiveSelect<TSource, TResult>(this IQueryable<TSource> source,
                                                                            Func<TSource, IQueryable<TSource>> childSelector,
                                                                            Func<TSource, int, int, TResult> selector) => RecursiveSelect(source, childSelector, selector, 0);

        /// <summary>
        ///     Recursively iterates the <paramref name="source" /> IQueryable and returns a flat IQueryable result
        /// </summary>
        /// <typeparam name="TSource">The type of the source IQueryable</typeparam>
        /// <typeparam name="TResult">The type of the result IQueryable</typeparam>
        /// <param name="source">The IQueryable to recursively iterate</param>
        /// <param name="childSelector">The selector to use to recursively select the children of the source</param>
        /// <param name="selector">The selector to use to select the value to be used in the result</param>
        /// <param name="depth">The number of children deep to recurse through</param>
        /// <returns>
        ///     An IQueryable as a result of recursively iterating through the <paramref name="source" /> IQueryable and
        ///     applying the provided selectors
        /// </returns>
        private static IQueryable<TResult> RecursiveSelect<TSource, TResult>(this IQueryable<TSource> source,
                                                                             Func<TSource, IQueryable<TSource>> childSelector,
                                                                             Func<TSource, int, int, TResult> selector,
                                                                             int depth)
            => source.SelectMany((element, index) => Enumerable.Repeat(selector(element, index, depth), 1)
                                                               .Concat(RecursiveSelect(childSelector(element) ?? new List<TSource>().AsQueryable(),
                                                                                       childSelector,
                                                                                       selector,
                                                                                       depth + 1)));

        #endregion
    }
}
