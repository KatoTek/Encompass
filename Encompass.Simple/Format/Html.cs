using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Environment;
using static System.Net.WebUtility;
using static System.StringSplitOptions;

namespace Encompass.Simple.Format
{
    /// <summary>
    /// Provides ways of formatting into html
    /// </summary>
    public static class Html
    {
        private const string EXCEPTION_TABLE_HTML_FILE = "Encompass.Simple.Html.ExceptionTable.html";
        private const string HTML_BREAK_TAG = "<br />";
        private const string HTML_PARAGRAPH_CLOSING_TAG = "</p>";
        private const string HTML_PARAGRAPH_OPENING_TAG = "<p>";

        /// <summary>
        /// Html encodes a string and injects html line breaks where new lines exist
        /// </summary>
        /// <param name="value">The string to work with</param>
        /// <returns>An Html encoded string with html line breaks</returns>
        public static string Breaks(string value)
            => value?.Split(new[] { NewLine }, RemoveEmptyEntries).Aggregate(new StringBuilder(), (stringBuilder, paragraph) => stringBuilder.Append($"{HtmlEncode(paragraph)}{HTML_BREAK_TAG}{NewLine}")).ToString();

        /// <summary>
        /// Html encodes a string and injects paragraphs around the content
        /// </summary>
        /// <param name="value">The string to work with</param>
        /// <returns>An Html encoded string with paragraphs</returns>
        public static string Paragraphs(string value)
            =>
                value?.Split(new[] { NewLine }, RemoveEmptyEntries)
                      .Aggregate(new StringBuilder(), (stringBuilder, paragraph) => stringBuilder.Append($"{HTML_PARAGRAPH_OPENING_TAG}{HtmlEncode(paragraph)}{HTML_PARAGRAPH_CLOSING_TAG}{NewLine}"))
                      .ToString();

        /// <summary>
        /// Formats an Exception as an html table
        /// </summary>
        /// <param name="exception">The Exception to format</param>
        /// <returns>The Exception as a string of html</returns>
        public static string Table(Exception exception)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(EXCEPTION_TABLE_HTML_FILE))
            {
                if (stream == null)
                    return null;

                using (var reader = new StreamReader(stream))
                {
                    return exception == null
                               ? string.Empty
                               : string.Format(reader.ReadToEnd().Trim(), Breaks(exception.GetType().FullName), Breaks(exception.Message), Breaks(exception.Source), Breaks(exception.StackTrace), Table(exception.InnerException));
                }
            }
        }
    }
}
