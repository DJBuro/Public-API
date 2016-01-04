using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.WebOrdering.BrowserAutomation.Commands
{
    public class LoginCommand
    {
        private readonly string userName;
        public string password { get; set; }

        public LoginCommand(string userName)
        {
            this.userName = userName;
        }

        public LoginCommand WithPassword(string password)
        {
            this.password = password;
            return this;
        }

        public void Login()
        {
            var loginInput = Driver.Instance.FindElement(By.Id("loginFormInput"));
            loginInput.SendKeys(userName);

            var passwordInput = Driver.Instance.FindElement(By.Id("password"));
            passwordInput.SendKeys(password);

            var loginWrapper = Driver.Instance.FindElement(By.Id("wp-loginButton"));
            if (loginWrapper != null)
            {
                var loginButton = loginWrapper.FindElement(By.TagName("a"));
                loginButton.Click();
            }
        }
    }
}
