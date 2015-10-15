using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web.Profile;
using System.Web.Security;
using static System.Web.Security.Membership;

namespace Encompass.Membership
{
    /// <summary>
    /// A service that simplifies the interaction with Membership Provider
    /// </summary>
    [EnableClientAccess]
    public class MembershipService : DomainService
    {
        /// <summary>
        /// Adds a user to Membership Provider
        /// </summary>
        /// <param name="user">The user to add</param>
        [Insert]
        public void AddUser(SimpleUser user)
        {
            if (GetUser(user.Username) == null)
                CreateUser(user.Username, GeneratePassword(20, 2), user.Email);

            UpdateProfile(user);
        }

        /// <summary>
        /// Deletes a user from Membership Provider
        /// </summary>
        /// <param name="user">The user to delete</param>
        [Delete]
        public void DeleteUser(SimpleUser user)
        {
            System.Web.Security.Membership.DeleteUser(user.Username);
            CustomizeDeleteUser(user);
        }

        /// <summary>
        /// Gets a user from Membership Provider by username
        /// </summary>
        /// <param name="username">The username of the user to retrieve</param>
        /// <returns>The user with the matching username as a SimpleUser</returns>
        public SimpleUser GetUser(string username)
        {
            var membershipUser = System.Web.Security.Membership.GetUser(username);
            return membershipUser != null ? new SimpleUser(membershipUser) : null;
        }

        /// <summary>
        /// Gets all users from Membership Provider as a collection of SimpleUser
        /// </summary>
        /// <returns>Gets all users from MembershipProvider as a collection of SimpleUser</returns>
        public IEnumerable<SimpleUser> GetUsers() => GetAllUsers().Cast<MembershipUser>().Select(user => new SimpleUser(user)).OrderBy(user => user.FullName);

        /// <summary>
        /// Updates a user in Membership Provider
        /// </summary>
        /// <param name="user">The user to update</param>
        [Update]
        public void UpdateUser(SimpleUser user)
        {
            System.Web.Security.Membership.UpdateUser(user.MembershipUser);
            UpdateProfile(user);
        }

        /// <summary>
        /// Customizable action performed when deleting a user
        /// </summary>
        /// <param name="user">The user to delete</param>
        protected virtual void CustomizeDeleteUser(SimpleUser user) {}

        /// <summary>
        /// Updates a user's profile
        /// </summary>
        /// <param name="user">The user who's profile needs to be updated</param>
        protected virtual void UpdateProfile(SimpleUser user)
        {
            dynamic profile = ProfileBase.Create(user.Username);
            profile.FullName = user.FullName;
            profile.Save();
        }
    }
}
