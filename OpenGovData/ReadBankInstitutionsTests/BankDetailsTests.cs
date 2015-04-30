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
            _bd.ReadStoreValues(2);
            Assert.IsTrue(_bd.FileLenght == 2);
            //_bd = new BankDetails();
            //_bd.ReadStoreValues(0);
            //Assert.IsTrue(_bd.FileLenght == 27598);
        }
    }
}