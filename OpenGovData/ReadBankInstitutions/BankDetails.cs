using Anotar.NLog;
using CsvHelper;
using InputDataModel;
using SharpCompress.Common;
using SharpCompress.Reader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace ReadBankInstitutions
{
    public class BankDetails
    {
        #region Private Fields

        private const string DataFile = "App_Data/Institutions2.csv";
        private const string DestinationDirectory = "App_Data/";
        private const string FileUrl = "https://www2.fdic.gov/idasp/Institutions2.zip";
        private const string InstitutionsFile = "App_Data/Institutions2.zip";

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BankDetails"/> class.
        /// </summary>
        public BankDetails()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int FileLenght { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Reads the store values.
        /// </summary>
        public void ReadStoreValues()
        {
            GetExternalData();
            UncompressExternalData();
            var reader = File.OpenText(DataFile);
            var csv = new CsvReader(reader);
            var read = csv.Read();
            var fields = csv.FieldHeaders;

            Type type = typeof(Institution);
            var ip = type.GetProperties();
            var institutionProperties =
                ip.ToDictionary(propertyInfo => propertyInfo.Name.ToLower());

            do
            {
                var institution = new Institution();

                var i = 0;
                i = fields.Aggregate(i, (current, field) => PopulateInstitution(
                    csv, current, field, institutionProperties, institution));

                if (++FileLenght % 500 == 0)
                    Console.WriteLine("Processed {0} rows", FileLenght);

                read = csv.Read();
            } while (read);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Asserts the application data folder.
        /// </summary>
        private static void AssertAppDataFolder()
        {
            if (!Directory.Exists("App_Data"))
            {
                try
                {
                    Directory.CreateDirectory("App_Data");
                }
                catch (Exception exception)
                {
                    LogTo.Error("Could not create App_Data folder; application exiting");
                    Environment.Exit(1);
                }
            }
        }

        /// <summary>
        /// Gets the last Sunday.
        /// </summary>
        /// <returns></returns>
        private static DateTime GetLastSunday()
        {
            var lastSunday = DateTime.Now;
            lastSunday = lastSunday.AddDays(-(int)lastSunday.DayOfWeek);
            return lastSunday;
        }

        /// <summary>
        /// Gets the external data.
        /// </summary>
        /// <returns></returns>
        [LogToErrorOnException]
        private bool GetExternalData()
        {
            AssertAppDataFolder();
            if (File.Exists(InstitutionsFile))
            {
                var creationTime = File.GetCreationTime(InstitutionsFile);
                var lastSunday = GetLastSunday();
                if (creationTime < lastSunday)
                    GetExternalFile();
            }
            else
            {
                GetExternalFile();
            }
            if (File.Exists(InstitutionsFile))
                return true;
            return false;
        }

        /// <summary>
        /// Gets the external file.
        /// </summary>
        [LogToErrorOnException]
        private void GetExternalFile()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile(FileUrl, InstitutionsFile);
                }
                catch (Exception exception)
                {
                    LogTo.Error("Could not get file from external site; application terminating");
                    Environment.Exit(1);
                }
            }
        }

        /// <summary>
        /// Parses the bool value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="canParse">if set to <c>true</c> [can parse].</param>
        /// <returns></returns>
        private static bool ParseBoolValue(string fieldValue, out bool canParse)
        {
            canParse = false;
            if (string.IsNullOrEmpty(fieldValue)) return false;

            var parsedValue = false;
            const string validValues = "10YyNnTtFf";
            const string trueValue = "1YyTt";
            fieldValue = fieldValue.Trim().Substring(0, 1);
            canParse = validValues.Contains(fieldValue);
            parsedValue = trueValue.Contains(fieldValue);
            return parsedValue;
        }

        /// <summary>
        /// Parses the date value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="canParse">if set to <c>true</c> [can parse].</param>
        /// <returns></returns>
        private DateTime? ParseDateValue(string fieldValue, out bool canParse)
        {
            DateTime parsedDateValue;
            canParse = DateTime.TryParse(fieldValue, out parsedDateValue);
            return parsedDateValue;
        }

        /// <summary>
        /// Parses the double value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="canParse">if set to <c>true</c> [can parse].</param>
        /// <returns></returns>
        private double ParseDoubleValue(string fieldValue, out bool canParse)
        {
            double parsedValue;
            canParse = double.TryParse(fieldValue, out parsedValue);
            return parsedValue;
        }

        /// <summary>
        /// Parses the int value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="canParse">if set to <c>true</c> [can parse].</param>
        /// <returns></returns>
        private int ParseIntValue(string fieldValue, out bool canParse)
        {
            int parsedValue;
            canParse = int.TryParse(fieldValue, out parsedValue);
            return parsedValue;
        }

        /// <summary>
        /// Populates the institution.
        /// </summary>
        /// <param name="csv">The CSV.</param>
        /// <param name="i">The i.</param>
        /// <param name="field">The field.</param>
        /// <param name="institutionProperties">The institution properties.</param>
        /// <param name="institution">The institution.</param>
        /// <returns></returns>
        [LogToErrorOnException]
        private int PopulateInstitution(CsvReader csv, int i, string field, Dictionary<string, PropertyInfo> institutionProperties,
            Institution institution)
        {
            //var fieldValue = csv.GetField<string>(field);
            //switching to index read; about 30 times faster.
            var fieldValue = csv.GetField<string>(i++);

            var prop = institutionProperties[field.Replace("_", "").ToLower()];
            if (prop == null) return i;

            try
            {
                bool canParse;
                if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                {
                    var parsedValue = ParseIntValue(fieldValue, out canParse);
                    if (canParse) prop.SetValue(institution, parsedValue);
                }
                else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                {
                    var parsedValue = ParseDoubleValue(fieldValue, out canParse);
                    if (canParse) prop.SetValue(institution, parsedValue);
                }
                else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                {
                    var parsedValue = ParseBoolValue(fieldValue, out canParse);
                    if (canParse) prop.SetValue(institution, parsedValue);
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    var parsedValue = ParseDateValue(fieldValue, out canParse);
                    if (canParse) prop.SetValue(institution, parsedValue);
                }
                else
                {
                    prop.SetValue(institution, fieldValue);
                }
            }
            catch (Exception e)
            {
                LogTo.Error("Error while parsing input file\r\n\tDetailes:{0}",
                    e.Message);
                Environment.Exit(1);
            }
            return i;
        }

        /// <summary>
        /// Uncompresses the external data.
        /// </summary>
        private void UncompressExternalData()
        {
            if (File.Exists(DataFile) && File.Exists(InstitutionsFile))
            {
                if (File.GetCreationTime(DataFile) > File.GetCreationTime(InstitutionsFile))
                    return;
            }
            using (Stream stream = File.OpenRead(InstitutionsFile))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    reader.WriteEntryToDirectory(DestinationDirectory, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                }
            }
        }

        #endregion Private Methods
    }
}