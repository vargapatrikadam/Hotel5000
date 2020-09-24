using Core.Helpers.PasswordHasher;
using Core.Interfaces;
using Core.Interfaces.PasswordHasher;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Domain.Services.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly HashingOptions _options;

        public PasswordHasher(ISetting<HashingOptions> settings)
        {
            _options = settings.Option;
        }

        public bool Check(string hash, string password)
        {
            var hashParts = hash.Split('.', 3);

            if (hashParts.Length != 3)
                return false;

            var iterations = Convert.ToInt32(hashParts[0]);
            var salt = Convert.FromBase64String(hashParts[1]);
            var key = Convert.FromBase64String(hashParts[2]);

            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256))
            {
                var keyToCheck = algorithm.GetBytes(_options.KeySize);

                return keyToCheck.SequenceEqual(key);
            }
        }

        public string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                _options.SaltSize,
                _options.Iterations,
                HashAlgorithmName.SHA256))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(_options.KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);
                return string.Format("{0}.{1}.{2}", _options.Iterations, salt, key);
            }
        }
    }
}