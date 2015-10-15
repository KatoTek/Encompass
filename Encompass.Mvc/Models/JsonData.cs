namespace Encompass.Mvc.Models
{
    /// <summary>
    /// Common object that can be used when returning data as JSON.
    /// </summary>
    public class JsonData
    {
        /// <summary>
        /// The data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// The errors.
        /// </summary>
        public object Errors { get; set; }
    }
}
