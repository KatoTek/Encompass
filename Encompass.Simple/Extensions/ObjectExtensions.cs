using System;
using System.Collections;
using static Encompass.Simple.Does;
using static Encompass.Simple.Is;

#pragma warning disable 1573

namespace Encompass.Simple.Extensions
{
    /// <summary>
    ///     Object extension methods
    /// </summary>
    public static class ObjectExtensions
    {
        #region methods

        /// <inheritdoc cref="PropertyExist(object, string)" />
        /// <param name="source">
        ///     <inheritdoc cref="PropertyExist(object, string)" select="/param[@name='obj']/node()" />
        /// </param>
        public static bool DoesPropertyExist(this object source, string propertyName) => PropertyExist(source, propertyName);

        /// <inheritdoc cref="PropertyExist{T}(object, string, out T)" />
        /// <param name="source">
        ///     <inheritdoc cref="PropertyExist{T}(object, string, out T)" select="/param[@name='obj']/node()" />
        /// </param>
        public static bool DoesPropertyExist<T>(this object source, string propertyName, out T value) => PropertyExist(source, propertyName, out value);

        /// <summary>
        ///     Gets an attribute from an object
        /// </summary>
        /// <typeparam name="TA">The type of the attribute to get</typeparam>
        /// <param name="source">The object to get the attribute from</param>
        /// <returns>The attribute from the object</returns>
        public static TA GetAttribute<TA>(this object source) where TA : Attribute
        {
            if (source == null)
                return null;

            var type = source.GetType();
            var memberInfo = type.GetMember(source.ToString());

            if (memberInfo.Length <= 0)
                return null;

            var attributes = memberInfo[0].GetCustomAttributes(typeof(TA), false);
            if (attributes.Length > 0)
                return (TA)attributes[0];

            return null;
        }

        /// <inheritdoc cref="NotNullThen{T}(T, Action{T})" />
        /// <param name="source">
        ///     <inheritdoc cref="NotNullThen{T}(T, Action{T})" select="/param[@name='obj']/node()" />
        /// </param>
        public static T IfIsNotNullThen<T>(this T source, Action<T> action) => NotNullThen(source, action);

        /// <inheritdoc cref="NotNullThenGet{T, TResult}(T, Func{T, TResult})" />
        /// <param name="source">
        ///     <inheritdoc cref="NotNullThenGet{T, TResult}(T, Func{T, TResult})" select="/param[@name='obj']/node()" />
        /// </param>
        public static TResult IfIsNotNullThenGet<T, TResult>(this T source, Func<T, TResult> func) => NotNullThenGet(source, func);

        /// <inheritdoc cref="NullThen{T}(T, Action)" />
        /// <param name="source">
        ///     <inheritdoc cref="NullThen{T}(T, Action)" select="/param[@name='obj']/node()" />
        /// </param>
        public static T IfIsNullThen<T>(this T source, Action action) => NullThen(source, action);

        /// <inheritdoc cref="NullThenSet{T}(T, T)" />
        /// <param name="source">
        ///     <inheritdoc cref="NullThenSet{T}(T, T)" select="/param[@name='obj']/node()" />
        /// </param>
        public static T IfIsNullThenSet<T>(this T source, T value) => NullThenSet(source, value);

        /// <inheritdoc cref="NullThenSet{T}(T, Func{T})" />
        /// <param name="source">
        ///     <inheritdoc cref="NullThenSet{T}(T, Func{T})" select="/param[@name='obj']/node()" />
        /// </param>
        public static T IfIsNullThenSet<T>(this T source, Func<T> func) => NullThenSet(source, func);

        /// <inheritdoc cref="AllNotNull(object[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNotNull(object[])" select="/param[@name='objs']/node()" />
        /// </param>
        public static bool IsAllNotNull(this IEnumerable source) => AllNotNull(source);

        /// <inheritdoc cref="AllNull(object[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AllNull(object[])" select="/param[@name='objs']/node()" />
        /// </param>
        public static bool IsAllNull(this IEnumerable source) => AllNull(source);

        /// <inheritdoc cref="AnyNull(object[])" />
        /// <param name="source">
        ///     <inheritdoc cref="AnyNull(object[])" select="/param[@name='objs']/node()" />
        /// </param>
        public static bool IsAnyNull(this IEnumerable source) => AnyNull(source);

        #endregion
    }
}

#pragma warning restore 1573
