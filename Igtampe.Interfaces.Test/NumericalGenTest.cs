using Igtampe.IDGenerators;
using NUnit.Framework;

namespace Igtampe.Test {
    public class NumericalGeneratorTests {

        private class X1 : Identifiable<string> { }

        private class X2 : Identifiable<string> { }

        [SetUp]
        public void Setup() {
        }

        [Test]
        public void NumericalGeneratorTest() {

            NumericalGenerator NG5 = new(5);
            NumericalGenerator NG7 = new(7);

            Assert.AreEqual(5, NG5.Generate().Length, "NG5 did not generate an ID of length 5");
            Assert.AreEqual(7, NG7.Generate().Length, "NG7 did not generate an ID of length 7");

        }
    }
}