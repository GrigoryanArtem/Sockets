using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Sockets.Chat.Client.GUI.Models.ValidationRules
{
    public class UserNameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
        {
            string username = (value as String);

            if (String.IsNullOrEmpty(username))
                return new ValidationResult(false, @"Username can't be empty");

            if (!CheckByRegex(username))
                return new ValidationResult(false, @"Incorrect username format");

            return new ValidationResult(true, null);
        }

        private bool CheckByRegex(string username)
        {
            return Regex.Match(username, ValidationConstants.UserNameRegex).Success;
        }
    }
}
