using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.PasswordHasher
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Check(string hash, string password);
    }
}