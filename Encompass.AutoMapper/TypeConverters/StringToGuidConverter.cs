using System;
using AutoMapper;
using static System.String;

namespace Encompass.AutoMapper.TypeConverters
{
    /// <summary>
    ///     Conversion class for converting a string to a Guid
    /// </summary>
    public class StringToGuidConverter : ITypeConverter<string, Guid>
    {
        #region methods

        /// <summary>
        ///     Converts a string to a Guid
        /// </summary>
        /// <param name="context">Context information regarding resolution of a destination value</param>
        /// <returns>The string from the ResolutionContext.SourceValue as a Guid</returns>
        public Guid Convert(ResolutionContext context)
        {
            var val = (string)context.SourceValue;

            return IsNullOrWhiteSpace(val)
                       ? new Guid()
                       : new Guid(val);
        }

        #endregion
    }
}
