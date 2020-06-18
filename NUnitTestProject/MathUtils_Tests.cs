using NUnit.Framework;
using HelloWorld;


namespace Tests
{
    public class MathUtils_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MathUtils_PrimeFactors_1()
        {
            var result = MathUtils.GetPrimeFactorization(1);

            Assert.AreEqual(result, "1");
        }

        [Test]
        public void MathUtils_PrimeFactors_100()
        {
            var result = MathUtils.GetPrimeFactorization(100);

            Assert.AreEqual(result, "2,2,5,5");
        }

        [Test]
        public void MathUtils_PrimeFactors_100string()
        {
            var result = MathUtils.GetPrimeFactorization("100");

            Assert.AreEqual(result, "2,2,5,5");
        }
    }
}
