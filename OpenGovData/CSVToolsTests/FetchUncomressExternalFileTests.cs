using CSVTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVTools.Tests
{
    [TestClass()]
    public class FetchUncomressExternalFileTests
    {
        private const string DataFile = "App_Data/Institutions2.csv";
        private const string DestinationDirectory = "App_Data/";
        private const string FileUrl = "https://www2.fdic.gov/idasp/Institutions2.zip";
        private const string InstitutionsFile = "App_Data/Institutions2.zip";
        private FetchUncomressExternalFile _fuef;

        [TestInitialize]
        public void Setup()
        {
            _fuef = new FetchUncomressExternalFile();
        }

        [TestMethod]
        public void GetExternalFile()
        {
            bool result = _fuef.GetExternalFile(FileUrl, DestinationDirectory, InstitutionsFile);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UncompressExternalDataTest()
        {
            bool result = _fuef.UncompressExternalFile(InstitutionsFile, DestinationDirectory);
            Assert.IsTrue(result);
        }
    }
}