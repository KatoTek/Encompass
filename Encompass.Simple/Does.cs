namespace Encompass.Simple
{
    /// <summary>
    /// Exposes methods that are read as "Does" {MethodName}. Intended to simplify syntax and read like English.
    /// </summary>
    public static class Does
    {
        /// <summary>
        /// Determines if a property exists on an object
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="propertyName">The name of the property to check for</param>
        /// <returns>True if the property exists</returns>
        public static bool PropertyExist(object obj, string propertyName) => obj.GetType().GetProperty(propertyName) != null;

        /// <summary>
        /// Determines if a property exists on an object and sets the <paramref name="value"/> ref property
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/> that will be set</typeparam>
        /// <param name="obj">The object to check</param>
        /// <param name="propertyName">The name of the property to check for</param>
        /// <param name="value">The ref parameter that will be set based on the property value</param>
        /// <returns>True if the property exists</returns>
        public static bool PropertyExist<T>(object obj, string propertyName, out T value)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);

            value = (T)propertyInfo?.GetValue(obj, null);

            return propertyInfo != null;
        }
    }
}
