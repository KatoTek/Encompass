using System;
using System.Web.Mvc;

namespace Encompass.Mvc.Arguments
{
    /// <summary>
    ///     Event arguments for the Handle Error event
    /// </summary>
    public class HandleErrorEventArgs : EventArgs
    {
        #region constructors

        /// <param name="exceptionContext">The exception context</param>
        public HandleErrorEventArgs(ExceptionContext exceptionContext)
        {
            ExceptionContext = exceptionContext;
        }

        #endregion

        #region properties

        /// <summary>
        ///     The exception context
        /// </summary>
        public ExceptionContext ExceptionContext { get; set; }

        /// <summary>
        ///     The object that is returned after an error is handled
        /// </summary>
        public object ReturnValue { get; set; }

        #endregion
    }
}
