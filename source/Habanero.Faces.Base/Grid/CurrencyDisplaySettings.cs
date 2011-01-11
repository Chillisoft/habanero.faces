namespace Habanero.Faces.Base
{
    /// <summary>
    /// Stores date display settings that define how dates should
    /// be displayed to users in various user interfaces
    /// </summary>
    public class CurrencyDisplaySettings
    {
        /// <summary>
        /// Gets and sets the .Net style date format string that
        /// determines how a date is displayed in a grid.
        /// Set this value to null to use the short
        /// date format of the underlying user's environment.
        /// The format for this string is the same as that of
        /// DateTime.ToString(), including shortcuts such as
        /// "d" which use the short date format of the culture
        /// on the user's machine.
        /// </summary>
        public string GridDoubleFormat { get; set; }
    }
}