using System.Configuration;

namespace Encompass.Concepts.Mail.Configuration
{
    /// <summary>
    /// A configuration element representing an email address
    /// </summary>
    public class EmailAddress : ConfigurationElement
    {
        /// <summary>
        /// An email address
        /// </summary>
        [ConfigurationProperty("address", IsKey = true, IsRequired = true)]
        public string Address => (string)base["address"];
    }
}
