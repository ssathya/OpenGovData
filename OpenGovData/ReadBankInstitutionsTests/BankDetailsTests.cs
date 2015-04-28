using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReadBankInstitutions.Tests
{
    [TestClass()]
    public class BankDetailsTests
    {
        private BankDetails _bd;

        [TestInitialize]
        public void Setup()
        {
            _bd = new BankDetails();
        }

        [TestMethod]
        public void ReadExternalData()
        {
            _bd.ReadStoreValues();
            Assert.IsTrue(_bd.FileLenght == 27598);
        }
    }
}