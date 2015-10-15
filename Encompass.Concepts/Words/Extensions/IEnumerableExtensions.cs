using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static Encompass.Concepts.Words.Stopwords;

namespace Encompass.Concepts.Words.Extensions
{
    /// <summary>
    /// IEnumerable extension methods
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class IEnumerableExtensions
    {
        /// <inheritdoc cref="FilterOut(IEnumerable{string})"/>
        public static IQueryable<string> FilterOutStopwords(this IEnumerable<string> terms) => FilterOut(terms);
    }
}
