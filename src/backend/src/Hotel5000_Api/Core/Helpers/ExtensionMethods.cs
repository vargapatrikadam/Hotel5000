using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static User WithoutPassword(this User user)
        {
            user.Password = null;
            return user;
        }

        public static bool ValidatePassword(this string password, out string errorMessage)
        {
            var input = password;
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Password should not be empty";
                return false;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,40}");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (!hasLowerChar.IsMatch(input))
            {
                errorMessage = "Password should contain At least one lower case letter";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                errorMessage = "Password should contain At least one upper case letter";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                errorMessage = "Password should not be less than 8 or greater than 40 characters";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                errorMessage = "Password should contain At least one numeric value";
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool ValidateEmail(this string email, out string errorMessage)
        {
            var input = email;
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Email should not be empty";
                return false;
            }

            if (!new EmailAddressAttribute().IsValid(email))
            {
                errorMessage = "Email is not valid";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}