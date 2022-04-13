using NUnit.Framework;

namespace Igtampe.Test {
    public class Tests {

        private class X1 : Identifiable<string> { }

        private class X2 : Identifiable<string> { }
        private class X3 : Identifiable<int> { }

        [SetUp]
        public void Setup() {
        }

        [Test]
        public void IdentifiableEqualsTest() {

            //Create all these things
            X1 O1X1 = new() { ID = "Sloop" };
            X1 O2X1 = new() { ID = "Sloop" };
            X1 O3X1 = new() { ID = "Sleep" };

            X2 O1X2 = new() { ID = "Sloop" };
            X2 O2X2 = new() { ID = "Sleep" };

            X3 O1X3 = new() { ID = 0 };

            Assert.AreEqual(O1X1, O2X1, "Identifiables of same type and same ID did not match");
            Assert.AreNotEqual(O1X1, O3X1, "Identifiables of same type and different ID matched");

            Assert.AreNotEqual(O1X1, O1X2, "Identifiables of different type but same ID type and same ID matched");
            Assert.AreNotEqual(O1X2, O2X2, "Identifiables of different type but same ID type and different ID matched");

            Assert.AreNotEqual(O1X3, O1X2, "Identifiables of different type and different ID type and different ID matched");

            Assert.Pass();
        }
    }
}