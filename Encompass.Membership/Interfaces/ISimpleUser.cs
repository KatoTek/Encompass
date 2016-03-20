using System;
using System.Web.Profile;
using System.Web.Security;

namespace Encompass.Membership.Interfaces
{
    /// <summary>
    ///     A contract for the object that simplifies working with Membership Provider users and profiles
    /// </summary>
    public interface ISimpleUser
    {
        #region properties

        /// <summary>
        ///     Email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        ///     Full Name
        /// </summary>
        string FullName { get; set; }

        /// <summary>
        ///     Last Activity Date
        /// </summary>
        DateTime LastActivityDate { get; set; }

        /// <summary>
        ///     <see cref="MembershipUser" /> representation of the current <see cref="ISimpleUser" />
        /// </summary>
        MembershipUser MembershipUser { get; }

        /// <summary>
        ///     A reference to the user's profile
        /// </summary>
        ProfileBase Profile { get; set; }

        /// <summary>
        ///     UserId
        /// </summary>
        Guid UserId { get; set; }

        /// <summary>
        ///     Username
        /// </summary>
        string Username { get; set; }

        #endregion
    }
}
