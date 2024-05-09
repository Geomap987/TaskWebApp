using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApp.E2ETests.PageSelector
{
    public class TaskTrackerAddTaskPage
    {
        public static By AddTaskNameInput => By.CssSelector(".add-form__input_name");
        public static By AddTaskDescriptionInput => By.CssSelector(".add-form__input_description");
        public static By AddTaskSubmitButton => By.CssSelector(".add-form__button");
    }
}
