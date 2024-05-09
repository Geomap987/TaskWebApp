using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApp.E2ETests.PageSelector
{
    public class TaskTrackerIndexPage
    {
        public static By AddTaskButton => By.CssSelector(".navigation-button");
        public static By TaskCard => By.CssSelector(".photo-grid__card");
    }
}
