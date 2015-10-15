using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Encompass.Concepts.Mail.Configuration;
using Encompass.Simple.Extensions;
using static System.Environment;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace Encompass.Concepts.Mail
{
    /// <summary>
    /// Provides methods for sending emails
    /// </summary>
    public class Mailer
    {
        private const string EXCEPTION_STYLE_RESOURCE_NAME = "Encompass.Concepts.Mail.Exception.css";
        private const string HTML_BREAK_TAG = "<br />";
        private const string HTML_DIV_FORMAT = "<div>{0}</div>";

        /// <summary>
        /// Sends the specified <see cref="MailMessage"/> to an SMTP server for delivery
        /// </summary>
        /// <param name="mailMessage">The message to send</param>
        public static void Send(MailMessage mailMessage)
        {
            using (var client = new SmtpClient())
                client.Send(mailMessage);
        }

        /// <summary>
        /// Sends an <see cref="HtmlMailMessage"/> created using the specified <paramref name="message"/>
        /// </summary>
        /// <param name="message">The content of the email</param>
        /// <param name="emailInfosConfigSectionName">The section name in the config file containing the <see cref="EmailInfo"/> for the message to send</param>
        public static void Send(string message, string emailInfosConfigSectionName) => Send(message, string.Empty, emailInfosConfigSectionName);

        /// <summary>
        /// Sends an <see cref="HtmlMailMessage"/> created using the specified <paramref name="message"/> and <paramref name="style"/>
        /// </summary>
        /// <param name="message">The content of the email</param>
        /// <param name="style">The css to use to style the email body</param>
        /// <param name="emailInfosConfigSectionName">The section name in the config file containing the <see cref="EmailInfo"/> for the message to send</param>
        public static void Send(string message, string style, string emailInfosConfigSectionName)
        {
            var config = Config.GetSection(emailInfosConfigSectionName);
            foreach (var info in config.EmailInfos)
                Send(new HtmlMailMessage((EmailInfo)info, message, style));
        }

        /// <inheritdoc cref="Send(string, System.Collections.Generic.IEnumerable{System.Exception}, string, string)"/>
        /// <param name="exception">The exception to include in the email</param>
        public static void Send(Exception exception, string emailInfosConfigSectionName) => Send(null, exception, emailInfosConfigSectionName);

        /// <inheritdoc cref="Send(string, IEnumerable{Exception}, string, string)"/>
        /// <param name="exception">The exception to include in the email</param>
        public static void Send(Exception exception, string style, string emailInfosConfigSectionName) => Send(null, exception, style, emailInfosConfigSectionName);

        /// <inheritdoc cref="Send(string, IEnumerable{Exception}, string, string)"/>
        public static void Send(IEnumerable<Exception> exceptions, string emailInfosConfigSectionName) => Send(null, exceptions, emailInfosConfigSectionName);

        /// <inheritdoc cref="Send(string, IEnumerable{Exception}, string, string)"/>
        public static void Send(IEnumerable<Exception> exceptions, string style, string emailInfosConfigSectionName) => Send(null, exceptions, style, emailInfosConfigSectionName);

        /// <inheritdoc cref="Send(string, IEnumerable{Exception}, string, string)"/>
        /// <param name="exception">The exception to include in the email</param>
        public static void Send(string content, Exception exception, string emailInfosConfigSectionName) => Send(content, new[] { exception }, emailInfosConfigSectionName);

        /// <inheritdoc cref="Send(string, IEnumerable{Exception}, string, string)"/>
        /// <param name="exception">The exception to include in the email</param>
        public static void Send(string content, Exception exception, string style, string emailInfosConfigSectionName) => Send(content, new[] { exception }, style, emailInfosConfigSectionName);

        /// <inheritdoc cref="Send(string, IEnumerable{Exception}, string, string)"/>
        public static void Send(string content, IEnumerable<Exception> exceptions, string emailInfosConfigSectionName)
        {
            var style = string.Empty;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(EXCEPTION_STYLE_RESOURCE_NAME))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                        style = reader.ReadToEnd();
                }
            }

            Send(content, exceptions, style, emailInfosConfigSectionName);
        }

        /// <summary>
        /// Formats the specified exceptions as Html and sends them in an email
        /// </summary>
        /// <param name="content">Content to include at the beginning of the email body</param>
        /// <param name="exceptions">The exceptions to include in the email</param>
        /// <param name="style">The css to use to style the email body</param>
        /// <param name="emailInfosConfigSectionName">The section name in the config file containing the <see cref="EmailInfo"/> for the message to send</param>
        public static void Send(string content, IEnumerable<Exception> exceptions, string style, string emailInfosConfigSectionName)
        {
            var emailBlocks = new List<string>();

            if (content.IsNotNullAndNotWhiteSpace())
                emailBlocks.Add(HTML_DIV_FORMAT.FormatWith(content));

            if (exceptions != null)
                emailBlocks.AddRange(exceptions.Select(exception => exception.ToHtmlTable()));

            Send(emailBlocks.Count() > 1 ? emailBlocks.Aggregate(new StringBuilder(), (stringBuilder, block) => stringBuilder.Append($"{block}{HTML_BREAK_TAG}{HTML_BREAK_TAG}{NewLine}")).ToString() : emailBlocks.FirstOrDefault(),
                 style,
                 emailInfosConfigSectionName);
        }
    }
}

#pragma warning restore CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
