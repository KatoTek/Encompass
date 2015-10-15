using System.Text;
using System.Web.Mvc;
using Encompass.Mvc.Models;
using Encompass.Mvc.Results;
using Newtonsoft.Json;
using static System.Web.Mvc.JsonRequestBehavior;
using static Newtonsoft.Json.Formatting;

namespace Encompass.Mvc.Extensions
{
    /// <summary>
    /// <see cref="Controller"/> extension methods.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data) => controller.JsonCamelCase(data, None, null, null, DenyGet);

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, Formatting formatting) => controller.JsonCamelCase(data, formatting, null, null, DenyGet);

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, string contentType) => controller.JsonCamelCase(data, None, contentType, null, DenyGet);

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, string contentType, Encoding contentEncoding)
            => controller.JsonCamelCase(data, None, contentType, contentEncoding, DenyGet);

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, JsonRequestBehavior behavior) => controller.JsonCamelCase(data, None, null, null, behavior);

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, string contentType, JsonRequestBehavior behavior) => controller.JsonCamelCase(data, None, contentType, null, behavior);

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
            => controller.JsonCamelCase(data, None, contentType, contentEncoding, behavior);

        /// <summary>
        /// Serializes an object into json and camel cases the property names.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="data">The object to serialize.</param>
        /// <param name="formatting">The Newtonsoft.Json.Formatting options.</param>
        /// <param name="contentType">The type of the content. Can be null.</param>
        /// <param name="contentEncoding">The content encoding. Can be null.</param>
        /// <param name="behavior">Specifies whether HTTP GET requests from the client are allowed.</param>
        /// <returns>A json result containing the serialized object with camel cased property names.</returns>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, Formatting formatting, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
            => new JsonCamelCaseResult { Data = data, Formatting = formatting, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, Formatting formatting, string contentType) => controller.JsonCamelCase(data, formatting, contentType, null, DenyGet);

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, Formatting formatting, string contentType, Encoding contentEncoding)
            => controller.JsonCamelCase(data, formatting, contentType, contentEncoding, DenyGet);

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, Formatting formatting, JsonRequestBehavior behavior) => controller.JsonCamelCase(data, formatting, null, null, behavior);

        /// <inheritdoc cref="JsonCamelCase(Controller, object, Formatting, string, Encoding, JsonRequestBehavior)"/>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data, Formatting formatting, string contentType, JsonRequestBehavior behavior)
            => controller.JsonCamelCase(data, formatting, contentType, null, behavior);

        /// <summary>
        /// Converts a regular <see cref="JsonResult"/> to a <see cref="JsonCamelCaseResult"/>.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="jsonResult">The <see cref="JsonResult"/> to convert.</param>
        /// <returns>The <paramref name="jsonResult"/> as a <see cref="JsonCamelCaseResult"/>.</returns>
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, JsonResult jsonResult) => new JsonCamelCaseResult(jsonResult);

        /// <inheritdoc cref="JsonData(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonData(this Controller controller, object data) => controller.JsonData(data, null, null, DenyGet);

        /// <inheritdoc cref="JsonData(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonData(this Controller controller, object data, string contentType) => controller.JsonData(data, contentType, null, DenyGet);

        /// <inheritdoc cref="JsonData(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonData(this Controller controller, object data, string contentType, Encoding contentEncoding) => controller.JsonData(data, contentType, contentEncoding, DenyGet);

        /// <inheritdoc cref="JsonData(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonData(this Controller controller, object data, JsonRequestBehavior behavior) => controller.JsonData(data, null, null, behavior);

        /// <inheritdoc cref="JsonData(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonData(this Controller controller, object data, string contentType, JsonRequestBehavior behavior) => controller.JsonData(data, contentType, null, behavior);

        /// <summary>
        /// Serializes an object into JSON and adds it to the <see cref="Encompass.Mvc.Models.JsonData"/> return type which is then assigned to the Data property of the returned <see cref="JsonResult"/>.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="data">The object to serialize.</param>
        /// <param name="contentType">The type of the content. Can be null.</param>
        /// <param name="contentEncoding">The content encoding. Can be null.</param>
        /// <param name="behavior">Specifies whether HTTP GET requests from the client are allowed.</param>
        /// <returns>A <see cref="JsonResult"/> containing the serialized <paramref name="data"/> object as a <see cref="Encompass.Mvc.Models.JsonData"/> object. The <see cref="Encompass.Mvc.Models.JsonData"/>.Errors property
        /// is populated with any ModelState errors from the <paramref name="controller."/></returns>
        public static JsonResult JsonData(this Controller controller, object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            var jsonData = new JsonData { Data = data };

            if (controller.ModelState != null && !controller.ModelState.IsValid)
                jsonData.Errors = controller.ModelState.SerializeErrors();

            return new JsonResult { Data = jsonData, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };
        }

        /// <summary>
        /// Converts the Data of a regular <see cref="JsonResult"/> to a <see cref="Encompass.Mvc.Models.JsonData"/> object and setting the <paramref name="controller"/>.ModelState errors as the <see cref="Encompass.Mvc.Models.JsonData"/>.Errors
        /// </summary>
        /// <param name="controller">The controller</param>
        /// <param name="jsonResult">The <see cref="JsonResult"/> to convert.</param>
        /// <returns>A <see cref="JsonResult"/> using the provided <paramref name="jsonResult"/> and converting the Data to a <see cref="Encompass.Mvc.Models.JsonData"/> object. The <see cref="Encompass.Mvc.Models.JsonData"/>.Errors property
        /// is populated with any ModelState errors from the <paramref name="controller."/></returns>
        public static JsonResult JsonData(this Controller controller, JsonResult jsonResult)
        {
            var jsonData = new JsonData { Data = jsonResult.Data };

            if (controller.ModelState != null && !controller.ModelState.IsValid)
                jsonData.Errors = controller.ModelState.SerializeErrors();

            return new JsonResult
                   {
                       Data = jsonData,
                       ContentType = jsonResult.ContentType,
                       ContentEncoding = jsonResult.ContentEncoding,
                       JsonRequestBehavior = jsonResult.JsonRequestBehavior,
                       MaxJsonLength = int.MaxValue,
                       RecursionLimit = jsonResult.RecursionLimit
                   };
        }

        /// <summary>
        /// Serializes the <paramref name="data"/> object into JSON and returns it as a <see cref="JsonResult"/> allowing for the maximum possible length of JSON data. Intended to be used for large
        /// objects where the JSON length would be very long.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="data">The object to serialize.</param>
        /// <param name="contentType">The type of the content. Can be null.</param>
        /// <param name="contentEncoding">The content encoding. Can be null.</param>
        /// <param name="behavior">Specifies whether HTTP GET requests from the client are allowed.</param>
        /// <returns>A <see cref="JsonResult"/> containing the serialized <paramref name="data"/> object.</returns>
        public static JsonResult JsonMax(this Controller controller, object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
            => new JsonResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior, MaxJsonLength = int.MaxValue };

        /// <summary>
        /// Converts a <see cref="JsonResult"/> into another <see cref="JsonResult"/> with the maximum possible length of JSON data.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="jsonResult">The <see cref="JsonResult"/> to convert.</param>
        /// <returns>A new <see cref="JsonResult"/> with the MaxJsonLength property set to the highest possible value.</returns>
        public static JsonResult JsonMax(this Controller controller, JsonResult jsonResult)
            =>
                new JsonResult
                {
                    Data = jsonResult.Data,
                    ContentType = jsonResult.ContentType,
                    ContentEncoding = jsonResult.ContentEncoding,
                    JsonRequestBehavior = jsonResult.JsonRequestBehavior,
                    MaxJsonLength = int.MaxValue,
                    RecursionLimit = jsonResult.RecursionLimit
                };

        /// <inheritdoc cref="JsonMax(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonMax(this Controller controller, object data) => controller.JsonMax(data, null, null, DenyGet);

        /// <inheritdoc cref="JsonMax(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonMax(this Controller controller, object data, JsonRequestBehavior behavior) => controller.JsonMax(data, null, null, behavior);

        /// <inheritdoc cref="JsonMax(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonMax(this Controller controller, object data, string contentType) => controller.JsonMax(data, contentType, null, DenyGet);

        /// <inheritdoc cref="JsonMax(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonMax(this Controller controller, object data, string contentType, Encoding contentEncoding) => controller.JsonMax(data, contentType, contentEncoding, DenyGet);

        /// <inheritdoc cref="JsonMax(Controller, object, string, Encoding, JsonRequestBehavior)"/>
        public static JsonResult JsonMax(this Controller controller, object data, string contentType, JsonRequestBehavior behavior) => controller.JsonMax(data, contentType, null, behavior);

        /// <summary>
        /// Serializes a <paramref name="data"/> object into JSON and returns it as a <see cref="JsonpResult"/>.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="callBackFunction">The function to use on callback</param>
        /// <param name="data">The object to serialize.</param>
        /// <param name="contentType">The type of the content. Can be null.</param>
        /// <param name="contentEncoding">The content encoding. Can be null.</param>
        /// <returns>A <see cref="JsonpResult"/> with the serialized <paramref name="data"/>.</returns>
        public static JsonpResult Jsonp(this Controller controller, string callBackFunction, object data, string contentType, Encoding contentEncoding)
            => new JsonpResult { Data = data, CallbackFunction = callBackFunction, ContentType = contentType, ContentEncoding = contentEncoding };

        /// <summary>
        /// Converts a <see cref="JsonResult"/> to a <paramref name="jsonResult"/>.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="jsonResult">The <see cref="JsonResult"/> to convert.</param>
        /// <returns>A <see cref="JsonpResult"/> with the serialized data.</returns>
        public static JsonpResult Jsonp(this Controller controller, JsonResult jsonResult) => new JsonpResult(jsonResult);

        /// <inheritdoc cref="Jsonp(Controller, string, object, string, Encoding)"/>
        public static JsonpResult Jsonp(this Controller controller, string callBackFunction, object data, string contentType) => controller.Jsonp(callBackFunction, data, contentType, null);

        /// <inheritdoc cref="Jsonp(Controller, string, object, string, Encoding)"/>
        public static JsonpResult Jsonp(this Controller controller, string callBackFunction, object data, Encoding contentEncoding) => controller.Jsonp(callBackFunction, data, null, contentEncoding);

        /// <inheritdoc cref="Jsonp(Controller, string, object, string, Encoding)"/>
        public static JsonpResult Jsonp(this Controller controller, object data, string contentType, Encoding contentEncoding) => controller.Jsonp(null, data, contentType, contentEncoding);

        /// <inheritdoc cref="Jsonp(Controller, string, object, string, Encoding)"/>
        public static JsonpResult Jsonp(this Controller controller, object data, Encoding contentEncoding) => controller.Jsonp(null, data, null, contentEncoding);

        /// <inheritdoc cref="Jsonp(Controller, string, object, string, Encoding)"/>
        public static JsonpResult Jsonp(this Controller controller, object data, string contentType) => controller.Jsonp(null, data, contentType, null);
    }
}
