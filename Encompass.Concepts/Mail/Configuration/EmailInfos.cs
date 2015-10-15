using System.Configuration;

namespace Encompass.Concepts.Mail.Configuration
{
    /// <summary>
    /// A configuration collection of emails
    /// </summary>
    [ConfigurationCollection(typeof(EmailInfo))]
    public class EmailInfos : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="EmailInfo"/>
        /// </summary>
        /// <returns>A new <see cref="EmailInfo"/></returns>
        protected override ConfigurationElement CreateNewElement() => new EmailInfo();

        /// <summary>
        /// Gets the Key for an <see cref="EmailInfo"/> element
        /// </summary>
        /// <param name="element">An <see cref="EmailInfo"/></param>
        /// <returns>The Key of the specified <see cref="EmailInfo"/> element</returns>
        protected override object GetElementKey(ConfigurationElement element) => ((EmailInfo)element).Key;
    }
}
