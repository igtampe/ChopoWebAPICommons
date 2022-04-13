using NUnit.Framework;
using System.IO;

namespace Igtampe.ChopoAuth.Test {
    public class EqualityTests {
        [SetUp]
        public void Setup() { }

        [Test]
        public void EqualityTest() {
            User U1 = new() { Username = "D" };
            User U2 = new() { Username = "D" };
            User U3 = new() { Username = "E" };

            Assert.AreEqual(U1, U2, "U1 and U2 were not equal");
            Assert.AreNotEqual(U1, U3, "U1 and U3 were somehow equal");
        }

        [TearDown]
        public void TearDown() { if (File.Exists("AUTH_SALT.txt")) { File.Delete("AUTH_SALT.txt"); } }
    }
}