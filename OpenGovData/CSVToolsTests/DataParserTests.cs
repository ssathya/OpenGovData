using CSVTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;

namespace CSVToolsTests
{
    [TestClass()]
    public class DataParserTests
    {
        private bool _parseSuccess;

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod()]
        public void ParseBooleanValuesTest()
        {
            //arrange
            var testVal = "ty1";
            //assert true values
            foreach (var testResult in testVal.Select(testStr => DataParser.ParseBoolValue(
                testStr.ToString(CultureInfo.InvariantCulture), out _parseSuccess)))
            {
                Assert.IsTrue(_parseSuccess);
                Assert.IsTrue(testResult);
            }
            testVal = "fn0";
            //assert false values
            foreach (var testResult in testVal.Select(testStr => DataParser.ParseBoolValue(
                testStr.ToString(CultureInfo.InvariantCulture), out _parseSuccess)))
            {
                Assert.IsTrue(_parseSuccess);
                Assert.IsFalse(testResult);
            }
        }

        [TestMethod]
        public void ParseBooleanValueIncorrectValue()
        {
            //arrange
            const string testVal = "q";
            //act
            var testResult = DataParser.ParseBoolValue(testVal, out _parseSuccess);
            //assert
            Assert.IsFalse(_parseSuccess);
            Assert.IsFalse(testResult);
        }

        [TestMethod]
        public void ParseDateValue()
        {
            const string testDate = "1/1/1990";
            DateTime testResult = DataParser.ParseDateValue(testDate, out _parseSuccess);
            //assert
            Assert.IsTrue(_parseSuccess);
            Assert.AreEqual(DateTime.Parse("1/1/1990"), testResult);
        }

        [TestMethod]
        public void ParseIncorrectDateValue()
        {
            const string testDate = "13/41/1990";

            DateTime testResult = DataParser.ParseDateValue(testDate, out _parseSuccess);
            //assert
            Assert.IsFalse(_parseSuccess);
        }

        [TestMethod]
        public void ParseIntValue()
        {
            const string testData = "1028";
            int testResult = DataParser.ParseIntValue(testData, out _parseSuccess);
            //assert
            Assert.IsTrue(_parseSuccess);
            Assert.AreEqual(1028, testResult);
        }

        [TestMethod]
        public void ParseIncorrectIntValue()
        {
            const string testData = "1028a";
            int testResult = DataParser.ParseIntValue(testData, out _parseSuccess);
            //assert
            Assert.IsFalse(_parseSuccess);
        }

        [TestMethod]
        public void ParseDoubleValue()
        {
            const string testData = "1028.99";
            double testResult = DataParser.ParseDoubleValue(testData, out _parseSuccess);
            //assert
            Assert.IsTrue(_parseSuccess);
            Assert.AreEqual(1028.99, testResult);
        }

        [TestMethod]
        public void ParseIncorrectDoubleValue()
        {
            const string testData = "102a8.99";
            double testResult = DataParser.ParseDoubleValue(testData, out _parseSuccess);
            //assert
            Assert.IsFalse(_parseSuccess);
        }
    }
}