using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using static System.String;
using static Encompass.Mvc.Resources.Exceptions;

namespace Encompass.Mvc.Extensions
{
    /// <summary>
    ///     <see cref="ModelStateDictionary" /> extension methods.
    /// </summary>
    public static class ModelStateExtensions
    {
        #region methods

        /// <summary>
        ///     Serializes all errors in the provided <see cref="ModelStateDictionary" />.
        /// </summary>
        /// <param name="modelState">The <see cref="ModelStateDictionary" /> to get the errors from and serialize.</param>
        /// <returns>Serialized model state errors.</returns>
        public static object SerializeErrors(this ModelStateDictionary modelState) => (from entry in modelState
                                                                                       where entry.Value.Errors.Any()
                                                                                       select entry).ToDictionary(entry => entry.Key, entry => SerializeModelState(entry.Value));

        private static string GetErrorMessage(ModelError error, ModelState modelState)
        {
            if (!IsNullOrEmpty(error.ErrorMessage))
                return error.ErrorMessage;

            return modelState.Value == null
                       ? error.ErrorMessage
                       : Format(CultureInfo.CurrentCulture, ValueNotValidForProperty, modelState.Value.AttemptedValue);
        }

        private static Dictionary<string, object> SerializeModelState(ModelState modelState) => new Dictionary<string, object>
                                                                                                {
                                                                                                    ["errors"] = (from error in modelState.Errors
                                                                                                                  select
                                                                                                                      GetErrorMessage(error, modelState))
                                                                                                        .ToArray()
                                                                                                };

        #endregion
    }
}
