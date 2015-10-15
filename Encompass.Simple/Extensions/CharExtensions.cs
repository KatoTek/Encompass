using System;

namespace Encompass.Simple.Extensions
{
    /// <summary>
    /// Char extension methods
    /// </summary>
    public static class CharExtensions
    {
        /// <inheritdoc cref="Encompass.Simple.Extensions.StringExtensions.GetEnum{T}(string, bool, bool)"/>
        public static T GetEnum<T>(this char source, bool ignoreWhiteSpace = false) where T : struct, IConvertible => source.ToString().GetEnum<T>(ignoreWhiteSpace);
    }
}
