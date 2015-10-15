using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using static System.String;
using static System.StringComparison;
using static System.Web.Mvc.JsonRequestBehavior;
using static Newtonsoft.Json.Formatting;
using static Newtonsoft.Json.JsonConvert;

#pragma warning disable 1591

namespace Encompass.Mvc.Results
{
    /// <summary>
    /// Represents a JSON result with property names converted to camel case.
    /// </summary>
    public class JsonCamelCaseResult : ActionResult
    {
        private const string DEFAULT_CONTENT_TYPE = "application/json";

        public JsonCamelCaseResult()
        {
            JsonRequestBehavior = DenyGet;
            Formatting = None;
        }

        /// <param name="jsonResult">The <see cref="JsonResult"/> to convert to a <see cref="JsonCamelCaseResult"/>.</param>
        public JsonCamelCaseResult(JsonResult jsonResult)
        {
            ContentEncoding = jsonResult.ContentEncoding;
            ContentType = jsonResult.ContentType;
            Data = jsonResult.Data;
            Formatting = None;
            JsonRequestBehavior = jsonResult.JsonRequestBehavior;
        }

        /// <summary>
        /// The content encoding.
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// The content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// The formatting options.
        /// </summary>
        public Formatting Formatting { get; set; }

        /// <summary>
        /// Specifies whether HTTP GET requests from the client are allowed.
        /// </summary>
        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        /// <summary>
        /// Serializes an object into JSON and camel cases the property names and returns that as the action result.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
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
            response.ContentType = !IsNullOrWhiteSpace(ContentType) ? ContentType : DEFAULT_CONTENT_TYPE;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            jsonSerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });

            response.Write(SerializeObject(Data, Formatting, jsonSerializerSettings));
        }
    }
}

#pragma warning restore 1591
