﻿namespace Core.Services.PasswordHasher
{
    public class HashingOptions
    {
        public int Iterations { get; set; }
        public int SaltSize { get; set; }
        public int KeySize { get; set; }
    }
}