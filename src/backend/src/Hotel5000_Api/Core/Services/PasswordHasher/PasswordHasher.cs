using Core.Interfaces;
using Core.Interfaces.PasswordHasher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Core.Services.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly HashingOptions options;
        public PasswordHasher(IOption<HashingOptions> options)
        {
            this.options = options.option;
        }
        public bool Check(string hash, string password)
        {
            string[] hashParts = hash.Split('.', 3);

            if (hashParts.Length != 3)
                return false;

            int iterations = Convert.ToInt32(hashParts[0]);
            byte[] salt = Convert.FromBase64String(hashParts[1]);
            byte[] key = Convert.FromBase64String(hashParts[2]);

            using(Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256))
            {
                byte[] keyToCheck = algorithm.GetBytes(options.KeySize);

                return keyToCheck.SequenceEqual(key);
            }
        }

        public string Hash(string password)
        {
            using(Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(
                password,
                options.SaltSize,
                options.Iterations,
                HashAlgorithmName.SHA256))
            {
                string key = Convert.ToBase64String(algorithm.GetBytes(options.KeySize));
                string salt = Convert.ToBase64String(algorithm.Salt);
                return string.Format("{0}.{1}.{2}", options.Iterations, salt, key);
            }
        }
    }
}
