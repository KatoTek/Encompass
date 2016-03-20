using System;
using System.Web.Mvc;
using static System.Convert;
using static System.DateTimeKind;

namespace Encompass.Mvc.ModelBinders
{
    /// <summary>
    ///     Maps a browser request containing <see cref="DateTime" /> properties to a data object.
    /// </summary>
    public class DateTimeModelBinder : DefaultModelBinder
    {
        #region methods

        /// <summary>
        ///     Binds Date date properties in the model by converting the value to a <see cref="DateTime" /> object.
        /// </summary>
        /// <param name="controllerContext">
        ///     The context within which the controller operates. The context information includes the
        ///     controller, HTTP content, request context, and route data.
        /// </param>
        /// <param name="bindingContext">
        ///     The context within which the model is bound. The context includes information such as the
        ///     model object, model name, model type, property filter, and value provider.
        /// </param>
        /// <returns>The bound object.</returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (!value.AttemptedValue.StartsWith("/Date("))
                return base.BindModel(controllerContext, bindingContext);

            try
            {
                var date = new DateTime(1970, 01, 01, 0, 0, 0, Utc).ToUniversalTime();
                var attemptedValue = value.AttemptedValue.Replace("/Date(", "")
                                          .Replace(")/", "");
                var milliSecondsOffset = ToDouble(attemptedValue);
                var result = date.AddMilliseconds(milliSecondsOffset);
                result = result.ToUniversalTime();
                return result;
            }
            catch
            {
                return base.BindModel(controllerContext, bindingContext);
            }
        }

        #endregion
    }
}
