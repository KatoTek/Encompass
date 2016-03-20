using System.Configuration;

namespace Encompass.Concepts.Mail.Configuration
{
    /// <summary>
    ///     A configuration element representing the components of an email
    /// </summary>
    public class EmailInfo : ConfigurationElement
    {
        #region properties

        /// <summary>
        ///     The <see cref="EmailAddress" />es to BCC
        /// </summary>
        [ConfigurationProperty("bccs", IsRequired = false)]
        public EmailAddresses Bccs => (EmailAddresses)this["bccs"];

        /// <summary>
        ///     The <see cref="EmailAddress" />es to CC
        /// </summary>
        [ConfigurationProperty("ccs", IsRequired = false)]
        public EmailAddresses Ccs => (EmailAddresses)this["ccs"];

        /// <summary>
        ///     The <see cref="EmailAddress" /> the email is coming from
        /// </summary>
        [ConfigurationProperty("from", IsRequired = true)]
        public EmailAddress From => (EmailAddress)this["from"];

        /// <summary>
        ///     A unique name for the email element
        /// </summary>
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key => (string)this["key"];

        /// <summary>
        ///     The subject of the email
        /// </summary>
        [ConfigurationProperty("subject", IsRequired = false)]
        public string Subject => (string)this["subject"];

        /// <summary>
        ///     The <see cref="EmailAddress" />es to send the email to
        /// </summary>
        [ConfigurationProperty("tos", IsRequired = false)]
        public EmailAddresses Tos => (EmailAddresses)this["tos"];

        #endregion
    }
}
