using Andromeda.WebOrdering.BrowserAutomation.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.WebOrdering.BrowserAutomation
{
    public class MainMenu
    {
        public LoginPage GotoLogin()
        {
            var button = Driver.Instance.FindElement(By.Id("mainMenuLoginButton"));

            if (button != null)
            {
                button.Click();

                var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
                wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
            }

            return new LoginPage();
        }

        //public HomePage GotoHome()
        //{
        //    var button = Driver.Instance.FindElement(By.Id("mainMenuHomeButton"));

        //    if (button != null)
        //    {
        //        button.Click();

        //        var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
        //        wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
        //    }

        //    return new HomePage();
        //}

        //public HomePage GotoOrderNow()
        //{  
        //    var button = Driver.Instance.FindElement(By.Id("menuOrderNow"));

        //    if (button != null)
        //    {
        //        button.Click();

        //        var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
        //        wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
        //    }

        //    return new HomePage();
        //}

        //public HomePage GotoStoreDetails()
        //{
 
        //    var button = Driver.Instance.FindElement(By.Id("menuStoreDetails"));

        //    if (button != null)
        //    {
        //        button.Click();

        //        var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
        //        wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
        //    }

        //    return new HomePage();
        //}

        //public HomePage GotoSelectStore()
        //{  
        //    var button = Driver.Instance.FindElement(By.Id("mainMenuSelectStoreButton"));

        //    if (button != null)
        //    {
        //        button.Click();

        //        var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
        //        wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
        //    }

        //    return new HomePage();
        //}

        //public HomePage GotoDeliveryAreaCheck()
        //{
        //    var button = Driver.Instance.FindElement(By.Id("mainMenuDeliveryAreaCheckButton"));

        //    if (button != null)
        //    {
        //        button.Click();

        //        var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
        //        wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
        //    }

        //    return new HomePage();
        //}

        //public HomePage GotoReturnToParent()
        //{  
        //    var button = Driver.Instance.FindElement(By.Id("mainMenuReturnToParentButton"));

        //    if (button != null)
        //    {
        //        button.Click();

        //        var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
        //        wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
        //    }

        //    return new HomePage();
        //}

        //public HomePage GotoMyProfile()
        //{
        //    var button = Driver.Instance.FindElement(By.Id("mainMenuMyProfileButton"));

        //    if (button != null)
        //    {
        //        button.Click();

        //        var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
        //        wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
        //    }

        //    return new HomePage();
        //}

        //public HomePage GotoMyOrders()
        //{ 
        //    var button = Driver.Instance.FindElement(By.Id("mainMenuMyOrdersButton"));

        //    if (button != null)
        //    {
        //        button.Click();

        //        var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
        //        wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
        //    }

        //    return new HomePage();
        //}

        //public HomePage GotoLogout()
        //{
      
        //    var button = Driver.Instance.FindElement(By.Id("mainMenuLogoutButton"));

        //    if (button != null)
        //    {
        //        button.Click();

        //        var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
        //        wait.Until(d => d.FindElement(By.Id("loginContent")) != null);
        //    }

        //    return new HomePage();
        //}

        public FeedbackPage GotoFeedback()
        {
            var button = Driver.Instance.FindElement(By.Id("mainMenuFeedbackFormButton"));

            if (button != null)
            {
                button.Click();

                var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
                wait.Until(d => d.FindElement(By.Id("feedbackContent")) != null);
            }

            return new FeedbackPage();
        }
    }
}
