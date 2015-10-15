using System.Web.Mvc;
using Encompass.Mvc.Exceptions;
using static System.String;

#pragma warning disable 1591

namespace Encompass.Mvc.Attributes
{
    /// <summary>
    /// Extends <see cref="AuthorizeAttribute"/> which specifies that access to a controller or action method is restricted to users who meet the authorization requirement.
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute() {}

        /// <param name="roles">An array of role names for which to allow access.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// [CustomAuthorize("Admin,Manager")]
        /// ]]>
        /// </code>
        /// </example>
        public CustomAuthorizeAttribute(params string[] roles)
        {
            Roles = Join(",", roles);
        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute"/>.
        /// The <paramref name="filterContext"/> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            throw new UnauthorizedHttpException(Roles);
        }
    }
}

#pragma warning restore 1591
