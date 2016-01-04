using Andromeda.WebOrdering.BrowserAutomation.Commands;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.WebOrdering.BrowserAutomation.Pages
{
    public class FeedbackPage
    {
        public FeedbackCommand ProvideFeedback(string feedback)
        {
            return new FeedbackCommand(feedback);
        }

        public bool IsAt
        {
            get
            {
                var element = Driver.Instance.FindElements(By.Id("feedbackContent"));
                return element.Count == 1;
            }
        }

        public string ErrorMessage
        {
            get
            {
                string errorText = null;

                var feedbackErrorWrapper = Driver.Instance.FindElement(By.Id("feedbackError"));
                if (feedbackErrorWrapper != null)
                {
                    var error = feedbackErrorWrapper.FindElement(By.TagName("p"));
                    errorText = error.Text;
                }

                return errorText;
            }
        }
    }
}
