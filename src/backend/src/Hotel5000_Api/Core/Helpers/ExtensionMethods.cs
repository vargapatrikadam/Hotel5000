using Core.Entities.LodgingEntities;
using Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public static ICollection<User> WithoutPasswords(this ICollection<User> users)
        {
            return users.Select(x => x.WithoutPassword()).ToList();
        }
        public static bool ValidatePassword(this string password, out Errors? error)
        {
            var input = password;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = Errors.PASSWORD_IS_EMPTY;
                return false;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,40}");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (!hasLowerChar.IsMatch(input))
            {
                error = Errors.PASSWORD_NOT_CONTAINS_LOWERCASE;
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                error = Errors.PASSWORD_NOT_CONTAINS_UPPERCASE;
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                error = Errors.PASSWORD_LENGTH_INCORRECT;
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                error = Errors.PASSWORD_NOT_CONTAINS_NUMERIC;
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool ValidateEmail(this string email, out Errors? error)
        {
            var input = email;
            error = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                error = Errors.EMAIL_IS_EMPTY;
                return false;
            }

            if (!new EmailAddressAttribute().IsValid(email))
            {
                error = Errors.EMAIL_INVALID;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}