using AutoMapper;
#pragma warning disable 1591

namespace Encompass.AutoMapper.TypeConverters
{
    public class DefaultToNullConverter<T> : ITypeConverter<T, T?> where T : struct
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="context">Resolution context</param>
        /// <returns>
        /// Destination object
        /// </returns>
        public T? Convert(ResolutionContext context)
        {
            var val = (T)context.SourceValue;

            return val.Equals(default(T)) ? null : new T?(val);
        }
    }
}
#pragma warning restore 1591