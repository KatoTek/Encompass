using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using static AutoMapper.Mapper;

namespace Encompass.AutoMapper.TypeConverters
{
    /// <summary>
    /// Conversion class for converting an IList of a source type to an IList of a destination type
    /// </summary>
    /// <typeparam name="TSource">The type of the source collection</typeparam>
    /// <typeparam name="TDestination">The type of the destination collection</typeparam>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class IListToIListConverter<TSource, TDestination> : ITypeConverter<IList<TSource>, IList<TDestination>>
    {
        /// <summary>
        /// Converts an IList of a source type to an IList of a destination type
        /// </summary>
        /// <param name="context">Context information regarding resolution of a destination value</param>
        /// <returns>An IList of the destination type</returns>
        public IList<TDestination> Convert(ResolutionContext context) => ((IList<TSource>)context.SourceValue)?.Select(source => Map<TDestination>(source)).ToList();
    }
}
