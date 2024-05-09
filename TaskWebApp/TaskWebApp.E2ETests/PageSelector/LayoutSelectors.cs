using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApp.E2ETests.PageSelector
{
    public class LayoutSelectors
    {
        public static By LoginLink => By.CssSelector(".login-link");
        public static By LogoutLink => By.CssSelector(".logout-link");
    }
}
