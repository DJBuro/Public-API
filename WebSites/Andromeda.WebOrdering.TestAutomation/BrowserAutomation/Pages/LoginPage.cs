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
    public class LoginPage
    {
        public LoginCommand LoginAs(string userName)
        {
            return new LoginCommand(userName);
        }

        public bool IsAt
        {
            get
            {
                var element = Driver.Instance.FindElement(By.Id("loginContent"));
                return element != null;
            }
        }
    }
}
