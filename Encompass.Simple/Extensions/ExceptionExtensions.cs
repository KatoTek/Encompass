using System;
using System.Text;
using static System.Environment;
using static Encompass.Simple.Format.Html;

namespace Encompass.Simple.Extensions
{
    /// <summary>
    /// Exception extension methods
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <inheritdoc cref="Table(Exception)"/>
        /// <param name="source"><inheritdoc cref="Table(Exception)" select="/param[@name='exception']/node()"/></param>
        public static string ToHtmlTable(this Exception source) => Table(source);

        /// <summary>
        /// Formats an Exception to a string of text
        /// </summary>
        /// <param name="exception">The Exception to format</param>
        /// <returns>The Exception as a string of text</returns>
        public static string ToText(this Exception exception)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Message: {exception.Message}{NewLine}Source: {exception.Source}{NewLine}StackTrace: {exception.StackTrace}");

            if (exception.InnerException != null)
                stringBuilder.Append($"{NewLine}{NewLine}{ToText(exception.InnerException)}");

            return stringBuilder.ToString();
        }
    }
}
