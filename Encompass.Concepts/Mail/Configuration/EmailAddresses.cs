using System.Configuration;

namespace Encompass.Concepts.Mail.Configuration
{
    /// <summary>
    ///     A configuration collection of email address
    /// </summary>
    [ConfigurationCollection(typeof(EmailAddress))]
    public class EmailAddresses : ConfigurationElementCollection
    {
        #region methods

        /// <summary>
        ///     Creates a new <see cref="EmailAddress" />
        /// </summary>
        /// <returns>A new <see cref="EmailAddress" /></returns>
        protected override ConfigurationElement CreateNewElement() => new EmailAddress();

        /// <summary>
        ///     Gets the Address for an <see cref="EmailAddress" /> element
        /// </summary>
        /// <param name="element">An <see cref="EmailAddress" /></param>
        /// <returns>The Address of the specified <see cref="EmailAddress" /> element</returns>
        protected override object GetElementKey(ConfigurationElement element) => ((EmailAddress)element).Address;

        #endregion
    }
}
