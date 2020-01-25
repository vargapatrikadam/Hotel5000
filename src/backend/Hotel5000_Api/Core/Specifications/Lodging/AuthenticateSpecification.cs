using Core.Entities.Lodging;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Lodging
{
    public class AuthenticateSpecification : BaseSpecification<User>
    {
        public AuthenticateSpecification(string username, string password, string email)
            : base (p => (p.Username == username || p.Email == email) && p.Password == password)
        {
            AddInclude(p => p.Role);
        }
    }
}
