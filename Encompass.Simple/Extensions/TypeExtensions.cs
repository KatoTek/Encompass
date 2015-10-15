using System;
using System.Linq;

namespace Encompass.Simple.Extensions
{
    /// <summary>
    /// Type extension methods
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets an attribute of type <typeparamref name="T"/> property <paramref name="propertyName"/> from the supplied <paramref name="source"/> type
        /// </summary>
        /// <typeparam name="T">The type of the attribute to get</typeparam>
        /// <param name="source">The type of the object that contains the <paramref name="propertyName"/> for which to get the attribute</param>
        /// <param name="propertyName">The name of the property for which to get the attribute</param>
        /// <returns>The property attribute</returns>
        public static T GetPropertyAttribute<T>(this Type source, string propertyName)
            where T : Attribute
        {
            var property = source.GetProperty(propertyName);

            var attributes = property?.GetCustomAttributes(typeof(T), false);
            if (attributes != null && attributes.Length > 0)
                return ((T)attributes.First());

            return null;
        }
    }
}