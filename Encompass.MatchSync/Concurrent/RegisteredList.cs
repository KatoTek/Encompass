using System;
using System.Collections.Generic;

namespace Encompass.MatchSync.Concurrent
{
    /// <summary>
    /// Class that defines a special type of collection that helps facilitate concurrency syncing
    /// </summary>
    /// <typeparam name="T">The type of the List</typeparam>
    [Serializable]
    public class RegisteredList<T> : List<T>
    {
        /// <param name="token">The <see cref="ConcurrencyToken"/> of the <see cref="RegisteredList{T}"/></param>
        public RegisteredList(ConcurrencyToken token)
        {
            Token = token;
        }

        /// <param name="token">The <see cref="ConcurrencyToken"/> of the <see cref="RegisteredList{T}"/></param>
        /// <param name="capacity">The number of elements that the new list can initially store</param>
        public RegisteredList(ConcurrencyToken token, int capacity)
            : base(capacity)
        {
            Token = token;
        }

        /// <param name="token">The <see cref="ConcurrencyToken"/> of the <see cref="RegisteredList{T}"/></param>
        /// <param name="collection">The collection whose elements are copied to the new list</param>
        public RegisteredList(ConcurrencyToken token, IEnumerable<T> collection)
            : base(collection)
        {
            Token = token;
        }

        /// <param name="collection">The collection whose elements are copied to the new list</param>
        public RegisteredList(RegisteredList<T> collection)
            : base(collection)
        {
            Token = collection.Token;
        }

        /// <summary>The <see cref="ConcurrencyToken"/> of the <see cref="RegisteredList{T}"/></summary>
        public ConcurrencyToken Token { get; }

        /// <summary>
        /// Converts the elements in the current <see cref="RegisteredList{T}"/> to another type, and returns the list containing the converted elements
        /// </summary>
        /// <typeparam name="TOutput">The type to convert to</typeparam>
        /// <param name="converter">A <see cref="Converter{T, TOutput}"/> delegate that converts each element from one type to another type</param>
        /// <returns>The list containing the converted elements</returns>
        public new RegisteredList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => new RegisteredList<TOutput>(Token, base.ConvertAll(converter));
    }
}
