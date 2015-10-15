using System;
using System.Web.Profile;
using System.Web.Security;
using static System.Web.Security.Membership;

#pragma warning disable 1591

namespace Encompass.Membership
{
    /// <summary>
    /// A object that simplifies working with Membership Provider users and profiles
    /// </summary>
    public class SimpleUser
    {
        public SimpleUser() {}

        /// <param name="user">The MembershipUser to convert to a SimpleUser</param>
        public SimpleUser(MembershipUser user)
        {
            if (user != null)
            {
                if (user.ProviderUserKey != null)
                    UserId = (Guid)user.ProviderUserKey;

                Username = user.UserName;
                Email = user.Email;
            }

            Profile.Initialize(Username, true);
            FullName = (string)Profile.GetPropertyValue("FullName");
        }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Full Name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Last Activity Date
        /// </summary>
        public DateTime LastActivityDate { get; set; }

        /// <summary>
        /// <see cref="MembershipUser"/> representation of the current <see cref="SimpleUser"/>
        /// </summary>
        public MembershipUser MembershipUser
        {
            get
            {
                var user = GetUser(Username);
                if (user != null)
                    user.Email = Email;

                return user;
            }
        }

        /// <summary>
        /// A reference to the user's profile
        /// </summary>
        public ProfileBase Profile { get; set; } = new ProfileBase();

        /// <summary>
        /// UserId
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
    }
}

#pragma warning restore 1591
