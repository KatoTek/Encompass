using System.Configuration;
using Encompass.Concepts.Mail.Configuration.Exceptions;

namespace Encompass.Concepts.Mail.Configuration
{
    /// <summary>
    ///     Defines the structure of an email configuration section
    /// </summary>
    /// <example>
    ///     <code language="xml">
    ///  <![CDATA[
    ///  <!-- The following configuration goes into the application's app.config or web.config -->
    ///  <configuration>
    ///      <configSections>
    ///          <section name="myapp.mail.unhandledException" type="Encompass.Concepts.Mail.Configuration.Config, Encompass.Concepts" />
    ///      </configSections>
    ///      <myapp.mail.unhandledException>
    ///          <emailinfos>
    /// 	            <add key="default" subject="MyApp Unhandled Exception">
    /// 		            <from address="myapp.debug@mtu-online.com" />
    /// 		            <tos>
    /// 			            <add address="john.doe@mtu-online.com" />
    /// 			            <add address="jane.doe@mtu-online.com" />
    /// 		            </tos>
    ///              </add>
    ///         </emailinfos>
    ///      </myapp.mail.unhandledException>
    ///  </configuration>
    ///  ]]>
    ///  </code>
    /// </example>
    public class Config : ConfigurationSection
    {
        #region properties

        /// <summary>
        ///     A configuration property exposing EmailInfos
        /// </summary>
        [ConfigurationProperty("emailinfos")]
        public EmailInfos EmailInfos => (EmailInfos)this["emailinfos"];

        #endregion

        #region methods

        /// <inheritdoc cref="ConfigurationManager.GetSection(string)" />
        /// <param name="section">
        ///     <inheritdoc cref="ConfigurationManager.GetSection(string)" select="/param[@name='sectionName']/node()" />
        /// </param>
        /// <exception cref="MailConfigurationSectionMissingException">The specified <paramref name="section" /> does not exist</exception>
        /// <returns>The specified configuration section object</returns>
        public static Config GetSection(string section)
        {
            var config = ConfigurationManager.GetSection(section) as Config;
            if (config != null)
                return config;

            throw new MailConfigurationSectionMissingException(section);
        }

        #endregion
    }
}
