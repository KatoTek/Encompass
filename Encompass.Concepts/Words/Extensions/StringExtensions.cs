using System.Linq;
using static Encompass.Concepts.Words.Stopwords;

namespace Encompass.Concepts.Words.Extensions
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringExtensions
    {
        /// <inheritdoc cref="FilterOut(string, string[])"/>
        public static IQueryable<string> FilterOutStopwords(this string text, string[] separators = null) => FilterOut(text, separators ?? new[] { " " });
    }
}
