using System;
using static Encompass.Simple.Compare;

namespace Encompass.Simple.Extensions
{
    /// <summary>
    ///     Nullable type extension methods
    /// </summary>
    public static class NullablesExtensions
    {
        #region methods

        /// <inheritdoc cref="Nullables{T}(Nullable{T}, Nullable{T})" />
        /// <param name="source">
        ///     <inheritdoc cref="Nullables{T}(Nullable{T}, Nullable{T})" select="/param[@name='a']/node()" />
        /// </param>
        /// <param name="other">
        ///     <inheritdoc cref="Nullables{T}(Nullable{T}, Nullable{T})" select="/param[@name='b']/node()" />
        /// </param>
        public static int CompareThisTo<T>(this T? source, T? other) where T : struct, IComparable => Nullables(source, other);

        /// <inheritdoc cref="Strings(string, string)" />
        /// <param name="source">
        ///     <inheritdoc cref="Strings(string, string)" select="/param[@name='a']/node()" />
        /// </param>
        /// <param name="other">
        ///     <inheritdoc cref="Strings(string, string)" select="/param[@name='b']/node()" />
        /// </param>
        public static int CompareThisTo(this string source, string other) => Strings(source, other);

        #endregion
    }
}
