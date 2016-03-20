using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using Encompass.Concepts.Mail.Configuration;
using static System.String;

namespace Encompass.Concepts.Mail
{
    /// <summary>
    ///     A <see cref="MailMessage" /> where the message body is in Html format
    /// </summary>
    public class HtmlMailMessage : MailMessage
    {
        #region fields

        private const string EMAIL_HTML_RESOURCE_NAME = "Encompass.Concepts.Html.Email.html";
        private bool _disposed;
        private string _htmlBody;
        private string _initialBody;
        private string _style;

        #endregion

        #region constructors

        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message</param>
        public HtmlMailMessage(string body, MailAddress from)
            : this(Empty, body, Empty, from, new MailAddressCollection()) {}

        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="from">
        ///     A <see cref="T:System.Net.Mail.MailAddress" /> that contains the address of the sender of the e-mail
        ///     message
        /// </param>
        /// <param name="to">
        ///     A collection of <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses of the
        ///     recipients of the e-mail message
        /// </param>
        public HtmlMailMessage(string body, MailAddress from, MailAddressCollection to)
            : this(Empty, body, Empty, from, to) {}

        /// <param name="subject">A <see cref="T:System.String" /> that contains the subject text</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="from">
        ///     A <see cref="T:System.Net.Mail.MailAddress" /> that contains the address of the sender of the e-mail
        ///     message
        /// </param>
        public HtmlMailMessage(string subject, string body, MailAddress from)
            : this(subject, body, Empty, from, new MailAddressCollection()) {}

        /// <param name="subject">A <see cref="T:System.String" /> that contains the subject text</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message</param>
        public HtmlMailMessage(string subject, string body, string from)
            : this(subject, body, Empty, new MailAddress(from), new MailAddressCollection()) {}

        /// <param name="subject">A <see cref="T:System.String" /> that contains the subject text</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="from">
        ///     A <see cref="T:System.Net.Mail.MailAddress" /> that contains the address of the sender of the e-mail
        ///     message
        /// </param>
        /// <param name="to">
        ///     A collection of <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses of the
        ///     recipients of the e-mail message
        /// </param>
        public HtmlMailMessage(string subject, string body, MailAddress from, MailAddressCollection to)
            : this(subject, body, Empty, from, to) {}

        /// <param name="subject">A <see cref="T:System.String" /> that contains the subject text</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="style">A <see cref="T:System.String" /> that contains the css to apply to the message</param>
        /// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message</param>
        public HtmlMailMessage(string subject, string body, string style, string from)
            : this(subject, body, style, new MailAddress(from), new MailAddressCollection()) {}

        /// <param name="subject">A <see cref="T:System.String" /> that contains the subject text</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="style">A <see cref="T:System.String" /> that contains the css to apply to the message</param>
        /// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message</param>
        /// <param name="to">A <see cref="T:System.String" /> that contains the address of the recipient of the e-mail message</param>
        public HtmlMailMessage(string subject, string body, string style, string from, string to)
            : this(subject, body, style, new MailAddress(from), new MailAddressCollection())
        {
            To.Add(to);
        }

        /// <param name="subject">A <see cref="T:System.String" /> that contains the subject text</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="style">A <see cref="T:System.String" /> that contains the css to apply to the message</param>
        /// <param name="from">
        ///     A <see cref="T:System.Net.Mail.MailAddress" /> that contains the address of the sender of the e-mail
        ///     message
        /// </param>
        public HtmlMailMessage(string subject, string body, string style, MailAddress from)
            : this(subject, body, style, from, new MailAddressCollection()) {}

        /// <param name="emailInfo">An <see cref="EmailInfo" /> that defines the e-mail message</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        public HtmlMailMessage(EmailInfo emailInfo, string body)
            : this(emailInfo, body, Empty) {}

        /// <param name="emailInfo">An <see cref="EmailInfo" /> that defines the e-mail message</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="style">A <see cref="T:System.String" /> that contains the css to apply to the message</param>
        public HtmlMailMessage(EmailInfo emailInfo, string body, string style)
            : this(emailInfo.Subject, body, style, new MailAddress(emailInfo.From.Address))
        {
            foreach (var to in emailInfo.Tos)
                AddTo(((EmailAddress)to).Address);

            foreach (var cc in emailInfo.Ccs)
                AddCc(((EmailAddress)cc).Address);

            foreach (var bcc in emailInfo.Bccs)
                AddBcc(((EmailAddress)bcc).Address);
        }

        /// <param name="subject">A <see cref="T:System.String" /> that contains the subject text</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body</param>
        /// <param name="style">A <see cref="T:System.String" /> that contains the css to apply to the message</param>
        /// <param name="from">
        ///     A <see cref="T:System.Net.Mail.MailAddress" /> that contains the address of the sender of the e-mail
        ///     message
        /// </param>
        /// <param name="to">
        ///     A collection of <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses of the
        ///     recipients of the e-mail message
        /// </param>
        public HtmlMailMessage(string subject, string body, string style, MailAddress from, MailAddressCollection to)
        {
            base.IsBodyHtml = true;
            base.Subject = subject;
            From = from;
            _style = style;
            _initialBody = body;

            foreach (var t in to)
                To.Add(t);

            SetBody();
        }

        #endregion

        #region properties

        /// <summary>
        ///     Gets or sets the message body
        /// </summary>
        public new string Body
        {
            get { return base.Body; }
            set
            {
                _initialBody = value;
                SetBody();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the mail message body is in Html
        /// </summary>
        public new bool IsBodyHtml => base.IsBodyHtml;

        /// <summary>
        ///     Gets or sets the css to be applied to the message body
        /// </summary>
        public string Style
        {
            get { return _style; }
            set
            {
                _style = value;
                SetBody();
            }
        }

        /// <summary>
        ///     Gets or sets the subject line for this e-mail message.
        /// </summary>
        public new string Subject
        {
            get { return base.Subject; }
            set
            {
                base.Subject = value;
                SetBody();
            }
        }

        #endregion

        #region methods

        /// <summary>
        ///     Adds an address to the BCC collection
        /// </summary>
        /// <param name="bcc">A <see cref="T:System.String" /> that contains the address to add</param>
        /// <returns>The <see cref="HtmlMailMessage" /> the BCC was added to</returns>
        public HtmlMailMessage AddBcc(string bcc)
        {
            Bcc.Add(bcc);
            return this;
        }

        /// <inheritdoc cref="AddBcc(string)" />
        /// <param name="bcc">A <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses to add</param>
        public HtmlMailMessage AddBcc(MailAddress bcc)
        {
            Bcc.Add(bcc);
            return this;
        }

        /// <inheritdoc cref="AddBcc(string)" />
        /// <param name="bcc">A collection of <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses to add</param>
        public HtmlMailMessage AddBcc(IEnumerable<MailAddress> bcc)
        {
            if (bcc == null)
                return this;

            foreach (var b in bcc)
                Bcc.Add(b);

            return this;
        }

        /// <inheritdoc cref="AddBcc(string)" />
        /// <param name="bcc">An array of <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses to add</param>
        public HtmlMailMessage AddBcc(MailAddress[] bcc)
        {
            if (bcc == null)
                return this;

            foreach (var b in bcc)
                Bcc.Add(b);

            return this;
        }

        /// <inheritdoc cref="AddBcc(string)" />
        /// <param name="bcc">A collection of <see cref="T:System.String" /> that contains the addresses to add</param>
        public HtmlMailMessage AddBcc(IEnumerable<string> bcc)
        {
            if (bcc == null)
                return this;

            foreach (var b in bcc)
                Bcc.Add(b);

            return this;
        }

        /// <summary>
        ///     Adds an address to the CC collection
        /// </summary>
        /// <param name="cc">A <see cref="T:System.String" /> that contains the address to add</param>
        /// <returns>The <see cref="HtmlMailMessage" /> the CC was added to</returns>
        public HtmlMailMessage AddCc(string cc)
        {
            CC.Add(cc);
            return this;
        }

        /// <inheritdoc cref="AddCc(string)" />
        /// <param name="cc">A <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses to add</param>
        public HtmlMailMessage AddCc(MailAddress cc)
        {
            CC.Add(cc);
            return this;
        }

        /// <inheritdoc cref="AddCc(string)" />
        /// <param name="cc">A collection of <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses to add</param>
        public HtmlMailMessage AddCc(IEnumerable<MailAddress> cc)
        {
            if (cc == null)
                return this;

            foreach (var c in cc)
                CC.Add(c);

            return this;
        }

        /// <inheritdoc cref="AddCc(string)" />
        /// <param name="cc">An array of <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses to add</param>
        public HtmlMailMessage AddCc(MailAddress[] cc)
        {
            if (cc == null)
                return this;

            foreach (var c in cc)
                CC.Add(c);

            return this;
        }

        /// <inheritdoc cref="AddCc(string)" />
        /// <param name="cc">A collection of <see cref="T:System.String" /> that contains the addresses to add</param>
        public HtmlMailMessage AddCc(IEnumerable<string> cc)
        {
            if (cc == null)
                return this;

            foreach (var c in cc)
                CC.Add(c);

            return this;
        }

        /// <summary>
        ///     Adds an address to the To collection
        /// </summary>
        /// <param name="to">A <see cref="T:System.String" /> that contains the address to add</param>
        /// <returns>The <see cref="HtmlMailMessage" /> the To was added to</returns>
        public HtmlMailMessage AddTo(string to)
        {
            To.Add(to);
            return this;
        }

        /// <inheritdoc cref="AddTo(string)" />
        /// <param name="to">A <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses to add</param>
        public HtmlMailMessage AddTo(MailAddress to)
        {
            To.Add(to);
            return this;
        }

        /// <inheritdoc cref="AddTo(string)" />
        /// <param name="to">A collection of <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses to add</param>
        public HtmlMailMessage AddTo(IEnumerable<MailAddress> to)
        {
            if (to == null)
                return this;

            foreach (var t in to)
                To.Add(t);

            return this;
        }

        /// <inheritdoc cref="AddTo(string)" />
        /// <param name="to">An array of <see cref="T:System.Net.Mail.MailAddress" /> that contains the addresses to add</param>
        public HtmlMailMessage AddTo(MailAddress[] to)
        {
            if (to == null)
                return this;

            foreach (var t in to)
                To.Add(t);

            return this;
        }

        /// <inheritdoc cref="AddTo(string)" />
        /// <param name="to">A collection of <see cref="T:System.String" /> that contains the addresses to add</param>
        public HtmlMailMessage AddTo(IEnumerable<string> to)
        {
            if (to == null)
                return this;

            foreach (var t in to)
                To.Add(t);

            return this;
        }

        /// <summary>
        ///     Sets the From address for the message
        /// </summary>
        /// <param name="from">A <see cref="T:System.String" /> that contains the address to set as the From</param>
        /// <returns>The <see cref="HtmlMailMessage" /> for which the From was set</returns>
        public HtmlMailMessage SetFrom(string from)
        {
            From = new MailAddress(from);
            return this;
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _disposed = true;
            }

            base.Dispose(disposing);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        private void SetBody()
        {
            if (IsNullOrWhiteSpace(_htmlBody))
            {
                using (var stream = Assembly.GetExecutingAssembly()
                                            .GetManifestResourceStream(EMAIL_HTML_RESOURCE_NAME))
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                            _htmlBody = reader.ReadToEnd();
                    }
                }
            }

            base.Body = _htmlBody.Replace("<title></title>",
                                          !IsNullOrWhiteSpace(Subject)
                                              ? $"<title>{Subject}</title>"
                                              : Empty)
                                 .Replace("<style></style>",
                                          !IsNullOrWhiteSpace(Style)
                                              ? $"<style>{Style}</style>"
                                              : Empty)
                                 .Replace("<body></body>",
                                          !IsNullOrWhiteSpace(_initialBody)
                                              ? $"<body>{_initialBody}</body>"
                                              : Empty)
                                 .Trim();
        }

        #endregion
    }
}
