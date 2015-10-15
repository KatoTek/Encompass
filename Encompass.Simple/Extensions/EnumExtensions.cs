using System;

namespace Encompass.Simple.Extensions
{
    /// <summary>
    /// Enum extension methods
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets an attribute of a specified type from an enum value
        /// </summary>
        /// <typeparam name="T">The Attribute type to get</typeparam>
        /// <param name="source">The enum value for which to get the attribute</param>
        /// <returns>The Attribute of the enum</returns>
        public static T GetAttribute<T>(this Enum source) where T : Attribute
        {
            var type = source.GetType();
            var memberInfo = type.GetMember(source.ToString());

            if (memberInfo.Length <= 0)
                return null;

            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            if (attributes.Length > 0)
                return ((T)attributes[0]);

            return null;
        }
    }
}
