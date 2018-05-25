// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Sockets.Chat.Client.GUI.Models.ValidationRules
{
    public class IpAddressRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo ci)
        {
            string username = (value as String);

            if (String.IsNullOrEmpty(username))
                return new ValidationResult(false, @"IP-address can't be empty");

            if (!CheckByRegex(username))
                return new ValidationResult(false, @"Incorrect IP-address format");

            return new ValidationResult(true, null);
        }

        private bool CheckByRegex(string username)
        {
            return Regex.Match(username, ValidationConstants.IpAddressRegex).Success;
        }
    }
}
