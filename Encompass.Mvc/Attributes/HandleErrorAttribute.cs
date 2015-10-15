using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Encompass.Mvc.Arguments;
using Encompass.Mvc.Exceptions;
using Encompass.Mvc.Models;

#pragma warning disable 1591

namespace Encompass.Mvc.Attributes
{
    /// <summary>
    /// Extends <see cref="System.Web.Mvc.HandleErrorAttribute"/> which represents an attribute that is used to handle an exception that is thrown by an action method.
    /// Adds support for custom exception logging and builds/returns the response based on the request type.<br/>
    /// For standard requests, the application error <see cref="ViewResult"/> is returned with the TempData populated with the TempData from the ExceptionContext.Controller.<br/>
    /// For Ajax requests, a <see cref="JsonResult"/> object is returned. The JSON structure of the <see cref="JsonResult"/> is defined in the Example section wit the Title "Ajax request JSON response structure"
    /// <example>
    /// <code language="json" title="Ajax request JSON response structure">
    /// <![CDATA[
    /// {
    ///     "Errors":
    ///     [
    ///         "Exception Message"
    ///     ]
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// </summary>
    public class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        private const string INTERNAL_SERVER_ERROR = "Internal Server Error";
        private const string RETURN_VALUE_KEY_DEFAULT = "HandleErrorReturnValue";
        private readonly string _returnValueKey;

        public HandleErrorAttribute()
        {
            _returnValueKey = RETURN_VALUE_KEY_DEFAULT;
        }

        /// <param name="returnValueKey">The key of the custom data that is returned after an error is handled.</param>
        public HandleErrorAttribute(string returnValueKey)
        {
            _returnValueKey = returnValueKey;
        }

        /// <summary>
        /// Event handlers for logging the exception
        /// </summary>
        public static event EventHandler<HandleErrorEventArgs> LogException;

        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The action-filter context.</param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException(nameof(filterContext));

            if (filterContext.IsChildAction)
                return;

            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
                return;

            var exception = filterContext.Exception;
            if (!ExceptionType.IsInstanceOfType(exception))
                return;

            var args = new HandleErrorEventArgs(filterContext);

            try
            {
                OnLogException(args);
            }
            finally
            {
                if (args.ReturnValue != null)
                {
                    if (filterContext.Controller.TempData.ContainsKey(_returnValueKey))
                        filterContext.Controller.TempData[_returnValueKey] = args.ReturnValue;
                    else
                        filterContext.Controller.TempData.Add(_returnValueKey, args.ReturnValue);
                }

                var item = (string)filterContext.RouteData.Values["controller"];
                var str = (string)filterContext.RouteData.Values["action"];
                var handleErrorInfo = new HandleErrorInfo(filterContext.Exception, item, str);

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var message = filterContext.Exception is UnauthorizedHttpException ? filterContext.Exception.Message : INTERNAL_SERVER_ERROR;

                    if (filterContext.Exception is UnauthorizedHttpException)
                    {
                        filterContext.Result = new JsonResult
                                               {
                                                   JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                                                   ContentType = "application/json",
                                                   ContentEncoding = Encoding.UTF8,
                                                   Data = new JsonData { Data = new { }, Errors = new { Server = new { errors = new[] { message } } } }
                                               };
                    }
                }
                else
                    filterContext.Result = new ViewResult { ViewName = View, MasterName = Master, ViewData = new ViewDataDictionary<HandleErrorInfo>(handleErrorInfo), TempData = filterContext.Controller.TempData };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = (new HttpException(null, exception)).GetHttpCode();
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }

        private void OnLogException(HandleErrorEventArgs handleErrorEventArgs) => LogException?.Invoke(this, handleErrorEventArgs);
    }
}

#pragma warning restore 1591
