using NUnit.Framework;
using System;
using System.Threading;

namespace Igtampe.ChopoSessionManager.Test {
    public class Tests {

        ISessionManager Manager = SessionManager.Manager;

        const string U1 = "Hello";

        [SetUp]
        public void Setup() { }

        [Test]
        public void LoginLogout() {

            //Ensure the manager is empty
            Assert.AreEqual(0, Manager.Count, "Manager was not empty");

            //Login
            Guid SessionID = Manager.LogIn(U1);
            Assert.AreEqual(1, Manager.Count, "Manager was empty");

            Assert.True(Manager.LogOut(SessionID), "Logout was not successful");
            Assert.AreEqual(0, Manager.Count, "Manager was not empty");

        }

        [Test]
        public void LoginLogoutAll() {

            //Ensure the manager is empty
            Assert.AreEqual(0, Manager.Count, "Manager was not empty");

            //Login
            Manager.LogIn(U1);
            Manager.LogIn(U1);
            Manager.LogIn(U1);
            Manager.LogIn(U1);

            Assert.AreEqual(4, Manager.Count, "Not all logins went through");

            Assert.AreEqual(4, Manager.LogOutAll(U1), "Logout was not successful");
            Assert.AreEqual(0, Manager.Count, "Manager was not empty");

        }

        [Test]
        public void SessionExpiration() {

            //Ensure the manager is empty
            Assert.AreEqual(0, Manager.Count, "Manager was not empty");

            Session.ExtendTime = TimeSpan.FromSeconds(5);

            Guid ID = Manager.LogIn(U1);
            Session? S = Manager.FindSession(ID);
            Assert.False(S is null, "Session was already not found");

            //sleep for 5 seconds
            Thread.Sleep(2000);

            S = Manager.FindSession(ID);
            Assert.False(S is null, "Session was not found before it should've been able to expire");

            //Sleep for 3.5 seconds
            Thread.Sleep(3500);

            S = Manager.FindSession(ID);
            Assert.False(S is null, "Session was not found but it should've been extended");

            //Empty this out
            Thread.Sleep(6000);

            S = Manager.FindSession(ID);
            Assert.True(S is null, "Session was found even though it should've already expired");

            Assert.AreEqual(0, Manager.Count, "Session was not removed even though it already expired");


            Session.ExtendTime = TimeSpan.FromHours(12);

        }

        [Test]
        public void RemoveExpired() {

            Session.ExtendTime = TimeSpan.FromSeconds(4);

            Manager.LogIn(U1);
            Manager.LogIn(U1);
            Manager.LogIn(U1);
            Manager.LogIn(U1);

            //Wait 6 seconds for the sessions to expire
            Thread.Sleep(6000);

            Assert.AreEqual(4, Manager.RemoveExpiredSessions());

            Session.ExtendTime = TimeSpan.FromHours(12);
        }
    }
}