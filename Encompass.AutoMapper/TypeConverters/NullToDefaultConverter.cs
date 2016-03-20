using AutoMapper;

namespace Encompass.AutoMapper.TypeConverters
{
    /// <summary>
    ///     Conversion class for converting a nullable type to a non-nullable type
    /// </summary>
    /// <typeparam name="T">The type of the object that will be converted</typeparam>
    public class NullToDefaultConverter<T> : ITypeConverter<T?, T> where T : struct
    {
        #region methods

        /// <summary>
        ///     Converts a nullable object to a non-nullable type using its default value if the current value is null
        /// </summary>
        /// <param name="context">Context information regarding resolution of a destination value</param>
        /// <returns>
        ///     A non-nullable object with the specified type. If the value was null before conversion, the default value of
        ///     the current type is used.
        /// </returns>
        public T Convert(ResolutionContext context)
        {
            var val = (T?)context.SourceValue;

            return val ?? default(T);
        }

        #endregion
    }
}
