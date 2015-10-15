using System;
using AutoMapper;
using static System.String;

namespace Encompass.AutoMapper.TypeConverters
{
    /// <summary>
    /// Conversion class for converting a Guid to a string
    /// </summary>
    public class GuidToStringConverter : ITypeConverter<Guid, string>
    {
        /// <summary>
        /// Converts a Guid to a string
        /// </summary>
        /// <param name="context">Context information regarding resolution of a destination value</param>
        /// <returns>The Guid from the ResolutionContext.SourceValue as a string</returns>
        public string Convert(ResolutionContext context)
        {
            var val = ((Guid)context.SourceValue).ToString();

            return val == new Guid().ToString() ? Empty : val;
        }
    }
}
