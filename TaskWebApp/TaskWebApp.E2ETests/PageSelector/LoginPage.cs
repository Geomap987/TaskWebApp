using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApp.E2ETests.PageSelector
{
    public class LoginPage
    {
        public static By UserNameInput => By.CssSelector("#UserName");
        public static By PasswordInput => By.CssSelector("#Password");
        public static By SubmitButton => By.CssSelector("[type=submit]");

    }
}
