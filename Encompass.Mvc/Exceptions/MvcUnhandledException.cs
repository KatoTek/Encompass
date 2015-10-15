using System;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Web.Routing;
using static System.Environment;

namespace Encompass.Mvc.Exceptions
{
    /// <summary>
    /// Represents an unhandled exception in MVC
    /// </summary>
    public class MvcUnhandledException : Exception
    {
        /// <param name="routeData">Encapsulated information about the route.</param>
        public MvcUnhandledException(RouteData routeData)
            : base(GetRouteDataMessage(routeData))
        {
            SetProperties(routeData);
        }

        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public MvcUnhandledException(RouteData routeData, string message)
            : base(GetRouteDataMessage(routeData, message))
        {
            SetProperties(routeData);
        }

        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MvcUnhandledException(RouteData routeData, string message, Exception innerException)
            : base(GetRouteDataMessage(routeData, message), innerException)
        {
            SetProperties(routeData);
        }

        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MvcUnhandledException(RouteData routeData, Exception innerException)
            : base(GetRouteDataMessage(routeData), innerException)
        {
            SetProperties(routeData);
        }

        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        public MvcUnhandledException(RouteData routeData, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SetProperties(routeData);
        }

        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="user">The user that caused the exception.</param>
        public MvcUnhandledException(RouteData routeData, IPrincipal user)
            : base(GetRouteDataMessage(routeData, user))
        {
            SetProperties(routeData, user);
        }

        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="user">The user that caused the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public MvcUnhandledException(RouteData routeData, IPrincipal user, string message)
            : base(GetRouteDataMessage(routeData, user, message))
        {
            SetProperties(routeData, user);
        }

        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="user">The user that caused the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MvcUnhandledException(RouteData routeData, IPrincipal user, string message, Exception innerException)
            : base(GetRouteDataMessage(routeData, user, message), innerException)
        {
            SetProperties(routeData, user);
        }

        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="user">The user that caused the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MvcUnhandledException(RouteData routeData, IPrincipal user, Exception innerException)
            : base(GetRouteDataMessage(routeData, user), innerException)
        {
            SetProperties(routeData, user);
        }

        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="user">The user that caused the exception.</param>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        public MvcUnhandledException(RouteData routeData, IPrincipal user, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SetProperties(routeData, user);
        }

        /// <summary>
        /// The Action from the route data.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The Area from the route data.
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// The Controller from the route data.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// The Id from the route data.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The current user using the system.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Formats the information about that route that threw an exception.
        /// </summary>
        /// <param name="routeData">Encapsulated information about the route.</param>
        /// <param name="user">The user that caused the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <returns>A formatted string that describes the route that threw an exception.</returns>
        public static string GetRouteDataMessage(RouteData routeData, IPrincipal user, string message)
        {
            var messageBuilder = new StringBuilder($"{(string.IsNullOrWhiteSpace(message) ? "Unhandled Exception" : message)}{NewLine}");
            if (routeData != null)
            {
                object areaObject;
                object controllerObject;
                object actionObject;
                object idObject;

                if (routeData.DataTokens.TryGetValue("area", out areaObject))
                {
                    var area = (string)areaObject;
                    if (!string.IsNullOrWhiteSpace(area))
                        messageBuilder.Append($"{NewLine}Area: \"{area}\"");
                }

                if (routeData.Values.TryGetValue("controller", out controllerObject))
                {
                    var controller = (string)controllerObject;
                    if (!string.IsNullOrWhiteSpace(controller))
                        messageBuilder.Append($"{NewLine}Controller: \"{controller}\"");
                }

                if (routeData.Values.TryGetValue("action", out actionObject))
                {
                    var action = (string)actionObject;
                    if (!string.IsNullOrWhiteSpace(action))
                        messageBuilder.Append($"{NewLine}Action: \"{action}\"");
                }

                if (routeData.Values.TryGetValue("id", out idObject))
                {
                    var id = (string)idObject;
                    if (!string.IsNullOrWhiteSpace(id))
                        messageBuilder.Append($"{NewLine}Id: \"{id}\"");
                }
            }

            if (user != null)
                messageBuilder.Append($"{NewLine}User: \"{user.Identity.Name}\"");

            return messageBuilder.ToString();
        }

        private static string GetRouteDataMessage(RouteData routeData) => GetRouteDataMessage(routeData, null, null);
        private static string GetRouteDataMessage(RouteData routeData, string message) => GetRouteDataMessage(routeData, null, message);
        private static string GetRouteDataMessage(RouteData routeData, IPrincipal user) => GetRouteDataMessage(routeData, user, null);

        private void SetProperties(RouteData routeData, IPrincipal user = null)
        {
            if (routeData != null)
            {
                object controller;
                object action;
                object area;
                object id;

                if (routeData.DataTokens.TryGetValue("area", out area))
                    Area = (string)area;

                if (routeData.Values.TryGetValue("controller", out controller))
                    Controller = (string)controller;

                if (routeData.Values.TryGetValue("action", out action))
                    Action = (string)action;

                if (routeData.Values.TryGetValue("id", out id))
                    Id = (string)id;
            }

            if (user != null)
                User = user.Identity.Name;
        }
    }
}
