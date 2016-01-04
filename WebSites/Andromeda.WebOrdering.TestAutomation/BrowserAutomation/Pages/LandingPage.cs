using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.WebOrdering.BrowserAutomation.Pages
{
    public class LandingPage
    {
        public static HomePage GoTo()
        {
            Driver.Instance.Navigate().GoToUrl("http://localhost/websites/weborderingDEV/debug");
         //   var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(10));
        //    wait.Until(d => d.SwitchTo().ActiveElement().GetAttribute("id") == "user_login");

            return new HomePage();
        }

        //public static LoginCommand LoginAs(string userName)
        //{
        //    return new LoginCommand(userName);
        //}
    }
}
