using System.Web;
using static System.String;

#pragma warning disable 1591

namespace Encompass.Mvc.Exceptions
{
    /// <summary>
    /// Extends <see cref="HttpException"/> which describes an exception that occurred during the processing of HTTP requests.
    /// This exception is specifically designated to be used when a user in not authorized for the request.
    /// </summary>
    public class UnauthorizedHttpException : HttpException
    {
        public UnauthorizedHttpException()
            : base(403, "You do not have the required authorization.") {}

        /// <param name="missingRoles">The roles that are required and the user is not assigned to.</param>
        public UnauthorizedHttpException(params string[] missingRoles)
            : base(403, $"You do not have the required role(s) '{Join(", ", missingRoles)}'.") {}

        /// <param name="httpCode">The HTTP response status code displayed on the client.</param>
        public UnauthorizedHttpException(int httpCode)
            : base(httpCode, "You do not have the required authorization.") {}

        /// <param name="httpCode">The HTTP response status code displayed on the client.</param>
        /// <param name="missingRoles">The roles that are required and the user is not assigned to.</param>
        public UnauthorizedHttpException(int httpCode, params string[] missingRoles)
            : base(httpCode, $"You do not have the required role(s) '{Join(", ", missingRoles)}'.") {}
    }
}

#pragma warning restore 1591
