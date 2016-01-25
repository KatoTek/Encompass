using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web.Profile;
using System.Web.Security;
using Encompass.Membership.Interfaces;
using static System.Web.Security.Membership;

namespace Encompass.Membership
{
    /// <summary>
    ///     A service that simplifies the interaction with Membership Provider
    /// </summary>
    [EnableClientAccess]
    public class MembershipService : DomainService
    {
        /// <summary>
        ///     Adds a user to Membership Provider
        /// </summary>
        /// <param name="user">The user to add</param>
        [Insert]
        public void AddUser(ISimpleUser user)
        {
            if (this.GetUser(user.Username) == null)
                CreateUser(user.Username, GeneratePassword(20, 2), user.Email);

            this.UpdateProfile(user);
        }

        /// <summary>
        ///     Deletes a user from Membership Provider
        /// </summary>
        /// <param name="user">The user to delete</param>
        [Delete]
        public void DeleteUser(ISimpleUser user)
        {
            System.Web.Security.Membership.DeleteUser(user.Username);
            this.CustomizeDeleteUser(user);
        }

        /// <summary>
        ///     Gets a user from Membership Provider by username
        /// </summary>
        /// <param name="username">The username of the user to retrieve</param>
        /// <returns>The user with the matching username as a SimpleUser</returns>
        public SimpleUser GetUser(string username)
        {
            return this.GetUser<SimpleUser>(username);
        }

        /// <summary>
        ///     Gets a user from Membership Provider by username
        /// </summary>
        /// <typeparam name="T">The type of ISimpleUser to return</typeparam>
        /// <param name="username">The username of the user to retrieve</param>
        /// <returns>The user with the matching username as a type of T</returns>
        public T GetUser<T>(string username) where T : ISimpleUser
        {
            var membershipUser = System.Web.Security.Membership.GetUser(username);
            return GetUserInstance<T>(membershipUser);
        }

        private static T GetUserInstance<T>(MembershipUser membershipUser) where T : ISimpleUser
        {
            return membershipUser != null ? (T) Activator.CreateInstance(typeof (T), membershipUser) : default(T);
        }

        /// <summary>
        ///     Gets all users from Membership Provider as a collection of SimpleUser
        /// </summary>
        /// <returns>Gets all users from MembershipProvider as a collection of SimpleUser</returns>
        public IEnumerable<ISimpleUser> GetUsers() => this.GetUsers<SimpleUser>();

        /// <summary>
        ///     Gets all users from Membership Provider as a collection of ISimpleUser
        /// </summary>
        /// <typeparam name="T">The type of ISimpleUser</typeparam>
        /// <returns>Gets all users from MembershipProvider as a collection of type T</returns>
        public IEnumerable<T> GetUsers<T>() where T : ISimpleUser
            =>
                Enumerable.OrderBy(GetAllUsers().Cast<MembershipUser>().Select(GetUserInstance<T>),
                    user => user.FullName);

        /// <summary>
        ///     Updates a user in Membership Provider
        /// </summary>
        /// <param name="user">The user to update</param>
        [Update]
        public void UpdateUser(ISimpleUser user)
        {
            System.Web.Security.Membership.UpdateUser(user.MembershipUser);
            this.UpdateProfile(user);
        }

        /// <summary>
        ///     Customizable action performed when deleting a user
        /// </summary>
        /// <param name="user">The user to delete</param>
        protected virtual void CustomizeDeleteUser(ISimpleUser user)
        {
        }

        /// <summary>
        ///     Updates a user's profile
        /// </summary>
        /// <param name="user">The user who's profile needs to be updated</param>
        protected virtual void UpdateProfile(ISimpleUser user)
        {
            dynamic profile = ProfileBase.Create(user.Username);
            profile.FullName = user.FullName;
            this.CustomizeUpdateProfile(user, profile);
            profile.Save();
        }

        /// <summary>
        ///     Customizable action performed when updating a user's profile
        /// </summary>
        /// <param name="user">The user who's profile needs to be updated</param>
        /// <param name="profile">The user profile that is being updated</param>
        protected virtual void CustomizeUpdateProfile(ISimpleUser user, dynamic profile)
        {
        }
    }
}