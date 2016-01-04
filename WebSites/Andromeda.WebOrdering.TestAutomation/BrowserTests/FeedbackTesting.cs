using Andromeda.WebOrdering.BrowserAutomation;
using Andromeda.WebOrdering.BrowserAutomation.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.WebOrdering.BrowserTests
{
    [TestClass]
    public class FeedbackTesting
    {
        [TestInitialize]
        public void Init()
        {
            Driver.Initialise();
        }

        [TestMethod]
        public void SmokeTest()
        {
            HomePage homePage = LandingPage.GoTo();

            Assert.IsTrue(homePage.IsAt, "Expected home page but didn't get it");

            FeedbackPage feedbackPage = homePage.MainMenu.GotoFeedback();

            Assert.IsTrue(feedbackPage.IsAt, "Expected feedback page but didn't get it");

            feedbackPage.ProvideFeedback("this is some feedback").WithName("bob").WithEmail("robert.dunn@androtech.com").WithCategory(2).Submit();
        }

        [TestMethod]
        public void TestFedbackRequired()
        {
            HomePage homePage = LandingPage.GoTo();

            Assert.IsTrue(homePage.IsAt, "Expected home page but didn't get it");

            FeedbackPage feedbackPage = homePage.MainMenu.GotoFeedback();

            Assert.IsTrue(feedbackPage.IsAt, "Expected feedback page but didn't get it");

            feedbackPage.ProvideFeedback("").WithName("bob").WithEmail("robert.dunn@androtech.com").WithCategory(2).Submit();

            Assert.AreEqual("Please enter some feedback", feedbackPage.ErrorMessage);
        }

        //[TestCleanup]
        //public void CleanUp()
        //{
        //    Driver.Close();
        //}
    }
}
