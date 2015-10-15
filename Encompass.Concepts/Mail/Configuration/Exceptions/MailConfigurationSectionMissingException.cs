using System;
using System.Runtime.Serialization;
using static System.String;

namespace Encompass.Concepts.Mail.Configuration.Exceptions
{
    /// <summary>
    /// Exception type for a missing mail configuration section
    /// </summary>
    public class MailConfigurationSectionMissingException : Exception
    {
        private const string DEFAULT_MESSAGE_FORMAT = "Configuration section \"{0}\" is either missing or corrupt";

        /// <param name="section">The name of the section that is missing</param>
        public MailConfigurationSectionMissingException(string section)
            : base(Format(DEFAULT_MESSAGE_FORMAT, section))
        {
            Section = section;
        }

        /// <param name="section">The name of the section that is missing</param>
        /// <param name="alternateMessage">An alternate message that describes the error</param>
        public MailConfigurationSectionMissingException(string section, string alternateMessage)
            : base(alternateMessage)
        {
            Section = section;
        }

        /// <param name="section">The name of the section that is missing</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public MailConfigurationSectionMissingException(string section, Exception innerException)
            : base(Format(DEFAULT_MESSAGE_FORMAT, section), innerException)
        {
            Section = section;
        }

        /// <param name="section">The name of the section that is missing</param>
        /// <param name="alternateMessage">An alternate message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public MailConfigurationSectionMissingException(string section, string alternateMessage, Exception innerException)
            : base(alternateMessage, innerException)
        {
            Section = section;
        }

        /// <param name="section">The name of the section that is missing</param>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination</param>
        public MailConfigurationSectionMissingException(string section, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Section = section;
        }

        /// <summary>
        /// The name of the section that is missing
        /// </summary>
        public string Section { get; set; }
    }
}
