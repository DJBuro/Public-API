using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Andromeda.WebOrdering.BrowserAutomation;

namespace BrowserTests
{
    [TestClass]
    public class EndToEndTests
    {
        [TestInitialize]
        public void Init()
        {
            Driver.Initialise();
        }

        //[TestMethod]
        //public void EndToEnd()
        //{
        //    HomePage homePage = LandingPage.GoTo();

        //    Assert.IsTrue(homePage.IsAt, "Expected home page but didn't get it");

        //    LoginPage loginPage = homePage.MainMenu.GotoLogin();

        //    Assert.IsTrue(loginPage.IsAt, "Expected login page but didn't get it");

        //    loginPage.LoginAs("robert.dunn@androtech.com").WithPassword("aaaaaaaa");
        //}

        [TestCleanup]
        public void CleanUp()
        {
            Driver.Close();
        }
    }
}
