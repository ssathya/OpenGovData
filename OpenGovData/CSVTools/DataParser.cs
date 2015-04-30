using System;

namespace CSVTools
{
    public class DataParser
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParser"/> class.
        /// </summary>
        public DataParser()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Parses the bool value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="parseSuccess">if set to <c>true</c> [parse success].</param>
        /// <returns></returns>
        public static bool ParseBoolValue(string fieldValue, out bool parseSuccess)
        {
            parseSuccess = false;
            if (string.IsNullOrEmpty(fieldValue)) return false;

            var parsedValue = false;
            const string validValues = "10YyNnTtFf";
            const string trueValue = "1YyTt";
            fieldValue = fieldValue.Trim().Substring(0, 1);
            parseSuccess = validValues.Contains(fieldValue);
            parsedValue = trueValue.Contains(fieldValue);
            return parsedValue;
        }

        /// <summary>
        /// Parses the date value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="parseSuccess">if set to <c>true</c> [parse success].</param>
        /// <returns></returns>
        public static DateTime ParseDateValue(string fieldValue, out bool parseSuccess)
        {
            DateTime parsedDateValue;
            parseSuccess = DateTime.TryParse(fieldValue, out parsedDateValue);
            return parsedDateValue;
        }

        /// <summary>
        /// Parses the double value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="parseSuccess">if set to <c>true</c> [parse success].</param>
        /// <returns></returns>
        public static double ParseDoubleValue(string fieldValue, out bool parseSuccess)
        {
            double parsedValue;
            parseSuccess = double.TryParse(fieldValue, out parsedValue);
            return parsedValue;
        }

        /// <summary>
        /// Parses the int value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="parseSuccess">if set to <c>true</c> [parse success].</param>
        /// <returns></returns>
        public static int ParseIntValue(string fieldValue, out bool parseSuccess)
        {
            int parsedValue;
            parseSuccess = int.TryParse(fieldValue, out parsedValue);
            return parsedValue;
        }

        #endregion Public Methods
    }
}