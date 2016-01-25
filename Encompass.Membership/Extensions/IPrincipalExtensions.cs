using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using Encompass.Membership.Interfaces;
using SWS = System.Web.Security;

namespace Encompass.Membership.Extensions
{
    /// <summary>
    ///     Extensions for the IPrincipal type
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class IPrincipalExtensions
    {
        /// <summary>
        ///     Converts an IPrincipal user to a SimpleUser and returns it
        /// </summary>
        /// <param name="iPrincipal">The IPrincipal user to convert</param>
        /// <returns>A SimpleUser representation of the supplied IPrincipal user</returns>
        public static SimpleUser GetUser(this IPrincipal iPrincipal)
        {
            return GetUser<SimpleUser>(iPrincipal);
        }

        /// <summary>
        ///     Converts an IPrincipal user to an ISimpleUser and returns it
        /// </summary>
        /// <typeparam name="T">The type of ISimpleUser to convert to and return</typeparam>
        /// <param name="iPrincipal">The IPrincipal user to convert</param>
        /// <returns>An ISimpleUser representation of the supplied IPrincipal user</returns>
        public static T GetUser<T>(this IPrincipal iPrincipal) where T : ISimpleUser
        {
            var membershipUser = SWS.Membership.GetUser(iPrincipal.Identity.Name);

            return membershipUser != null ? (T) Activator.CreateInstance(typeof (T), new {membershipUser}) : default(T);
        }

        /// <summary>
        ///     Determines if the IPrincipal user is in all specified roles
        /// </summary>
        /// <param name="iPrincipal">The IPrincipal user to convert</param>
        /// <param name="roles">The roles to validate against</param>
        /// <returns>True if the user is in all of the specified roles, otherwise false</returns>
        public static bool IsInAllRoles(this IPrincipal iPrincipal, params string[] roles)
            => roles.All(iPrincipal.IsInRole);

        /// <summary>
        ///     Determines if the IPrincipal user is in any of the specified roles
        /// </summary>
        /// <param name="iPrincipal">The IPrincipal user to convert</param>
        /// <param name="roles">The roles to validate against</param>
        /// <returns>True if the user is in any of the specified roles, otherwise false</returns>
        public static bool IsInAnyRole(this IPrincipal iPrincipal, params string[] roles)
            => roles.Any(iPrincipal.IsInRole);
    }
}