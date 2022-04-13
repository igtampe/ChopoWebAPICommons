using NUnit.Framework;
using System.IO;

namespace Igtampe.ChopoAuth.Test {
    public class Tests {
        [SetUp]
        public void Setup() { }

        [Test]
        public void SetPass() {

            //Let's make a user
            User U = new();
            
            U.UpdatePass("Cookies");
            Assert.True(U.CheckPass("Cookies"),"Password did not match");
            Assert.False(U.CheckPass("cookies"),"Password matched with incorrect password");

            U.UpdatePass("Galleta");
            Assert.False(U.CheckPass("Cookies"), "Password was not updated");
            Assert.True(U.CheckPass("Galleta"), "Password did not match");

        }

        [TearDown]
        public void TearDown() {
            if (File.Exists("AUTH_SALT.txt")) { File.Delete("AUTH_SALT.txt"); }
        }
    }
}