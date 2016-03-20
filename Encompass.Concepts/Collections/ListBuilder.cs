using System;
using System.Collections.Generic;
using System.Linq;
using static System.Enum;

namespace Encompass.Concepts.Collections
{
    /// <summary>
    ///     Exposes methods that helps build lists
    /// </summary>
    public static class ListBuilder
    {
        #region methods

        /// <summary>
        ///     Builds a list of all items in the supplied <typeparamref name="T" /> enum
        /// </summary>
        /// <typeparam name="T">The type of enum to work with. The type must be an Enum otherwise an exception is thrown</typeparam>
        /// <exception cref="ArgumentException">T must be of type System.Enum</exception>
        /// <returns>A list of all items in the <typeparamref name="T" /> enum</returns>
        public static List<T> FromEnum<T>() where T : struct, IConvertible
        {
            // Can't use type constraints on value types, so have to do check like this
            if (typeof(T).BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            return GetValues(typeof(T))
                .Cast<T>()
                .ToList();
        }

        #endregion
    }
}
