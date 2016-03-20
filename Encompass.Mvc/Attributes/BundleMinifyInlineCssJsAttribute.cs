using System.Web.Mvc;
using BundlingAndMinifyingInlineCssJs.ResponseFilters;

namespace Encompass.Mvc.Attributes
{
    /// <summary>
    ///     Attribute that can be used to utilize the BundleMinifyInlineJsCss library which bundles and minifies inline
    ///     javascript and css.
    /// </summary>
    public class BundleMinifyInlineCssJsAttribute : ActionFilterAttribute
    {
        #region methods

        /// <summary>
        ///     Bundles and minifies inline javascript and css for the current action.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
            => filterContext.HttpContext.Response.Filter = new BundleAndMinifyResponseFilter(filterContext.HttpContext.Response.Filter);

        #endregion
    }
}
