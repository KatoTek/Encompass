using System.Web.Mvc;
using System.Web.Routing;
using static System.Web.Mvc.UrlHelper;

namespace Encompass.Mvc.Extensions
{
    /// <summary>
    /// <see cref="UrlHelper"/> extension methods.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates an action based on the provided route information.
        /// </summary>
        /// <param name="url">The UrlHelper to use.</param>
        /// <param name="action">The action name.</param>
        /// <param name="controller">The controller name.</param>
        /// <param name="fragment">The fragment.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>A string that can be used as an action.</returns>
        public static string Action(this UrlHelper url, string action, string controller, string fragment, object routeValues)
            =>
                GenerateUrl(null,
                            action,
                            controller,
                            routeValues: new RouteValueDictionary(routeValues),
                            fragment: fragment,
                            protocol: null,
                            hostName: null,
                            routeCollection: url.RouteCollection,
                            requestContext: url.RequestContext,
                            includeImplicitMvcValues: true);
    }
}
