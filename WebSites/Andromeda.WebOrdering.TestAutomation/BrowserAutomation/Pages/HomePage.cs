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
    public class HomePage
    {
        public MainMenu MainMenu { get; set; }

        public HomePage()
        {
            this.MainMenu = new MainMenu();
        }

        public bool IsAt
        {
            get
            {
                var element = Driver.Instance.FindElements(By.Id("homeContent"));
                return element.Count == 1;
            }
        }
    }
}
