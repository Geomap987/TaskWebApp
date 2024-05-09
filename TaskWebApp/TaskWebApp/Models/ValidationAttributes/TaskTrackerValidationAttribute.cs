using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TaskWebApp.Models.ValidationAttributes
{
    public class TaskTrackerValidationAttribute : ValidationAttribute
    {
        private readonly Regex _regex;

        public TaskTrackerValidationAttribute()
        {
            _regex = new Regex(@"^[^<>&]+$");
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }
            var someStringValue = (string)value;
            return _regex.IsMatch(someStringValue);
        }
    }
}
