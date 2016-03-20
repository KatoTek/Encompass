using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static System.String;
using static System.StringComparison;
using static System.Web.Mvc.JsonRequestBehavior;

#pragma warning disable 1591
#pragma warning disable 0618 // JavaScriptSerializer is no longer obsolete

namespace Encompass.Mvc.Results
{
    /// <summary>
    ///     Represents a JSONP result which is a way to get data from another domain that bypasses CORS (Cross Origin Resource
    ///     Sharing) rules.
    ///     CORS is a set of "rules," about transferring data between sites that have a different domain name from the client.
    /// </summary>
    public class JsonpResult : ActionResult
    {
        #region fields

        private const string DEFAULT_CONTENT_TYPE = "application/x-javascript";

        #endregion

        #region constructors

        public JsonpResult() {}

        /// <param name="jsonResult">The <see cref="JsonResult" /> to convert to a <see cref="JsonpResult" />.</param>
        public JsonpResult(JsonResult jsonResult)
        {
            ContentEncoding = jsonResult.ContentEncoding;
            ContentType = jsonResult.ContentType;
            Data = jsonResult.Data;
            JsonRequestBehavior = jsonResult.JsonRequestBehavior;
        }

        #endregion

        #region properties

        /// <summary>
        ///     The function to use on callback
        /// </summary>
        public string CallbackFunction { get; set; }

        /// <summary>
        ///     The content encoding.
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        ///     The content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     The data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        ///     Specifies whether HTTP GET requests from the client are allowed.
        /// </summary>
        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        #endregion

        #region methods

        /// <summary>
        ///     Serializes an object into JSONP and returns that as the action result.
        /// </summary>
        /// <param name="context">
        ///     The context in which the result is executed. The context information includes the controller,
        ///     HTTP content, request context, and route data.
        /// </param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (JsonRequestBehavior == DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }

            var response = context.HttpContext.Response;
            response.ContentType = IsNullOrWhiteSpace(ContentType)
                                       ? DEFAULT_CONTENT_TYPE
                                       : ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            var request = context.HttpContext.Request;
            var callback = CallbackFunction ?? request.Params["callback"] ?? "callback";
            response.Write($"{callback}({new JavaScriptSerializer().Serialize(Data)});");
        }

        #endregion
    }
}

#pragma warning restore 0618
#pragma warning restore 1591
