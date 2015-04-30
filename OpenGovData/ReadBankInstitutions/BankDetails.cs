using Anotar.NLog;
using CsvHelper;
using CSVTools;
using InputDataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public readonly IList<Institution> institutions;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BankDetails"/> class.
        /// </summary>
        public BankDetails()
        {
            institutions = new List<Institution>();
        }

        #endregion Public Constructors

        #region Public Properties

        public int FileLenght { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Reads the store values.
        /// </summary>
        public void ReadStoreValues(int limit)
        {
            if (limit == 0)
                limit = int.MaxValue;
            var fuExternalFile =
                new FetchUncomressExternalFile();
            fuExternalFile.GetExternalFile(FileUrl, DestinationDirectory, InstitutionsFile);
            fuExternalFile.UncompressExternalFile(DataFile, DestinationDirectory);

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
                institutions.Add(institution);
            } while (read && FileLenght < limit);
        }

        #endregion Public Methods

        #region Private Methods

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
                    var parsedValue = DataParser.ParseIntValue(fieldValue, out canParse);
                    if (canParse) prop.SetValue(institution, parsedValue);
                }
                else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                {
                    var parsedValue = DataParser.ParseDoubleValue(fieldValue, out canParse);
                    if (canParse) prop.SetValue(institution, parsedValue);
                }
                else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                {
                    var parsedValue = DataParser.ParseBoolValue(fieldValue, out canParse);
                    if (canParse) prop.SetValue(institution, parsedValue);
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    var parsedValue = DataParser.ParseDateValue(fieldValue, out canParse);
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

        #endregion Private Methods
    }
}