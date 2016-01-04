using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.WebOrdering.BrowserAutomation.Commands
{
    public class FeedbackCommand
    {
        private string feedback;
        private string name;
        private string email;
        private int category;

        public FeedbackCommand(string feedback)
        {
            this.feedback = feedback;
        }

        public FeedbackCommand WithName(string name)
        {
            this.name = name;
            return this;
        }

        public FeedbackCommand WithEmail(string email)
        {
            this.email = email;
            return this;
        }

        public FeedbackCommand WithCategory(int category)
        {
            this.category = category;
            return this;
        }

        public void Submit()
        {
            var nameInput = Driver.Instance.FindElement(By.Id("feedbackNameInput"));
            nameInput.SendKeys(this.name);

            var emailInput = Driver.Instance.FindElement(By.Id("feedbackEmailInput"));
            emailInput.SendKeys(this.email);

            var categoryInput = Driver.Instance.FindElement(By.Id("feedbackCategorySelect"));
            var selectElement = new SelectElement(categoryInput);
            selectElement.SelectByText("General feedback about the website");

            var feedbackInput = Driver.Instance.FindElement(By.Id("feedbackFeedbackInput"));
            feedbackInput.SendKeys(this.feedback);

            var submitButton = Driver.Instance.FindElement(By.Id("submitFeedbackButton"));
            if (submitButton != null)
            {
                submitButton.Click();
            }
        }
    }
}
