using NUnit.Framework;
using System.IO;

namespace Igtampe.Hashbrown.Test {
    public class Tests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void HashPermanenceTest() {
            //Create two hashbrowns
            Hashbrown Hash1 = new("PEPPER");

            string HashedByOne = Hash1.Hash("Teacup");
            string HashedByOneDiff = Hash1.Hash("Biscuit");

            Hashbrown Hash2 = new("PEPPER");
            string HashedByTwo = Hash2.Hash("Teacup");
            string HashedByTwoDiff = Hash2.Hash("Cookie");

            //Although some might argue that a cookie is the same as a biscuit, we must say otherwise for this example.

            Assert.AreEqual(HashedByOne, HashedByTwo, "Hash1 did not hash the same way to Hash2");
            Assert.AreNotEqual(HashedByOneDiff, HashedByTwoDiff, "Hash1 and Hash2 hashed different text to the same text");

        }

        [TearDown]
        public void TearDown() {
            if (File.Exists("PEPPER.txt")) { File.Delete("PEPPER.txt"); }
        }
    }
}