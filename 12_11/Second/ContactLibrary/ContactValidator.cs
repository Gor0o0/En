
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContactLibrary
{
    public static class ContactValidator
    {
        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) &&
                   name.All(c => char.IsLetter(c) || c == ' ');
        }

        public static bool IsValidAge(string age)
        {
            return !string.IsNullOrEmpty(age) &&
                   age.All(char.IsDigit);
        }

        public static bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\+8\d{10}$");
        }

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(
                email,
                @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"
            );
        }
    }
}
